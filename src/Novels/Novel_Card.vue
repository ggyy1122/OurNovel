<template>
    <div class="novel-card">
        <div class="rank-badge">{{ rank }}</div>
        <img :src="novel.coverUrl || 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/e165315c-da2b-42c9-b3cf-c0457d168634.jpg'"
            alt="cover" class="cover" />
        <div class="info">
            <div class="title-row">
                <span class="title" @click="handleTitleClick">{{ novel.novelName }}</span>
                <span v-if="novel.status === '完结'" class="badge">完结</span>
                <span v-else class="badge">连载</span>
            </div>
            <div class="meta">
                <span>作者ID: {{ novel.authorId }}</span> |
                <span>{{ novel.totalWordCount || 0 }}字</span> |
                <span>{{ novel.status }}</span>
            </div>
            <div class="desc">{{ novel.introduction || '暂无简介' }}</div>
            <div class="update">创建时间: {{ formatDate(novel.createTime) }}</div>
        </div>
        <div class="side">
            <div class="hot">
                <span class="hot-num">{{ novel.recommendCount || 0 }}</span>
                <span class="hot-label">推荐</span>
            </div>
            <div class="actions">
                <button class="add-shelf" @click="handleAddShelf">加入书架</button>
                <button class="read" @click="handleRead">立即阅读</button>
                <button class="read" @click="handle_NovelInfro">作品主页</button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { defineProps } from 'vue';
import { useRouter } from 'vue-router';
import { SelectNovel_State } from '@/stores/index';
const selectNovelState = SelectNovel_State();

const props = defineProps({
    novel: Object,
    rank: Number
})
const router = useRouter();

const formatDate = (dateString) => {
    if (!dateString) return '未知时间'
    const date = new Date(dateString)
    return `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`
}

const handleRead = () => {
    // router.push({
    //     path: '/Novels/Novel_Layout/reader',
    //     query: {
    //         novel: JSON.stringify(props.novel)
    //     }
    // });
};
const handle_NovelInfro = () => {
    selectNovelState.resetNovel(props.novel.novelId,props.novel.authorId, props.novel.novelName,  props.novel.introduction, props.novel.createTime,props.novel.coverUrl,props.novel.score,props.novel.totalWordCount, props.novel.recommendCount,props.novel.collectCount, props.novel.status);
    router.push('/Novels/Novel_Info/home');
};

const handleTitleClick = () => {
    console.log('标题点击:', props.novel.novelId);
};

const handleAddShelf = (event) => {
    event.stopPropagation();
    console.log('加入书架:', props.novel.novelId);
};
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
    font-size: 18px;
    width: 42px;
    height: 48px;
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1;
    clip-path: polygon(50% 0%, 100% 25%, 100% 75%, 50% 100%, 0% 75%, 0% 25%);
}

.cover {
    width: 125px;
    height: 170px;
    border-radius: 7px;
    object-fit: cover;
    margin-left: 50px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
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
    font-size: 20px;
    font-weight: bold;
    color: #222;
    cursor: pointer;
}

.title:hover {
    color: #e67d06;
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
    font-size: 13px;
    color: #888;
}

.desc {
    margin-top: 2px;
    color: #555;
    font-size: 15px;
    line-height: 1.6;
    max-width: 520px;
    max-height: 4.8em;
    white-space: normal;
    overflow: hidden;
    text-overflow: ellipsis;
    display: -webkit-box;
    -webkit-box-orient: vertical;
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
    margin-right: 150px;
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
    background: #ece8e8;
    border: none;
    border-radius: 18px;
    padding: 8px 24px;
    font-size: 15px;
    color: #222;
    cursor: pointer;
    margin-bottom: 5px;
    transition: background 0.18s;
}

.add-shelf:hover {
    color: #ec9509;
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
    color: #222;
    box-shadow: 0 2px 8px rgba(250, 190, 50, 0.15);
}
</style>