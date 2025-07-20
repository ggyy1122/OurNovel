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
    <div style="text-align:center; margin-top:100px;">
        <button @click="goToLogin3">Novel的API测试</button>
    </div>
    <div style="text-align:center; margin-top:100px;">
        <button @click="goToLogin4">Collect的API测试</button>
    </div>
    <div style="text-align:center; margin-top:100px;">
        <button @click="goToLogin5">Rate的API测试</button>
    </div>
    <div style="text-align:center; margin-top:100px;">
        <button @click="goToLogin6">Category的API测试</button>
    </div>
    <div style="text-align:center; margin-top:100px;">
        <button @click="goToLogin7">NovelCategory的API测试</button>
    </div>
    <div style="text-align:center; margin-top:100px;">
        <button @click="goToLogin8">Chapter的API测试</button>
    </div>
    <div style="text-align:center; margin-top:100px;">
        <button @click="goToLogin9">Reader的API测试</button>
    </div>
    <div style="text-align:center; margin-top:100px;">
        <button @click="goToLogin10">Recommend的API测试</button>
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
function goToLogin3() {
    router.push('/Novel_api_test')
}
function goToLogin4() {
    router.push('/Collect_api_test')
}
function goToLogin5() {
    router.push('/Rate_api_test')
}
function goToLogin6() {
    router.push('/Category_api_test')
}
function goToLogin7() {
    router.push('/NovelCategory_api_test')
}
function goToLogin8() {
    router.push('/Chapter_api_test')
}
function goToLogin9() {
    router.push('/Reader_api_test')
}
function goToLogin10() {
    router.push('/Recommend_api_test')
}
</script>