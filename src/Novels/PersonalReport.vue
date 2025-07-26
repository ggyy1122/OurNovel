<template>
  <div>
    <h2>我的举报</h2>
    <div v-if="loading">加载中...</div>
    <div v-else-if="error">{{ error }}</div>
    <div v-else-if="reports.length === 0">暂无举报记录</div>
    <div v-else>
      <div v-for="report in reports" :key="report.reportId" class="report-item">
        <p>举报ID: {{ report.reportId }}</p>
        <p>原因: {{ report.reason }}</p>
        <p>时间: {{ report.reportTime }}</p>
        <p>进度: {{ report.progress }}</p>
        <p>被举报评论ID: {{ report.commentId }}</p>
        <p>被举报评论内容: {{ report.commentContent }}</p>

        <div v-if="report.managementLogs.length > 0">
          <h4>处理日志:</h4>
          <ul>
            <li v-for="log in report.managementLogs" :key="log.managementId">
              处理员ID: {{ log.managementId }}, 结果: {{ log.result }}, 时间: {{ log.time }}
            </li>
          </ul>
        </div>
        <hr>
      </div>
    </div>
  </div>
</template>

<script setup>
import { getReportsWithLogsByReader } from '@/API/Reader_API'
import { readerState } from '@/stores/index'
import { ref, onMounted } from 'vue'

const store = readerState()
const reports = ref([])
const loading = ref(false)
const error = ref(null)

async function fetchReports() {
    loading.value = true
    error.value = null
    try {
        if (!store.readerId) {
            throw new Error('未检测到登录读者ID')
        }
        const response = await getReportsWithLogsByReader(store.readerId)
        if (response.success && Array.isArray(response.data)) {
            reports.value = response.data
        } else {
            reports.value = []
            error.value = '未获取到举报记录'
        }

        console.log('举报记录加载完成：', reports.value)
    } catch (err) {
        error.value = err.message || '加载举报记录失败'
        reports.value = []
    } finally {
        loading.value = false
    }
}

onMounted(fetchReports)
</script>

<style scoped>
.report-item {
  background-color: #fff;
  border: 1px solid #ddd;
  border-radius: 6px;
  padding: 15px 20px;
  margin-bottom: 12px;
  box-shadow: 0 2px 5px rgb(0 0 0 / 0.1);
  transition: box-shadow 0.3s ease;
}

.report-item:hover {
  box-shadow: 0 4px 10px rgb(0 0 0 / 0.15);
}

.report-item p {
  margin: 6px 0;
  color: #333;
  font-size: 14px;
}

.report-item p:first-child {
  font-weight: 600;
  font-size: 16px;
  color: #222;
}

.report-item p:nth-child(3), /* 时间 */
.report-item p:nth-child(4)  /* 进度 */
{
  color: #666;
  font-size: 13px;
}

.report-item p:nth-child(4) {
  font-weight: 600;
}

.managementLogs {
  margin-top: 12px;
  padding-left: 15px;
  border-left: 3px solid #409eff; /* 蓝色侧边栏，提示处理日志 */
}

.managementLogs h4 {
  margin-bottom: 6px;
  color: #409eff;
  font-size: 14px;
  font-weight: 600;
}

.managementLogs ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.managementLogs li {
  margin-bottom: 6px;
  font-size: 13px;
  color: #555;
}

hr {
  border: none;
  border-bottom: 1px solid #eee;
  margin: 15px 0 0;
}
</style>
