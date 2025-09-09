<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:获取所有点赞记录</h1>
        <button @click="function1">获取所有点赞</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2:给评论点赞</h1>
        <input v-model="commentId2" placeholder="输入评论ID" />
        <input v-model="readerId2" placeholder="输入读者ID" />
        <button @click="function2">点赞</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <h1>3:取消点赞</h1>
        <input v-model="commentId3" placeholder="输入评论ID" />
        <input v-model="readerId3" placeholder="输入读者ID" />
        <button @click="function3">取消点赞</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

        <h1>4:检查是否已点赞</h1>
        <input v-model="commentId4" placeholder="输入评论ID" />
        <input v-model="readerId4" placeholder="输入读者ID" />
        <button @click="function4">检查</button>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>

        <h1>5:获取评论点赞数</h1>
        <input v-model="commentId5" placeholder="输入评论ID" />
        <button @click="function5">获取点赞数</button>
        <div v-if="apiResponse5">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse5 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import { 
    getAllLikes, 
    likeComment, 
    unlikeComment, 
    isLiked, 
    getLikeCount 
} from '@/API/Likes_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

// 1:获取所有点赞记录
const apiResponse1 = ref(null)
async function function1() {
    try {
        const response = await getAllLikes()
        apiResponse1.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

// 2:给评论点赞
const commentId2 = ref('')
const readerId2 = ref('')
const apiResponse2 = ref(null)
async function function2() {
    try {
        const response = await likeComment(commentId2.value, readerId2.value)
        apiResponse2.value = response
        console.log('点赞响应:', response)
    } catch (error) {
        console.error('点赞请求错误:', error)
        apiResponse2.value = { error: error.message }
    }
}

// 3:取消点赞
const commentId3 = ref('')
const readerId3 = ref('')
const apiResponse3 = ref(null)
async function function3() {
    try {
        const response = await unlikeComment(commentId3.value, readerId3.value)
        apiResponse3.value = response
        console.log('取消点赞响应:', response)
    } catch (error) {
        console.error('取消点赞请求错误:', error)
        apiResponse3.value = { error: error.message }
    }
}

// 4:检查是否已点赞
const commentId4 = ref('')
const readerId4 = ref('')
const apiResponse4 = ref(null)
async function function4() {
    try {
        const response = await isLiked(commentId4.value, readerId4.value)
        apiResponse4.value = response
        console.log('检查点赞响应:', response)
    } catch (error) {
        console.error('检查点赞请求错误:', error)
        apiResponse4.value = { error: error.message }
    }
}

// 5:获取评论点赞数
const commentId5 = ref('')
const apiResponse5 = ref(null)
async function function5() {
    try {
        const response = await getLikeCount(commentId5.value)
        apiResponse5.value = response
        console.log('点赞数响应:', response)
    } catch (error) {
        console.error('获取点赞数请求错误:', error)
        apiResponse5.value = { error: error.message }
    }
}
</script>