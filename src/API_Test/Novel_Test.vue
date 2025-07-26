<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:获取所有小说</h1>
        <button @click="function1">所有小说</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2:获取1本小说</h1>
        <input v-model="novelId2" placeholder="输入小说ID" type="number" />
        <button @click="function2">搜索</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <h1>3:创建小说</h1>
        <input v-model="novelName3" placeholder="输入小说名称" />
        <input v-model="authorId3" placeholder="输入作者ID" type="number" />
        <input v-model="introduction3" placeholder="输入小说简介" />
        <input v-model="coverUrl3" placeholder="输入封面URL" />
        <input v-model="score3" placeholder="输入评分" type="number" step="0.1" />
        <input v-model="totalWordCount3" placeholder="输入总字数" type="number" />
        <input v-model="recommendCount3" placeholder="输入推荐数" type="number" />
        <input v-model="collectedCount3" placeholder="输入收藏数" type="number" />
        <input v-model="status3" placeholder="输入状态" />
        <input v-model="originalNovelId3" placeholder="输入原始小说ID" type="number" />
        <input v-model="totalPrice3" placeholder="输入总价格" type="number" step="0.01" />
        <button @click="function3">创建小说</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

        <h1>4:更新小说</h1>
        <input v-model="novelId4" placeholder="输入小说ID" type="number" />
        <input v-model="novelName4" placeholder="输入小说名称" />
        <input v-model="authorId4" placeholder="输入作者ID" type="number" />
        <input v-model="introduction4" placeholder="输入小说简介" />
        <input v-model="coverUrl4" placeholder="输入封面URL" />
        <input v-model="score4" placeholder="输入评分" type="number" step="0.1" />
        <input v-model="totalWordCount4" placeholder="输入总字数" type="number" />
        <input v-model="recommendCount4" placeholder="输入推荐数" type="number" />
        <input v-model="collectedCount4" placeholder="输入收藏数" type="number" />
        <input v-model="status4" placeholder="输入状态" />
        <input v-model="originalNovelId4" placeholder="输入原始小说ID" type="number" />
        <input v-model="totalPrice4" placeholder="输入总价格" type="number" step="0.01" />
        <button @click="function4">更新小说</button>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>

        <h1>5:删除小说</h1>
        <input v-model="novelId5" placeholder="输入小说ID" type="number" />
        <button @click="function5">删除小说</button>
        <div v-if="apiResponse5">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse5 }}</pre>
        </div>

        <h1>6:审核小说</h1>
        <input v-model="novelId6" placeholder="输入小说ID" type="number" />
        <input v-model="newStatus6" placeholder="输入新状态（连载/完结）" />
        <input v-model="managerId6" placeholder="输入管理员ID" type="number" />
        <input v-model="result6" placeholder="输入审核结果" />
        <button @click="function6">审核</button>
        <div v-if="apiResponse6">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse6 }}</pre>
        </div>

        <h1>7:获取小说总字数</h1>
        <input v-model="novelId7" placeholder="输入小说ID" type="number" />
        <button @click="function7">获取字数</button>
        <div v-if="apiResponse7">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse7 }}</pre>
        </div>

        <h1>8:获取小说推荐数</h1>
        <input v-model="novelId8" placeholder="输入小说ID" type="number" />
        <button @click="function8">获取推荐数</button>
        <div v-if="apiResponse8">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse8 }}</pre>
        </div>

        <h1>9:获取小说收藏数</h1>
        <input v-model="novelId9" placeholder="输入小说ID" type="number" />
        <button @click="function9">获取收藏数</button>
        <div v-if="apiResponse9">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse9 }}</pre>
        </div>

        <h1>10:上传小说封面</h1>
        <input v-model="novelId10" placeholder="输入小说ID" type="number" />
        <input type="file" @change="file10 = $event.target.files[0]" accept="image/*" />
        <button @click="function10">上传封面</button>
        <div v-if="apiResponse10">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse10 }}</pre>
        </div>

        <h1>11:提交小说修改</h1>
        <input v-model="originalNovelId11" placeholder="输入原始小说ID" type="number" />
        <textarea v-model="editedNovelJson11" placeholder="输入修改后的小说JSON" rows="5" cols="50"></textarea>
        <button @click="function11">提交修改</button>
        <div v-if="apiResponse11">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse11 }}</pre>
        </div>

        <h1>12:获取最新发布章节</h1>
        <input v-model="novelId12" placeholder="输入小说ID" type="number" />
        <button @click="function12">获取最新章节</button>
        <div v-if="apiResponse12">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse12 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import {
    getAllNovels,
    getNovel,
    createNovel,
    updateNovel,
    deleteNovel,
    reviewNovel,
    getNovelWordCount,
    getNovelRecommendCount,
    getNovelCollectCount,
    uploadNovelCover,
    submitNovelEdit,
    getLatestPublishedChapter
} from '@/API/Novel_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

//1:获取所有小说
const apiResponse1 = ref(null)
async function function1() {
    try {
        const response = await getAllNovels()
        apiResponse1.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

//2:获取1本小说
const apiResponse2 = ref(null)
const novelId2 = ref('')
async function function2() {
    if (!novelId2.value) {
        apiResponse2.value = { error: '请输入小说ID' }
        return
    }
    try {
        const response = await getNovel(novelId2.value)
        apiResponse2.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse2.value = { error: error.message + '，请检查小说ID是否正确' }
    }
}

//3:创建小说
const novelName3 = ref('')
const authorId3 = ref('')
const introduction3 = ref('')
const coverUrl3 = ref('')
const score3 = ref('')
const totalWordCount3 = ref('')
const recommendCount3 = ref('')
const collectedCount3 = ref('')
const status3 = ref('')
const originalNovelId3 = ref('')
const totalPrice3 = ref('')
const apiResponse3 = ref(null)
async function function3() {
    try {
        const novelData = {
            novelName: novelName3.value,
            authorId: authorId3.value,
            introduction: introduction3.value,
            coverUrl: coverUrl3.value,
            score: score3.value || 0,
            totalWordCount: totalWordCount3.value || 0,
            recommendCount: recommendCount3.value || 0,
            collectedCount: collectedCount3.value || 0,
            status: status3.value || "待审核",
            originalNovelId: originalNovelId3.value || -1,
            totalPrice: totalPrice3.value || 0
        }
        const response = await createNovel(novelData)
        apiResponse3.value = response
        console.log('创建响应:', response)
    } catch (error) {
        console.error('创建小说请求错误:', error)
        apiResponse3.value = { error: error.message }
    }
}

//4:更新小说
const novelId4 = ref('')
const novelName4 = ref('')
const authorId4 = ref('')
const introduction4 = ref('')
const coverUrl4 = ref('')
const score4 = ref('')
const totalWordCount4 = ref('')
const recommendCount4 = ref('')
const collectedCount4 = ref('')
const status4 = ref('')
const originalNovelId4 = ref('')
const totalPrice4 = ref('')
const apiResponse4 = ref(null)
async function function4() {
    try {
        const updateData = {
            novelName: novelName4.value,
            authorId: authorId4.value,
            introduction: introduction4.value,
            coverUrl: coverUrl4.value,
            score: score4.value,
            totalWordCount: totalWordCount4.value,
            recommendCount: recommendCount4.value,
            collectedCount: collectedCount4.value,
            status: status4.value,
            originalNovelId: originalNovelId4.value,
            totalPrice: totalPrice4.value
        }
        const response = await updateNovel(novelId4.value, updateData)
        apiResponse4.value = response
        console.log('更新响应:', response)
    } catch (error) {
        console.error('更新小说请求错误:', error)
        apiResponse4.value = { error: error.message }
    }
}

//5:删除小说
const novelId5 = ref('')
const apiResponse5 = ref(null)
async function function5() {
    try {
        const response = await deleteNovel(novelId5.value)
        apiResponse5.value = response
        console.log('删除响应:', response)
    } catch (error) {
        console.error('删除小说请求错误:', error)
        apiResponse5.value = { error: error.message }
    }
}

//6:审核小说
const novelId6 = ref('')
const newStatus6 = ref('')
const managerId6 = ref('')
const result6 = ref('')
const apiResponse6 = ref(null)
async function function6() {
    try {
        const response = await reviewNovel(
            novelId6.value,
            newStatus6.value,
            managerId6.value,
            result6.value
        )
        apiResponse6.value = response
        console.log('审核响应:', response)
    } catch (error) {
        console.error('审核小说请求错误:', error)
        apiResponse6.value = { error: error.message }
    }
}

//7:获取小说总字数
const novelId7 = ref('')
const apiResponse7 = ref(null)
async function function7() {
    try {
        const response = await getNovelWordCount(novelId7.value)
        apiResponse7.value = response
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse7.value = { error: error.message }
    }
}

//8:获取小说推荐数
const novelId8 = ref('')
const apiResponse8 = ref(null)
async function function8() {
    try {
        const response = await getNovelRecommendCount(novelId8.value)
        apiResponse8.value = response
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse8.value = { error: error.message }
    }
}

//9:获取小说收藏数
const novelId9 = ref('')
const apiResponse9 = ref(null)
async function function9() {
    try {
        const response = await getNovelCollectCount(novelId9.value)
        apiResponse9.value = response
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse9.value = { error: error.message }
    }
}

//10:上传小说封面
const novelId10 = ref('')
const file10 = ref(null)
const apiResponse10 = ref(null)
async function function10() {
    if (!file10.value) {
        apiResponse10.value = { error: '请选择封面文件' }
        return
    }
    try {
        const response = await uploadNovelCover(novelId10.value, file10.value)
        apiResponse10.value = response
    } catch (error) {
        console.error('上传封面错误:', error)
        apiResponse10.value = { error: error.message }
    }
}

//11:提交小说修改
const originalNovelId11 = ref('')
const editedNovelJson11 = ref('')
const apiResponse11 = ref(null)
async function function11() {
    try {
        const editedNovel = JSON.parse(editedNovelJson11.value || '{}')
        const response = await submitNovelEdit(originalNovelId11.value, editedNovel)
        apiResponse11.value = response
    } catch (error) {
        console.error('提交修改错误:', error)
        apiResponse11.value = { error: error.message }
    }
}

//12:获取最新发布章节
const novelId12 = ref('')
const apiResponse12 = ref(null)
async function function12() {
    try {
        const response = await getLatestPublishedChapter(novelId12.value)
        apiResponse12.value = response
    } catch (error) {
        console.error('获取最新章节错误:', error)
        apiResponse12.value = { error: error.message }
    }
}
</script>