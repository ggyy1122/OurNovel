<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:获取所有分类</h1>
        <button @click="function1">所有分类</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2:获取1个分类</h1>
        <input v-model="categoryName2" placeholder="输入分类名称" />
        <button @click="function2">搜索</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <h1>3:创建分类</h1>
        <input v-model="categoryName3" placeholder="输入分类名称" />
        <button @click="function3">创建分类</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

        <h1>4:更新分类</h1>
        <input v-model="oldName4" placeholder="输入原始分类名称" />
        <input v-model="newName4" placeholder="输入新分类名称" />
        <button @click="function4">更新分类</button>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>

        <h1>5:重命名分类</h1>
        <input v-model="oldName5" placeholder="输入原始分类名称" />
        <input v-model="newName5" placeholder="输入新分类名称" />
        <button @click="function5">重命名分类</button>
        <div v-if="apiResponse5">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse5 }}</pre>
        </div>

        <h1>6:删除分类</h1>
        <input v-model="categoryName6" placeholder="输入分类名称" />
        <button @click="function6">删除分类</button>
        <div v-if="apiResponse6">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse6 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import { 
    getAllCategories, 
    getCategory, 
    createCategory, 
    updateCategory, 
    renameCategory, 
    deleteCategory 
} from '@/API/Category_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

//1:获取所有分类
const apiResponse1 = ref(null)
async function function1() {
    try {
        const response = await getAllCategories()
        apiResponse1.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

//2:获取1个分类
const apiResponse2 = ref(null)
const categoryName2 = ref('')
async function function2() {
    try {
        const response = await getCategory(categoryName2.value)
        apiResponse2.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse2.value = { error: error.message + '，请检查分类名称是否正确' }
    }
}

//3:创建分类
const categoryName3 = ref('')
const apiResponse3 = ref(null)
async function function3() {
    try {
        const response = await createCategory({
            categoryName: categoryName3.value
        })
        apiResponse3.value = response
        console.log('创建响应:', response)
    } catch (error) {
        console.error('创建分类请求错误:', error)
        apiResponse3.value = { error: error.message }
    }
}

//4:更新分类
const oldName4 = ref('')
const newName4 = ref('')
const apiResponse4 = ref(null)
async function function4() {
    try {
        const response = await updateCategory(oldName4.value, {
            categoryName: newName4.value
        })
        apiResponse4.value = response
        console.log('更新响应:', response)
    } catch (error) {
        console.error('更新分类请求错误:', error)
        apiResponse4.value = { error: error.message }
    }
}

//5:重命名分类
const oldName5 = ref('')
const newName5 = ref('')
const apiResponse5 = ref(null)
async function function5() {
    try {
        const response = await renameCategory(oldName5.value, newName5.value)
        apiResponse5.value = response
        console.log('重命名响应:', response)
    } catch (error) {
        console.error('重命名分类请求错误:', error)
        apiResponse5.value = { error: error.message }
    }
}

//6:删除分类
const categoryName6 = ref('')
const apiResponse6 = ref(null)
async function function6() {
    try {
        const response = await deleteCategory(categoryName6.value)
        apiResponse6.value = response
        console.log('删除响应:', response)
    } catch (error) {
        console.error('删除分类请求错误:', error)
        apiResponse6.value = { error: error.message }
    }
}
</script>