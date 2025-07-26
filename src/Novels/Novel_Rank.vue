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
                    @click="selectRankingType(side)">
                    {{ side }}
                </button>
            </div>
            <div class="content">
                <div class="rank-header">
                    <span class="rank-title">{{ selectedSideTab }}</span>
                    <div class="sub-tabs">
                        <button v-for="t in rankType" :key="t" :class="{ active: t === selectedRankType }"
                            @click="changeRankLimit(t)">
                            {{ t }}
                        </button>
                    </div>
                </div>
                <div class="novel-list">
                    <template v-if="loading">
                        <div class="loading">加载中...</div>
                    </template>
                    <template v-else-if="!rankedNovels || rankedNovels.length === 0">
                        <div class="no-data">暂无数据</div>
                    </template>
                    <template v-else>
                        <template v-for="(novel, index) in rankedNovels" :key="novel.novelId">
                            <Novel_Card :novel="novel" :rank="index + 1" />
                            <hr v-if="index < rankedNovels.length - 1" class="novel-divider" />
                        </template>
                    </template>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import Novel_Card from './Novel_Card.vue'
import { getCollectRanking, getRecommendRanking, getScoreRanking } from '@/API/Ranking_API'

const tabs = ['全部', '已完结', '连载中']
const sideTabs = ['收藏榜', '推荐榜', '评分榜']
const rankType = ['前10榜', '前20榜']

const selectedTab = ref('全部')
const selectedSideTab = ref('收藏榜')
const selectedRankType = ref('前10榜')
const rankedNovels = ref([])
const loading = ref(false)

const rankLimit = computed(() => {
    return selectedRankType.value === '前10榜' ? 10 : 20
})

// 获取排行榜数据
async function fetchRankingData() {
    try {
        loading.value = true
        let response
        switch (selectedSideTab.value) {
            case '收藏榜':
                response = await getCollectRanking(rankLimit.value)
                break
            case '推荐榜':
                response = await getRecommendRanking(rankLimit.value)
                break
            case '评分榜':
                response = await getScoreRanking(rankLimit.value)
                break
            default:
                response = await getCollectRanking(rankLimit.value)
        }
        rankedNovels.value = Array.isArray(response) ? response : []
        if (selectedTab.value !== '全部') {
            const statusFilter = selectedTab.value === '已完结' ? '完结' : '连载'
            rankedNovels.value = rankedNovels.value.filter(novel => novel.status === statusFilter)
        }
    } catch (error) {
        console.error('获取排行榜数据失败:', error)
        rankedNovels.value = []
    } finally {
        loading.value = false
    }
}

function selectRankingType(type) {
    selectedSideTab.value = type
}
function changeRankLimit(type) {
    selectedRankType.value = type
}
watch([selectedSideTab, selectedRankType, selectedTab], () => {
    fetchRankingData()
})
onMounted(() => {
    fetchRankingData()
})
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

.sub-tabs {
    display: flex;
    align-items: center;
    margin-right: 150px;
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
    margin: 0.1rem 0;
}

.loading, .no-data {
    text-align: center;
    padding: 50px;
    font-size: 16px;
    color: #888;
}
</style>