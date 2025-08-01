<template>
  <div class="detail-page">
    <!-- 顶部操作按钮区域 -->
    <div class="top-actions">
      <button class="btn back" @click="$router.push('/author/novels')">
        返回列表
      </button>

      <button class="btn delete" @click="confirmDelete">
        删除小说
      </button>
    </div>

    <!-- 小说详情内容区域 -->
    <div class="novel-content">
      <!-- 小说头部信息 -->
      <div class="novel-header">
        <!-- 小说封面图片 -->
        <img 
          :src="novel.cover_url" 
          class="novel-cover" 
          @error="handleImageError" 
        >
        <!-- 小说元数据 -->
        <div class="novel-meta">
          <h2>{{ novel.novel_name }}</h2>  
          
          <div class="categories">
            <span v-for="(category, index) in novel.categories" :key="index" class="category-tag">
              {{ category }}
            </span>
            <span v-if="novel.categories.length === 0" class="no-category">暂无分类</span>
          </div>
          <p class="status" :class="novel.status.toLowerCase()">{{ novel.status }}</p>
          <p class="create-time"> 创建时间 ：{{ formatDateTime(novel.create_time) }}</p>
          <p class="word-count">{{ novel.total_word_count }}字 </p> 
          <p class="intro">作品简介：{{ novel.introduction }}</p> 
          
          <!-- 操作按钮 -->
          <div class="actions">
            
            <router-link 
              :to="'/author/novels/' + novel.novel_id + '/chapters'" 
              class="btn"
            >
              章节列表
            </router-link>
           
            <router-link 
              :to="'/author/novels/'+novel.novel_id+'/edit'" 
              class="btn"
            >
              编辑信息
            </router-link>
          </div>
        </div>
      </div>
      
      <!-- 小说统计数据 -->
      <div class="novel-stats">
        <!-- 评论数 -->
         <router-link 
          :to="{ name: 'CommentList', params: { id: novel.novel_id } }" 
          class="stat-card"
        >
          <h3>评论数</h3>
          <p class="stat-value">{{ novel.comment_count }}</p>
        </router-link>
        <!-- 收藏数 -->
        <router-link 
          :to="{ name: 'CollectList', params: { id: novel.novel_id } }" 
          class="stat-card"
        >
          <h3>收藏数</h3>
          <p class="stat-value">{{ novel.collected_count}}</p>
        </router-link>
        <!-- 推荐数 -->
        <router-link 
          :to="{ name: 'RecomendList', params: { id: novel.novel_id } }" 
          class="stat-card"
        >
          <h3>推荐数</h3>
          <p class="stat-value">{{ novel.recommend_count }}</p>
        </router-link>
        <!-- 评分 -->
        <router-link 
          :to="{ name: 'RatingList', params: { id: novel.novel_id } }" 
          class="stat-card"
        >
          <h3>评分</h3>
          <p class="stat-value">{{ novel.score }}</p>
        </router-link>
      </div>
    </div>

    <!-- 删除确认对话框 -->
    <div v-if="showDeleteDialog" class="delete-dialog">
      <div class="dialog-content">
        <h3>确认删除小说？</h3>
        <p>删除后将无法恢复，所有章节内容也将被永久删除</p>
        <div class="dialog-actions">
          <button class="dialog-btn cancel" @click="cancelDelete">取消</button>
          <button class="dialog-btn confirm" @click="deleteNovel">确认删除</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
// 导入小说状态
import { useNovel } from '@/stores/CurrentNovel'
import { useChapters } from '@/stores/Chapters'
import { onMounted } from 'vue'

const {
  novel,             
  showDeleteDialog,   
  handleImageError,   
  confirmDelete,      
  cancelDelete,       
  deleteNovel         
} = useNovel()

// 初始化章节store
const chaptersStore = useChapters()

// 组件挂载时获取章节
onMounted(() => {
  if (novel.value?.novel_id) {
    chaptersStore.setNovelId(novel.value.novel_id)
    chaptersStore.fetchChapters(novel.value.novel_id)
  }
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
</script>

<style scoped>
/* 页面容器样式 */
.detail-page {
  max-width: 1200px;  
  margin: 0 auto;     
  padding: 20px;      
}

/* 顶部操作按钮区域样式 */
.top-actions {
  display: flex;
  justify-content: space-between;  
  margin-bottom: 20px; 
}

/* 基础按钮样式 */
.btn {
  cursor: pointer;
  padding: 8px 16px;
  border-radius: 4px;
  text-decoration: none;
  font-weight: 500;
  transition: all 0.3s;  
}

/* 返回按钮特殊样式 */
.back {
  background-color: #f0f0f0;
  color: #333;
  border: 1px solid #ddd;
}

/* 删除按钮特殊样式 */
.delete {
  background-color: #e74c3c;
  color: white;
  border: none;
}

/* 小说内容区域样式 */
.novel-content {
  margin-bottom: 40px;
}

/* 小说头部信息布局 */
.novel-header {
  display: flex;
  gap: 30px;  
}

/* 小说封面图片样式 */
.novel-cover {
  width: 240px;
  height: 320px;
  object-fit: cover;  
  border-radius: 4px;
  box-shadow: 0 4px 8px rgba(0,0,0,0.1);  
}

/* 小说元数据区域样式 */
.novel-meta {
  flex: 1;  
}

.novel-meta h2 {
  font-size: 24px;
  margin: 0 0 10px;
}

/* 小说状态标签样式 */
.status {
  display: inline-block;
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 14px;
  font-weight: bold;
}

.status.连载 {
  background-color: #3498db;
  color: white;
}

.status.完结 {
  background-color: #2ecc71;
  color: white;
}

.status.待审核 {
  background-color:#f39c12;
  color: white;
}

.status.封禁 {
  background-color: #e74c3c;
  color: white;
}

/* 创建时间样式 */
 .create-time {
  color: #181c1c; 
  margin: 5px 0;
}

/* 分类和字数样式 */
 .word-count {
  color: #7f8c8d; 
  margin: 5px 0;
}

/* 分类标签样式 */
.categories {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin: 10px 0;
}

.category-tag {
  background-color: #e8f4ff;
  color: #3498db;
  padding: 4px 12px;
  border-radius: 16px;
  font-size: 13px;
  display: inline-flex;
  align-items: center;
  transition: all 0.2s;
}


.no-category {
  color: #999;
  font-size: 14px;
}

/* 简介文本样式 */
.intro {
  margin: 15px 0;
  line-height: 1.6;  
}

/* 操作按钮组样式 */
.actions {
  margin-top: 20px;
  display: flex;
  gap: 10px;
}

.actions .btn {
  background-color: #3498db;
  color: white;
}

/* 统计卡片网格布局 */
.novel-stats {
  display: grid;
  grid-template-columns: repeat(4, 1fr);  
  gap: 20px; 
}
/* 统计卡片容器 */
.novel-stats {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 25px;
  margin-top: 40px;
}

/* 单个统计卡片 */
.stat-card {
  background: white;
  padding: 25px;
  border-radius: 12px;
  text-align: center;
  box-shadow: 0 6px 15px rgba(0, 0, 0, 0.05);
  transition: all 0.3s ease;
  border: 1px solid rgba(0, 0, 0, 0.05);
  cursor: pointer;
  text-decoration: none;
  display: block;
}

/* 卡片悬停效果 */
.stat-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
  border-color: rgba(0, 0, 0, 0.1);
}

/* 卡片标题 */
.stat-card h3 {
  margin: 0 0 15px;
  color: #6c757d;
  font-size: 16px;
  font-weight: 500;
  letter-spacing: 0.5px;
}

/* 卡片数值 */
.stat-value {
  font-size: 32px;
  font-weight: 700;
  margin: 0;
  color: #4361ee;
  position: relative;
  display: inline-block;
}

/* 为不同统计项添加不同颜色 */
.stat-card:nth-child(1) .stat-value {
  color: #4361ee; /* 评论数 - 蓝色 */
}

.stat-card:nth-child(2) .stat-value {
  color: #eb2f96; /* 收藏数 - 粉色 */
}

.stat-card:nth-child(3) .stat-value {
  color: #52c41a; /* 推荐数 - 绿色 */
}

.stat-card:nth-child(4) .stat-value {
  color: #f39c12; /* 评分 - 橙色 */
}

/* 评分特殊样式 */
.stat-card:last-child .stat-value::after {
  content: '分';
  font-size: 16px;
  margin-left: 3px;
  color: #6c757d;
  font-weight: normal;
}

/* 删除对话框样式 */
.delete-dialog {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0,0,0,0.5);  
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;  
}

/* 对话框内容区域 */
.dialog-content {
  background-color: white;
  padding: 25px;
  border-radius: 8px;
  max-width: 500px;
  width: 90%;
}

.dialog-content h3 {
  margin: 0 0 15px;
}

.dialog-content p {
  margin: 0 0 20px;
  color: #7f8c8d;
}

/* 对话框操作按钮区域 */
.dialog-actions {
  display: flex;
  justify-content: flex-end;  
  gap: 10px;  
}

/* 对话框按钮基础样式 */
.dialog-btn {
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
}

/* 取消按钮样式 */
.cancel {
  background-color: #f0f0f0;
  color: #333;
  border: 1px solid #ddd;
}

/* 确认按钮样式 */
.confirm {
  background-color: #e74c3c;
  color: white;
  border: none;
}
</style>