<template>
    <div style="text-align:center; margin-top:100px;">
        <button @click="goToLogin">管理员登录</button>
    </div>
</template>

<script setup>
import { useRouter } from 'vue-router'
const router = useRouter()
function goToLogin() {
    router.push('/Admin/Admin_Login')
}
</script>