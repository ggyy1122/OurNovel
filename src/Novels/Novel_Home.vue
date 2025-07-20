<template>
    <div class="reader-state">
        <h3>读者状态信息</h3>
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

// 获取 store 实例
const store = readerState()
</script>

<style scoped>
.reader-state {
    padding: 20px;
    border: 1px solid #eee;
    border-radius: 4px;
}

.reader-state div {
    margin: 8px 0;
    padding: 4px;
    background: #f9f9f9;
}
</style>