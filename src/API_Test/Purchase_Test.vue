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

    </div>
</template>

<script setup>
import { ref } from 'vue'
import { purchaseChapter } from '@/API/Purchase_API'

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
        apiResponse.value = { error: error.message+'  该章节无需购买' }
    }
}
</script>