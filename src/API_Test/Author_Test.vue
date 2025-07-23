<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>


        <h1>1:获取所有作者</h1>
        <button @click="function1">所有作者</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>


        <h1>2:获取1个作者</h1>
        <input v-model="authorId2" placeholder="输入作者ID" />
        <button @click="fuction2">搜索</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>


        <h1>3:创建作者</h1>
        <input v-model="authorName3" placeholder="输入作者名称" />
        <input v-model="password3" placeholder="输入密码" />
        <input v-model="earning3" placeholder="输入收入" />
        <input v-model="phone3" placeholder="输入电话" />
        <input v-model="avatarUrl3" placeholder="输入头像URL" />
        <button @click="fuction3">创建作者</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>


        <h1>4:更新作者</h1>
        <input v-model="authorId4" placeholder="输入作者ID" />
        <input v-model="authorName4" placeholder="输入作者名称" />
        <input v-model="password4" placeholder="输入密码" />
        <input v-model="earning4" placeholder="输入收入" />
        <input v-model="phone4" placeholder="输入电话" />
        <input v-model="avatarUrl4" placeholder="输入头像URL" />
        <button @click="fuction4">更新作者</button>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>


        <h1>5:删除作者</h1>
        <input v-model="authorId5" placeholder="输入作者ID" />
        <button @click="fuction5">删除作者</button>
        <div v-if="apiResponse5">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse5 }}</pre>
        </div>

        <h1>6: 获取作者作品数量</h1>
        <input v-model="authorId6" placeholder="输入作者ID" type="number" />
        <button @click="function6" :disabled="!authorId6">获取作品数量</button>
        <div v-if="apiResponse6">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse6 }}</pre>
        </div>

        <h1>7: 获取作者作品总字数</h1>
        <input v-model="authorId7" placeholder="输入作者ID" type="number" />
        <button @click="function7" :disabled="!authorId7">获取总字数</button>
        <div v-if="apiResponse7">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse7 }}</pre>
        </div>

        <h1>8: 获取作者注册天数</h1>
        <input v-model="authorId8" placeholder="输入作者ID" type="number" />
        <button @click="function8" :disabled="!authorId8">获取注册天数</button>
        <div v-if="apiResponse8">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse8 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import {
    getAllAuthors, getAuthor, createAuthor, updateAuthor, deleteAuthor, getAuthorNovelCount,
    getAuthorTotalWordCount,
    getAuthorRegisterDays
} from '@/API/Author_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

//1:获取所有作者
const apiResponse1 = ref(null)

async function function1() {
    try {
        const response = await getAllAuthors()
        apiResponse1.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

//2:获取1个作者
const apiResponse2 = ref(null)
const authorId2 = ref('')
async function fuction2() {
    try {
        const response = await getAuthor(authorId2.value)
        apiResponse2.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse2.value = { error: error.message + '，请检查作者ID是否正确' }
    }
}


//3:创建作者
const authorName3 = ref('')
const password3 = ref('')
const earning3 = ref('')
const phone3 = ref('')
const avatarUrl3 = ref('')
const apiResponse3 = ref(null)
async function fuction3() {
    try {
        const authorData = {
            authorName: authorName3.value,
            password: password3.value,
            earning: earning3.value,
            phone: phone3.value,
            avatarUrl: avatarUrl3.value
        }
        const response = await createAuthor(authorData)
        apiResponse3.value = response
        console.log('创建响应:', response)
    } catch (error) {
        console.error('创建作者请求错误:', error)
        apiResponse3.value = { error: error.message }
    }
}


//4:更新作者
const authorId4 = ref('')
const authorName4 = ref('')
const password4 = ref('')
const earning4 = ref('')
const phone4 = ref('')
const avatarUrl4 = ref('')
const apiResponse4 = ref(null)
async function fuction4() {
    try {
        const authorData = {
            authorId: authorId4.value,
            authorName: authorName4.value,
            password: password4.value,
            earning: earning4.value,
            phone: phone4.value,
            avatarUrl: avatarUrl4.value
        }
        const response = await updateAuthor(authorId4.value, authorData)
        apiResponse4.value = response
        console.log('更新响应:', response)
    } catch (error) {
        console.error('更新作者请求错误:', error)
        apiResponse4.value = { error: error.message }
    }
}


//5:删除作者
const authorId5 = ref('')
const apiResponse5 = ref(null)
async function fuction5() {
    try {
        const response = await deleteAuthor(authorId5.value)
        apiResponse5.value = response
        console.log('删除响应:', response)
    } catch (error) {
        console.error('删除作者请求错误:', error)
        apiResponse5.value = { error: error.message }
    }
}

// 6: 获取作者作品数量
const authorId6 = ref('')
const apiResponse6 = ref(null)
async function function6() {
    try {
        const response = await getAuthorNovelCount(authorId6.value)
        apiResponse6.value = response
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse6.value = { error: error.message }
    }
}

// 7: 获取作者作品总字数
const authorId7 = ref('')
const apiResponse7 = ref(null)
async function function7() {
    try {
        const response = await getAuthorTotalWordCount(authorId7.value)
        apiResponse7.value = response
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse7.value = { error: error.message }
    }
}

// 8: 获取作者注册天数
const authorId8 = ref('')
const apiResponse8 = ref(null)
async function function8() {
    try {
        const response = await getAuthorRegisterDays(authorId8.value)
        apiResponse8.value = response
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse8.value = { error: error.message }
    }
}

</script>