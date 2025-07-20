<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:获取所有推荐记录</h1>
        <button @click="function1">所有推荐</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2:获取读者推荐的小说</h1>
        <input v-model="readerId2" placeholder="输入读者ID" />
        <button @click="function2">搜索</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <h1>3:获取小说的推荐读者</h1>
        <input v-model="novelId3" placeholder="输入小说ID" />
        <button @click="function3">搜索</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

        <h1>4:添加推荐</h1>
        <input v-model="novelId4" placeholder="输入小说ID" />
        <input v-model="readerId4" placeholder="输入读者ID" />
        <input v-model="reason4" placeholder="输入推荐理由(可选)" />
        <button @click="function4">添加推荐</button>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>

        <h1>5:取消推荐</h1>
        <input v-model="novelId5" placeholder="输入小说ID" />
        <input v-model="readerId5" placeholder="输入读者ID" />
        <button @click="function5">取消推荐</button>
        <div v-if="apiResponse5">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse5 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import { 
    getAllRecommends, 
    getRecommendsByReader, 
    getRecommendsByNovel, 
    addRecommend, 
    deleteRecommend 
} from '@/API/Recommend_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

//1:获取所有推荐记录
const apiResponse1 = ref(null)
async function function1() {
    try {
        const response = await getAllRecommends()
        apiResponse1.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

//2:获取读者推荐的小说
const apiResponse2 = ref(null)
const readerId2 = ref('')
async function function2() {
    try {
        const response = await getRecommendsByReader(readerId2.value)
        apiResponse2.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse2.value = { error: error.message + '，请检查读者ID是否正确' }
    }
}

//3:获取小说的推荐读者
const apiResponse3 = ref(null)
const novelId3 = ref('')
async function function3() {
    try {
        const response = await getRecommendsByNovel(novelId3.value)
        apiResponse3.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse3.value = { error: error.message + '，请检查小说ID是否正确' }
    }
}

//4:添加推荐
const novelId4 = ref('')
const readerId4 = ref('')
const reason4 = ref('')
const apiResponse4 = ref(null)
async function function4() {
    try {
        const response = await addRecommend(novelId4.value, readerId4.value, reason4.value)
        apiResponse4.value = response
        console.log('添加推荐响应:', response)
    } catch (error) {
        console.error('添加推荐请求错误:', error)
        apiResponse4.value = { error: error.message }
    }
}

//5:取消推荐
const novelId5 = ref('')
const readerId5 = ref('')
const apiResponse5 = ref(null)
async function function5() {
    try {
        const response = await deleteRecommend(novelId5.value, readerId5.value)
        apiResponse5.value = response
        console.log('取消推荐响应:', response)
    } catch (error) {
        console.error('取消推荐请求错误:', error)
        apiResponse5.value = { error: error.message }
    }
}
</script>