<template>
    <div class="rankings">
        <img src="@/assets/rank.png" alt="排行榜" class="banner" />
        <div class="tabs">
            <button v-for="tab in tabs" :key="tab" :class="{ active: tab === selectedTab }" @click="selectedTab = tab">
                {{ tab }}
            </button>
        </div>
        <div class="main">
            <div class="sidebar">
                <div class="sidebar-title">热门作品排行</div>
                <button v-for="side in sideTabs" :key="side" :class="{ active: side === selectedSideTab }"
                    @click="selectedSideTab = side">
                    {{ side }}
                </button>
            </div>
            <div class="content">
                <div class="rank-header">
                    <span class="rank-title">{{ selectedSideTab }}</span>
                    <div class="sub-tabs">
                        <button v-for="t in rankType" :key="t" :class="{ active: t === selectedRankType }"
                            @click="selectedRankType = t">
                            {{ t }}
                        </button>
                    </div>
                </div>
                <div class="novel-list">
                    <template v-for="(novel, idx) in filteredNovels" :key="novel.id">
                        <Novel_Card :novel="novel" :rank="idx + 1" />
                        <hr v-if="idx < filteredNovels.length - 1" class="novel-divider" />
                    </template>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import Novel_Card from './Novel_Card.vue'

const tabs = ['男生榜', '女生榜', '出生榜']
const sideTabs = ['大热榜', '新书榜', '完结榜', '收藏榜', '更新榜']
const rankType = ['日榜', '月榜']

const selectedTab = ref('女生榜')
const selectedSideTab = ref('大热榜')
const selectedRankType = ref('日榜')

// 写死的小说数据
const novels = [
    {
        id: 1,
        tab: '女生榜',
        side: '大热榜',
        type: '日榜',
        title: '封总，太太想跟你离婚很久了',
        author: '云中网',
        category: '现代言情',
        status: '总裁豪门',
        wordCount: '53.28万字',
        desc: '结婚七年，封庭深待她冷淡如对，容辞一直微笑面对。因为她深爱着他，也相信终有一天，她能抚他的心结疙。可她等来的却是他对另一个女人的一见钟情，呵护备至。她...',
        update: '最新更新 第325章再用失邀请函 2025-07-13 01:00:05',
        hot: '339.9万',
        badge: '蝉联 榜首',
        cover: require('@/assets/novel_cover.jpg')
    },
    {
        id: 2,
        tab: '女生榜',
        side: '大热榜',
        type: '日榜',
        title: '皇叔借点功德，王妃把你画猛了',
        author: '安卿吖',
        category: '古代言情',
        status: '传奇爆款',
        wordCount: '42.87万字',
        desc: '《奔跑吧》同款！又名《皇妃有令》《护妻狂魔+团宠+爽文》白莲花妹妹抢了她的婚约，还把她送去给一纨绔当玩物。她终于爬上了皇叔的马车，被他一身功德闪瞎了眼...',
        update: '最新更新 第1336章等轩辕宫 2025-07-13 00:30:12',
        hot: '197.5万',
        badge: '',
        cover: require('@/assets/novel_cover.jpg')
    },
    {
        id: 3,
        tab: '女生榜',
        side: '大热榜',
        type: '日榜',
        title: '皇叔借点功德，王妃把你画猛了',
        author: '安卿吖',
        category: '古代言情',
        status: '传奇爆款',
        wordCount: '42.87万字',
        desc: '《奔跑吧》同款！又名《皇妃有令》《护妻狂魔+团宠+爽文》白莲花妹妹抢了她的婚约，还把她送去给一纨绔当玩物。她终于爬上了皇叔的马车，被他一身功德闪瞎了眼...',
        update: '最新更新 第1336章等轩辕宫 2025-07-13 00:30:12',
        hot: '197.5万',
        badge: '',
        cover: require('@/assets/novel_cover.jpg')
    },
    {
        id: 4,
        tab: '女生榜',
        side: '大热榜',
        type: '日榜',
        title: '皇叔借点功德，王妃把你画猛了',
        author: '安卿吖',
        category: '古代言情',
        status: '传奇爆款',
        wordCount: '42.87万字',
        desc: '《奔跑吧》同款！又名《皇妃有令》《护妻狂魔+团宠+爽文》白莲花妹妹抢了她的婚约，还把她送去给一纨绔当玩物。她终于爬上了皇叔的马车，被他一身功德闪瞎了眼...',
        update: '最新更新 第1336章等轩辕宫 2025-07-13 00:30:12',
        hot: '197.5万',
        badge: '',
        cover: require('@/assets/novel_cover.jpg')
    }
    // 更多小说...
]

// 过滤展示的小说
const filteredNovels = computed(() =>
    novels.filter(n =>
        n.tab === selectedTab.value &&
        n.side === selectedSideTab.value &&
        n.type === selectedRankType.value
    )
)
</script>

<style scoped>
.rankings {
    background: #fff;
    font-family: "PingFang SC", "Microsoft YaHei", Arial, sans-serif;
}

.banner {
    width: 100%;
    object-fit: cover;
    margin-bottom: 20px;
}

.tabs {
    display: flex;
    justify-content: center;
    margin-bottom: 24px;
}

.tabs button {
    background: #f4f2f2;
    border: none;
    padding: 10px 34px;
    font-size: 18px;
    font-weight: bold;
    color: #333;
    border-radius: 28px;
    margin: 0 10px;
    cursor: pointer;
    transition: background 0.2s;
}

.tabs button:hover {
    color: #e47f0d;
}

.tabs button.active {
    background: #ffe357;
    color: #333;
}

.main {
    display: flex;
    justify-content: center;
    gap: 40px;
}

.sidebar {
    background: #fafafa;
    border-radius: 8px;
    padding: 16px 0;
    min-width: 140px;
    display: flex;
    flex-direction: column;
    gap: 6px;
    margin-left: 30px;
}

.sidebar-title {
    font-size: 20px;
    font-weight: bold;
    color: #222;
    padding: 8px 22px 10px 22px;
}

.sidebar button {
    background: none;
    border: none;
    padding: 10px 22px;
    font-size: 18px;
    color: #333;
    text-align: left;
    cursor: pointer;
    border-radius: 6px;
    transition: background 0.18s;
}

.sidebar button:hover {
    background-color: #f0deb8;
    color: #e47f0d;
}

.sidebar button.active {
    background: #ffe357;
    color: #f59906;
}

.content {
    flex: 1;
    min-width: 600px;
}

.rank-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 22px;
    background: #f4f2f2;
}

.rank-title {
    font-size: 24px;
    font-weight: bold;
    color: #222;
    margin-left: 10px;
}

.sub-tabs button {
    background: none;
    border: none;
    font-size: 18px;
    color: #333;
    margin-left: 10px;
    padding: 6px 18px;
    border-radius: 14px;
    cursor: pointer;
    transition: background 0.18s;
}

.sub-tabs button:hover {
    color: #e47f0d;
}

.sub-tabs button.active {
    background: #ffe357;
    color: #f29705;
}

.novel-list {
    display: flex;
    flex-direction: column;
}

.novel-divider {
    border: none;
    border-top: 1px solid #b2b6bb;
    margin: 1.5rem 0;
}
</style>