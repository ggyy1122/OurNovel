<template>
  <div class="income-page">
    <!-- 头部标题 -->
    <div class="page-header">
      <h1>作者收益中心</h1>
      <div class="total-income">
        <span class="label">累计收益：</span>
        <span class="value">{{ formatCurrency(totalIncome) }} &nbsp;&nbsp;&nbsp;</span>
        <span class="label">合人民币：</span>
        <span class="value">¥{{totalIncome/100}}</span>
      </div>
    </div>

    <!-- 时期收益展示 -->
    <div class="filter-section">
      <div class="quick-stats">
        <div class="quick-stat">
          <span class="stat-label">今日收益</span>
          <span class="stat-value">{{ formatCurrency(todayIncome) }}</span>
        </div>
        <div class="quick-stat">
          <span class="stat-label">近7天收益</span>
          <span class="stat-value">{{ formatCurrency(last7DaysIncome) }}</span>
        </div>
        <div class="quick-stat">
          <span class="stat-label">近30天收益</span>
          <span class="stat-value">{{ formatCurrency(last30DaysIncome) }}</span>
        </div>
      </div>
    </div>

    <!-- 收入趋势图 -->
    <div class="chart-container">
      <h2>收益趋势</h2>
      <div ref="chart" style="height: 300px;"></div>
    </div>

    <!-- 筛选框 -->
    <div class="filter-section">
      <div class="filter-row">
        <div class="filter-group">
          <label for="date-range">时间范围：</label>
          <select id="date-range" v-model="selectedRange">
            <option v-for="range in timeRanges" :key="range.value" :value="range.value">
              {{ range.label }}
            </option>
          </select>
        </div>
        
        <div class="filter-group">
          <label for="income-type">收入类型：</label>
          <select id="income-type" v-model="filterType">
            <option value="all">全部类型</option>
            <option v-for="type in incomeTypes" :key="type" :value="type">{{ type }}</option>
          </select>
        </div>
        
        <div class="filter-group">
          <label for="novel-filter">筛选作品：</label>
          <select id="novel-filter" v-model="filterNovelId">
            <option value="all">全部作品</option>
            <option 
              v-for="novel in novelsFromStore" 
              :key="novel.novel_id" 
              :value="novel.novel_id"
            >
              {{ novel.novel_name }} (ID: {{ novel.novel_id }})
            </option>
          </select>
        </div>
        
        <div class="filter-group">
          <label for="sort-by">排序方式：</label>
          <select id="sort-by" v-model="sortBy">
            <option value="time-desc">时间 (最新)</option>
            <option value="time-asc">时间 (最早)</option>
            <option value="amount-desc">金额 (高→低)</option>
            <option value="amount-asc">金额 (低→高)</option>
          </select>
        </div>
      </div>
    </div>
    
    <!-- 收入明细表格 -->
    <div class="income-table-container">
      <div class="table-header">
        <h2>收入明细</h2>
        <div class="table-summary">
          共 {{ filteredIncomes.length }} 条记录，合计 {{ formatCurrency(filteredTotal) }}
        </div>
      </div>
      
      <div class="table-wrapper">
        <table class="income-table">
          <thead>
            <tr>
              <th style="width: 100px">记录ID</th>
              <th style="width: 120px">收入类型</th>
              <th style="width: 150px">金额</th>
              <th style="width: 200px">时间</th>
              <th style="width: 200px">来源作品</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="loading">
              <td colspan="5" class="loading-cell">
                <div class="loading-spinner"></div>
                加载中...
              </td>
            </tr>
            <tr v-else-if="pagedIncomes.length === 0">
              <td colspan="5" class="empty-cell">暂无收入记录</td>
            </tr>
            <tr v-for="income in pagedIncomes" :key="income.id">
              <td>{{ income.id }}</td>
              <td>{{ income.type }}</td>
              <td>{{ formatCurrency(income.amount) }}</td>
              <td>{{ formatDateTime(income.createTime) }}</td>
              <td>{{ getNovelName(income.novelId) }} (ID: {{ income.novelId || '未知' }})</td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- 分页控件 -->
      <div class="pagination">
        <button 
          @click="prevPage" 
          :disabled="currentPage === 1"
          class="page-btn"
        >
          上一页
        </button>
        <span class="page-info">
          第 {{ currentPage }} 页 / 共 {{ totalPages }} 页
        </span>
        <button 
          @click="nextPage" 
          :disabled="currentPage >= totalPages"
          class="page-btn"
        >
          下一页
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { getAuthorIncomes, getAuthorTotalIncome } from '@/API/AuthorIncome_API'
import { novelsStore } from '@/stores/Novels'
import { authorStore } from '@/stores/CurrentAuthor'
import * as echarts from 'echarts'

export default {
  data() {
    return {
      authorId: authorStore.currentAuthor.author_id,
      incomes: [],
      totalIncome: 0,
      loading: true,
      filterType: 'all',
      filterNovelId: 'all',
      incomeTypes: ['打赏', '章节购买', '整本买断', '分成'],
      novelOptions: [],
      novelsFromStore: [],
      novelNameMap: new Map(),
      currentPage: 1,
      pageSize: 10,
      chart: null,
      timeRanges: [
        { value: 'today', label: '今日' },
        { value: '7days', label: '近7天' },
        { value: '30days', label: '近30天' },
        { value: '90days', label: '近90天' },
        { value: '180days', label: '近180天' },
        { value: '365days', label: '近一年' },
        { value: 'all', label: '全部' }
      ],
      selectedRange: '30days',
      sortBy: 'time-desc'
    }
  },
  computed: {
    //收入计算
    todayIncome() {
      const todayStart = new Date()
      todayStart.setHours(0, 0, 0, 0)
      const todayEnd = new Date(todayStart)
      todayEnd.setDate(todayStart.getDate() + 1)
      
      return this.incomes
        .filter(income => {
          const incomeDate = new Date(income.createTime)
          return incomeDate >= todayStart && incomeDate < todayEnd
        })
        .reduce((sum, income) => sum + income.amount, 0)
    },
    last7DaysIncome() {
      const now = new Date()
      const sevenDaysAgo = new Date(now)
      sevenDaysAgo.setDate(now.getDate() - 7)
      
      return this.incomes
        .filter(income => {
          const incomeDate = new Date(income.createTime)
          return incomeDate >= sevenDaysAgo && incomeDate <= now
        })
        .reduce((sum, income) => sum + income.amount, 0)
    },
    last30DaysIncome() {
      const now = new Date()
      const thirtyDaysAgo = new Date(now)
      thirtyDaysAgo.setDate(now.getDate() - 30)
      
      return this.incomes
        .filter(income => {
          const incomeDate = new Date(income.createTime)
          return incomeDate >= thirtyDaysAgo && incomeDate <= now
        })
        .reduce((sum, income) => sum + income.amount, 0)
    },
    //筛选功能
    filteredIncomes() {
      let filtered = [...this.incomes]
      
      // 按类型筛选
      if (this.filterType !== 'all') {
        filtered = filtered.filter(income => income.type === this.filterType)
      }
      
      // 按小说筛选
      if (this.filterNovelId !== 'all') {
        filtered = filtered.filter(income => income.novelId === this.filterNovelId)
      }
      
      // 按时间范围筛选
      if (this.selectedRange !== 'all') {
        const now = new Date()
        filtered = filtered.filter(income => {
          const incomeDate = new Date(income.createTime)
          const daysAgo = new Date(now)
          
          switch (this.selectedRange) {
            case 'today': {
              const todayStart = new Date(now)
              todayStart.setHours(0, 0, 0, 0)
              const todayEnd = new Date(todayStart)
              todayEnd.setDate(todayStart.getDate() + 1)
              return incomeDate >= todayStart && incomeDate < todayEnd
            }
            case '7days': 
              daysAgo.setDate(now.getDate() - 7)
              return incomeDate >= daysAgo && incomeDate <= now
            case '30days':
              daysAgo.setDate(now.getDate() - 30)
              return incomeDate >= daysAgo && incomeDate <= now
            case '90days':
              daysAgo.setDate(now.getDate() - 90)
              return incomeDate >= daysAgo && incomeDate <= now
            case '180days':
              daysAgo.setDate(now.getDate() - 180)
              return incomeDate >= daysAgo && incomeDate <= now
            case '365days':
              daysAgo.setDate(now.getDate() - 365)
              return incomeDate >= daysAgo && incomeDate <= now
            default:
              return true
          }
        })
      }
      
      // 排序
      switch (this.sortBy) {
        case 'time-desc':
          return filtered.sort((a, b) => new Date(b.createTime) - new Date(a.createTime))
        case 'time-asc':
          return filtered.sort((a, b) => new Date(a.createTime) - new Date(b.createTime))
        case 'amount-desc':
          return filtered.sort((a, b) => b.amount - a.amount)
        case 'amount-asc':
          return filtered.sort((a, b) => a.amount - b.amount)
        default:
          return filtered
      }
    },
    filteredTotal() {
      return this.filteredIncomes.reduce((sum, income) => sum + income.amount, 0)
    },
    totalPages() {
      return Math.ceil(this.filteredIncomes.length / this.pageSize)
    },
    pagedIncomes() {
      const start = (this.currentPage - 1) * this.pageSize
      const end = start + this.pageSize
      return this.filteredIncomes.slice(start, end)
    }
  },
  // 获取收入记录和书
  async created() {
    await this.fetchIncomeData()
    await this.fetchNovelsFromStore()
  },
  mounted() {
    this.initChart()
  },
  beforeUnmount() {
    if (this.chart) {
      this.chart.dispose()
    }
  },
  watch: {
    filteredIncomes() {
      this.currentPage = 1
      this.updateChart()
    },
    selectedRange() {
      this.updateChart()
    },
    filterNovelId() {
      this.currentPage = 1
      this.updateChart()
    }
  },

  methods: {
    // 从Novels读取小说数据
    async fetchNovelsFromStore() {
      try {
        if (novelsStore.novels.length === 0) {
          await novelsStore.fetchNovels()
        }
        this.novelsFromStore = novelsStore.novels
      } catch (error) {
        console.error('获取小说列表失败:', error)
        this.novelsFromStore = []
      }
    },
    // 找对应小说名
    getNovelName(novelId) {
      if (!novelId) return '未知作品'
      const novel = novelsStore.getNovelById(novelId)
      if (novel) {
        return novel.novel_name
      }
      return `未知小说(ID: ${novelId})`
    },
    //获取收入
    async fetchIncomeData() {
      try {
        this.loading = true
        
        console.log('开始获取收入数据...')
        const totalRes = await getAuthorTotalIncome(this.authorId)
        console.log('总收入响应:', totalRes)
        
        const incomesRes = await getAuthorIncomes(this.authorId)
        console.log('收入明细响应:', incomesRes)
        
        this.totalIncome = totalRes?.totalIncome || 0
        this.incomes = Array.isArray(incomesRes) ? incomesRes : []
        
        this.buildNovelFilterOptions()
        
      } catch (error) {
        console.error('获取收益数据失败:', error)
        this.$message.error('获取收益数据失败，请稍后重试')
      } finally {
        this.loading = false
        this.$nextTick(() => {
          this.updateChart()
        })
      }
    },
    // 构建小说筛选选项
    buildNovelFilterOptions() {
      const novelSet = new Set()
      this.novelNameMap = new Map()
      
      this.incomes.forEach(income => {
        if (income.novelId) {
          novelSet.add(income.novelId)
          if (income.novelName) {
            this.novelNameMap.set(income.novelId, income.novelName)
          }
        }
      })
      
      this.novelOptions = Array.from(novelSet).map(id => ({
        value: id,
        label: `${this.novelNameMap.get(id) || '未知小说'} (ID: ${id})`
      }))
      
      this.novelOptions.unshift({
        value: 'all',
        label: '全部作品'
      })
    },
    initChart() {
      this.chart = echarts.init(this.$refs.chart)
      this.updateChart()
    },
    updateChart() {
      if (!this.chart) return
      
      const dateMap = {}
      this.filteredIncomes.forEach(income => {
        const date = new Date(income.createTime).toLocaleDateString()
        if (!dateMap[date]) {
          dateMap[date] = { 打赏: 0, 章节购买: 0, 整本买断: 0, 分成: 0 }
        }
        dateMap[date][income.type] += income.amount
      })
      
      const dates = Object.keys(dateMap).sort()
      const seriesData = [
        { name: '打赏', type: 'line', data: [] },
        { name: '章节购买', type: 'line', data: [] },
        { name: '整本买断', type: 'line', data: [] },
        { name: '分成', type: 'line', data: [] }
      ]
      
      dates.forEach(date => {
        seriesData[0].data.push(dateMap[date]['打赏'])
        seriesData[1].data.push(dateMap[date]['章节购买'])
        seriesData[2].data.push(dateMap[date]['整本买断'])
        seriesData[3].data.push(dateMap[date]['分成'])
      })
      
      const option = {
        tooltip: {
          trigger: 'axis',
          formatter: params => {
            let result = params[0].axisValue + '<br/>'
            params.forEach(item => {
              result += `${item.marker} ${item.seriesName}: ${this.formatCurrency(item.value)}<br/>`
            })
            return result
          }
        },
        legend: {
          data: ['打赏', '章节购买', '整本买断', '分成']
        },
        xAxis: {
          type: 'category',
          data: dates
        },
        yAxis: {
          type: 'value',
          axisLabel: {
            formatter: value => this.formatCurrency(value)
          }
        },
        series: seriesData
      }
      
      this.chart.setOption(option)
    },
    prevPage() {
      if (this.currentPage > 1) {
        this.currentPage--
      }
    },
    nextPage() {
      if (this.currentPage < this.totalPages) {
        this.currentPage++
      }
    },
    formatCurrency(amount) {
      return amount.toLocaleString('zh-CN', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      })+' 虚拟币';
    },
    formatDateTime(dateString) {
      if (!dateString) return ''
      const date = new Date(dateString)
      return date.toLocaleString('zh-CN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
      }).replace(/\//g, '-')
    },
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

.income-page {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
  font-family: 'Segoe UI', system-ui, sans-serif;
}

/* 头部样式优化 */
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

.total-income {
  display: flex;
  align-items: baseline;
  font-size: 18px;
}

.total-income .label {
  color: var(--text-gray);
  margin-right: 8px;
}

.total-income .value {
  font-weight: 700;
  font-size: 28px;
  color: var(--primary-color);
}

/* 筛选区域样式 */
.filter-section {
  background: rgba(255,255,255,0.7);
  backdrop-filter: blur(5px);
  padding: 20px;
  border-radius: 16px;
  margin-bottom: 30px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.05);
}

.filter-row {
  display: flex;
  gap: 20px;
  margin-bottom: 20px;
  flex-wrap: wrap;
}

.filter-group {
  display: flex;
  align-items: center;
  flex: 1;
  min-width: 200px;
}

.filter-group label {
  margin-right: 10px;
  color: var(--text-gray);
  font-size: 14px;
  white-space: nowrap;
}

.filter-group select {
  padding: 10px 16px;
  border-radius: 8px;
  border: 1px solid rgba(0,0,0,0.1);
  background: white;
  font-size: 14px;
  transition: all 0.3s ease;
  width: 100%;
}

.filter-group select:focus {
  outline: none;
  border-color: var(--primary-color);
  box-shadow: 0 0 0 2px rgba(67,97,238,0.1);
}

.quick-stats {
  display: flex;
  gap: 15px;
  margin-top: 15px;
  flex-wrap: wrap;
}

.quick-stat {
  flex: 1;
  min-width: 150px;
  background: rgba(248,249,250,0.7);
  padding: 15px;
  border-radius: 8px;
  display: flex;
  flex-direction: column;
}

.quick-stat .stat-label {
  font-size: 14px;
  color: var(--text-gray);
  margin-bottom: 5px;
}

.quick-stat .stat-value {
  font-size: 20px;
  font-weight: 600;
  color: var(--text-dark);
}

/* 图表区域 */
.chart-container {
  background: rgba(255,255,255,0.7);
  backdrop-filter: blur(5px);
  padding: 25px;
  border-radius: 16px;
  margin-bottom: 30px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.05);
}

.chart-container h2 {
  font-size: 20px;
  color: var(--text-dark);
  margin-bottom: 20px;
}

/* 表格区域 */
.income-table-container {
  background: rgba(255,255,255,0.7);
  backdrop-filter: blur(5px);
  padding: 25px;
  border-radius: 16px;
  box-shadow: 0 4px 12px rgba (0,0,0,0.05);
}

.table-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.table-header h2 {
  font-size: 20px;
  color: var(--text-dark);
  margin: 0;
}

.table-summary {
  color: var(--text-gray);
  font-size: 14px;
}

.table-wrapper {
  border-radius: 12px;
  overflow: hidden;
  border: 1px solid rgba(0,0,0,0.05);
}

.income-table {
  width: 100%;
  border-collapse: collapse;
}

.income-table th {
  background: rgba(248,249,250,0.7);
  padding: 16px;
  text-align: left;
  color: var(--text-gray);
  font-weight: 600;
  position: sticky;
  top: 0;
}

.income-table td {
  padding: 16px;
  border-bottom: 1px solid rgba(0,0,0,0.05);
}

.income-table tr:last-child td {
  border-bottom: none;
}

.income-table tr:hover td {
  background: rgba(248,249,250,0.7);
}

/* 分页控件美化 */
.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
  margin-top: 30px;
}

.page-btn {
  padding: 10px 20px;
  border-radius: 8px;
  border: none;
  background: rgba(67,97,238,0.1);
  color: var(--primary-color);
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s ease;
}

.page-btn:hover:not(:disabled) {
  background: rgba(67,97,238,0.2);
}

.page-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.page-info {
  color: var(--text-gray);
  font-size: 14px;
}

/* 加载状态 */
.loading-cell, .empty-cell {
  text-align: center;
  padding: 40px;
  color: var(--text-gray);
}

.loading-cell {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.loading-spinner {
  width: 40px;
  height: 40px;
  border: 3px solid rgba(67,97,238,0.2);
  border-radius: 50%;
  border-top-color: var(--primary-color);
  animation: spin 1s linear infinite;
  margin-bottom: 15px;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

/* 小说名称样式 */
.novel-name {
  font-weight: 500;
}

.novel-id {
  color: var(--text-gray);
  font-size: 0.9em;
  margin-left: 6px;
}
</style>