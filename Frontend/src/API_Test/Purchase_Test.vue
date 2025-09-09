<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>购买章节</h1>
        <input v-model="readerId" placeholder="输入读者ID" />
        <input v-model="novelId" placeholder="输入小说ID" />
        <input v-model="chapterId" placeholder="输入章节ID" />
        <button @click="purchase">购买章节</button>
        <div v-if="apiResponse">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse }}</pre>
        </div>

        <h1>检查是否已购买</h1>
        <input v-model="readerId2" placeholder="输入读者ID" type="number" />
        <input v-model="novelId2" placeholder="输入小说ID" type="number" />
        <input v-model="chapterId2" placeholder="输入章节ID" type="number" />
        <button @click="function2" :disabled="!readerId2 || !novelId2 || !chapterId2">检查购买状态</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import { purchaseChapter, checkPurchase } from '@/API/Purchase_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

const readerId = ref('')
const novelId = ref('')
const chapterId = ref('')
const apiResponse = ref(null)

async function purchase() {
    try {
        const purchaseData = {
            readerId: readerId.value,
            novelId: novelId.value,
            chapterId: chapterId.value
        }
        const response = await purchaseChapter(purchaseData)
        apiResponse.value = response
        console.log('购买响应:', response)
    } catch (error) {
        console.error('购买请求错误:', error)
        apiResponse.value = { error: error.message + '余额不足' }
    }
}

// 检查是否已购买
const readerId2 = ref('')
const novelId2 = ref('')
const chapterId2 = ref('')
const apiResponse2 = ref(null)

async function function2() {
    try {
        const response = await checkPurchase(
            readerId2.value,
            novelId2.value,
            chapterId2.value
        )
        apiResponse2.value = response
        console.log('检查购买状态响应:', response)
    } catch (error) {
        console.error('检查购买状态请求错误:', error)
        apiResponse2.value = { error: error.message }
    }
}
</script>