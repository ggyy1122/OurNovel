<template>
    <div class="reader-card">
        <div class="reader-info">
            <img :src="'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' + (reader.avatarUrl || '07850080-e498-47a4-8d3a-fd94fb47e561.jpg')"
                alt="读者头像" class="avatar" />
            <div class="details">
                <h3>{{ reader.readerName }}</h3>
                <p class="meta">性别: {{ reader.gender || '未设置' }}</p>
                <p class="meta">联系方式: {{ maskedPhone }}</p>
            </div>
        </div>
        <div class="stats">
            <div class="stat-item">
                <span class="stat-value">{{ reader.isCollectVisible === '是' ? '公开' : '私密' }}</span>
                <span class="stat-label">收藏状态</span>
            </div>
            <div class="stat-item">
                <span class="stat-value">{{ reader.isRecommendVisible === '是' ? '公开' : '私密' }}</span>
                <span class="stat-label">推荐状态</span>
            </div>
            <div class="stat-item">
                <span class="stat-value">¥{{ reader.balance.toFixed(2) }}</span>
                <span class="stat-label">账户余额</span>
            </div>
        </div>
        <button class="follow-btn" @click="handle_follow">关注</button>
    </div>
</template>

<script setup>
import { defineProps, computed } from 'vue';
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";

const props = defineProps({
    reader: {
        type: Object,
        required: true
    }
});

const maskedPhone = computed(() => {
    if (!props.reader.phone) return '未公开';
    return props.reader.phone.replace(/(\d{3})\d{4}(\d{4})/, '$1****$2');
});

async function handle_follow() {
    toast("我是" + props.reader.readerName + "，我不同意你关注！", {
        "type": "error",
        "dangerouslyHTMLString": true
    })
}
</script>

<style scoped>
.reader-card {
    background: #fff;
    border-radius: 8px;
    padding: 20px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    transition: transform 0.3s;
    width: 340px;
}

.reader-card:hover {
    transform: translateY(-5px);
}

.reader-info {
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
}

.details h3 {
    margin: 0 0 5px 0;
    font-size: 18px;
    color: #333;
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
    background: #ffea80;
}
</style>