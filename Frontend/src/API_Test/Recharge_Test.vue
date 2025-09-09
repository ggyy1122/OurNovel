<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:发起充值</h1>
        <input v-model="readerId1" placeholder="输入用户ID" type="number" />
        <input v-model="amount1" placeholder="输入充值金额" type="number" />
        <button @click="function1">发起充值</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
            <a v-if="apiResponse1.PaymentUrl" :href="apiResponse1.PaymentUrl" target="_blank">前往支付页面</a>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue'
import { startRecharge } from '@/API/Recharge_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

// 1:发起充值
const apiResponse1 = ref(null)
const readerId1 = ref('')
const amount1 = ref('')

async function function1() {
    try {
        const rechargeData = {
            ReaderId: parseInt(readerId1.value),
            Amount: parseFloat(amount1.value)
        }
        const response = await startRecharge(rechargeData)
        apiResponse1.value = response
        console.log('充值响应:', response)
    } catch (error) {
        console.error('充值请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}
</script>