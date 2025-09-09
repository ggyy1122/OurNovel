<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:获取所有评论</h1>
        <button @click="function1">所有评论</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2:获取1个评论</h1>
        <input v-model="commentId2" placeholder="输入评论ID" />
        <button @click="fuction2">搜索</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <h1>3:创建评论</h1>
        <input v-model="readerId3" placeholder="输入读者ID" />
        <input v-model="novelId3" placeholder="输入小说ID" />
        <input v-model="chapterId3" placeholder="输入章节ID" />
        <input v-model="title3" placeholder="输入评论标题" />
        <input v-model="content3" placeholder="输入评论内容" />
        <input v-model="likes3" placeholder="输入点赞数" />
        <input v-model="status3" placeholder="输入状态" />
        <button @click="fuction3">创建评论</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

        <h1>4:更新评论</h1>
        <input v-model="commentId4" placeholder="输入评论ID" />
        <input v-model="readerId4" placeholder="输入读者ID" />
        <input v-model="novelId4" placeholder="输入小说ID" />
        <input v-model="chapterId4" placeholder="输入章节ID" />
        <input v-model="title4" placeholder="输入评论标题" />
        <input v-model="content4" placeholder="输入评论内容" />
        <input v-model="likes4" placeholder="输入点赞数" />
        <input v-model="status4" placeholder="输入状态" />
        <button @click="fuction4">更新评论</button>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>

        <h1>5:删除评论</h1>
        <input v-model="commentId5" placeholder="输入评论ID" />
        <button @click="fuction5">删除评论</button>
        <div v-if="apiResponse5">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse5 }}</pre>
        </div>

        <h1>6:设置评论状态</h1>
        <input v-model="commentId6" placeholder="输入评论ID" />
        <input v-model="status6" placeholder="输入状态" />
        <input v-model="managerId6" placeholder="输入管理员ID" />
        <button @click="fuction6">设置状态</button>
        <div v-if="apiResponse6">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse6 }}</pre>
        </div>

        <h1>7:获取章节评论</h1>
        <input v-model="novelId7" placeholder="输入小说ID" />
        <input v-model="chapterId7" placeholder="输入章节ID" />
        <button @click="fuction7">获取评论</button>
        <div v-if="apiResponse7">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse7 }}</pre>
        </div>

        <h1>8:获取小说评论</h1>
        <input v-model="novelId8" placeholder="输入小说ID" />
        <button @click="fuction8">获取评论</button>
        <div v-if="apiResponse8">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse8 }}</pre>
        </div>

        <h1>9:递归删除评论</h1>
        <input v-model="commentId9" placeholder="输入评论ID" />
        <button @click="fuction9">递归删除</button>
        <div v-if="apiResponse9">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse9 }}</pre>
        </div>

        <h1>10:审核评论</h1>
        <input v-model="commentId10" placeholder="输入评论ID" />
        <input v-model="status10" placeholder="输入新状态" />
        <button @click="fuction10">审核评论</button>
        <div v-if="apiResponse10">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse10 }}</pre>
        </div>

        <h1>11:获取小说顶级评论</h1>
        <input v-model="novelId11" placeholder="输入小说ID" />
        <button @click="function11">获取评论</button>
        <div v-if="apiResponse11">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse11 }}</pre>
        </div>

        <h1>12:获取点赞最多的评论</h1>
        <input v-model="novelId12" placeholder="输入小说ID" />
        <input v-model="topN12" placeholder="输入前N名" />
        <button @click="function12">获取评论</button>
        <div v-if="apiResponse12">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse12 }}</pre>
        </div>

        <h1>13:获取章节顶级评论</h1>
        <input v-model="novelId13" placeholder="输入小说ID" />
        <input v-model="chapterId13" placeholder="输入章节ID" />
        <button @click="function13">获取评论</button>
        <div v-if="apiResponse13">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse13 }}</pre>
        </div>

        <h1>14:获取章节点赞最多的评论</h1>
        <input v-model="novelId14" placeholder="输入小说ID" />
        <input v-model="chapterId14" placeholder="输入章节ID" />
        <input v-model="topN14" placeholder="输入前N名" />
        <button @click="function14">获取评论</button>
        <div v-if="apiResponse14">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse14 }}</pre>
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
    setCommentStatus,
    getCommentsByChapter,
    getCommentsByNovel,
    deleteCommentRecursive,
    reviewComment,
    getTopLevelComments,
    getTopLikedComments,
    getTopLevelCommentsByChapter,
    getTopLikedCommentsByChapter
} from '@/API/Comment_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

// 1:获取所有评论
const apiResponse1 = ref(null)
async function function1() {
    try {
        const response = await getAllComments()
        apiResponse1.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

// 2:获取1个评论
const apiResponse2 = ref(null)
const commentId2 = ref('')
async function fuction2() {
    try {
        const response = await getComment(commentId2.value)
        apiResponse2.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse2.value = { error: error.message + '，请检查评论ID是否正确' }
    }
}

// 3:创建评论
const readerId3 = ref('')
const novelId3 = ref('')
const chapterId3 = ref('')
const title3 = ref('')
const content3 = ref('')
const likes3 = ref('')
const status3 = ref('')
const apiResponse3 = ref(null)
async function fuction3() {
    try {
        const commentData = {
            readerId: readerId3.value,
            novelId: novelId3.value,
            chapterId: chapterId3.value,
            title: title3.value,
            content: content3.value,
            likes: likes3.value || 0,
            status: status3.value || "通过"
        }
        const response = await createComment(commentData)
        apiResponse3.value = response
        console.log('创建响应:', response)
    } catch (error) {
        console.error('创建评论请求错误:', error)
        apiResponse3.value = { error: error.message }
    }
}

// 4:更新评论
const commentId4 = ref('')
const readerId4 = ref('')
const novelId4 = ref('')
const chapterId4 = ref('')
const title4 = ref('')
const content4 = ref('')
const likes4 = ref('')
const status4 = ref('')
const apiResponse4 = ref(null)
async function fuction4() {
    try {
        const updateData = {
            commentId: commentId4.value,
            readerId: readerId4.value,
            novelId: novelId4.value,
            chapterId: chapterId4.value,
            title: title4.value,
            content: content4.value,
            likes: likes4.value,
            status: status4.value
        }
        const response = await updateComment(commentId4.value, updateData)
        apiResponse4.value = response
        console.log('更新响应:', response)
    } catch (error) {
        console.error('更新评论请求错误:', error)
        apiResponse4.value = { error: error.message }
    }
}

// 5:删除评论
const commentId5 = ref('')
const apiResponse5 = ref(null)
async function fuction5() {
    try {
        const response = await deleteComment(commentId5.value)
        apiResponse5.value = response
        console.log('删除响应:', response)
    } catch (error) {
        console.error('删除评论请求错误:', error)
        apiResponse5.value = { error: error.message }
    }
}

// 6:设置评论状态
const commentId6 = ref('')
const status6 = ref('')
const managerId6 = ref('')
const apiResponse6 = ref(null)
async function fuction6() {
    try {
        const response = await setCommentStatus(commentId6.value, status6.value, managerId6.value)
        apiResponse6.value = response
        console.log('设置状态响应:', response)
    } catch (error) {
        console.error('设置状态请求错误:', error)
        apiResponse6.value = { error: error.message }
    }
}

// 7:获取章节评论
const novelId7 = ref('')
const chapterId7 = ref('')
const apiResponse7 = ref(null)
async function fuction7() {
    try {
        const response = await getCommentsByChapter(novelId7.value, chapterId7.value)
        apiResponse7.value = response
        console.log('章节评论响应:', response)
    } catch (error) {
        console.error('获取章节评论请求错误:', error)
        apiResponse7.value = { error: error.message }
    }
}

// 8:获取小说评论
const novelId8 = ref('')
const apiResponse8 = ref(null)
async function fuction8() {
    try {
        const response = await getCommentsByNovel(novelId8.value)
        apiResponse8.value = response
        console.log('小说评论响应:', response)
    } catch (error) {
        console.error('获取小说评论请求错误:', error)
        apiResponse8.value = { error: error.message }
    }
}

// 9:递归删除评论
const commentId9 = ref('')
const apiResponse9 = ref(null)
async function fuction9() {
    try {
        const response = await deleteCommentRecursive(commentId9.value)
        apiResponse9.value = response
        console.log('递归删除响应:', response)
    } catch (error) {
        console.error('递归删除请求错误:', error)
        apiResponse9.value = { error: error.message }
    }
}

// 10:审核评论
const commentId10 = ref('')
const status10 = ref('')
const apiResponse10 = ref(null)
async function fuction10() {
    try {
        const response = await reviewComment(commentId10.value, status10.value)
        apiResponse10.value = response
        console.log('审核评论响应:', response)
    } catch (error) {
        console.error('审核评论请求错误:', error)
        apiResponse10.value = { error: error.message }
    }
}

// 11:获取小说顶级评论
const novelId11 = ref('')
const apiResponse11 = ref(null)
async function function11() {
    try {
        const response = await getTopLevelComments(novelId11.value)
        apiResponse11.value = response
        console.log('顶级评论响应:', response)
    } catch (error) {
        console.error('获取顶级评论请求错误:', error)
        apiResponse11.value = { error: error.message }
    }
}

// 12:获取点赞最多的评论
const novelId12 = ref('')
const topN12 = ref('')
const apiResponse12 = ref(null)
async function function12() {
    try {
        const response = await getTopLikedComments(novelId12.value, topN12.value)
        apiResponse12.value = response
        console.log('点赞最多评论响应:', response)
    } catch (error) {
        console.error('获取点赞最多评论请求错误:', error)
        apiResponse12.value = { error: error.message }
    }
}

// 13:获取章节顶级评论
const novelId13 = ref('')
const chapterId13 = ref('')
const apiResponse13 = ref(null)
async function function13() {
    try {
        const response = await getTopLevelCommentsByChapter(novelId13.value, chapterId13.value)
        apiResponse13.value = response
        console.log('章节顶级评论响应:', response)
    } catch (error) {
        console.error('获取章节顶级评论请求错误:', error)
        apiResponse13.value = { error: error.message }
    }
}

// 14:获取章节点赞最多的评论
const novelId14 = ref('')
const chapterId14 = ref('')
const topN14 = ref('')
const apiResponse14 = ref(null)
async function function14() {
    try {
        const response = await getTopLikedCommentsByChapter(novelId14.value, chapterId14.value, topN14.value)
        apiResponse14.value = response
        console.log('章节点赞最多评论响应:', response)
    } catch (error) {
        console.error('获取章节点赞最多评论请求错误:', error)
        apiResponse14.value = { error: error.message }
    }
}
</script>