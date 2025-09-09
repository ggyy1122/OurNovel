<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:获取所有举报记录</h1>
        <button @click="function1">所有举报</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2:获取单个举报详情</h1>
        <input v-model="reportId2" placeholder="输入举报ID" />
        <button @click="function2">搜索</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <h1>3:举报评论</h1>
        <input v-model="commentId3" placeholder="输入评论ID" />
        <input v-model="readerId3" placeholder="输入读者ID" />
        <input v-model="reason3" placeholder="输入举报原因" />
        <button @click="function3">提交举报</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

        <h1>4:处理举报</h1>
        <input v-model="reportId4" placeholder="输入举报ID" />
        <input v-model="progress4" placeholder="输入处理进度" />
        <input v-model="managerId4" placeholder="输入管理员ID" />
        <button @click="function4">处理举报</button>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import { 
    getAllReports, 
    getReport, 
    reportComment, 
    processReport 
} from '@/API/Report_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

// 1:获取所有举报记录
const apiResponse1 = ref(null)
async function function1() {
    try {
        const response = await getAllReports()
        apiResponse1.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

// 2:获取单个举报详情
const reportId2 = ref('')
const apiResponse2 = ref(null)
async function function2() {
    try {
        const response = await getReport(reportId2.value)
        apiResponse2.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse2.value = { error: error.message + '，请检查举报ID是否正确' }
    }
}

// 3:举报评论
const commentId3 = ref('')
const readerId3 = ref('')
const reason3 = ref('')
const apiResponse3 = ref(null)
async function function3() {
    try {
        const response = await reportComment(commentId3.value, readerId3.value, reason3.value)
        apiResponse3.value = response
        console.log('举报响应:', response)
    } catch (error) {
        console.error('举报请求错误:', error)
        apiResponse3.value = { error: error.message }
    }
}

// 4:处理举报
const reportId4 = ref('')
const progress4 = ref('')
const managerId4 = ref('')
const apiResponse4 = ref(null)
async function function4() {
    try {
        const response = await processReport(reportId4.value, progress4.value, managerId4.value)
        apiResponse4.value = response
        console.log('处理举报响应:', response)
    } catch (error) {
        console.error('处理举报请求错误:', error)
        apiResponse4.value = { error: error.message }
    }
}
</script>