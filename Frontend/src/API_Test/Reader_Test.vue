<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:获取所有读者</h1>
        <button @click="function1">所有读者</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2:获取1个读者</h1>
        <input v-model="readerId2" placeholder="输入读者ID" />
        <button @click="function2">搜索</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <h1>3:创建读者(这里创建不会产生密钥，密码是登不上的)</h1>
        <input v-model="readerName3" placeholder="输入读者名称" />
        <input v-model="password3" placeholder="输入密码" />
        <input v-model="phone3" placeholder="输入电话" />
        <select v-model="gender3">
            <option value="">选择性别 (男/女)</option>
            <option value="男">男</option>
            <option value="女">女</option>
        </select>
        <input v-model="balance3" placeholder="输入余额" />
        <input v-model="avatarUrl3" placeholder="输入头像URL" />
        <input v-model="backgroundUrl3" placeholder="输入背景URL" />
        <select v-model="isCollectVisible3">
            <option value="">收藏是否可见 (是/否)</option>
            <option value="是">是</option>
            <option value="否">否</option>
        </select>
        <select v-model="isRecommendVisible3">
            <option value="">推荐是否可见 (是/否)</option>
            <option value="是">是</option>
            <option value="否">否</option>
        </select>
        <button @click="function3">创建读者</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

        <h1>4:更新读者(这里也是，修改后密码是登不上的，还是密钥的问题。目前改密码在登录时忘记密码处)</h1>
        <input v-model="readerId4" placeholder="输入读者ID" />
        <input v-model="readerName4" placeholder="输入读者名称" />
        <input v-model="password4" placeholder="输入密码" />
        <input v-model="phone4" placeholder="输入电话" />
        <select v-model="gender4">
            <option value="">选择性别 (男/女)</option>
            <option value="男">男</option>
            <option value="女">女</option>
        </select>
        <input v-model="balance4" placeholder="输入余额" />
        <input v-model="avatarUrl4" placeholder="输入头像URL" />
        <input v-model="backgroundUrl4" placeholder="输入背景URL" />
        <select v-model="isCollectVisible4">
            <option value="">收藏是否可见 (是/否)</option>
            <option value="是">是</option>
            <option value="否">否</option>
        </select>
        <select v-model="isRecommendVisible4">
            <option value="">推荐是否可见 (是/否)</option>
            <option value="是">是</option>
            <option value="否">否</option>
        </select>
        <button @click="function4">更新读者</button>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>

        <h1>5:删除读者</h1>
        <input v-model="readerId5" placeholder="输入读者ID" />
        <button @click="function5">删除读者</button>
        <div v-if="apiResponse5">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse5 }}</pre>
        </div>

        <h1>6:上传读者头像</h1>
        <input type="file" @change="handleFileUpload" />
        <input v-model="readerId6" placeholder="输入读者ID" />
        <button @click="function6">上传头像</button>
        <div v-if="apiResponse6">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse6 }}</pre>
        </div>

        <h1>7:获取读者余额</h1>
        <input v-model="readerId7" placeholder="输入读者ID" />
        <button @click="function7">获取余额</button>
        <div v-if="apiResponse7">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse7 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import {
    getAllReaders,
    getReader,
    createReader,
    updateReader,
    deleteReader,
    uploadReaderAvatar,
    getReaderBalance
} from '@/API/Reader_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

//1:获取所有读者
const apiResponse1 = ref(null)
async function function1() {
    try {
        const response = await getAllReaders()
        apiResponse1.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

//2:获取1个读者
const apiResponse2 = ref(null)
const readerId2 = ref('')
async function function2() {
    try {
        const response = await getReader(readerId2.value)
        apiResponse2.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse2.value = { error: error.message + '，请检查读者ID是否正确' }
    }
}

//3:创建读者
const readerName3 = ref('')
const password3 = ref('')
const phone3 = ref('')
const gender3 = ref('')
const balance3 = ref('')
const avatarUrl3 = ref('')
const backgroundUrl3 = ref('')
const isCollectVisible3 = ref('')
const isRecommendVisible3 = ref('')
const apiResponse3 = ref(null)
async function function3() {
    try {
        const readerData = {
            readerName: readerName3.value,
            password: password3.value,
            phone: phone3.value,
            gender: gender3.value,
            balance: balance3.value || 0,
            avatarUrl: avatarUrl3.value,
            backgroundUrl: backgroundUrl3.value,
            isCollectVisible: isCollectVisible3.value,
            isRecommendVisible: isRecommendVisible3.value
        }
        const response = await createReader(readerData)
        apiResponse3.value = response
        console.log('创建响应:', response)
    } catch (error) {
        console.error('创建读者请求错误:', error)
        apiResponse3.value = { error: error.message }
    }
}

//4:更新读者
const readerId4 = ref('')
const readerName4 = ref('')
const password4 = ref('')
const phone4 = ref('')
const gender4 = ref('')
const balance4 = ref('')
const avatarUrl4 = ref('')
const backgroundUrl4 = ref('')
const isCollectVisible4 = ref('')
const isRecommendVisible4 = ref('')
const apiResponse4 = ref(null)
async function function4() {
    try {
        const updateData = {
            readerId: readerId4.value,
            readerName: readerName4.value,
            password: password4.value,
            phone: phone4.value,
            gender: gender4.value,
            balance: balance4.value,
            avatarUrl: avatarUrl4.value,
            backgroundUrl: backgroundUrl4.value,
            isCollectVisible: isCollectVisible4.value,
            isRecommendVisible: isRecommendVisible4.value
        }
        const response = await updateReader(readerId4.value, updateData)
        apiResponse4.value = response
        console.log('更新响应:', response)
    } catch (error) {
        console.error('更新读者请求错误:', error)
        apiResponse4.value = { error: error.message }
    }
}

//5:删除读者
const readerId5 = ref('')
const apiResponse5 = ref(null)
async function function5() {
    try {
        const response = await deleteReader(readerId5.value)
        apiResponse5.value = response
        console.log('删除响应:', response)
    } catch (error) {
        console.error('删除读者请求错误:', error)
        apiResponse5.value = { error: error.message }
    }
}

//6:上传读者头像
const readerId6 = ref('')
const avatarFile = ref(null)
const apiResponse6 = ref(null)
function handleFileUpload(event) {
    avatarFile.value = event.target.files[0]
}
async function function6() {
    try {
        if (!avatarFile.value) {
            throw new Error('请选择头像文件')
        }
        const response = await uploadReaderAvatar(readerId6.value, avatarFile.value)
        apiResponse6.value = response
        console.log('上传头像响应:', response)
    } catch (error) {
        console.error('上传头像请求错误:', error)
        apiResponse6.value = { error: error.message }
    }
}

//7:获取读者余额
const readerId7 = ref('')
const apiResponse7 = ref(null)
async function function7() {
    try {
        const response = await getReaderBalance(readerId7.value)
        apiResponse7.value = response
        console.log('获取余额响应:', response)
    } catch (error) {
        console.error('获取余额请求错误:', error)
        apiResponse7.value = { error: error.message }
    }
}
</script>