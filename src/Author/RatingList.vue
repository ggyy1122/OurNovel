<template>
  <div class="rating-page">
    <!-- 页面头部 -->
    <div class="page-header">
      <button @click="goBack" class="back-btn">返回书籍</button>
      <h1>{{ novel.novel_name }} - 评分记录</h1>
    </div>

    <!-- 统计卡片 -->
    <div class="stats-container">
      <div class="stat-card">
        <h3>平均评分</h3>
        <p class="stat-value">{{ averageScore }}</p>
      </div>
      <div class="stat-card">
        <h3>评分总数</h3>
        <p class="stat-value">{{ ratingCount }}</p>
      </div>
      <div class="stat-card">
        <h3>最近评分</h3>
        <p class="stat-value">{{ latestRatingTime }}</p>
      </div>
    </div>

    <!-- 筛选和排序控制 -->
    <div class="controls">
      <div class="filter-control">
        <label for="score-filter">评分筛选：</label>
        <select id="score-filter" v-model="scoreFilter">
          <option value="all">全部评分</option>
          <option value="9">9分及以上</option>
          <option value="7">7分及以上</option>
          <option value="5">5分及以上</option>
          <option value="3">3分及以上</option>
          <option value="1">1分及以上</option>
        </select>
      </div>
      <div class="gender-filter">
        <label for="gender-filter">性别筛选：</label>
        <select id="gender-filter" v-model="genderFilter">
          <option value="all">全部</option>
          <option value="男">男</option>
          <option value="女">女</option>
          <option value="unknown">未知</option>
        </select>
      </div>
      <div class="sort-control">
        <label for="sort-by">排序方式：</label>
        <select id="sort-by" v-model="sortBy">
          <option value="time-desc">最新在前</option>
          <option value="time-asc">最旧在前</option>
          <option value="score-desc">评分从高到低</option>
          <option value="score-asc">评分从低到高</option>
        </select>
      </div>
      <div class="page-size-control">
        <label for="page-size">每页显示：</label>
        <select id="page-size" v-model="pageSize">
          <option value="10">10条</option>
          <option value="20">20条</option>
          <option value="50">50条</option>
        </select>
      </div>
    </div>

    <!-- 评分列表 -->
    <div class="ratings-list">
      <div v-if="isLoading" class="loading">加载中...</div>
      <div v-else-if="error" class="error">{{ error }}</div>
      <div v-else-if="!hasRatings" class="empty">暂无评分记录</div>
      
      <div v-else class="rating-item" v-for="rating in paginatedRatings" :key="rating.ratingTime">
        <div class="user-info">
          <img :src="getFullAvatarUrl(rating.reader?.avatarUrl) || defaultAvatar" class="avatar" alt="用户头像">
          <div class="user-details">
            <div class="user-meta">
              <span class="username">{{ rating.reader?.readerName || '匿名用户' }}</span>
              <span class="user-id">ID: {{ rating.reader?.readerId || '未知' }}</span>
              <span class="gender" :class="getGenderClass(rating.reader?.gender)">
                {{ formatGender(rating.reader?.gender) }}
              </span>
            </div>
            <div class="rating-meta">
              <span class="rating-time">{{ formatDateTime(rating.ratingTime) }}</span>
            </div>
          </div>
        </div>
        <div class="rating-score">
          <span class="score">{{ rating.score || 0 }}</span>
          <span class="out-of">/10</span>
        </div>
      </div>
    </div>

    <!-- 分页控件 -->
    <div class="pagination" v-if="hasRatings">
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
        :disabled="currentPage === totalPages"
        class="page-btn"
      >
        下一页
      </button>
    </div>
  </div>
</template>

<script setup>
// 导入状态和数据
import { ref, computed, onMounted, watch } from 'vue'
import { useNovel } from '@/stores/CurrentNovel'
import { getRatesByNovel } from '@/API/Rate_API'

// 获取数据
const novelStore = useNovel()
const novel = ref(novelStore.novel || {})
const ratings = ref([])
const isLoading = ref(false)
const error = ref(null)
const defaultAvatar = ref('path/to/default/avatar.jpg')

// 筛选和排序状态
const scoreFilter = ref('all')
const genderFilter = ref('all')
const sortBy = ref('time-desc')

// 分页状态
const currentPage = ref(1)
const pageSize = ref(10)

// 监听分页大小变化时重置当前页
watch(pageSize, () => {
  currentPage.value = 1
})

// 安全计算属性
const safeRatings = computed(() => ratings.value || [])
const hasRatings = computed(() => safeRatings.value.length > 0)
const ratingCount = computed(() => safeRatings.value.length)

// 筛选和排序后的评分列表
const filteredAndSortedRatings = computed(() => {
  let filtered = [...safeRatings.value]
  
  // 应用评分筛选
  if (scoreFilter.value !== 'all') {
    const minScore = parseInt(scoreFilter.value)
    filtered = filtered.filter(rating => (rating.score || 0) >= minScore)
  }
  
  // 应用性别筛选
  if (genderFilter.value !== 'all') {
    filtered = filtered.filter(rating => {
      const gender = rating.reader?.gender
      if (genderFilter.value === 'unknown') {
        return !gender
      }
      return gender === genderFilter.value
    })
  }
  
  // 应用排序
  switch (sortBy.value) {
    case 'time-desc':
      filtered.sort((a, b) => new Date(b.ratingTime || 0) - new Date(a.ratingTime || 0))
      break
    case 'time-asc':
      filtered.sort((a, b) => new Date(a.ratingTime || 0) - new Date(b.ratingTime || 0))
      break
    case 'score-desc':
      filtered.sort((a, b) => (b.score || 0) - (a.score || 0))
      break
    case 'score-asc':
      filtered.sort((a, b) => (a.score || 0) - (b.score || 0))
      break
  }
  
  return filtered
})

// 分页相关计算属性
const totalPages = computed(() => {
  return Math.ceil(filteredAndSortedRatings.value.length / pageSize.value)
})

const paginatedRatings = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value
  const end = start + pageSize.value
  return filteredAndSortedRatings.value.slice(start, end)
})

// 分页方法
const prevPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--
  }
}

const nextPage = () => {
  if (currentPage.value < totalPages.value) {
    currentPage.value++
  }
}

const averageScore = computed(() => {
  if (!hasRatings.value) return '0.0'
  const sum = safeRatings.value.reduce((total, rating) => 
    total + (Number(rating.score) || 0), 0)
  return (sum / ratingCount.value).toFixed(1)
})

const latestRatingTime = computed(() => {
  if (!hasRatings.value) return '暂无'
  const latest = safeRatings.value.reduce((latest, rating) => 
    new Date(rating.ratingTime || 0) > new Date(latest.ratingTime || 0) ? rating : latest
  )
  return formatDateTime(latest.ratingTime)
})

// 格式化性别
const formatGender = (gender) => {
  if (!gender) return '未知'
  return gender === '男' ? '男' : '女'
}

// 获取性别对应的样式类
const getGenderClass = (gender) => {
  if (!gender) return 'gender-unknown'
  return gender === '男' ? 'gender-male' : 'gender-female'
}

// 获取头像完整URL
const getFullAvatarUrl = (partialPath) => {
  if (!partialPath) return require('@/assets/default-avatar.jpeg');
    
  if (partialPath.startsWith('http')) return partialPath;

  const ossBase = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/'
  return `${ossBase}${partialPath.replace(/^\//, '')}`
}

// 格式化日期时间
const formatDateTime = (dateString) => {
  try {
    const date = new Date(dateString)
    if (isNaN(date)) return '日期无效'
    
    const year = date.getFullYear()
    const month = (date.getMonth() + 1).toString().padStart(2, '0')
    const day = date.getDate().toString().padStart(2, '0')
    const hours = date.getHours().toString().padStart(2, '0')
    const minutes = date.getMinutes().toString().padStart(2, '0')
    
    return `${year}-${month}-${day} ${hours}:${minutes}`
  } catch {
    return '日期无效'
  }
}

// 返回按钮功能
const goBack = () => {
  window.history.back()
}

// 获取评分数据
const fetchRatings = async () => {
  isLoading.value = true
  error.value = null
  
  try {
    if (!novel.value?.novel_id) {
      throw new Error('无法获取小说ID')
    }

    const response = await getRatesByNovel(novel.value.novel_id)
    ratings.value = Array.isArray(response) ? response : []
    
    console.log('性别分布统计:', {
      男: ratings.value.filter(r => r.reader?.gender === '男').length,
      女: ratings.value.filter(r => r.reader?.gender === '女').length,
      未知: ratings.value.filter(r => !r.reader?.gender).length
    })
    
    if (ratings.value.length === 0) {
      console.warn('API返回空评分列表', response)
    }
  } catch (err) {
    error.value = `加载失败: ${err.message || '未知错误'}`
    console.error('评分加载错误:', {
      novelId: novel.value?.novel_id,
      error: err,
      response: err.response?.data
    })
  } finally {
    isLoading.value = false
  }
}

// 组件挂载时加载数据
onMounted(() => {
  fetchRatings()
})
</script>

<style scoped>
/* 评分页面整体容器样式 */
.rating-page {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

/* 页面头部样式（包含返回按钮和标题） */
.page-header {
  display: flex;
  align-items: center;
  gap: 20px;
  margin-bottom: 30px;
}

/* 返回按钮样式 */
.back-btn {
  cursor: pointer;
  padding: 8px 16px;
  border-radius: 4px;
  text-decoration: none;
  font-weight: 500;
  transition: all 0.3s;  
  background-color: #f0f0f0;
  color: #333;
  border: 1px solid #ddd;
}

/* 统计卡片容器样式 */
.stats-container {
  display: flex;
  gap: 15px;
  margin-bottom: 30px;
}

/* 单个统计卡片样式 */
.stat-card {
  flex: 1;
  background: #fff;
  border-radius: 8px;
  padding: 15px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

/* 统计数值样式 */
.stat-value {
  font-size: 24px;
  font-weight: bold;
  margin-top: 10px;
}

/* 控制区域样式（包含筛选和排序控制） */
.controls {
  display: flex;
  flex-wrap: wrap;
  gap: 15px;
  margin-bottom: 20px;
  padding: 15px;
  background: #f8f8f8;
  border-radius: 8px;
}

/* 筛选控制组样式 */
.filter-control, 
.gender-filter, 
.sort-control,
.page-size-control {
  display: flex;
  align-items: center;
  gap: 8px;
}

.filter-control select, 
.gender-filter select, 
.sort-control select,
.page-size-control select {
  padding: 8px 12px;
  border-radius: 4px;
  border: 1px solid #ddd;
  min-width: 120px;
}

/* 评分列表样式 */
.ratings-list {
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.rating-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px;
  border-bottom: 1px solid #f0f0f0;
}

/* 用户信息样式 */
.user-info {
  display: flex;
  align-items: center;
  gap: 12px;
  flex: 1;
  min-width: 0;
}

.avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  object-fit: cover;
  flex-shrink: 0;
}

.user-details {
  flex: 1;
  min-width: 0;
}

.user-meta {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 4px;
  flex-wrap: wrap;
}

.username {
  font-weight: bold;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 120px;
}

.user-id {
  font-size: 12px;
  color: #666;
  white-space: nowrap;
}

/* 性别标签样式 */
.gender {
  font-size: 12px;
  padding: 2px 6px;
  border-radius: 4px;
  white-space: nowrap;
}

.gender-male {
  background-color: #e6f7ff;
  color: #1890ff;
  border: 1px solid #91d5ff;
}

.gender-female {
  background-color: #fff0f6;
  color: #eb2f96;
  border: 1px solid #ffadd2;
}

.gender-unknown {
  background-color: #f6ffed;
  color: #52c41a;
  border: 1px solid #b7eb8f;
}

/* 评分元信息样式（时间和分数） */
.rating-meta {
  display: flex;
  align-items: center;
  gap: 10px;
}

/* 评分时间样式 */
.rating-time {
  font-size: 12px;
  color: #888;
}

/* 评分分数区域样式 */
.rating-score {
  display: flex;
  align-items: baseline;
  flex-shrink: 0;
  margin-left: 15px;
}

/* 分数值样式 */
.score {
  font-size: 18px;
  font-weight: bold;
  color: #ff9800;
}
/* 总分标识样式（如"/10"） */
.out-of {
  font-size: 14px;
  color: #888;
}

/* 加载状态提示样式 */
.loading, .error, .empty {
  padding: 20px;
  text-align: center;
  color: #888;
}

/* 分页控件容器样式 */
.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 20px;
  margin-top: 20px;
  padding: 15px;
}

/* 分页按钮样式 */
.page-btn {
  padding: 8px 16px;
  background-color: #f0f0f0;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.page-btn:hover:not(:disabled) {
  background-color: #e0e0e0;
}

.page-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

/* 分页信息文字样式 */
.page-info {
  font-size: 14px;
  color: #666;
}
</style>