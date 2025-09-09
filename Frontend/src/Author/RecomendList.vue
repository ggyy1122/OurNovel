<template>
  <div class="recommend-page">
    <!-- 页面头部 -->
    <div class="page-header">
      <button @click="goBack" class="back-btn">返回书籍</button>
      <h1>{{ novel.novel_name }} - 推荐记录</h1>
    </div>

    <!-- 统计卡片 -->
    <div class="stats-container">
      <div class="stat-card">
        <h3>推荐总数</h3>
        <p class="stat-value">{{ recommendCount }}</p>
      </div>
    </div>

    <!-- 筛选控制 -->
    <div class="controls">
      <div class="gender-filter">
        <label for="gender-filter">性别筛选：</label>
        <select id="gender-filter" v-model="genderFilter">
          <option value="all">全部</option>
          <option value="男">男</option>
          <option value="女">女</option>
          <option value="unknown">未知</option>
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

    <!-- 推荐列表 -->
    <div class="recommends-list">
      <div v-if="isLoading" class="loading">加载中...</div>
      <div v-else-if="error" class="error">{{ error }}</div>
      <div v-else-if="!hasRecommends" class="empty">暂无推荐记录</div>
      
      <div v-else class="recommend-item" v-for="(recommend, index) in paginatedRecommends" :key="index">
        <div class="user-info">
          <img :src="getFullAvatarUrl(recommend.reader?.avatarUrl) || defaultAvatar" class="avatar" alt="用户头像">
          <div class="user-details">
            <div class="user-meta">
              <span class="username">{{ recommend.reader?.readerName || '匿名用户' }}</span>
              <span class="user-id">ID: {{ recommend.reader?.readerId || '未知' }}</span>
              <span class="gender" :class="getGenderClass(recommend.reader?.gender)">
                {{ formatGender(recommend.reader?.gender) }}
              </span>
            </div>
            <div class="recommend-reason" v-if="recommend.reason">
              {{ recommend.reason }}
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- 分页控件 -->
    <div class="pagination" v-if="hasRecommends">
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
import { ref, computed, onMounted, watch } from 'vue'
import { useNovel } from '@/stores/CurrentNovel'
import { getRecommendsByNovel } from '@/API/Recommend_API'

const novelStore = useNovel()

// 安全获取数据
const novel = ref(novelStore.novel || {})
const recommends = ref([])
const isLoading = ref(false)
const error = ref(null)
const defaultAvatar = ref('path/to/default/avatar.jpg')

// 筛选状态
const genderFilter = ref('all')

// 分页状态
const currentPage = ref(1)
const pageSize = ref(10)

// 监听分页大小变化时重置当前页
watch(pageSize, () => {
  currentPage.value = 1
})

// 安全计算属性
const safeRecommends = computed(() => recommends.value || [])
const hasRecommends = computed(() => safeRecommends.value.length > 0)
const recommendCount = computed(() => safeRecommends.value.length)

// 筛选后的推荐列表
const filteredRecommends = computed(() => {
  if (genderFilter.value === 'all') {
    return [...safeRecommends.value]
  }
  
  return safeRecommends.value.filter(recommend => {
    const gender = recommend.reader?.gender
    if (genderFilter.value === 'unknown') {
      return !gender
    }
    return gender === genderFilter.value
  })
})

// 分页相关计算属性
const totalPages = computed(() => {
  return Math.ceil(filteredRecommends.value.length / pageSize.value)
})

const paginatedRecommends = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value
  const end = start + pageSize.value
  return filteredRecommends.value.slice(start, end)
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

// 头像URL处理
const getFullAvatarUrl = (partialPath) => {
  if (!partialPath) return require('@/assets/default-avatar.jpeg');
    
  if (partialPath.startsWith('http')) return partialPath;

  const ossBase = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/'
  return `${ossBase}${partialPath.replace(/^\//, '')}`
}

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

// 返回按钮功能
const goBack = () => {
  window.history.back()
}

// 获取推荐数据
const fetchRecommends = async () => {
  isLoading.value = true
  error.value = null
  
  try {
    if (!novel.value?.novel_id) {
      throw new Error('无法获取小说ID')
    }
    const response = await getRecommendsByNovel(novel.value.novel_id)
    recommends.value = Array.isArray(response) ? response : []
    if (recommends.value.length === 0) {
      console.warn('API返回空推荐列表', response)
    }
  } catch (err) {
    error.value = `加载失败: ${err.message || '未知错误'}`
    console.error('推荐加载错误:', {
      novelId: novel.value?.novel_id,
      error: err,
      response: err.response?.data
    })
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  fetchRecommends()
})
</script>

<style scoped>
/* 推荐页面整体容器样式 */
.recommend-page {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

/* 页面头部区域 */
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

/* 统计卡片区域 */
.stats-container {
  display: flex;
  gap: 15px;
  margin-bottom: 30px;
}

.stat-card {
  flex: 1;
  background: #fff;
  border-radius: 8px;
  padding: 15px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.stat-value {
  font-size: 24px;
  font-weight: bold;
  margin-top: 10px;
}

/* 筛选控制区域 */
.controls {
  display: flex;
  flex-wrap: wrap;
  gap: 15px;
  margin-bottom: 20px;
  padding: 15px;
  background: #f8f8f8;
  border-radius: 8px;
}

/* 下拉选择框样式 */
.gender-filter,
.page-size-control {
  display: flex;
  align-items: center;
  gap: 8px;
}

.gender-filter select,
.page-size-control select {
  padding: 8px 12px;
  border-radius: 4px;
  border: 1px solid #ddd;
  min-width: 120px;
}

/* 推荐列表样式 */
.recommends-list {
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.recommend-item {
  padding: 15px;
  border-bottom: 1px solid #f0f0f0;
}

/* 用户信息展示 */
.user-info {
  display: flex;
  align-items: flex-start;
  gap: 12px;
}

.avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  object-fit: cover;
}

.user-details {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.user-meta {
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  gap: 10px;
  margin-bottom: 8px;
}

.username {
  font-weight: bold;
  max-width: 120px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.user-id {
  font-size: 12px;
  color: #666;
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

/* 推荐理由样式 */
.recommend-reason {
  padding: 8px 12px;
  background-color: #f8f8f8;
  border-radius: 4px;
  font-size: 14px;
  color: #555;
}

.loading, .error, .empty {
  padding: 20px;
  text-align: center;
  color: #888;
}

/* 分页控件样式 */
.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 20px;
  margin-top: 20px;
  padding: 15px;
}

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

/* 页信息 */
.page-info {
  font-size: 14px;
  color: #666;
}
</style>


