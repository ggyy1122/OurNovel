<template>
  <div class="user-comments">
    <h2>我发布的评论</h2>

    <div v-if="loading" class="status-message">加载中...</div>
    <div v-else-if="error" class="status-message error">错误{{ error }}</div>
    <div v-else-if="userComments.length === 0" class="no-comments">暂无评论</div>

    <div v-else>
      <div
        v-for="comment in userComments"
        :key="comment.commentId"
        class="comment-bubble"
      >
        <div class="comment-header" @click="toggleReplies(comment.commentId)" style="cursor: pointer;">
          <span class="novel-id">
            小说 ID：{{ comment.novelId }}
            <span v-if="comment.preComId"> | 回复：评论ID {{ comment.preComId }}</span>
            <span v-else> | 一级评论</span>
          </span>
          <span class="comment-title">{{ comment.title }}</span>
          <span class="toggle-replies-indicator">
            {{ expandedCommentId === comment.commentId ? '收起回复 ▲' : '展开回复 ▼' }}
          </span>
        </div>

        <div class="comment-content">
          {{ comment.content || '无内容' }}
        </div>

        <div class="comment-meta">
          <span>点赞：{{ comment.likes }}</span>
          <span>状态：{{ comment.status }}</span>
          <span>时间：{{ comment.createTime || '未知' }}</span>
        </div>

        <div>
          <button class="delete-btn" @click.stop="handleDelete(comment.commentId)">删除</button>
        </div>

        <!-- 回复区域 -->
        <div v-if="expandedCommentId === comment.commentId" class="replies">
          <div v-if="loadingReplies" class="loading">回复加载中...</div>
          <div v-else>
            <div v-if="!repliesMap[comment.commentId] || repliesMap[comment.commentId].length === 0" class="no-replies">暂无回复</div>
            <ul>
              <li v-for="reply in repliesMap[comment.commentId]" :key="reply.commentId" class="reply-item">
                <div class="reply-header">
                  <strong>用户：{{ reply.readerId || '无用户' }}</strong>（评论ID：{{ reply.commentId }}）
                </div>
                <div class="reply-title">{{ reply.title || '无标题' }}</div>
                <div class="reply-content">{{ reply.content || '无内容' }}</div>
                <div class="reply-meta">
                  点赞：{{ reply.likes }} |
                  时间：{{ reply.createTime ? new Date(reply.createTime).toLocaleString() : '未知时间' }}
                </div>
              </li>
            </ul>
          </div>
        </div>

      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import { getAllComments, deleteCommentRecursive, getComment } from '@/API/Comment_API'
import { getRepliesByParentId, getReplyByCommentId } from '@/API/CommentReply_API'
import { readerState } from '@/stores/index'

const store = readerState()
const readerId = store.readerId
const userComments = ref([])

const loading = ref(true)
const error = ref(null)

const expandedCommentId = ref(null)
const repliesMap = ref({})
const loadingReplies = ref(false)

onMounted(async () => {
  try {
    const allComments = await getAllComments()
    const filtered = allComments.filter(comment => comment.readerId === readerId)

    for (const c of filtered) {
      try {
        const replyRelation = await getReplyByCommentId(c.commentId)
        c.preComId = replyRelation.preComId || null
      } catch {
        c.preComId = null
      }
    }

    userComments.value = filtered
  } catch (err) {
    error.value = '获取评论失败'
    console.error(err)
  } finally {
    loading.value = false
  }
})

async function handleDelete(commentId) {
  const confirmed = window.confirm('确定删除该评论吗？')
  if (!confirmed) return

  try {
    await deleteCommentRecursive(commentId)
    userComments.value = userComments.value.filter(c => c.commentId !== commentId)
    if (expandedCommentId.value === commentId) {
      expandedCommentId.value = null
    }
  } catch (err) {
    alert('删除失败，请稍后重试')
    console.error(err)
  }
}

async function toggleReplies(commentId) {
  if (expandedCommentId.value === commentId) {
    expandedCommentId.value = null
  } else {
    expandedCommentId.value = commentId
    if (!repliesMap.value[commentId]) {
      loadingReplies.value = true
      try {
        const replySummaries = await getRepliesByParentId(commentId)
        const detailedReplies = await Promise.all(
          replySummaries.map(reply => getComment(reply.commentId))
        )
        repliesMap.value[commentId] = detailedReplies
      } catch (e) {
        repliesMap.value[commentId] = []
        console.error('获取回复失败', e)
      } finally {
        loadingReplies.value = false
      }
    }
  }
}
</script>

<style scoped>
.user-comments {
  margin-left: 24px;
  margin-top: 30px;
}

.no-comments {
  color: #999;
  font-style: italic;
  font-size: 14px;
}

.comment-bubble {
  background-image: linear-gradient(to top, #ffffff 0%, #f9faec 99%, #f2ebd7 100%);
  border-radius: 8px;
  padding: 16px 20px;
  margin-bottom: 16px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
  transition: box-shadow 0.3s ease;
}

.comment-bubble:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.12);
}

.comment-header {
  display: flex;
  align-items: baseline;
  gap: 10px;
  margin-bottom: 10px;
}

.novel-id {
  font-size: 15px;
  color: #f5c021;
  background-color: #f1eedb;
  padding: 2px 6px;
  border-radius: 10px;
}

.comment-title {
  font-weight: bold;
  font-size: 18px;
  color: #222;
}

.comment-content {
  font-size: 16px;
  color: #444;
  line-height: 1.6;
  margin-bottom: 10px;
  white-space: pre-wrap;
}

.comment-meta {
  font-size: 15px;
  color: #777;
  display: flex;
  gap: 16px;
  flex-wrap: wrap;
}

.delete-btn {
  background-color: #e7bc3c;
  border: none;
  color: white;
  padding: 6px 12px;
  border-radius: 12px;
  font-size: 13px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.delete-btn:hover {
  background-color: #c0ac2b;
}

.replies {
  margin-top: 12px;
  padding-left: 20px;
  border-left: 2px solid #f5c021;
}

.reply-item {
  margin-bottom: 8px;
  font-size: 14px;
  color: #555;
}

.reply-meta {
  margin-left: 10px;
  color: #aaa;
  font-size: 12px;
}

.loading, .no-replies {
  font-style: italic;
  color: #999;
  font-size: 13px;
}

.toggle-replies-indicator {
  margin-left: auto;
  font-size: 17px;
  color: #999;
  user-select: none;
}
</style>
