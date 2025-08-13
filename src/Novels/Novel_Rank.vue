<template>
    <div class="rankings">
        <img src="@/assets/rank.png" alt="排行榜" class="banner" />
        <div class="novel-category-wrapper">
            <img :src="backgroundImages[currentBgIndex]" alt="background" class="background-image" />
            <div class="rank-content">
                <div class="tabs">
                    <button v-for="tab in tabs" :key="tab" :class="{ active: tab === selectedTab }"
                        @click="selectedTab = tab">
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
                                <template v-for="(novel, index) in paginatedNovels" :key="novel.novelId">
                                    <Novel_Card :novel="novel" :rank="(currentPage - 1) * pageSize + index + 1" />
                                    <hr v-if="index < paginatedNovels.length - 1" class="novel-divider" />
                                </template>
                            </template>
                        </div>
                        <!-- 分页控件 -->
                        <div v-if="rankedNovels.length > 0" class="pagination">
                            <button :disabled="currentPage === 1" @click="changePage(currentPage - 1)" class="page-btn">
                                上一页
                            </button>
                            <template v-for="page in visiblePages" :key="page">
                                <button :class="['page-btn', currentPage === page ? 'active' : '']"
                                    @click="changePage(page)">
                                    {{ page }}
                                </button>
                            </template>
                            <button :disabled="currentPage === totalPages" @click="changePage(currentPage + 1)"
                                class="page-btn">
                                下一页
                            </button>
                            <div class="page-jump">
                                <span>跳转至</span>
                                <input type="number" v-model.number="jumpPage" min="1" :max="totalPages"
                                    @keyup.enter="jumpToPage">
                                <span>/ {{ totalPages }} 页</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted, watch } from 'vue'
import Novel_Card from './Novel_Card.vue'
import { getCollectRanking, getRecommendRanking, getScoreRanking } from '@/API/Ranking_API'
import { useRoute } from 'vue-router'

const route = useRoute()

const tabs = ['全部', '已完结', '连载中']
const sideTabs = ['收藏榜', '推荐榜', '评分榜']
const rankType = ['前10榜', '前20榜']

// 分页相关状态
const currentPage = ref(1)
const pageSize = ref(10) // 每页显示10条
const jumpPage = ref(1)

const selectedTab = ref('全部')
const selectedSideTab = ref(route.query.type || '收藏榜')
const selectedRankType = ref('前10榜')
const rankedNovels = ref([])
const loading = ref(false)

const rankLimit = computed(() => {
    return selectedRankType.value === '前10榜' ? 10 : 20
})

// 计算属性
const totalPages = computed(() => Math.ceil(rankedNovels.value.length / pageSize.value))
const paginatedNovels = computed(() => {
    const start = (currentPage.value - 1) * pageSize.value
    const end = start + pageSize.value
    return rankedNovels.value.slice(start, end)
})
const visiblePages = computed(() => {
    const maxVisible = 5 // 最多显示5个页码
    const half = Math.floor(maxVisible / 2)
    let start = Math.max(1, currentPage.value - half)
    let end = Math.min(totalPages.value, start + maxVisible - 1)

    if (end - start + 1 < maxVisible) {
        start = Math.max(1, end - maxVisible + 1)
    }

    return Array.from({ length: end - start + 1 }, (_, i) => start + i)
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
        // 过滤掉"待审核"和"封禁"状态的小说
        rankedNovels.value = Array.isArray(response)
            ? response.filter(novel => novel.status === '连载' || novel.status === '完结')
            : []
        if (selectedTab.value !== '全部') {
            const statusFilter = selectedTab.value === '已完结' ? '完结' : '连载'
            rankedNovels.value = rankedNovels.value.filter(novel => novel.status === statusFilter)
        }
        // 重置到第一页
        currentPage.value = 1
        jumpPage.value = 1
    } catch (error) {
        console.error('获取排行榜数据失败:', error)
        rankedNovels.value = []
    } finally {
        loading.value = false
    }
}

// 分页相关方法
function changePage(page) {
    if (page < 1 || page > totalPages.value) return
    currentPage.value = page
    jumpPage.value = page
    window.scrollTo({ top: 0, behavior: 'smooth' })
}

function jumpToPage() {
    const page = Math.max(1, Math.min(jumpPage.value, totalPages.value))
    changePage(page)
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

// 背景图轮播相关
const backgroundImages = [
    require('@/assets/bac1.jpg'),
    require('@/assets/bac2.jpg'),
    require('@/assets/bac3.jpg'),
    require('@/assets/bac4.jpg')
]
const currentBgIndex = ref(0)

const scrollToTop = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
};

// 轮播背景图
let bgInterval
onMounted(() => {
    fetchRankingData()
    scrollToTop()
    // 启动背景轮播
    bgInterval = setInterval(() => {
        currentBgIndex.value = (currentBgIndex.value + 1) % backgroundImages.length
    }, 3500)
})

// 组件卸载时清除定时器
onUnmounted(() => {
    clearInterval(bgInterval)
})
</script>

<style scoped>
.rankings {
    background-color: #fdfafd;
    font-family: "PingFang SC", "Microsoft YaHei", Arial, sans-serif;
}

.banner {
    width: 100%;
    object-fit: cover;
    margin-bottom: 20px;
}

.novel-category-wrapper {
    position: relative;
    min-height: 100vh;
}

.background-image {
    position: absolute;
    top: -10px;
    left: 0;
    width: 100%;
    object-fit: cover;
    filter: blur(0.5px) brightness(0.9);
    z-index: 0;
    mask-image: linear-gradient(to bottom, black 30%, rgba(0, 0, 0, 0.7) 60%, transparent 100%);
    -webkit-mask-image: linear-gradient(to bottom, black 30%, rgba(0, 0, 0, 0.7) 60%, transparent 100%);
    transition: opacity 1s ease-in-out;
}

.rank-content {
    position: relative;
    z-index: 1;
}

.tabs {
    display: flex;
    justify-content: center;
    margin-bottom: 24px;
}

.tabs button {
    background: rgba(244, 242, 242, 0.6);
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
    background: rgba(244, 242, 242, 0.6);
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

.loading,
.no-data {
    text-align: center;
    padding: 50px;
    font-size: 16px;
    color: #888;
}

/* 分页样式 */
.pagination {
    display: flex;
    justify-content: center;
    align-items: center;
    margin-top: 30px;
    gap: 8px;
    flex-wrap: wrap;
    margin-bottom: 45px;
}

.page-btn {
    padding: 6px 12px;
    border: 1px solid #ddd;
    background: #fff;
    color: #333;
    border-radius: 4px;
    cursor: pointer;
    transition: all 0.2s;
}

.page-btn:hover:not(:disabled) {
    background: #f5f5f5;
}

.page-btn.active {
    background: #ffe357;
    color: #fff;
    border-color: #ffe357;
    font-weight: bold;
}

.page-btn:disabled {
    opacity: 0.5;
    cursor: not-allowed;
}

.page-jump {
    margin-left: 15px;
    display: flex;
    align-items: center;
    gap: 8px;
}

.page-jump input {
    width: 50px;
    padding: 6px;
    border: 1px solid #ddd;
    border-radius: 4px;
    text-align: center;
}
</style>