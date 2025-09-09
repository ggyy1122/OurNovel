<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1: 获取所有小说与分类关系</h1>
        <button @click="getAllRelations">获取所有关系</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2: 添加小说分类关系</h1>
        <input v-model="novelId2" placeholder="输入小说ID" type="number" />
        <input v-model="categoryName2" placeholder="输入分类名称" />
        <button @click="addRelation">添加关系</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <h1>3: 删除小说分类关系</h1>
        <input v-model="novelId3" placeholder="输入小说ID" type="number" />
        <input v-model="categoryName3" placeholder="输入分类名称" />
        <button @click="deleteRelation">删除关系</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

        <h1>4: 获取小说全部分类</h1>
        <input v-model="novelId4" placeholder="输入小说ID" type="number" />
        <button @click="getCategoriesForNovel">获取分类</button>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>

        <h1>5: 获取分类下所有小说</h1>
        <input v-model="categoryName5" placeholder="输入分类名称" />
        <button @click="getNovelsInCategory">获取小说</button>
        <div v-if="apiResponse5">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse5 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import {
    getAllNovelCategories,
    addNovelCategory,
    deleteNovelCategory,
    getCategoriesByNovel,
    getNovelsByCategory
} from '@/API/NovelCategory_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

// 1: 获取所有关系
const apiResponse1 = ref(null)
async function getAllRelations() {
    try {
        const response = await getAllNovelCategories()
        apiResponse1.value = response
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

// 2: 添加关系
const novelId2 = ref('')
const categoryName2 = ref('')
const apiResponse2 = ref(null)
async function addRelation() {
    try {
        const response = await addNovelCategory(
            parseInt(novelId2.value),
            categoryName2.value
        )
        apiResponse2.value = response
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse2.value = { error: error.message }
    }
}

// 3: 删除关系
const novelId3 = ref('')
const categoryName3 = ref('')
const apiResponse3 = ref(null)
async function deleteRelation() {
    try {
        const response = await deleteNovelCategory(
            parseInt(novelId3.value),
            categoryName3.value
        )
        apiResponse3.value = response
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse3.value = { error: error.message }
    }
}

// 4: 获取小说分类
const novelId4 = ref('')
const apiResponse4 = ref(null)
async function getCategoriesForNovel() {
    try {
        const response = await getCategoriesByNovel(parseInt(novelId4.value))
        apiResponse4.value = response
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse4.value = { error: error.message }
    }
}

// 5: 获取分类小说
const categoryName5 = ref('')
const apiResponse5 = ref(null)
async function getNovelsInCategory() {
    try {
        const response = await getNovelsByCategory(categoryName5.value)
        apiResponse5.value = response
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse5.value = { error: error.message }
    }
}
</script>