<template>
    <button @click="goback">返回</button>
    <!-- 单独元素使用 -->
    <div class="store-properties">
        <h2>个人信息</h2>

        <div class="property-item">
            <span class="property-key">ID</span>:
            <span class="property-value">{{ store.readerId }}</span>
        </div>

        <div class="property-item">
            <span class="property-key">Name</span>:
            <span class="property-value">{{ store.readerName }}</span>
        </div>
    </div>

    <!-- v-for遍历使用 -->
    <div class="reader-state">
        <h3>读者信息</h3>
        <!-- 遍历所有属性并打印 -->
        <div v-for="([key, value], index) in Object.entries(store.$state)" :key="index">
            <strong>{{ key }}:</strong>
            <span>
                <!-- 处理特殊类型（如数组、null）的显示 -->
                {{ typeof value === 'object' ? JSON.stringify(value) : value ?? 'null' }}
            </span>
        </div>
    </div>
</template>

<script setup>
import { readerState } from '@/stores/index'
import { useRouter } from 'vue-router'

// 获取 store 实例
const store = readerState()
const router = useRouter()
function goback() {
    router.push('/Novels/Novel_Layout/home')
}
</script>

<style scoped></style>