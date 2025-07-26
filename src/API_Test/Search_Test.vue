<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1: 搜索小说</h1>
        <input v-model="novelKeyword" placeholder="输入小说关键词" />
        <button @click="searchbyNovels">搜索小说</button>
        <div v-if="novelResults">
            <h3>搜索结果：</h3>
            <pre>{{ novelResults }}</pre>
        </div>

        <h1>2: 搜索作者</h1>
        <input v-model="authorKeyword" placeholder="输入作者关键词" />
        <button @click="searchbyAuthors">搜索作者</button>
        <div v-if="authorResults">
            <h3>搜索结果：</h3>
            <pre>{{ authorResults }}</pre>
        </div>

        <h1>3: 搜索读者</h1>
        <input v-model="readerKeyword" placeholder="输入读者关键词" />
        <button @click="searchbyReaders">搜索读者</button>
        <div v-if="readerResults">
            <h3>搜索结果：</h3>
            <pre>{{ readerResults }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import { searchNovels, searchAuthors, searchReaders } from '@/API/Search_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

// 搜索小说
const novelKeyword = ref('')
const novelResults = ref(null)
async function searchbyNovels() {
    try {
        const response = await searchNovels(novelKeyword.value)
        novelResults.value = response
        console.log('小说搜索结果:', response)
    } catch (error) {
        console.error('搜索小说错误:', error)
        novelResults.value = { error: error.message }
    }
}

// 搜索作者
const authorKeyword = ref('')
const authorResults = ref(null)
async function searchbyAuthors() {
    try {
        const response = await searchAuthors(authorKeyword.value)
        authorResults.value = response
        console.log('作者搜索结果:', response)
    } catch (error) {
        console.error('搜索作者错误:', error)
        authorResults.value = { error: error.message }
    }
}

// 搜索读者
const readerKeyword = ref('')
const readerResults = ref(null)
async function searchbyReaders() {
    try {
        const response = await searchReaders(readerKeyword.value)
        readerResults.value = response
        console.log('读者搜索结果:', response)
    } catch (error) {
        console.error('搜索读者错误:', error)
        readerResults.value = { error: error.message }
    }
}
</script>