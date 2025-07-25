<template>
    <div id="app" style="text-align:center; margin-top:100px;">

        <button @click="Home">返回登录页面</button>

        <h1>1:获取所有交易记录</h1>
        <button @click="function1">所有交易</button>
        <div v-if="apiResponse1">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse1 }}</pre>
        </div>

        <h1>2:获取单个交易详情</h1>
        <input v-model="transactionId2" placeholder="输入交易ID" />
        <button @click="function2">搜索</button>
        <div v-if="apiResponse2">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse2 }}</pre>
        </div>

        <h1>3:创建交易记录</h1>
        <input v-model="readerId3" placeholder="输入读者ID" />
        <select v-model="transType3">
            <option value="打赏">打赏</option>
            <option value="充值">充值</option>
            <option value="解锁章节">解锁章节</option>
        </select>
        <input v-model="amount3" placeholder="输入金额" />
        <button @click="function3">创建交易</button>
        <div v-if="apiResponse3">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse3 }}</pre>
        </div>

        <h1>4:更新交易记录</h1>
        <input v-model="transactionId4" placeholder="输入交易ID" />
        <input v-model="readerId4" placeholder="输入读者ID" />
        <select v-model="transType4">
            <option value="打赏">打赏</option>
            <option value="充值">充值</option>
            <option value="解锁章节">解锁章节</option>
        </select>
        <input v-model="amount4" placeholder="输入金额" />
        <button @click="function4">更新交易</button>
        <div v-if="apiResponse4">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse4 }}</pre>
        </div>

        <h1>5:删除交易记录</h1>
        <input v-model="transactionId5" placeholder="输入交易ID" />
        <button @click="function5">删除交易</button>
        <div v-if="apiResponse5">
            <h3>响应数据：</h3>
            <pre>{{ apiResponse5 }}</pre>
        </div>

        <h1>6:整本小说买断</h1>
        <input v-model="readerId6" placeholder="输入读者ID" />
        <input v-model="novelId6" placeholder="输入小说ID" />
        <button @click="function6">执行买断</button>
        <div v-if="apiResponse6">
             <h3>响应数据：</h3>
             <pre>{{ apiResponse6 }}</pre>
        </div>
       
       
    </div>
</template>

<script setup>
import { ref } from 'vue'
import {
    getAllTransactions,
    getTransaction,
    createTransaction,
    updateTransaction,
    deleteTransaction
} from '@/API/Transaction_API'

import { useRouter } from 'vue-router'
const router = useRouter()
function Home() {
    router.push('/')
}

// 1:获取所有交易记录
const apiResponse1 = ref(null)
async function function1() {
    try {
        const response = await getAllTransactions()
        apiResponse1.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse1.value = { error: error.message }
    }
}

// 2:获取单个交易详情
const transactionId2 = ref('')
const apiResponse2 = ref(null)
async function function2() {
    try {
        const response = await getTransaction(transactionId2.value)
        apiResponse2.value = response
        console.log('API 响应:', response)
    } catch (error) {
        console.error('API 请求错误:', error)
        apiResponse2.value = { error: error.message + '，请检查交易ID是否正确' }
    }
}

// 3:创建交易记录
const readerId3 = ref('')
const transType3 = ref('打赏')
const amount3 = ref('')
const apiResponse3 = ref(null)
async function function3() {
    try {
        const transactionData = {
            readerId: readerId3.value,
            transType: transType3.value,
            amount: amount3.value,
            time: new Date().toISOString() 
        }
        const response = await createTransaction(transactionData)
        apiResponse3.value = response
        console.log('创建响应:', response)
    } catch (error) {
        console.error('创建交易请求错误:', error)
        apiResponse3.value = { error: error.message }
    }
}

// 4:更新交易记录
const transactionId4 = ref('')
const readerId4 = ref('')
const transType4 = ref('打赏')
const amount4 = ref('')
const apiResponse4 = ref(null)
async function function4() {
    try {
        const updateData = {
            readerId: readerId4.value,
            transactionId: transactionId4.value,
            transType: transType4.value,
            amount: amount4.value,
            time: new Date().toISOString()
        }
        const response = await updateTransaction(transactionId4.value, updateData)
        apiResponse4.value = response
        console.log('更新响应:', response)
    } catch (error) {
        console.error('更新交易请求错误:', error)
        apiResponse4.value = { error: error.message }
    }
}

// 5:删除交易记录
const transactionId5 = ref('')
const apiResponse5 = ref(null)
async function function5() {
    try {
        const response = await deleteTransaction(transactionId5.value)
        apiResponse5.value = response
        console.log('删除响应:', response)
    } catch (error) {
        console.error('删除交易请求错误:', error)
        apiResponse5.value = { error: error.message }
    }
}

import { purchaseWholeNovel } from '@/API/Transaction_API'

const readerId6 = ref('')
const novelId6 = ref('')
const apiResponse6 = ref(null)

async function function6() {
  try {
    const dto = {
      readerId: parseInt(readerId6.value),
      novelId: parseInt(novelId6.value)
    }
    const response = await purchaseWholeNovel(dto)
    apiResponse6.value = response
    console.log('买断响应:', response)
  } catch (error) {
    console.error('整本买断请求错误:', error)
    apiResponse6.value = { error: error.message }
  }
}



</script>