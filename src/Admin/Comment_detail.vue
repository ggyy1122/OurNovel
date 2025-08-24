<template>
   <div v-if="loading" class="loading-indicator">
      <div class="spinner"></div>
      <span>加载中...</span>
  </div>
  <div v-else>
    <div class="reported-comment-container">
    <!-- 标题 -->
    <h1 class="comment-title">评论内容</h1>

    <!-- 评论内容 -->
     <div class="background">
        <div class="comment-content-row">
      <img
        class="commenter-avatar"
        :src="comment.commentUrl"
        alt="头像"
      />
      <div class="comment-content-col">
        <div class="commenter-name">{{ comment.commenterName }}</div>
        <div class="comment-content-text">{{ comment.commentContent }}</div>
        <div class="comment-info-row">
          <span class="comment-time">{{ comment.commitTime }}</span>
          <span class="comment-info-gap"></span>
          <span class="comment-likes">
            <i class="fas fa-thumbs-up"></i>
             {{ comment.likes ?? 0 }}赞</span>
        </div>
      </div>
    </div>
    
    <!-- 评论元信息 -->
    <div class="comment-meta">
      <p><strong>举报者：</strong>{{ comment.readerName }}</p>
      <p><strong>举报原因：</strong>{{ comment.reason }}</p>
      <p><strong>举报时间：</strong>{{ comment.reportTime }}</p>
      
    </div>
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
        reason: '',
        reportTime: '',
        progress:'',
        commentId: '',
        commentContent:'',
        commitTime:'',
        commenterName:'',
        commentUrl:'',
        likes:0,
        readerId: '',
        readerUrl:'',
        readerName: '',
})
const getReportData=async () => {
  loading.value = true
  try {
    const response = await getReportById(route.query.id)
    if(response){
      comment.value  = {
        reason: response.reason,
        reportTime: response.reportTime,
        progress: response.progress,
        commentId: response.commentId,
        commentContent:'未知',
        commitTime:'未知',
        commenterId:'未知',
        commenterName:'未知',
        commentUrl:'未知',
        likes:0,
        readerId: response.readerId,
        readerUrl:'未知',
        readerName: '未知',
      }
    }
    const readerResponse = await getReader(comment.value.readerId)
    if(readerResponse){ 
      comment.value.readerName = readerResponse.readerName,
      comment.value.readerUrl= getFullCoverUrl(readerResponse.avatarUrl)
    }
    const commentResponse = await getComment(comment.value.commentId)
    if(commentResponse){
      comment.value.commentContent = commentResponse.content,
      comment.value.commitTime = commentResponse.createTime,
      comment.value.commenterId = commentResponse.readerId,
      comment.value.likes=commentResponse.likes
    }
    const commenterResponse = await getReader(comment.value.commenterId)
    if(commenterResponse){
      comment.value.commenterName = commenterResponse.readerName,
      comment.value.commentUrl = getFullCoverUrl(commenterResponse.avatarUrl)
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

//----------------------------------------------------------------------------------------------------------------
 // 统一的封面URL处理方法
const getFullCoverUrl=(partialPath)=> {
    if (!partialPath) {
      // 随机默认封面
      return 'https://picsum.photos/300/400?random=' + Math.floor(Math.random() * 1000)
    }
    if (partialPath.startsWith('http')) return partialPath
    const ossBase = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/'
    return `${ossBase}${partialPath.replace(/^\//, '')}`
  }
//----------------------------------------------------------------------------------------------------------------
const router = useRouter();

const goBack = () => {
  router.go(-1); // 返回上一页
  // 或者指定路由：router.push('/reported-comments-list')
};
//----------------------------------------------------------------------------------------------------------------
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
  padding: 15px;
  border-radius: 6px;
  margin-top:20px;
  margin-bottom: 20px;
}

.comment-meta p {
  margin: 8px 0;
  color: #555;
}

.comment-content-row {
  display: flex;
  align-items: flex-start;
  background-color: #FFFFF9;
  padding: 20px;
  border-radius: 6px;
  margin-bottom: 10px;
}

.commenter-avatar {
  width: 56px;
  height: 56px;
  border-radius: 50%;
  object-fit: cover;
  margin-right: 18px;
  background: #eee;
  flex-shrink: 0;
}

.comment-content-col {
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
}

.commenter-name {
  color: #2979ff;
  font-size: 14px;
  font-weight: 500;
  margin-bottom: 8px;
}

.comment-content-text {
  color: #222;
  font-size: 15px;
  line-height: 1.6;
  white-space: pre-line;
}

.comment-info-row {
  display: flex;
  align-items: center;
  margin-top: 8px;
  font-size: 12px;
  color: #888;
}

.comment-time {
  margin-right: 8px;
}

.comment-info-gap {
  display: inline-block;
  width: 4px;
  height: 4px;
  border-radius: 50%;
  background-color: #ccc;
  margin-right: 8px;
}

.comment-likes {
  color: #333;
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

.background {
  background-color: #f9f9fc;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}
</style>