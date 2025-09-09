<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:获取作者收入记录列表</h1>
        <input v-model="authorId1" placeholder="输入作者ID" />
        <button @click="function1">获取收入记录</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2:获取作者总收入</h1>
        <input v-model="authorId2" placeholder="输入作者ID" />
        <button @click="function2">获取总收入</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue'
import { 
    getAuthorIncomes, 
    getAuthorTotalIncome
} from '@/API/AuthorIncome_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

// 1: 获取作者收入记录列表
const apiResponse1 = ref(null)
const authorId1 = ref('')
async function function1() {
    try {
        const response = await getAuthorIncomes(authorId1.value)
        apiResponse1.value = response
        console.log('收入记录列表:', response)
    } catch (error) {
        console.error('获取收入记录错误:', error)
        apiResponse1.value = { error: error.message + '，请检查作者ID是否正确' }
    }
}

// 2: 获取作者总收入
const apiResponse2 = ref(null)
const authorId2 = ref('')
async function function2() {
    try {
        const response = await getAuthorTotalIncome(authorId2.value)
        apiResponse2.value = response
        console.log('作者总收入:', response)
    } catch (error) {
        console.error('获取总收入错误:', error)
        apiResponse2.value = { error: error.message + '，请检查作者ID是否正确' }
    }
}
</script>