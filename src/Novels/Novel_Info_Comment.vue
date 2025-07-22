<template>
  <div class="comment-section">
    <h2 class="section-title">📌 精选评论</h2>

    <div v-if="comments.length > 0" class="comment-grid">
      <div
        v-for="comment in comments"
        :key="comment.commentId"
        class="comment-card"
      >
        <div class="comment-header">
          <div class="avatar-placeholder">👤</div>
          <div class="comment-info">
            <h3 class="comment-title">{{ comment.title }}</h3>
            <p class="comment-subtitle">
              第 {{ comment.chapterId }} 章 · {{ formatTime(comment.createTime) }}
            </p>
          </div>
          <div class="likes" @click="toggleLike(comment)">
            <span
              :class="['like-icon', { liked: likedCommentIds.has(comment.commentId) }]"
            >
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
import {
  likeComment,
  unlikeComment,
  isLiked
} from '@/API/Likes_API'

import { SelectNovel_State, readerState } from '@/stores/index'

const selectNovelState = SelectNovel_State()
const readerStore = readerState()
const readerId = readerStore.readerId
const novelId = selectNovelState.novelId

const comments = ref([])
const likedCommentIds = ref(new Set())

onMounted(async () => {
  try {
    const response = await getTopLikedComments(novelId, 10)
    comments.value = response || []

    for (const comment of comments.value) {
      const res = await isLiked(comment.commentId, readerId)
      if (res?.isLiked === true || res === true) {
        likedCommentIds.value.add(comment.commentId)
      }
    }
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
    } else {
      await unlikeComment(id, readerId)
      likedCommentIds.value.delete(id)
      comment.likes -= 1
    }
  } catch (error) {
    console.error('点赞操作失败:', error)
  }
}

function formatTime(isoString) {
  const date = new Date(isoString)
  return date.toLocaleString()
}
</script>

<style scoped>
.comment-section {
  padding: 24px;
  background: #f9f9fb;
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

.avatar-placeholder {
  width: 40px;
  height: 40px;
  background-color: #e0e0e0;
  border-radius: 50%;
  font-size: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 12px;
  flex-shrink: 0;
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
