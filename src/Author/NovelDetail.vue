<template>
  <!-- 页面容器 -->
  <div class="page-container">
    <!-- 顶部操作按钮区域 -->
    <div class="top-actions">

      <router-link to="/author/novels" class="btn back">
        返回列表
      </router-link>

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
          <p class="status" :class="novel.status.toLowerCase()">{{ novel.status }}</p>
          <p class="category">{{ novel.category }}</p>  
          <p class="word-count">{{ novel.total_word_count }}字</p> 
          <p class="intro">{{ novel.introduction }}</p> 
          
          <!-- 操作按钮组 -->
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
        <!-- 阅读量 -->
        <div class="stat-card">
          <h3>阅读量</h3>
          <p class="stat-value">{{ novel.view_count }}</p>
        </div>
        <!-- 收藏数 -->
        <div class="stat-card">
          <h3>收藏数</h3>
          <p class="stat-value">{{ novel.collected_count }}</p>
        </div>
        <!-- 推荐数 -->
        <div class="stat-card">
          <h3>推荐数</h3>
          <p class="stat-value">{{ novel.recommend_count }}</p>
        </div>
        <!-- 评分 -->
        <div class="stat-card">
          <h3>评分</h3>
          <p class="stat-value">{{ novel.score }}</p>
        </div>
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
// 导入小说状态管理
import { useNovel } from '@/stores/CurrentNovel'

// 从store中解构需要的状态和方法
const {
  novel,              // 当前小说数据
  showDeleteDialog,   // 删除对话框显示状态
  handleImageError,   // 图片加载失败处理函数
  confirmDelete,      // 显示删除确认对话框
  cancelDelete,       // 取消删除操作
  deleteNovel         // 执行删除操作
} = useNovel()
</script>

<style scoped>
/* 页面容器样式 */
.page-container {
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

/* 连载中状态特殊样式 */
.status.连载中 {
  background-color: #3498db;
  color: white;
}

/* 已完结状态特殊样式 */
.status.已完结 {
  background-color: #2ecc71;
  color: white;
}

/* 分类和字数样式 */
.category, .word-count {
  color: #7f8c8d; 
  margin: 5px 0;
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

/* 单个统计卡片样式 */
.stat-card {
  margin-top: 40px;
  background-color: #ecdcdc;
  padding: 30px;
  border-radius: 8px;
  text-align: center;
}

.stat-card h3 {
  margin: 0 0 10px;
  color: #7f8c8d;
  font-size: 16px;
}

.stat-value {
  font-size: 24px;
  font-weight: bold;
  margin: 0;
  color: #2c3e50;
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