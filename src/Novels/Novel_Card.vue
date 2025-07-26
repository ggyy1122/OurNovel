<template>
    <div class="novel-card">
        <div class="rank-badge">{{ rank }}</div>
        <img :src="'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' + (novel.coverUrl || 'a3dc347b-45dd-4d89-8b9d-65b75477ee3d.jpg')"
            alt="cover" class="cover" @click="handle_NovelInfro" />
        <div class="info">
            <div class="title-row">
                <span class="title" @click="handle_NovelInfro">{{ novel.novelName }}</span>
                <span class="badge">{{ novel.status }}</span>
                <span class="badge">评分★：{{ novel.score || 0 }}分</span>
            </div>
            <div class="meta">
                <span>作者: {{ author_name }}</span> |
                <span>字数: {{ novel.totalWordCount || 0 }}字</span>
            </div>
            <div class="desc">{{ novel.introduction || '暂无简介' }}</div>
            <div class="update">创建时间: {{ formatDate(novel.createTime) }}</div>
        </div>
        <div class="side">
            <div class="hot">
                <span>推荐数:{{ novel.recommendCount || 0 }}</span>
                <span>收藏数:{{ collectedcount }}</span>
            </div>
            <div class="actions">
                <button class="add-shelf" @click="handleAddShelf" :class="{ 'in-shelf': isFavorite }"
                    :disabled="isFavorite">
                    {{ isFavorite ? '已在书架' : '加入书架' }}
                </button>
                <button class="read" @click="handleRead">立即阅读</button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { defineProps, ref } from 'vue';
import { useRouter } from 'vue-router';
import { SelectNovel_State, readerState } from '@/stores/index';
import { getAuthor } from '@/API/Author_API'
import { addOrUpdateCollect } from '@/API/Collect_API'
import { getChapter } from '@/API/Chapter_API';
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
const selectNovelState = SelectNovel_State();
const reader_state = readerState();
const props = defineProps({
    novel: Object,
    rank: Number
})
const collectedcount = ref(props.novel.collectedCount || 0);
const router = useRouter();
const formatDate = (dateString) => {
    if (!dateString) return '未知时间'
    const date = new Date(dateString)
    return `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`
}
const isFavorite = ref(reader_state.isFavorite(props.novel.novelId));
//加入书架
async function handleAddShelf() {
    if (isFavorite.value) return;
    const readerId = reader_state.readerId;
    const response = await addOrUpdateCollect(props.novel.novelId, readerId, 'yes');
    if (response) {
        isFavorite.value = true;
        reader_state.favoriteBooks.push({
            novelId: props.novel.novelId,
            novel: selectNovelState, // 保存完整作品信息
            readerId,
            isPublic: "yes",
            collectTime: new Date().toISOString()
        });
        collectedcount.value += 1; // 更新收藏数
        toast("加入书架成功！", {
            "type": "success",
            "dangerouslyHTMLString": true
        })
    } else {
        toast("加入书架失败：" + response.message, {
            "type": "error",
            "dangerouslyHTMLString": true
        })
    }
}
const author_name = ref('');
const responsePromise = getAuthor(props.novel.authorId);
responsePromise.then(response => {
    author_name.value = response.authorName;
}).catch(error => {
    console.error('获取作者信息失败:', error);
});
async function handle() {
    const response = await getAuthor(props.novel.authorId);
    selectNovelState.resetNovel(props.novel.novelId, props.novel.authorId, props.novel.novelName, props.novel.introduction, props.novel.createTime, props.novel.coverUrl, props.novel.score, props.novel.totalWordCount, props.novel.recommendCount, props.novel.collectedCount, props.novel.status, props.novel.totalPrice,
        response.authorName, response.phone, response.avatarUrl);

    try {
        const response = await responsePromise;
        selectNovelState.resetNovel(
            props.novel.novelId,
            props.novel.authorId,
            props.novel.novelName,
            props.novel.introduction,
            props.novel.createTime,
            props.novel.coverUrl,
            props.novel.score,
            props.novel.totalWordCount,
            props.novel.recommendCount,
            props.novel.collectedCount,
            props.novel.status,
            response.authorName,
            response.phone,
            response.avatarUrl
        );
    } catch (error) {
        console.error('处理失败:', error);

}
//立即阅读
async function handleRead() {
    try {
        handle();
        const response = await getChapter(props.novel.novelId, 1);
        selectNovelState.resetChapter(
            response.chapterId,
            response.title,
            response.content,
            response.wordCount,
            response.pricePerKilo,
            response.calculatedPrice,
            response.isCharged,
            response.publishTime,
            response.status
        );
        router.push('/Novels/reader');
    } catch (error) {
        toast("该小说还没有章节！", {
            "type": "warning",
            "dangerouslyHTMLString": true
        })
    }
}
//作品主页
async function handle_NovelInfro() {
    handle();
    router.push('/Novels/Novel_Info/home');
}
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

.cover:hover {
    transform: scale(1.05);
    transition: transform 0.3s ease;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
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
    font-weight: 700;
    font-size: 16px;
    color: #fa9437;
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

.add-shelf.in-shelf {
    background: #f0f0f0;
    color: #999;
    cursor: default;
}

.add-shelf:disabled {
    opacity: 1;
}
</style>