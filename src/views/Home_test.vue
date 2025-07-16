<template>
    <div style="text-align:center; margin-top:100px;">
        <button @click="goToLogin">登录</button>
    </div>
    <div style="text-align:center; margin-top:100px;">
        <button @click="goToLogin1">Author的API测试</button>
    </div>
        <div style="text-align:center; margin-top:100px;">
        <button @click="goToLogin2">Comment的API测试</button>
    </div>
</template>

<script setup>
import { useRouter } from 'vue-router'
const router = useRouter()

function goToLogin() {
    router.push('/L_R/login')
}
function goToLogin1() {
    router.push('/Author_api_test')
}
function goToLogin2() {
    router.push('/Comment_api_test')
}
</script>