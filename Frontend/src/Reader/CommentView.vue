<template>
  <div class="user-comments">
    <h2>我发布的评论及回复</h2>

    <div v-if="loading" class="status-message">加载中...</div>
    <div v-else-if="error" class="status-message error">错误: {{ error }}</div>
    <div v-else-if="userComments.length === 0" class="no-comments">暂无评论</div>

    <div v-else>
      <div
        v-for="item in userComments"
        :key="item.parentComment.commentId"
        class="comment-bubble"
      >
        <div class="comment-header" @click="toggleReplies(item.parentComment.commentId)" style="cursor: pointer;">
          <span class="novel-id">
            小说 ID：{{ item.parentComment.novelId }}
            <span v-if="item.parentComment.preComId"> | 回复：评论ID {{ item.parentComment.preComId }}</span>
            <span v-else> | 一级评论</span>
          </span>

          <span class="comment-title">{{ item.parentComment.title }}</span>

          <!-- 作者ID展示 -->
         <span
  v-if="item.parentComment.readerId !== readerId"
  class="author-id"
  title="该评论的作者ID"
>
  作者ID：{{ item.parentComment.readerId || '未知' }}
</span>

          <!-- 如果一级评论不是本人发布，显示红色提示文字 -->
          <span
            v-if="item.parentComment.readerId !== readerId"
            class="other-user-indicator"
            title="这是他人发布的一级评论，你的回复在这里"
          >
            【回复他人评论】
          </span>

          <span class="toggle-replies-indicator">
            {{ expandedCommentId === item.parentComment.commentId ? '收起回复 ▲' : '展开回复 ▼' }}
          </span>
        </div>

        <div class="comment-content">
          {{ item.parentComment.content || '无内容' }}
        </div>

        <div class="comment-meta">
          <span>点赞：{{ item.parentComment.likes }}</span>
          <span>状态：{{ item.parentComment.status }}</span>
          <span>时间：{{ item.parentComment.createTime || '未知' }}</span>
        </div>

        <button class="delete-btn" @click.stop="handleDelete(item.parentComment.commentId)">删除</button>

        <div v-if="expandedCommentId === item.parentComment.commentId" class="replies">
          <div v-if="!item.childComments || item.childComments.length === 0" class="no-replies">暂无回复</div>
          <ul v-else>
            <li
              v-for="reply in item.childComments"
              :key="reply.commentId"
              class="reply-item"
            >
              <div class="reply-header">
                <strong>用户ID：{{ reply.readerId || '无用户' }}</strong>（评论ID：{{ reply.commentId }}）
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
</template>

<script setup>
import { onMounted, ref } from 'vue'
import { getCommentsByReaderId, deleteCommentRecursive } from '@/API/Comment_API' // 你的新API
import { readerState } from '@/stores/index'

const store = readerState()
const readerId = store.readerId
const userComments = ref([])  // 存放 {parentComment, childComments} 对象数组

const loading = ref(true)
const error = ref(null)

const expandedCommentId = ref(null)

onMounted(async () => {
  try {
    const res = await getCommentsByReaderId(readerId)
    userComments.value = res || []
  } catch (err) {
    // 如果是 404，表示无评论，给空数组，不显示错误
    if (err.response && err.response.status === 404) {
      userComments.value = []
      error.value = null
    } else {
      error.value = '获取评论失败'
      console.error(err)
    }
  } finally {
    loading.value = false
  }
})

async function handleDelete(commentId) {
  if (!window.confirm('确定删除该评论吗？')) return

  try {
    await deleteCommentRecursive(commentId)
    userComments.value = userComments.value.filter(c => c.parentComment.commentId !== commentId)
    if (expandedCommentId.value === commentId) expandedCommentId.value = null
  } catch (err) {
    alert('删除失败，请稍后重试')
    console.error(err)
  }
}

function toggleReplies(commentId) {
  expandedCommentId.value = expandedCommentId.value === commentId ? null : commentId
}
</script>

<style scoped>
.user-comments {
  margin-left: 24px;
  margin-top: 30px;
}

.no-comments {
    text-align: center;
  font-size: 18px;
  margin: 20px 0;
}

.comment-bubble {
  background-color: #ffffff; 
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
  flex-wrap: wrap;
}

.novel-id {
  font-size: 15px;
  color: #ed424b;
  background-color:  #fff0f0;
  padding: 2px 6px;
  border-radius: 10px;
  white-space: nowrap;
}

.comment-title {
  font-weight: bold;
  font-size: 18px;
  color: #222;
  white-space: nowrap;
}

/* 作者ID样式 */
.author-id {
  font-size: 14px;
  color: #555;
  background-color: #f0f0f0;
  padding: 2px 6px;
  border-radius: 6px;
  margin-left: 8px;
  user-select: none;
  white-space: nowrap;
}

/* 回复他人评论提示 */
.other-user-indicator {
  color: #e55353;
  font-weight: 600;
  margin-left: 10px;
  background-color: #ffe6e6;
  padding: 2px 6px;
  border-radius: 6px;
  font-size: 14px;
  user-select: none;
  white-space: nowrap;
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
  background-color:  #ed424b;
  border: none;
  color: white;
  padding: 6px 12px;
  border-radius: 12px;
  font-size: 13px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.delete-btn:hover {
  background-color:  #c6575d;
}

.replies {
  margin-top: 12px;
  padding-left: 20px;
  border-left: 2px solid  #ed424b;
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
