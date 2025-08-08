<template>
   <div v-if="loading" class="loading-indicator">
      <div class="spinner"></div>
      <span>加载中...</span>
  </div>
  <div v-else>
       <div class="reported-comment-container">
    <!-- 标题 -->
    <h1 class="comment-title">评论内容</h1>
    
    <!-- 评论元信息 -->
    <div class="comment-meta">
      <p><strong>举报者：</strong>{{ comment.readerName }}</p>
    </div>
    
    <!-- 评论内容 -->
    <div class="comment-content">
      {{ comment.commentContent}}
    </div>
    
    <!-- 返回按钮 -->
    <button @click="goBack" class="back-button">
      返回
    </button>
  </div>
  </div>
 
</template>

<script setup>
import { ref, onMounted} from 'vue'
import {getReportById} from '@/API/Report_API'
import { useRouter,useRoute } from 'vue-router'
import{getReader} from '@/API/Reader_API'
import{getComment} from '@/API/Comment_API'
//----------------------------------------------------------------------------------------------------------------
//获取评论数据
const route = useRoute()
const comment=ref({
  reportId: '',
  reason: '',
  reportTime: '',
  progress: '',
  commentId: '',
  readerId: '',
  commentContent: '未知',
  readerName: '未知'
})
const getReportData=async () => {
  loading.value = true
  try {
    const response = await getReportById(route.query.id)
    if(response){
      comment.value  = {
        reportId: response.reportId,
        reason: response.reason,
        reportTime: response.reportTime,
        progress: response.progress,
        commentId: response.commentId,
        readerId: response.readerId,
        commentContent:'未知',
        readerName: '未知'
      }
    }
    const readerResponse = await getReader(comment.value.readerId)
    if(readerResponse){ 
      comment.value.readerName = readerResponse.readerName
    }
    const commentResponse = await getComment(comment.value.commentId)
    if(commentResponse){
      comment.value.commentContent = commentResponse.content
    }
  } catch (error) {
    console.error('获取举报数据失败:', error)
  } finally {
    loading.value = false
  }
 
}
//----------------------------------------------------------------------------------------------------------------
const loading = ref(false)  // 加载状态
//----------------------------------------------------------------------------------------------------------------
const router = useRouter();



const goBack = () => {
  router.go(-1); // 返回上一页
  // 或者指定路由：router.push('/reported-comments-list')
};
onMounted(() => {
  getReportData() // 页面加载时调用 store 方法
})
</script>

<style scoped>
/* 加载动画样式 */
.loading-indicator {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  margin: 20px 0;
  color: #666;
  font-size: 16px;
}

.spinner {
  width: 24px;
  height: 24px;
  border: 3px solid #ccc;
  border-top-color: #486482;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}
.reported-comment-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 30px;
  font-family: 'PingFang SC', 'Microsoft YaHei', sans-serif;
}

.comment-title {
  font-size: 24px;
  font-weight: bold;
  color: #333;
  margin-bottom: 20px;
  padding-bottom: 10px;
  border-bottom: 1px solid #eee;
}

.comment-meta {
  background-color: #f8f8f8;
  padding: 15px;
  border-radius: 6px;
  margin-bottom: 20px;
}

.comment-meta p {
  margin: 8px 0;
  color: #555;
}

.comment-content {
  background-color: #f5f5f7;
  padding: 20px;
  border-radius: 6px;
  line-height: 1.6;
  color: #333;
  white-space: pre-line; /* 保留换行符 */
}

.back-button {
  display: block;
  margin-top: 30px;
  padding: 10px 20px;
  background-color: #f0f0f0;
  color: #333;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
  transition: background-color 0.3s;
}

.back-button:hover {
  background-color: #42b983;
}
</style>