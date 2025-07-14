<template>
    <div style="text-align:center; margin-top:100px;">
        <button @click="goToLogin">登录</button>
    </div>
    <div id="app" style="text-align:center; margin-top:100px;">
        <h1>Oracle API 测试</h1>
        <button @click="testOracleApi">测试 API 连接</button>
        <div v-if="apiResponse">
            <h3>API 响应数据：</h3>
            <pre>{{ apiResponse }}</pre>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { fetchOracleData } from '@/API/oracleAPI'

const router = useRouter()
const apiResponse = ref(null)

function goToLogin() {
    router.push('/L_R/login')
}

async function testOracleApi() {
    try {
        const response = await fetchOracleData({
            testParam: 'test'
        })
        apiResponse.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse.value = { error: error.message }
    }
}
</script>