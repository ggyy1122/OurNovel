<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:获取收藏榜单</h1>
        <input v-model="collectTopN" placeholder="输入前几名（默认10）" />
        <button @click="function1">获取收藏榜单</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2:获取推荐榜单</h1>
        <input v-model="recommendTop" placeholder="输入前几名（默认10）" />
        <button @click="function2">获取推荐榜单</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <h1>3:获取评分榜单</h1>
        <input v-model="scoreTop" placeholder="输入前几名（默认10）" />
        <button @click="function3">获取评分榜单</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import { 
    getCollectRanking, 
    getRecommendRanking, 
    getScoreRanking 
} from '@/API/Ranking_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

// 1:获取收藏榜单
const collectTopN = ref('')
const apiResponse1 = ref(null)
async function function1() {
    try {
        const topN = collectTopN.value ? parseInt(collectTopN.value) : 10
        const response = await getCollectRanking(topN)
        apiResponse1.value = response
        console.log('收藏榜单响应:', response)
    } catch (error) {
        console.error('获取收藏榜单请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

// 2:获取推荐榜单
const recommendTop = ref('')
const apiResponse2 = ref(null)
async function function2() {
    try {
        const top = recommendTop.value ? parseInt(recommendTop.value) : 10
        const response = await getRecommendRanking(top)
        apiResponse2.value = response
        console.log('推荐榜单响应:', response)
    } catch (error) {
        console.error('获取推荐榜单请求错误:', error)
        apiResponse2.value = { error: error.message }
    }
}

// 3:获取评分榜单
const scoreTop = ref('')
const apiResponse3 = ref(null)
async function function3() {
    try {
        const top = scoreTop.value ? parseInt(scoreTop.value) : 10
        const response = await getScoreRanking(top)
        apiResponse3.value = response
        console.log('评分榜单响应:', response)
    } catch (error) {
        console.error('获取评分榜单请求错误:', error)
        apiResponse3.value = { error: error.message }
    }
}
</script>