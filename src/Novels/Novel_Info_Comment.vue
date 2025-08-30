<template>
  <div class="comment-section">
    <h2 class="section-title">📌 精选评论</h2>

    <div v-if="comments.length > 0" class="comment-grid">
      <div v-for="comment in comments" :key="comment.commentId" class="comment-card">
        <div class="comment-header">
          <!-- 替换 👤 为头像，其它不动 -->
          <div class="avatar" @click="goReaderHome(comment.readerId)">
            <img :src="getReaderAvatar(comment.readerId)" alt="用户头像" class="user-avatar" @error="handleAvatarError" />
          </div>

          <div class="comment-info">
            <h3 class="comment-title" @click="goReaderHome(comment.readerId)">{{ getReaderName(comment.readerId) }}</h3>
            <p class="comment-subtitle">
              第 {{ comment.chapterId }} 章 · {{ formatTime(comment.createTime) }}
              <br />
              {{ comment.title }}
            </p>
          </div>

          <div class="likes" @click="toggleLike(comment)">
            <span :class="['like-icon', { liked: likedCommentIds.has(comment.commentId) }]">
              {{ likedCommentIds.has(comment.commentId) ? '❤️' : '🤍' }}
            </span>
            {{ comment.likes }}
          </div>
        </div>

        <p class="comment-content">{{ comment.content || '（无正文）' }}</p>
      </div>
    </div>

    <p v-else class="no-comments">暂无精选评论~</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getTopLikedComments } from '@/API/Comment_API'
import { likeComment, unlikeComment, isLiked } from '@/API/Likes_API'
import { getReader } from '@/API/Reader_API'  // 使用你已有的 API
import { SelectNovel_State, readerState } from '@/stores/index'
import 'vue3-toastify/dist/index.css'
import { toast } from 'vue3-toastify'
// 在 script 部分添加路由导入和函数
import { useRouter } from 'vue-router'

const router = useRouter()

function goReaderHome(readerId) {
  router.push(`/reader/${readerId}`)
}

const defaultAvatar = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/e165315c-da2b-42c9-b3cf-c0457d168634.jpg';
const selectNovelState = SelectNovel_State()
const readerStore = readerState()
const readerId = readerStore.readerId
const novelId = selectNovelState.novelId

const comments = ref([])
const likedCommentIds = ref(new Set())

// 头像/昵称缓存：readerId -> { avatarUrl(完整可用URL), readerName }
const readersMap = ref(new Map())

// 如果后端 avatarUrl 只返回 Key（文件名），用它拼完整 URL
const OSS_BASE = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/'

// 把任意 avatar 字段格式化成"可直接 <img :src> 的完整 URL"
function formatAvatarUrl(raw) {
  if (!raw) return defaultAvatar
  if (/^https?:\/\//i.test(raw)) return raw               // 已是 http(s)
  if (raw.startsWith(OSS_BASE)) return raw                 // 已带域名
  return OSS_BASE + encodeURIComponent(raw.replace(/^\/+/, '')) // 认为是 key
}

// 归一化 getReader 的返回结构
function normalizeReader(r, id) {
  const data = r?.data ?? r ?? {}
  return {
    readerId: data.readerId ?? id,
    readerName: data.readerName || `读者${id}`,
    avatarUrl: formatAvatarUrl(data.avatarUrl)
  }
}

onMounted(async () => {
  try {
    const response = await getTopLikedComments(novelId, 10)
    comments.value = Array.isArray(response) ? response : []

    // 点赞状态
    for (const c of comments.value) {
      const res = await isLiked(c.commentId, readerId)
      if (res?.isLiked === true || res === true) likedCommentIds.value.add(c.commentId)
    }

    // 批量补齐头像/昵称（用 getReader）
    const ids = Array.from(new Set(comments.value.map(c => c.readerId))).filter(Boolean)
    await Promise.all(ids.map(async (id) => {
      if (readersMap.value.has(id)) return
      try {
        const r = await getReader(id)
        readersMap.value.set(id, normalizeReader(r, id))
      } catch {
        readersMap.value.set(id, { avatarUrl: defaultAvatar, readerName: `读者${id}` })
      }
    }))
  } catch (error) {
    console.error('加载精选评论失败:', error)
  }
})

async function toggleLike(comment) {
  const id = comment.commentId
  const hasLiked = likedCommentIds.value.has(id)

  try {
    if (!hasLiked) {
      await likeComment(id, readerId)
      likedCommentIds.value.add(id)
      comment.likes += 1
      toast.success('点赞成功 🎉')
    } else {
      await unlikeComment(id, readerId)
      likedCommentIds.value.delete(id)
      comment.likes -= 1
      toast.info('取消点赞成功 🧹')
    }
  } catch (error) {
    console.error('点赞操作失败:', error)
    toast.error('点赞操作失败，请稍后重试！')
  }
}

function getReaderAvatar(id) {
  return readersMap.value.get(id)?.avatarUrl || defaultAvatar
}

// 添加获取读者名称的函数
function getReaderName(id) {
  return readersMap.value.get(id)?.readerName || `读者${id}`
}

function handleAvatarError(e) {
  e.target.src = defaultAvatar
}

function formatTime(isoString) {
  const date = new Date(isoString)
  return date.toLocaleString()
}
</script>

<style scoped>
.comment-section {
  padding: 24px;
  background: #edf1f1ff;
  border-radius: 12px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.04);
}

.section-title {
  font-size: 22px;
  margin-bottom: 16px;
  color: #333;
  font-weight: 600;
  text-align: left;
}

.comment-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 16px;
}

.comment-card {
  background-color: #fff;
  border-radius: 10px;
  padding: 16px;
  box-shadow: 0 2px 8px rgba(100, 100, 100, 0.1);
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.comment-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 6px 16px rgba(80, 80, 80, 0.15);
}

.comment-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 12px;
}

/* 头像容器（保持你原布局，仅替换 👤） */
.avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  overflow: hidden;
  margin-right: 12px;
  flex-shrink: 0;
  cursor: pointer;
  transition: transform 0.2s ease;
}

.avatar:hover {
  transform: scale(1.1);
}

.user-avatar {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.comment-info {
  flex: 1;
  text-align: left;
}

.comment-title {
  font-size: 16px;
  font-weight: 600;
  margin: 0;
  color: #333;
}

.comment-title:hover {
  color: #f0940a;
}

.comment-subtitle {
  font-size: 13px;
  color: #777;
  margin-top: 2px;
}

.likes {
  font-size: 14px;
  color: #ff5a5a;
  font-weight: bold;
  margin-left: 12px;
  white-space: nowrap;
  cursor: pointer;
  transition: transform 0.2s;
}

.likes:hover {
  transform: scale(1.15);
}

.like-icon {
  margin-right: 5px;
  transition: color 0.2s;
}

.like-icon.liked {
  color: #ff4d4f;
}

.comment-content {
  font-size: 14px;
  color: #444;
  line-height: 1.6;
  margin-top: 8px;
  white-space: pre-line;
}

.no-comments {
  font-size: 15px;
  color: #999;
  text-align: center;
  margin-top: 30px;
}
</style>