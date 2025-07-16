<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <!-- 1: 获取所有评论 -->
        <h1>1: 获取所有评论</h1>
        <button @click="function1">所有评论</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <!-- 2: 获取单个评论 -->
        <h1>2: 获取单个评论</h1>
        <input v-model="commentId2" placeholder="输入评论ID" />
        <button @click="function2">搜索</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <!-- 3: 创建评论 -->
        <h1>3: 创建评论</h1>
        <input v-model="createData.readerId" placeholder="读者ID" type="number" />
        <input v-model="createData.novelId" placeholder="小说ID" type="number" />
        <input v-model="createData.chapterId" placeholder="章节ID" type="number" />
        <input v-model="createData.title" placeholder="评论标题" />
        <input v-model="createData.content" placeholder="评论内容" />
        <input v-model="createData.likes" placeholder="点赞数" type="number" />
        <input v-model="createData.status" placeholder="状态(如:通过)" />
        <button @click="function3">创建评论</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

        <!-- 4: 更新评论 -->
        <h1>4: 更新评论</h1>
        <input v-model="commentId4" placeholder="输入评论ID" type="number" />
        <button @click="getCommentForUpdate">获取评论</button>
        <div v-if="originalComment4">
            <input v-model="updateData4.title" placeholder="新标题" />
            <input v-model="updateData4.content" placeholder="新内容" />
            <input v-model.number="updateData4.likes" placeholder="新点赞数" type="number" />
            <input v-model="updateData4.status" placeholder="新状态" />
            <button @click="function4">更新评论</button>
        </div>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>

        <!-- 5: 删除评论 -->
        <h1>5: 删除评论</h1>
        <input v-model="commentId5" placeholder="输入评论ID" type="number" />
        <button @click="function5">删除评论</button>
        <div v-if="apiResponse5">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse5 }}</pre>
        </div>

        <!-- 6: 点赞评论 -->
        <h1>6: 点赞评论</h1>
        <input v-model="commentId6" placeholder="输入评论ID" type="number" />
        <button @click="function6">点赞</button>
        <div v-if="apiResponse6">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse6 }}</pre>
        </div>

        <!-- 7: 设置评论状态 -->
        <h1>7: 设置评论状态</h1>
        <input v-model="statusData.commentId" placeholder="评论ID" type="number" />
        <input v-model="statusData.status" placeholder="新状态(如:封禁)" />
        <button @click="function7">设置状态</button>
        <div v-if="apiResponse7">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse7 }}</pre>
        </div>

        <!-- 8: 获取章节评论 -->
        <h1>8: 获取章节评论</h1>
        <input v-model="novelId8" placeholder="小说ID" type="number" />
        <input v-model="chapterId8" placeholder="章节ID" type="number" />
        <button @click="function8">获取评论</button>
        <div v-if="apiResponse8">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse8 }}</pre>
        </div>

        <!-- 9: 获取小说评论 -->
        <h1>9: 获取小说评论</h1>
        <input v-model="novelId9" placeholder="小说ID" type="number" />
        <button @click="function9">获取评论</button>
        <div v-if="apiResponse9">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse9 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import {
    getAllComments,
    getComment,
    createComment,
    updateComment,
    deleteComment,
    likeComment,
    setCommentStatus,
    getCommentsByChapter,
    getCommentsByNovel
} from '@/API/Comment_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

// 1: 获取所有评论
const apiResponse1 = ref(null)
async function function1() {
    try {
        const response = await getAllComments()
        apiResponse1.value = response
    } catch (error) {
        apiResponse1.value = { error: error.message }
    }
}

// 2: 获取单个评论
const apiResponse2 = ref(null)
const commentId2 = ref('')
async function function2() {
    if (!commentId2.value) return
    try {
        const response = await getComment(commentId2.value)
        apiResponse2.value = response
    } catch (error) {
        apiResponse2.value = { error: error.message + '，请检查评论ID是否正确' }
    }
}

// 3: 创建评论
const apiResponse3 = ref(null)
const createData = ref({
    readerId: '',
    novelId: '',
    chapterId: '',
    title: '',
    content: '',
    likes: 0,
    status: '通过',
    createTime: ''
})
async function function3() {
    try {
        const now = new Date().toISOString()
        createData.value.createTime = now     // 设置创建时间为当前时间
        const response = await createComment(createData.value)
        apiResponse3.value = response
        console.log('创建响应:', response)
    } catch (error) {
        apiResponse3.value = { error: error.message }
    }
}

// 4: 更新评论
const commentId4 = ref('')
const originalComment4 = ref(null) // 存储从API获取的原始评论数据
const updateData4 = ref({
    title: '',
    content: '',
    likes: 0,
    status: ''
})
const apiResponse4 = ref(null)
// 获取评论以填充表单
async function getCommentForUpdate() {
    if (!commentId4.value) return
    try {
        const response = await getComment(commentId4.value)
        originalComment4.value = response
        updateData4.value.title = response.title
        updateData4.value.content = response.content || ''
        updateData4.value.likes = response.likes
        updateData4.value.status = response.status
    } catch (error) {
        apiResponse4.value = { error: '获取评论失败: ' + error.message }
    }
}

// 更新评论
async function function4() {
    if (!commentId4.value || !originalComment4.value) {
        apiResponse4.value = { error: '请先获取评论' }
        return
    }
    try {
        const now = new Date().toISOString()
        // 构建完整的评论对象
        const commentData = {
            commentId: originalComment4.value.commentId,
            readerId: originalComment4.value.readerId,
            novelId: originalComment4.value.novelId,
            chapterId: originalComment4.value.chapterId,
            title: updateData4.value.title,
            content: updateData4.value.content,
            likes: updateData4.value.likes,
            status: updateData4.value.status,
            createTime: now // 使用当前时间
        }
        const response = await updateComment(
            commentId4.value,
            commentData
        )
        apiResponse4.value = response
        console.log('更新响应:', response)
    } catch (error) {
        apiResponse4.value = { error: error.message }
    }
}

// 5: 删除评论
const apiResponse5 = ref(null)
const commentId5 = ref('')
async function function5() {
    if (!commentId5.value) return
    try {
        const response = await deleteComment(commentId5.value)
        apiResponse5.value = response
    } catch (error) {
        apiResponse5.value = { error: error.message }
    }
}

// 6: 点赞评论
const apiResponse6 = ref(null)
const commentId6 = ref('')
async function function6() {
    if (!commentId6.value) return
    try {
        const response = await likeComment(commentId6.value)
        apiResponse6.value = response
    } catch (error) {
        apiResponse6.value = { error: error.message }
    }
}

// 7: 设置评论状态
const apiResponse7 = ref(null)
const statusData = ref({
    commentId: '',
    status: ''
})
async function function7() {
    if (!statusData.value.commentId || !statusData.value.status) return
    try {
        const response = await setCommentStatus({
            commentId: statusData.value.commentId,
            status: statusData.value.status
        })
        apiResponse7.value = response
    } catch (error) {
        apiResponse7.value = { error: error.message }
    }
}

// 8: 获取章节评论
const apiResponse8 = ref(null)
const novelId8 = ref('')
const chapterId8 = ref('')
async function function8() {
    if (!novelId8.value || !chapterId8.value) return
    try {
        const response = await getCommentsByChapter(novelId8.value, chapterId8.value)
        apiResponse8.value = response
    } catch (error) {
        apiResponse8.value = { error: error.message }
    }
}

// 9: 获取小说评论
const apiResponse9 = ref(null)
const novelId9 = ref('')
async function function9() {
    if (!novelId9.value) return
    try {
        const response = await getCommentsByNovel(novelId9.value)
        apiResponse9.value = response
    } catch (error) {
        apiResponse9.value = { error: error.message }
    }
}
</script>