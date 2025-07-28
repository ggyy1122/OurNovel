<template>
    <div class="author-card">
        <div class="author-info">
            <img :src="'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' + (author.avatarUrl || '07850080-e498-47a4-8d3a-fd94fb47e561.jpg')"
                alt="作者头像" class="avatar" @click="goAuthorHome" />
            <div class="details">
                <h3 @click="goAuthorHome">{{ author.authorName }}</h3>
                <p class="meta">已创作 {{ authorRegisterDays.registerDays }} 天</p>
                <p class="meta">联系方式 : {{ maskedPhone }}</p>
                <p class="meta">简介 : {{ author.introduction || 'Ta还有点神秘哦~' }}</p>
            </div>
        </div>
        <div class="stats">
            <div class="stat-item">
                <span class="stat-value">{{ authorNovelCount.novelCount }}</span>
                <span class="stat-label">作品数</span>
            </div>
            <div class="stat-item">
                <span class="stat-value">{{ authorWordCount.totalWordCount }}</span>
                <span class="stat-label">总字数</span>
            </div>
        </div>
        <button class="follow-btn" @click="goAuthorHome">进入主页</button>
    </div>
</template>

<script setup>
import { ref, onMounted, defineProps, computed } from 'vue';
import { getAuthorNovelCount, getAuthorTotalWordCount, getAuthorRegisterDays } from '@/API/Author_API';
import { useRouter } from 'vue-router';
const router = useRouter();

const props = defineProps({
    author: {
        type: Object,
        required: true
    }
});
const authorNovelCount = ref(0);
const authorWordCount = ref(0);
const authorRegisterDays = ref(0);

const fetchAuthorStats = async () => {
    try {
        const [novelCount, wordCount, registerDays] = await Promise.all([
            getAuthorNovelCount(props.author.authorId),
            getAuthorTotalWordCount(props.author.authorId),
            getAuthorRegisterDays(props.author.authorId)
        ]);
        authorNovelCount.value = novelCount;
        authorWordCount.value = wordCount;
        authorRegisterDays.value = registerDays;
    } catch (error) {
        console.error('获取作者统计信息失败:', error);
    }
};
const maskedPhone = computed(() => {
    if (!props.author.phone) return '未公开';
    return props.author.phone.replace(/(\d{3})\d{4}(\d{4})/, '$1****$2');
});

function goAuthorHome() {
    router.push(`/author/${props.author.authorId}`);
}

onMounted(() => {
    fetchAuthorStats();
});
</script>

<style scoped>
.author-card {
    background: #fff;
    border-radius: 8px;
    padding: 20px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    transition: transform 0.3s;
    width: 340px;
}

.author-card:hover {
    transform: translateY(-5px);
}

.author-info {
    display: flex;
    align-items: center;
    margin-bottom: 15px;
}

.avatar {
    width: 60px;
    height: 60px;
    border-radius: 50%;
    object-fit: cover;
    margin-right: 15px;
    border: 2px solid #ffd100;
    cursor: pointer;
    transition: transform 0.3s ease;
}

.avatar:hover {
    transform: scale(1.1);
}

.details h3 {
    margin: 0 0 5px 0;
    font-size: 18px;
    color: #333;
    cursor: pointer;
    transition: color 0.3s ease;
}

.details h3:hover {
    color: #f0940a;
}

.meta {
    margin: 0;
    font-size: 12px;
    color: #888;
}

.stats {
    display: flex;
    justify-content: space-between;
    margin-bottom: 15px;
    padding: 10px 0;
    border-top: 1px solid #eee;
    border-bottom: 1px solid #eee;
}

.stat-item {
    text-align: center;
    flex: 1;
}

.stat-value {
    display: block;
    font-size: 16px;
    font-weight: bold;
    color: #333;
    margin-bottom: 4px;
}

.stat-label {
    font-size: 12px;
    color: #888;
}

.follow-btn {
    width: 100%;
    padding: 8px;
    background: #ffd100;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    font-size: 14px;
    transition: background 0.2s;
}

.follow-btn:hover {
    background: #f3a806;
    color: #1f1e1e;
}
</style>