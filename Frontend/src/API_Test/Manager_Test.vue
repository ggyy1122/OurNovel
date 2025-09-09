<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:获取所有管理员</h1>
        <button @click="function1">所有管理员</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2:获取1个管理员</h1>
        <input v-model="managerId2" placeholder="输入管理员ID" />
        <button @click="function2">搜索</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <h1>3:创建管理员</h1>
        <input v-model="managerName3" placeholder="输入管理员名称" />
        <input v-model="password3" placeholder="输入密码" />
        <button @click="function3">创建管理员</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

        <h1>4:更新管理员</h1>
        <input v-model="managerId4" placeholder="输入管理员ID" />
        <input v-model="managerName4" placeholder="输入管理员名称" />
        <input v-model="password4" placeholder="输入密码" />
        <button @click="function4">更新管理员</button>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>

        <h1>5:删除管理员</h1>
        <input v-model="managerId5" placeholder="输入管理员ID" />
        <button @click="function5">删除管理员</button>
        <div v-if="apiResponse5">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse5 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import { 
    getAllManagers, 
    getManager, 
    createManager, 
    updateManager, 
    deleteManager 
} from '@/API/Manager_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

// 1:获取所有管理员
const apiResponse1 = ref(null)
async function function1() {
    try {
        const response = await getAllManagers()
        apiResponse1.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

// 2:获取1个管理员
const apiResponse2 = ref(null)
const managerId2 = ref('')
async function function2() {
    try {
        const response = await getManager(managerId2.value)
        apiResponse2.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse2.value = { error: error.message + '，请检查管理员ID是否正确' }
    }
}

// 3:创建管理员
const managerName3 = ref('')
const password3 = ref('')
const apiResponse3 = ref(null)
async function function3() {
    try {
        const managerData = {
            managerName: managerName3.value,
            password: password3.value
        }
        const response = await createManager(managerData)
        apiResponse3.value = response
        console.log('创建响应:', response)
    } catch (error) {
        console.error('创建管理员请求错误:', error)
        apiResponse3.value = { error: error.message }
    }
}

// 4:更新管理员
const managerId4 = ref('')
const managerName4 = ref('')
const password4 = ref('')
const apiResponse4 = ref(null)
async function function4() {
    try {
        const updateData = {
            managerId: managerId4.value,
            managerName: managerName4.value,
            password: password4.value
        }
        const response = await updateManager(managerId4.value, updateData)
        apiResponse4.value = response
        console.log('更新响应:', response)
    } catch (error) {
        console.error('更新管理员请求错误:', error)
        apiResponse4.value = { error: error.message }
    }
}

// 5:删除管理员
const managerId5 = ref('')
const apiResponse5 = ref(null)
async function function5() {
    try {
        const response = await deleteManager(managerId5.value)
        apiResponse5.value = response
        console.log('删除响应:', response)
    } catch (error) {
        console.error('删除管理员请求错误:', error)
        apiResponse5.value = { error: error.message }
    }
}
</script>