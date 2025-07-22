<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:获取所有评论回复关系</h1>
        <button @click="function1">获取所有</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2:根据评论ID获取回复关系</h1>
        <input v-model="commentId2" placeholder="输入评论ID" />
        <button @click="function2">搜索</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <h1>3:获取某评论下的直接回复</h1>
        <input v-model="parentId3" placeholder="输入父评论ID" />
        <button @click="function3">获取回复</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

        <h1>4:添加评论回复</h1>
        <input v-model="commentId4" placeholder="输入评论ID" />
        <input v-model="preComId4" placeholder="输入父评论ID（可选）" />
        <input v-model="commentLevel4" placeholder="输入评论层级(1-3)" />
        <button @click="function4">添加回复</button>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import { 
    getAllCommentReplies, 
    getReplyByCommentId, 
    getRepliesByParentId, 
    addCommentReply 
} from '@/API/CommentReply_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

// 1:获取所有评论回复关系
const apiResponse1 = ref(null)
async function function1() {
    try {
        const response = await getAllCommentReplies()
        apiResponse1.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

// 2:根据评论ID获取回复关系
const commentId2 = ref('')
const apiResponse2 = ref(null)
async function function2() {
    try {
        const response = await getReplyByCommentId(commentId2.value)
        apiResponse2.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse2.value = { error: error.message + '，请检查评论ID是否正确' }
    }
}

// 3:获取某评论下的直接回复
const parentId3 = ref('')
const apiResponse3 = ref(null)
async function function3() {
    try {
        const response = await getRepliesByParentId(parentId3.value)
        apiResponse3.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse3.value = { error: error.message }
    }
}

// 4:添加评论回复
const commentId4 = ref('')
const preComId4 = ref('')
const commentLevel4 = ref('')
const apiResponse4 = ref(null)
async function function4() {
    try {
        const replyData = {
            commentId: commentId4.value,
            preComId: preComId4.value || null,
            commentLevel: commentLevel4.value
        }
        const response = await addCommentReply(replyData)
        apiResponse4.value = response
        console.log('添加回复响应:', response)
    } catch (error) {
        console.error('添加回复请求错误:', error)
        apiResponse4.value = { error: error.message }
    }
}
</script>