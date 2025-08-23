<template>
  <div v-if="loading" class="loading-indicator">
      <div class="spinner"></div>
      <span>加载中...</span>
  </div>
  <div v-else>
      <h1 style="margin-left:140px">举报处理 - 待处理</h1>
  <div class="complaints-container">
    <table class="complaint-table">
      <thead>
        <tr>
          <th>评论内容</th>
          <th>举报原因</th>
          <th>举报时间</th>
          <th>操作</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in reports" :key="item.reportId">
          <td>{{ item.commentContent }}</td>
          <td>{{ item.reason }}</td>
          <td>{{ item.reportTime }}</td>
             <td>
              <button
                class="view-btn"
                @click="view(item.reportId)"
              >查看</button>
              <button 
                class="approve-btn" 
                @click="approve(novel.novelId)"
              >审核通过</button>
              <button 
                class="reject-btn" 
                @click="reject(novel.novelId)"
              >审核不通过</button>
            </td>
        </tr>
      </tbody>
    </table>
   
  </div>
  </div>
  
</template>

<script setup>
import { ref, onMounted } from 'vue'
import {getAllReports} from '@/API/Report_API'
import{getComment} from '@/API/Comment_API'
import{useRouter } from 'vue-router'
//----------------------------------------------------------------------------------------------------------------
//获取评论数据
const reports=ref([])
const getReportsData=async () => {
  loading.value = true
  try {
    const response = await getAllReports()
    reports.value  = response.filter(report => report.progress === '未处理').map(report => ({
      reportId: report.reportId,
      reason: report.reason,
      reportTime: report.reportTime,
      progress: report.progress,
      commentId: report.commentId,
      readerId: report.readerId,
      commentContent: '未知' // 初始值，稍后通过 API 获取
    }))
     // 逐个获取评论内容
    for (const report of reports.value) {
      try {
        const comment = await getComment(report.commentId)
        if (comment && comment.content) {
           if (comment.content.length > 10) {
              report.commentContent = comment.content.substring(0, 10) + '...'
            } else {
              report.commentContent = comment.content
            }
        }
      } catch (err) {
        report.commentContent = '获取失败'
      }
    }
  } catch (error) {
    console.error('获取举报数据失败:', error)
  } finally {
    loading.value = false
  }
 
}
//----------------------------------------------------------------------------------------------------------------
//从数据库获取信息时的加载中动画
const loading = ref(false)  // 加载状态
//----------------------------------------------------------------------------------------------------------------
//查看举报详情
const router = useRouter()
const view = (reportId) => {
  // 跳转到评论详情页面，传递举报ID
  router.push({ path: '/Admin/Admin_Layout/comment_detail', query: { id: reportId } })
}







onMounted(() => {
  getReportsData() // 页面加载时调用
})




</script>

<style scoped>
.complaints-container {
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
  padding: 24px;
  margin-left:145px;
  margin-right:143px;
  margin-top:0px;
  max-width: 1150px;
}
.complaint-table {
  width: 100%;
  border-collapse: collapse;
}
.complaint-table th, .complaint-table td {
  padding: 12px 8px;
  border-bottom: 1px solid #eee;
  text-align: center;
}
.complaint-table tr{
  transition: background-color 0.2s;
}
.complaint-table tr:hover {
  background-color: #E3E3E3;
}
.complaint-table th {
  background: #dde3ee;
}
.complaint-table th:nth-child(1),
.complaint-table td:nth-child(1) {
  width: 150px; /* 评论内容列 */
}
.complaint-table th:nth-child(2),
.complaint-table td:nth-child(2) {
  width: 160px; /* 举报原因列 */
}
.complaint-table th:nth-child(3),
.complaint-table td:nth-child(3) {
  width: 150px; /* 举报时间列 */
}
.complaint-table th:nth-child(4),
.complaint-table td:nth-child(4) {
  width: 250px; /* 操作列 */
}
.view-link {
  color: #42b983;
  cursor: pointer;
  text-decoration: underline;
}
.action-dots {
  cursor: pointer;
  font-size: 20px;
  padding: 0 8px;
}
.action-menu {
  position: absolute;
  right: 0;
  top: 24px;
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 4px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.12);
  min-width: 80px;
  z-index: 10;
}
.action-item {
  padding: 8px 16px;
  cursor: pointer;
  font-size: 14px;
}
.action-item:hover {
  background: #f0f0f0;
}
.batch-actions {
  margin-top: 16px;
  text-align: right;
}
.batch-actions button {
  margin-left: 12px;
  padding: 6px 18px;
  background: #42b983;
  color: #fff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
.batch-actions button:hover {
  background: #2c3e50;
}


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

.approve-btn {
  margin-right: 10px;
  padding: 6px 12px;
  border: none;
  background-color: #486482;
  color: #fff;
  border-radius: 4px;
  cursor: pointer;
}
.approve-btn:hover {
  background-color: #35495e;
}
.reject-btn {
  margin-right: 10px;
  padding: 6px 12px;
  border: none;
  background-color:  #ad7079;
  color: #fff;
  border-radius: 4px;
  cursor: pointer;
}
.reject-btn:hover {
  background: #90555f;
}
.view-btn {
  margin-right: 10px;
  padding: 6px 12px;
  border: none;
  background-color: #486482;
  color: #fff;
  border-radius: 4px;
  cursor: pointer;
}
.view-btn:hover {
  background-color: #35495e;
}
</style>