<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>打赏小说</h1>
        <input v-model="readerId" placeholder="输入读者ID" />
        <input v-model="novelId" placeholder="输入小说ID" />
        <input v-model="amount" placeholder="输入打赏金额" />
        <button @click="reward">打赏</button>
        <div v-if="apiResponse">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse }}</pre>
        </div>

    </div>
</template>

<script setup>
import { ref } from 'vue'
import { rewardNovel } from '@/API/Reward_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

const readerId = ref('')
const novelId = ref('')
const amount = ref('')
const apiResponse = ref(null)

async function reward() {
    try {
        const rewardData = {
            readerId: readerId.value,
            novelId: novelId.value,
            amount: amount.value
        }
        const response = await rewardNovel(rewardData)
        apiResponse.value = response
        console.log('打赏响应:', response)
    } catch (error) {
        console.error('打赏请求错误:', error)
        apiResponse.value = { error: error.message }
    }
}
</script>