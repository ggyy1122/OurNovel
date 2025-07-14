<template>
    <div class="novel-card">
        <div class="rank-badge">{{ rank }}</div>
        <img :src="novel.cover" alt="cover" class="cover" />
        <div class="info">
            <div class="title-row">
                <span class="title">{{ novel.title }}</span>
                <span v-if="novel.badge" class="badge">{{ novel.badge }}</span>
            </div>
            <div class="meta">
                <span>{{ novel.author }}</span> |
                <span>{{ novel.category }}</span> |
                <span>{{ novel.status }}</span> |
                <span>{{ novel.wordCount }}</span>
            </div>
            <div class="desc">{{ novel.desc }}</div>
            <div class="update">{{ novel.update }}</div>
        </div>
        <div class="side">
            <div class="hot">
                <span class="hot-num">{{ novel.hot }}</span>
                <span class="hot-label">热度</span>
            </div>
            <div class="actions">
                <button class="add-shelf">加入书架</button>
                <button class="read">立即阅读</button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { defineProps } from 'vue';

defineProps({
    novel: Object,
    rank: Number
})
</script>

<style scoped>
.novel-card {
    display: flex;
    align-items: flex-start;
    background: #fff;
    border-radius: 10px;
    box-shadow: 0 4px 14px rgba(0, 0, 0, 0.03);
    padding: 18px 18px;
    gap: 18px;
    position: relative;
}

.rank-badge {
    position: absolute;
    left: 0;
    top: 0;
    background: #ff4a4a;
    color: #fff;
    font-weight: bold;
    font-size: 22px;
    border-radius: 50%;
    width: 40px;
    height: 40px;
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1;
}

.cover {
    width: 94px;
    height: 128px;
    border-radius: 7px;
    object-fit: cover;
    margin-left: 50px;
}

.info {
    flex: 1;
    margin-left: 22px;
    display: flex;
    flex-direction: column;
    gap: 7px;
}

.title-row {
    display: flex;
    align-items: center;
    gap: 8px;
}

.title {
    font-size: 22px;
    font-weight: bold;
    color: #222;
}

.badge {
    background: #ffe357;
    color: #d28a1a;
    padding: 3px 9px;
    border-radius: 16px;
    font-size: 14px;
    font-weight: 500;
}

.meta {
    font-size: 15px;
    color: #888;
}

.desc {
    margin-top: 2px;
    color: #555;
    font-size: 16px;
    line-height: 1.6;
    max-width: 520px;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}

.update {
    color: #aaa;
    font-size: 13px;
}

.side {
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    gap: 18px;
    min-width: 120px;
    margin-left: 30px;
}

.hot {
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    font-size: 18px;
}

.hot-num {
    font-weight: 700;
    color: #fa9437;
}

.hot-label {
    font-size: 14px;
    color: #888;
}

.actions {
    display: flex;
    flex-direction: column;
    gap: 10px;
}

.add-shelf {
    background: #f8f8f8;
    border: none;
    border-radius: 18px;
    padding: 8px 24px;
    font-size: 15px;
    color: #555;
    cursor: pointer;
    margin-bottom: 5px;
    transition: background 0.18s;
}

.add-shelf:hover {
    background: #ffe357;
    color: #d28a1a;
}

.read {
    background: #ffe357;
    border: none;
    border-radius: 18px;
    padding: 8px 24px;
    font-size: 15px;
    color: #d28a1a;
    font-weight: 600;
    cursor: pointer;
    transition: box-shadow 0.18s;
}

.read:hover {
    box-shadow: 0 2px 8px rgba(250, 190, 50, 0.15);
}
</style>