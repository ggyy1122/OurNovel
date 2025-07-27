<template>
    <button @click="goback">返回</button>
    <h1>充值界面</h1>
</template>
<script setup>
import { useRouter } from 'vue-router';
const router = useRouter();
function goback() {
    router.back();
}
</script>