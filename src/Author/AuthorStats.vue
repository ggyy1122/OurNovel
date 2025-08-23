<template>
  <div class="stats-page">
    <!-- 头部标题 -->
    <div class="page-header">
      <h1>小说数据统计中心</h1>
      <div class="header-stats">
        <div class="stat-item">
          <span class="stat-label">作品总数</span>
          <span class="stat-value">{{ novels.length }}</span>
        </div>
        <div class="stat-item">
          <span class="stat-label">总字数</span>
          <span class="stat-value">{{ formatNumber(totalWordCount) }}</span>
        </div>
        <div class="stat-item">
          <span class="stat-label">平均评分</span>
          <span class="stat-value">{{ averageScore.toFixed(1) }}</span>
        </div>
        <div class="stat-item">
          <span class="stat-label">总推荐数</span>
          <span class="stat-value">{{ formatNumber(totalRecommendCount) }}</span>
        </div>
        <div class="stat-item">
          <span class="stat-label">总收藏数</span>
          <span class="stat-value">{{ formatNumber(totalCollectedCount) }}</span>
        </div>
      </div>
    </div>

    <!-- 主要统计卡片 -->
    <div class="main-stats-grid">
      <div class="stat-card">
        <div class="card-header">
          <h3>作品状态分布</h3>
        </div>
        <div class="card-content">
          <div ref="statusChart" style="height: 250px;"></div>
        </div>
      </div>

      <div class="stat-card">
        <div class="card-header">
          <h3>评分分布</h3>
        </div>
        <div class="card-content">
          <div ref="scoreChart" style="height: 250px;"></div>
        </div>
      </div>

      <div class="stat-card">
        <div class="card-header">
          <h3>字数分布</h3>
        </div>
        <div class="card-content">
          <div ref="wordCountChart" style="height: 250px;"></div>
        </div>
      </div>

      <div class="stat-card">
        <div class="card-header">
          <h3>详细数据统计</h3>
        </div>
        <div class="card-content">
          <div class="detailed-stats-grid">
            <div class="metric-card">
              <div class="metric-value">{{ oldestNovelDate }}</div>
              <div class="metric-label">首部小说日期</div>
            </div>
            
            <div class="metric-card">
              <div class="metric-value">{{ highScoreCount }}</div>
              <div class="metric-label">高分作品(≥8分)</div>
            </div>
            
            <div class="metric-card">
              <div class="metric-value">{{ highRecommendCount }}</div>
              <div class="metric-label">高推荐作品(≥100)</div>
            </div>
            
            <div class="metric-card">
              <div class="metric-value">{{ highCollectedCount }}</div>
              <div class="metric-label">高收藏作品(≥100)</div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- 小说数据表格 -->
    <div class="novels-table-section">
      <h2>所有小说数据</h2>
      <div class="table-container">
        <table class="novels-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>名称</th>
              <th>状态</th>
              <th>字数</th>
              <th>评分</th>
              <th>推荐数</th>
              <th>收藏数</th>
              <th>全本总价</th>
              <th>创建时间</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="novel in novels" :key="novel.novel_id">
              <td>{{ novel.novel_id }}</td>
              <td>{{ novel.novel_name }}</td>
              <td>{{ novel.status }}</td>
              <td>{{ formatNumber(novel.total_word_count) }}</td>
              <td>{{ novel.score.toFixed(1) }}</td>
              <td>{{ formatNumber(novel.recommend_count) }}</td>
              <td>{{ formatNumber(novel.collected_count) }}</td>
              <td>{{ novel.total_price }}</td>
              <td>{{ formatDate(novel.create_time) }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import { novelsStore } from '@/stores/Novels'
import * as echarts from 'echarts'

export default {
  setup() {
    const statusChart = ref(null)
    const scoreChart = ref(null)
    const wordCountChart = ref(null)
    const loading = ref(false)

    // 获取小说数据
    const fetchNovels = async () => {
      loading.value = true
      try {
        await novelsStore.fetchNovels()
      } catch (error) {
        console.error('加载小说数据失败:', error)
      } finally {
        loading.value = false
      }
    }

    // 计算属性
    const novels = computed(() => [...novelsStore.novels])
    const totalWordCount = computed(() => novels.value.reduce((sum, novel) => sum + novel.total_word_count, 0))
    const statusCount = computed(() => {
      const count = {}
      novels.value.forEach(novel => {
        count[novel.status] = (count[novel.status] || 0) + 1
      })
      return count
    })
    const averageScore = computed(() => {
      if (novels.value.length === 0) return 0
      const total = novels.value.reduce((sum, novel) => sum + novel.score, 0)
      return total / novels.value.length
    })
    const totalRecommendCount = computed(() => novels.value.reduce((sum, novel) => sum + novel.recommend_count, 0))
    const totalCollectedCount = computed(() => novels.value.reduce((sum, novel) => sum + novel.collected_count, 0))
    const highScoreCount = computed(() => novels.value.filter(novel => novel.score >= 8).length)
    const highRecommendCount = computed(() => novels.value.filter(novel => novel.recommend_count >= 100).length)
    const highCollectedCount = computed(() => novels.value.filter(novel => novel.collected_count >= 100).length)
    const oldestNovelDate = computed(() => {
      if (novels.value.length === 0) return '无数据'
      const dates = novels.value.map(novel => new Date(novel.create_time))
      const oldest = new Date(Math.min(...dates))
      return oldest.toLocaleDateString('zh-CN')
    })

    // 格式化时间
    const formatDate = (dateString) => {
      if (!dateString) return '未知'
      const date = new Date(dateString)
      return date.toLocaleString('zh-CN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit',
        hour12: false
      })
    }
    //格式化数字
    const formatNumber = (num) => {
      return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')
    }
    //获取状态颜色
    const getStatusColor = (status) => {
      const colorMap = {
        '连载': '#3498db',
        '完结': '#2ecc71',
        '待审核': '#f39c12',
        '封禁':'#e74c3c'
      }
      return colorMap[status] || '#6c757d'
    }
    // 初始化图表
    const initCharts = () => {
      initStatusChart()
      initScoreChart()
      initWordCountChart()
    }

    const initStatusChart = () => {
      const chart = echarts.init(statusChart.value)
      const option = {
        tooltip: {
          trigger: 'item',
          formatter: '{a} <br/>{b}: {c} ({d}%)'
        },
        legend: {
          orient: 'vertical',
          right: 10,
          top: 'center',
          data: Object.keys(statusCount.value)
        },
        series: [{
          name: '作品状态',
          type: 'pie',
          radius: ['40%', '70%'],
          avoidLabelOverlap: false,
          itemStyle: {
            borderRadius: 10,
            borderColor: '#fff',
            borderWidth: 2
          },
          label: {
            show: false,
            position: 'center'
          },
          emphasis: {
            label: {
              show: true,
              fontSize: '18',
              fontWeight: 'bold'
            }
          },
          labelLine : {
            show: false
          },
          data: Object.entries(statusCount.value).map(([name, value]) => ({
            value,
            name,
            itemStyle: {
              color: getStatusColor(name)
            }
          }))
        }]
      }
      chart.setOption(option)
    }

    const initScoreChart = () => {
      const chart = echarts.init(scoreChart.value)
      
      // 计算评分分布
      const scoreRanges = [
        { min: 0, max: 2, label: '0-2分' },
        { min: 2, max: 4, label: '2-4分' },
        { min: 4, max: 6, label: '4-6分' },
        { min: 6, max: 8, label: '6-8分' },
        { min: 8, max: Infinity, label: '8-10分' }
      ]
      
      const scoreData = scoreRanges.map(range => {
        const count = novels.value.filter(novel => 
          novel.score >= range.min && novel.score < range.max
        ).length
        return {
          value: count,
          name: range.label
        }
      })
      
      const option = {
        tooltip: {
          trigger: 'axis',
          axisPointer: {
            type: 'shadow'
          }
        },
        xAxis: {
          type: 'category',
          data: scoreData.map(item => item.name),
          axisLabel: {
            rotate: 0
          }
        },
        yAxis: {
          type: 'value',
          name: '作品数量'
        },
        series: [{
          name: '评分分布',
          type: 'bar',
          data: scoreData,
          itemStyle: {
            color: function(params) {
              const colorList = ['#FF6B6B', '#FFD166', '#06D6A0', '#118AB2', '#073B4C']
              return colorList[params.dataIndex]
            }
          }
        }]
      }
      chart.setOption(option)
    }

    const initWordCountChart = () => {
      const chart = echarts.init(wordCountChart.value)
      
      // 计算字数分布
      const wordCountRanges = [
        { min: 0, max: 50000, label: '0-5万' },
        { min: 50000, max: 100000, label: '5-10万' },
        { min: 100000, max: 200000, label: '10-20万' },
        { min: 200000, max: 500000, label: '20-50万' },
        { min: 500000, max: Infinity, label: '50万+' }
      ]
      
      const wordCountData = wordCountRanges.map(range => {
        const count = novels.value.filter(novel => 
          novel.total_word_count >= range.min && novel.total_word_count < range.max
        ).length
        return {
          value: count,
          name: range.label
        }
      })
      
      const option = {
        tooltip: {
          trigger: 'item',
          formatter: '{b}: {c} ({d}%)'
        },
        series: [{
          name: '字数分布',
          type: 'pie',
          radius: ['50%', '70%'],
          data: wordCountData,
          itemStyle: {
            borderRadius: 10,
            borderColor: '#fff',
            borderWidth: 2
          },
          label: {
            show: true,
            formatter: '{b}: {c}'
          },
          labelLine: {
            show: true
          }
        }]
      }
      chart.setOption(option)
    }

    
    onMounted(async () => {
      await fetchNovels()
      initCharts()
    })

    return {
      statusChart,
      scoreChart,
      wordCountChart,
      novels,
      totalWordCount,
      statusCount,
      averageScore,
      totalRecommendCount,
      totalCollectedCount,
      highScoreCount,
      highRecommendCount,
      highCollectedCount,
      oldestNovelDate,
      formatNumber,
      formatDate
    }
  }
}
</script>

<style scoped>
/* 基础样式 */
:root {
  --primary-color: #4361ee;
  --secondary-color: #3f37c9;
  --success-color: #4cc9f0;
  --warning-color: #f8961e;
  --danger-color: #f94144;
  --light-bg: #f8f9fa;
  --text-dark: #212529;
  --text-gray: #6c757d;
}

.stats-page {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
  font-family: 'Segoe UI', system-ui, sans-serif;
}

/* 头部样式 */
.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
  padding-bottom: 20px;
  border-bottom: 1px solid rgba(0,0,0,0.1);
}

.page-header h1 {
  font-size: 28px;
  font-weight: 600;
  color: var(--text-dark);
  margin: 0;
}

.header-stats {
  display: flex;
  gap: 30px;
}

.stat-item {
  text-align: center;
}

.stat-label {
  display: block;
  font-size: 14px;
  color: var(--text-gray);
  margin-bottom: 5px;
}

.stat-value {
  font-size: 24px;
  font-weight: 700;
  color: var(--primary-color);
}

/* 主要统计卡片网格 */
.main-stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 20px;
  margin-bottom: 30px;
}

.stat-card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.05);
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.stat-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 20px rgba(0,0,0,0.1);
}

.card-header {
  padding: 15px 20px;
  border-bottom: 1px solid rgba(0,0,0,0.05);
}

.card-header h3 {
  margin: 0;
  font-size: 18px;
  color: var(--text-dark);
}

.card-content {
  padding: 15px;
}

/* 详细统计网格 */
.detailed-stats-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 15px;
}

.metric-card {
  background: rgba(248,249,250,0.7);
  border-radius: 8px;
  padding: 20px;
  text-align: center;
  transition: all 0.3s ease;
}

.metric-card:hover {
  background: rgba(248,249,250,1);
  transform: translateY(-3px);
}

.metric-value {
  font-size: 28px;
  font-weight: 700;
  color: var(--primary-color);
  margin-bottom: 5px;
}

.metric-label {
  font-size: 14px;
  color: var(--text-gray);
}

/* 小说表格区域 */
.novels-table-section {
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.05);
  padding: 20px;
  margin-bottom: 30px;
}

.novels-table-section h2 {
  font-size: 20px;
  color: var(--text-dark);
  margin-bottom: 20px;
  padding-bottom: 10px;
  border-bottom: 1px solid rgba(0,0,0,0.05);
}

.table-container {
  overflow-x: auto;
}

.novels-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 15px;
}

.novels-table th,
.novels-table td {
  padding: 12px 15px;
  text-align: left;
  border-bottom: 1px solid #eee;
}

.novels-table th {
  background-color: #f8f9fa;
  font-weight: 600;
  color: var(--text-dark);
}

.novels-table tr:hover {
  background-color: #f8f9fa;
}

</style>