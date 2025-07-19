<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:获取所有章节</h1>
        <button @click="function1">所有章节</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2:获取小说所有章节</h1>
        <input v-model="novelId2" placeholder="输入小说ID" />
        <button @click="function2">搜索</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <h1>3:获取指定章节</h1>
        <input v-model="novelId3" placeholder="输入小说ID" />
        <input v-model="chapterId3" placeholder="输入章节ID" />
        <button @click="function3">搜索</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

        <h1>4:创建章节</h1>
        <input v-model="novelId4" placeholder="输入小说ID" />
        <input v-model="chapterId4" placeholder="输入章节ID" />
        <input v-model="title4" placeholder="输入章节标题" />
        <textarea v-model="content4" placeholder="输入章节内容" style="width:300px;height:100px;"></textarea>
        <input v-model="wordCount4" placeholder="输入字数" />
        <input v-model="pricePerKilo4" placeholder="输入千字单价" />
        <select v-model="isCharged4">
            <option value="">是否收费</option>
            <option value="是">是</option>
            <option value="否">否</option>
        </select>
        <input v-model="publishTime4" placeholder="输入发布时间" type="datetime-local" />
        <select v-model="status4">
            <option value="通过">通过</option>
            <option value="封禁">封禁</option>
        </select>
        <button @click="function4">创建章节</button>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>

        <h1>5:更新章节</h1>
        <input v-model="novelId5" placeholder="输入小说ID" />
        <input v-model="chapterId5" placeholder="输入章节ID" />
        <input v-model="title5" placeholder="输入章节标题" />
        <textarea v-model="content5" placeholder="输入章节内容" style="width:300px;height:100px;"></textarea>
        <input v-model="wordCount5" placeholder="输入字数" />
        <input v-model="pricePerKilo5" placeholder="输入千字单价" />
        <select v-model="isCharged5">
            <option value="">是否收费</option>
            <option value="是">是</option>
            <option value="否">否</option>
        </select>
        <input v-model="publishTime5" placeholder="输入发布时间" type="datetime-local" />
        <select v-model="status5">
            <option value="通过">通过</option>
            <option value="封禁">封禁</option>
        </select>
        <button @click="function5">更新章节</button>
        <div v-if="apiResponse5">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse5 }}</pre>
        </div>

        <h1>6:删除章节</h1>
        <input v-model="novelId6" placeholder="输入小说ID" />
        <input v-model="chapterId6" placeholder="输入章节ID" />
        <button @click="function6">删除章节</button>
        <div v-if="apiResponse6">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse6 }}</pre>
        </div>

        <h1>7:审核章节</h1>
        <input v-model="novelId7" placeholder="输入小说ID" />
        <input v-model="chapterId7" placeholder="输入章节ID" />
        <select v-model="newStatus7">
            <option value="通过">通过</option>
            <option value="封禁">封禁</option>
        </select>
        <button @click="function7">提交审核</button>
        <div v-if="apiResponse7">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse7 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import { 
    getAllChapters, 
    getChaptersByNovel, 
    getChapter, 
    createChapter, 
    updateChapter, 
    deleteChapter,
    reviewChapter 
} from '@/API/Chapter_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

// 1: 获取所有章节
const apiResponse1 = ref(null)
async function function1() {
    try {
        const response = await getAllChapters()
        apiResponse1.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

// 2: 获取小说所有章节
const apiResponse2 = ref(null)
const novelId2 = ref('')
async function function2() {
    try {
        const response = await getChaptersByNovel(novelId2.value)
        apiResponse2.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse2.value = { error: error.message + '，请检查小说ID是否正确' }
    }
}

// 3: 获取指定章节
const apiResponse3 = ref(null)
const novelId3 = ref('')
const chapterId3 = ref('')
async function function3() {
    try {
        const response = await getChapter(novelId3.value, chapterId3.value)
        apiResponse3.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse3.value = { error: error.message + '，请检查小说ID和章节ID是否正确' }
    }
}

// 4: 创建章节
const novelId4 = ref('')
const chapterId4 = ref('')
const title4 = ref('')
const content4 = ref('')
const wordCount4 = ref('')
const pricePerKilo4 = ref('0.50')
const isCharged4 = ref('')
const publishTime4 = ref('')
const status4 = ref('通过')
const apiResponse4 = ref(null)
async function function4() {
    try {
        const chapterData = {
            novelId: novelId4.value,
            chapterId: chapterId4.value,
            title: title4.value,
            content: content4.value,
            wordCount: wordCount4.value ? parseInt(wordCount4.value) : 0,
            pricePerKilo: pricePerKilo4.value ? parseFloat(pricePerKilo4.value) : 0.50,
            isCharged: isCharged4.value,
            publishTime: publishTime4.value,
            status: status4.value
        }
        const response = await createChapter(chapterData)
        apiResponse4.value = response
        console.log('创建响应:', response)
    } catch (error) {
        console.error('创建章节请求错误:', error)
        apiResponse4.value = { error: error.message }
    }
}

// 5: 更新章节
const novelId5 = ref('')
const chapterId5 = ref('')
const title5 = ref('')
const content5 = ref('')
const wordCount5 = ref('')
const pricePerKilo5 = ref('')
const isCharged5 = ref('')
const publishTime5 = ref('')
const status5 = ref('通过')
const apiResponse5 = ref(null)
async function function5() {
    try {
        const chapterData = {
            novelId: novelId5.value,
            chapterId: chapterId5.value,
            title: title5.value,
            content: content5.value,
            wordCount: wordCount5.value ? parseInt(wordCount5.value) : 0,
            pricePerKilo: pricePerKilo5.value ? parseFloat(pricePerKilo5.value) : 0.50,
            isCharged: isCharged5.value,
            publishTime: publishTime5.value,
            status: status5.value
        }
        const response = await updateChapter(novelId5.value, chapterId5.value, chapterData)
        apiResponse5.value = response
        console.log('更新响应:', response)
    } catch (error) {
        console.error('更新章节请求错误:', error)
        apiResponse5.value = { error: error.message }
    }
}

// 6: 删除章节
const novelId6 = ref('')
const chapterId6 = ref('')
const apiResponse6 = ref(null)
async function function6() {
    try {
        const response = await deleteChapter(novelId6.value, chapterId6.value)
        apiResponse6.value = response
        console.log('删除响应:', response)
    } catch (error) {
        console.error('删除章节请求错误:', error)
        apiResponse6.value = { error: error.message }
    }
}

// 7: 审核章节
const novelId7 = ref('')
const chapterId7 = ref('')
const newStatus7 = ref('通过')
const apiResponse7 = ref(null)
async function function7() {
    try {
        const response = await reviewChapter(novelId7.value, chapterId7.value, newStatus7.value)
        apiResponse7.value = response
        console.log('审核响应:', response)
    } catch (error) {
        console.error('审核章节请求错误:', error)
        apiResponse7.value = { error: error.message }
    }
}
</script>