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
          <th><input type="checkbox" v-model="selectAll" @change="toggleSelectAll" /></th>
          <th>评论者</th>
          <th>举报者</th>
          <th>举报原因</th>
          <th>举报时间</th>
          <th>操作</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in reports" :key="item.reportId">
          <td><input type="checkbox" v-model="selectedIds" :value="item.reportId" /></td>
          <td>{{ item.commentId }}</td>
          <td>{{ item.readerId }}</td>
          <td>{{ item.reason }}</td>
          <td>{{ item.reportTime }}</td>
          <td style="position:relative;">
            <router-link :to="{ path: '/Admin/Admin_Layout/comment_detail', query: { id: item.reportId } }" class="view-link">查看</router-link>
            <span class="action-dots" @click="openMenu(item.reportId)">⋯</span>
            <div v-if="activeMenu === item.reportId" class="action-menu">
              <div class="action-item" @click="approve(item.reportId)">审核通过</div>
              <div class="action-item" @click="reject(item.reportId)">审核不通过</div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
    <div v-if="selectedIds.length" class="batch-actions">
      <button @click="approveSelected">批量审核通过</button>
      <button @click="rejectSelected">批量审核不通过</button>
    </div>
   
  </div>
  </div>
  
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'
import {getAllReports} from '@/API/Report_API'
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
      readerId: report.readerId
    }))
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
const selectedIds = ref([])//被选中的
const selectAll = ref(false)//是否全选
const activeMenu = ref(null)

function toggleSelectAll() {
  if (selectAll.value) {
    selectedIds.value = reports.value.map(n => n.reportId)
  } else {
    selectedIds.value = []
  }
}
function openMenu(id) {
  activeMenu.value = activeMenu.value === id ? null : id
}



function approveSelected() {
  selectedIds.value = []
}
function rejectSelected() {
  selectedIds.value = []
}



function handleClickOutside(e) {
  if (activeMenu.value !== null) {
    const menus = document.querySelectorAll('.action-menu, .action-dots')
    let isMenu = false
    menus.forEach(menu => {
      if (menu.contains(e.target)) isMenu = true
    })
    if (!isMenu) activeMenu.value = null
  }
}
onMounted(() => {
  document.addEventListener('click', handleClickOutside)
  getReportsData() // 页面加载时调用
})
onBeforeUnmount(() => {
  document.removeEventListener('click', handleClickOutside)
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
  text-align: left;
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
</style>