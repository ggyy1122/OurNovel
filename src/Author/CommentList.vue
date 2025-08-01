<template>
  <div class="comments-page">
    <!-- 页面头部 -->
    <div class="page-header">
      <button @click="goBack" class="back-btn">返回书籍</button>
      <h1>{{ novel.novel_name }} - 读者评论</h1>
    </div>

    <!-- 统计卡片 -->
    <div class="stats-container">
      <div class="stat-card">
        <h3>评论总数</h3>
        <p class="stat-value">{{ commentsCount }}</p>
      </div>
      <div class="stat-card">
        <h3>总点赞数</h3>
        <p class="stat-value">{{ totalLikes }}</p>
      </div>
    </div>

    <!-- 筛选控制 -->
    <div class="controls">
      <div class="chapter-filter">
        <label for="chapter-filter">章节筛选：</label>
        <select id="chapter-filter" v-model="chapterFilter">
          <option value="all">全部章节</option>
          <option v-for="chapter in uniqueChapters" :key="chapter" :value="chapter">
            第{{ chapter }}章
          </option>
        </select>
      </div>
      <div class="sort-control">
        <label for="sort-by">排序方式：</label>
        <select id="sort-by" v-model="sortBy">
          <option value="time-desc">最新发布</option>
          <option value="time-asc">最早发布</option>
          <option value="like-desc">点赞最多</option>
          <option value="like-asc">点赞最少</option>
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

    <!-- 评论列表 -->
    <div class="comments-list">
      <div v-if="isLoading" class="loading">加载中...</div>
      <div v-else-if="error" class="error">{{ error }}</div>
      <div v-else-if="!hasComments" class="empty">暂无评论</div>
      
      <div v-else class="comment-item" v-for="(comment, index) in paginatedComments" :key="index">
        <div class="user-info">
          <img :src="getFullAvatarUrl(comment.readerInfo?.avatarUrl) || defaultAvatar" class="avatar" alt="用户头像">
          <div class="user-details">
            <div class="user-meta">
              <span class="username">{{ comment.readerInfo?.readerName || '匿名用户' }}</span>
              <span class="user-id">ID: {{ comment.readerId || comment.readerInfo?.readerId || '未知' }}</span>
              <span class="gender" :class="getGenderClass(comment.readerInfo?.gender)">
                {{ formatGender(comment.readerInfo?.gender) }}
              </span>
            </div>
            <div class="comment-meta">
              <span class="chapter">第{{ comment.chapterId }}章</span>
              <span class="comment-time">{{ formatDateTime(comment.createTime) }}</span>
            </div>
            
            <!-- 回复关系显示 -->
            <div v-if="comment.replyInfo" class="reply-info">
              <span class="reply-label">回复</span>
              <span class="reply-target">@{{ comment.replyInfo.targetUserName }}</span>
              <span class="reply-title">{{ comment.replyInfo.targetTitle }}</span>
              <span class="reply-content">{{ comment.replyInfo.targetContent }}</span>
            </div>
            
            <div class="comment-content">
              <h3 class="comment-title">{{ comment.title }}</h3>
              <p class="comment-text">{{ comment.content }}</p>
            </div>
          </div>
        </div>
        <div class="comment-actions">
          <span class="like-count">
            <i class="like-icon">❤</i> {{ comment.likes || 0 }}
          </span>
        </div>
      </div>
    </div>

    <!-- 分页控件 -->
    <div class="pagination" v-if="hasComments">
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
import { getCommentsByNovel } from '@/API/Comment_API'
import { getReplyByCommentId } from '@/API/CommentReply_API'
import { getReader } from '@/API/Reader_API'

const novelStore = useNovel()

// 安全获取数据
const novel = ref(novelStore.novel || {})
const comments = ref([])
const isLoading = ref(false)
const error = ref(null)
const defaultAvatar = ref('path/to/default/avatar.jpg')

// 筛选和排序状态
const chapterFilter = ref('all')
const sortBy = ref('time-desc')

// 分页状态
const currentPage = ref(1)
const pageSize = ref(10)

// 监听分页大小变化时重置当前页
watch(pageSize, () => {
  currentPage.value = 1
})

// 安全计算属性
const safeComments = computed(() => comments.value || [])
const hasComments = computed(() => safeComments.value.length > 0)
const commentsCount = computed(() => safeComments.value.length)
const totalLikes = computed(() => {
  return safeComments.value.reduce((sum, comment) => sum + (comment.likes || 0), 0)
})

// 获取所有章节ID（去重）
const uniqueChapters = computed(() => {
  const chapters = new Set()
  safeComments.value.forEach(comment => {
    chapters.add(comment.chapterId)
  })
  return Array.from(chapters).sort((a, b) => a - b)
})

// 格式化时间
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

// 筛选和排序后的评论列表
const filteredComments = computed(() => {
  let result = [...safeComments.value]
  
  // 章节筛选
  if (chapterFilter.value !== 'all') {
    result = result.filter(comment => comment.chapterId == chapterFilter.value)
  }
  
  // 排序
  switch (sortBy.value) {
    case 'time-desc':
      result.sort((a, b) => new Date(b.createTime) - new Date(a.createTime))
      break
    case 'time-asc':
      result.sort((a, b) => new Date(a.createTime) - new Date(b.createTime))
      break
    case 'like-desc':
      result.sort((a, b) => (b.likes || 0) - (a.likes || 0))
      break
    case 'like-asc':
      result.sort((a, b) => (a.likes || 0) - (b.likes || 0))
      break
  }
  
  return result
})

// 分页相关计算属性
const totalPages = computed(() => {
  return Math.ceil(filteredComments.value.length / pageSize.value)
})

const paginatedComments = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value
  const end = start + pageSize.value
  return filteredComments.value.slice(start, end)
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

// 获取头像完整URL
const getFullAvatarUrl = (partialPath) => {
  if (!partialPath) return require('@/assets/default-avatar.jpeg');
    
  if (partialPath.startsWith('http')) return partialPath;

  const ossBase = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/'
  return `${ossBase}${partialPath.replace(/^\//, '')}`
}

// 获取读者详细信息
const fetchReaderInfo = async (readerId) => {
  if (!readerId) return null
  
  try {
    const response = await getReader(readerId)
    return {
      readerId: response.readerId,
      readerName: response.readerName,
      gender: response.gender,
      avatarUrl: response.avatarUrl
    }
  } catch (error) {
    console.error('获取读者信息失败:', error)
    return null
  }
}

// 获取评论回复信息
const fetchReplyInfo = async (commentId) => {
  try {
    const response = await getReplyByCommentId(commentId)
    if (response && response.preComId) {
      // 查找被回复的评论
      const targetComment = comments.value.find(c => c.commentId === response.preComId)
      if (targetComment) {
        return {
          targetUserName: targetComment.readerInfo?.readerName || '匿名用户',
          targetTitle: targetComment.title || '已删除',
          targetContent: targetComment.content || ''
        }
      }
    }
    return null
  } catch (error) {
    console.error('获取回复信息失败:', error)
    return null
  }
}

// 返回按钮功能
const goBack = () => {
  window.history.back()
}

// 获取评论数据
const fetchComments = async () => {
  isLoading.value = true
  error.value = null
  
  try {
    if (!novel.value?.novel_id) {
      throw new Error('无法获取小说ID')
    }

    const response = await getCommentsByNovel(novel.value.novel_id)
    const rawComments = Array.isArray(response) ? response : []
    
    // 第一步：初始化评论列表（只填充基本数据）
    comments.value = rawComments.map(comment => ({
      ...comment,
      likes: comment.likes || 0,
      readerInfo: null,
      replyInfo: null
    }))
    
    // 第二步：先并行获取所有读者信息
    await Promise.all(comments.value.map(async comment => {
      comment.readerInfo = await fetchReaderInfo(comment.readerId)
    }))
    
    // 第三步：再获取回复信息（此时所有评论的readerInfo都已准备好）
    await Promise.all(comments.value.map(async comment => {
      if (comment.commentId) {
        comment.replyInfo = await fetchReplyInfo(comment.commentId)
      }
    }))
    
    if (comments.value.length === 0) {
      console.warn('API返回空评论列表', response)
    }
  } catch (err) {
    error.value = `加载失败: ${err.message || '未知错误'}`
    console.error('评论加载错误:', {
      novelId: novel.value?.novel_id,
      error: err,
      response: err.response?.data
    })
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  fetchComments()
})
</script>

<style scoped>
/* 评论页面整体容器样式 */
.comments-page {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

/* 页面头部样式 */
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

/* 控制区域样式 */
.controls {
  display: flex;
  flex-wrap: wrap;
  gap: 15px;
  margin-bottom: 20px;
  padding: 15px;
  background: #f8f8f8;
  border-radius: 8px;
}

/* 章节筛选器样式 */
.chapter-filter,
.sort-control,
.page-size-control {
  display: flex;
  align-items: center;
  gap: 8px;
}

.chapter-filter select,
.sort-control select,
.page-size-control select {
  padding: 8px 12px;
  border-radius: 4px;
  border: 1px solid #ddd;
  min-width: 120px;
}

/* 评论列表样式 */
.comments-list {
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.comment-item {
  padding: 15px;
  border-bottom: 1px solid #f0f0f0;
}

/* 用户信息样式 */
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

/* 评论元信息样式 */
.comment-meta {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 4px;
}

/* 章节信息样式 */
.chapter {
  font-size: 12px;
  color: #888;
}

/* 评论时间样式 */
.comment-time {
  font-size: 12px;
  color: #888;
}

/* 评论内容区域样式 */
.comment-content {
  margin-top: 4px;
}

/* 评论标题样式 */
.comment-title {
  font-size: 16px;
  margin-bottom: 0px;
  color: #333;
}

/* 评论正文样式 */
.comment-text {
  line-height: 1.6;
  color: #555;
  font-size: 14px;
}

/* 回复信息区域样式 */
.reply-info {
  background-color: #f9f9f9;
  border-left: 3px solid #ddd;
  padding: 8px 12px;
  margin: 8px 0;
  border-radius: 0 4px 4px 0;
  font-size: 13px;
  color: #666;
  display: flex;
  align-items: center;
  min-height: 32px;
}

/* 回复标签样式 */
.reply-label {
  color: #999;
  margin-right: 6px;
  flex-shrink: 0;
}

/* 回复目标用户样式 */
.reply-target {
  font-weight: bold;
  color: #1890ff;
  margin-right: 6px;
  flex-shrink: 0;
}

/* 回复标题样式 */
.reply-title {
  flex: 0 1 auto;
  font-weight: 600;
  color: #333;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 120px;
}

/* 回复内容样式 */
.reply-content {
  flex: 1;
  color: #666;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  padding-left: 8px;
  border-left: 1px dashed #d9d9d9;
}

/* 加载状态提示样式 */
.loading, .error, .empty {
  padding: 20px;
  text-align: center;
  color: #888;
}

/* 评论操作区域样式 */
.comment-actions {
  display: flex;
  justify-content: flex-end;
}

/* 点赞计数样式 */
.like-count {
  display: flex;
  align-items: center;
  gap: 4px;
  font-size: 14px;
  color: #f44336;
  cursor: pointer;
  transition: color 0.2s;
}

/* 点赞图标样式 */
.like-icon {
  font-size: 16px;
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

/* 分页按钮基础样式 */
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