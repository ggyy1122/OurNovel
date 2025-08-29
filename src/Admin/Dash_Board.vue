<template>
  <div class="dashboard">
    <header class="dashboard-header">
      <h2>数据概览</h2>
      <small class="muted">数据更新时间：{{ lastUpdated }}</small>
    </header>

    <section class="stats">
      <div
        class="stat-card"
        v-for="card in statCards"
        :key="card.key"
        :class="card.class"
      >
        <div class="stat-top">
          <h4>{{ card.title }}</h4>
          <small class="subtitle">{{ card.subtitle }}</small>
        </div>
        <div class="stat-value">{{ formatNumber(card.value) }}</div>
      </div>
    </section>

    <section class="lists">
      <div class="panel top-novels">
        <h3>平台最热收藏榜单</h3>
        <ol class="novel-list">
          <li v-for="n in topNovels" :key="n.novelId || n.id">
            <div class="title">{{ n.novelName ?? n.title ?? n.name ?? '未命名' }}</div>
            <div class="meta">收藏 {{ n.collectedCount ?? n.collects ?? n.collectCount ?? 0 }}</div>
          </li>
          <li v-if="topNovels.length === 0" class="muted">暂无数据</li>
        </ol>
      </div>
    </section>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import { getCollectRanking } from '@/API/Ranking_API'
import { getAllStatistics } from '@/API/Statistics_API'

export default {
  name: 'AdminDashboard',
  setup() {
    const lastUpdated = ref(new Date().toLocaleString())
    const topNovels = ref([])

    const stats = ref({
      totalNovels: 0,
      totalAuthors: 0,
      totalReaders: 0,
      pendingChapters: 0,
      pendingNovels: 0,
      pendingReports: 0
    })

    const statCards = computed(() => [
      { key: 'total-authors', title: '总作者数', subtitle: '平台注册作者', value: stats.value.totalAuthors, class: 'total-authors' },
      { key: 'total-readers', title: '总读者数', subtitle: '平台注册读者', value: stats.value.totalReaders, class: 'total-readers' },
      { key: 'total-novels', title: '总小说数', subtitle: '平台累计发布', value: stats.value.totalNovels, class: 'total-novels' },
      { key: 'pending-novels', title: '待审核小说数', subtitle: '待管理员审核', value: stats.value.pendingNovels, class: 'pending-novels' },
      { key: 'pending-chapters', title: '待审核章节数', subtitle: '作者提交待审章节', value: stats.value.pendingChapters, class: 'pending-chapters' },
      { key: 'pending-reports', title: '待处理举报数', subtitle: '平台安全管理', value: stats.value.pendingReports, class: 'pending-reports' }
    ])


    function formatNumber(n) {
      if (typeof n === 'number') return n >= 1000 ? (n / 1000).toFixed(1) + 'k' : String(n)
      return String(n ?? '')
    }

    async function loadStats() {
      try {
        const all = await getAllStatistics()
        stats.value.totalNovels = all.totalNovels || 0
        stats.value.totalAuthors = all.totalAuthors || 0
        stats.value.totalReaders = all.totalReaders || 0
        stats.value.pendingChapters = all.pendingChapters || 0
        stats.value.pendingNovels = all.pendingNovels || 0
        stats.value.pendingReports = all.pendingReports || 0

        lastUpdated.value = new Date().toLocaleString()
      } catch (err) {
        console.error('loadStats error', err)
      }
    }

    async function loadCollectTop() {
      try {
        const res = await getCollectRanking(10)
        let list = []
        if (!res) list = []
        else if (Array.isArray(res)) list = res
        else if (Array.isArray(res.data)) list = res.data
        else list = res.data || res || []
        if (!Array.isArray(list)) list = []
        topNovels.value = list.slice(0, 3).map(item => ({
          novelId: item.novelId ?? item.id,
          novelName: item.novelName ?? item.title ?? item.name,
          collectedCount: item.collectedCount ?? item.collects ?? item.collectCount ?? 0
        }))
      } catch (err) {
        console.error('loadCollectTop error', err)
        topNovels.value = []
      }
    }

    onMounted(() => {
      loadStats()
      loadCollectTop()
    })

    return {
      lastUpdated,
      statCards,
      topNovels,
      formatNumber
    }
  }
}
</script>


<style scoped>
.dashboard {
  padding: 24px;
  font-family: "Segoe UI", Roboto, "Helvetica Neue", Arial;
  color: #222;
  background-color: #fff;
}
.dashboard-header {
  display:flex;
  align-items:center;
  justify-content:space-between;
  margin-bottom: 18px;
}
.dashboard-header h2 { margin:0; font-size:30px; color:#1b2b5a }
.muted { color:#888; font-size:13px }

/* 六等分网格：一行固定 6 个卡片 */
.stats {
  display: grid;
  grid-template-columns: repeat(6, 1fr);
  gap: 18px;
  margin-top:8px;
}

/* 卡片基础样式：白底、细边框、圆角 */
.stat-card {
  background-color: #ffffff;
  border: 1px solid transparent;
  border-radius: 12px;
  padding: 18px;
  min-height: 96px;
  display:flex;
  flex-direction:column;
  justify-content:center;
  transition: transform 0.18s ease, box-shadow 0.18s ease;
  text-align: center;
}

/* 悬停上浮效果 */
.stat-card:hover {
  transform: translateY(-6px);
  box-shadow: 0 10px 24px rgba(0,0,0,0.12);
}

/* 彩色细边框 */
.stat-card.total-authors { border-color: #4CAF50; }
.stat-card.total-readers { border-color: #2196F3; }
.stat-card.total-novels { border-color: #FF9800; }
.stat-card.pending-novels { border-color: #9C27B0; }
.stat-card.pending-chapters { border-color: #E91E63; }
.stat-card.pending-reports { border-color: #F44336; }

/* 标题、子标题、数值 */
.stat-top h4 {
  margin: 0;
  font-size: 24px;
  font-weight: 600;
  color: #1b2b5a;
}

.subtitle {
  display:block;
  margin-top:6px;
  font-size: 13px;
  color: #666;
}

.stat-value {
  margin-top: 10px;
  font-size: 30px;   /* 大号字号 */
  font-weight: 600;  /* 去掉加粗，正常字重 */
  color: #111;
}

/* 列表 / 收藏榜 */
.lists { margin-top:20px }
.panel { background:#fff; padding:14px; border-radius:10px; border:1px solid #eee }
.panel h3 { margin:0 0 10px; color:#1b2b5a }
.novel-list { list-style:none; padding:0; margin:0 }
.novel-list li { padding:8px 0; border-bottom:1px dashed #eee }
.novel-list .title { font-weight:700 }
.novel-list .meta { font-size:13px; color:#666 }

/* 响应式 */
@media (max-width: 1200px) {
  .stats { grid-template-columns: repeat(3, 1fr); }
}
@media (max-width: 900px) {
  .stats { grid-template-columns: repeat(2, 1fr); }
}
@media (max-width: 600px) {
  .stats { grid-template-columns: repeat(1, 1fr); }
}
</style>