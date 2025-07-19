<template>
  <!-- 章节管理页面容器 -->
  <div class="page-container">
    <!-- 新增返回按钮 -->
    <button type="button" class="back-btn top-back-btn" @click="goBack">
      返回书籍
    </button>
    
    <!-- 页面头部 -->
    <div class="page-header">
      <h2>{{ novel.novel_name }} - 章节管理</h2>
      <!-- 创建新章节按钮 -->
      <button @click="createNewChapter" class="create-btn">
        <span>+</span> 写新章节
      </button>
    </div>
    
    <!-- 加载状态显示 -->
    <div v-if="isLoading" class="loading">加载中...</div>
    <!-- 错误状态显示 -->
    <div v-else-if="error" class="error">{{ error }}</div>
    
    <!-- 主编辑区域 -->
    <div v-else class="editor-layout">
      <!-- 左侧章节列表 -->
      <div class="chapter-list">
        <!-- 搜索框 -->
        <div class="search-box">
          <input 
            type="text" 
            placeholder="搜索章节..." 
            v-model="searchQuery"
          >
        </div>
        <!-- 章节列表容器 -->
        <div class="list-container">
          <!-- 章节项循环 -->
          <div 
            v-for="chapter in filteredChapters" 
            :key="chapter.chapter_id" 
            class="chapter-item"
            :class="{ active: activeChapterId === chapter.chapter_id }"
            @click="selectChapter(chapter)"
          >
            <!-- 章节标题 -->
            <div class="chapter-title">{{ chapter.title }}</div>
            <!-- 章节元信息 -->
            <div class="chapter-meta">
              <span class="word-count">{{ chapter.word_count }}字</span>
              <span class="status" :class="chapter.status">
                {{ getStatusText(chapter.status) }}
              </span>
              <span class="price">¥{{ chapter.calculated_price }}</span>
              <span class="publish-time" v-if="chapter.status === '已发布'">
                {{ formatDate(chapter.publish_time) }}
              </span>
            </div>
            <!-- 删除按钮 -->
            <button 
              class="delete-btn" 
              @click.stop="confirmDeleteChapter(chapter)"
            >
              <i class="icon-delete">×</i>
            </button>
          </div>
        </div>
      </div>
      
      <!-- 右侧编辑区 -->
      <div class="editor-area">
        <!-- 有选中章节时的编辑区域 -->
        <div v-if="activeChapter" class="editor-container">
          <!-- 编辑头部 -->
          <div class="editor-header">
            <!-- 章节标题输入 -->
            <input 
              type="text" 
              class="chapter-title-input" 
              v-model="activeChapter.title" 
              placeholder="输入章节标题"
              :disabled="activeChapter.status === '审核中' || activeChapter.status === '已发布'"
            >
            <!-- 编辑工具栏 -->
            <div class="editor-toolbar">
              <!-- 保存按钮 -->
              <button 
                class="save-btn" 
                @click="saveChapter" 
                :disabled="activeChapter.status === '审核中' || activeChapter.status === '已发布'"
              >
                保存
              </button>
              <!-- 状态切换按钮 -->
              <button 
                class="status-btn" 
                :class="activeChapter.status"
                @click="toggleChapterStatus"
                :disabled="!canChangeStatus"
              >
                {{ getStatusButtonText(activeChapter.status) }}
              </button>
            </div>
          </div>
          <!-- 内容编辑区 -->
          <textarea 
            class="content-editor" 
            v-model="activeChapter.content" 
            placeholder="在此输入章节内容..."
            @input="updateWordCount"
            :disabled="activeChapter.status === '审核中' || activeChapter.status === '已发布'"
          ></textarea>
          <!-- 编辑底部 -->
          <div class="editor-footer">
            <!-- 字数统计 -->
            <div class="word-count">字数: {{ activeChapter.word_count }}</div>
            <!-- 价格设置 -->
            <div class="price-setting">
              <label>每千字价格: ¥</label>
              <input 
                type="number" 
                v-model="activeChapter.price_per_kilo" 
                min="0" 
                step="0.01"
                @input="updateCalculatedPrice"
                :disabled="activeChapter.status === '审核中' || activeChapter.status === '已发布'"
              >
              <span class="final-price">最终价格: ¥{{ activeChapter.calculated_price }}</span>
            </div>
            <!-- 收费设置 -->
            <div class="charge-setting">
              <label>是否收费:</label>
              <select 
                v-model="activeChapter.is_charged" 
                :disabled="activeChapter.status === '审核中' || activeChapter.status === '已发布'"
              >
                <option value="是">是</option>
                <option value="否">否</option>
              </select>
            </div>
          </div>
        </div>
        <!-- 无选中章节时的提示 -->
        <div v-else class="empty-editor">
          <p>请从左侧选择章节或创建新章节</p>
        </div>
      </div>
    </div>

    <!-- 删除确认对话框 -->
    <ConfirmDialog
      v-if="showDeleteConfirm"
      title="确认删除章节"
      :message="'确定要删除章节 ${chapterToDelete?.title}吗？此操作无法撤销。'"
      @cancel="showDeleteConfirm = false"
      @confirm="deleteChapter"
    />
  </div>
</template>

<script setup>
// 导入状态管理
import { useRouter } from 'vue-router'
import { useNovel } from '@/stores/CurrentNovel'
import { useChapters } from '@/stores/Chapters'

// 获取小说和章节相关状态和方法
const { novel } = useNovel()
const {
  activeChapterId,
  searchQuery,
  showDeleteConfirm,
  chapterToDelete,
  isLoading,
  error,
  activeChapter,
  filteredChapters,
  canChangeStatus,
  saveChapter,
  createNewChapter,
  toggleChapterStatus,
  deleteChapter,
  updateWordCount,
  updateCalculatedPrice,
  getStatusText,
  getStatusButtonText,
  formatDate
} = useChapters(novel.value.novel_id)

// 返回上一级
const router = useRouter()
const goBack = () => {
  router.go(-1)
}

// 选择章节
const selectChapter = (chapter) => {
  activeChapterId.value = chapter.chapter_id
}

// 确认删除章节
const confirmDeleteChapter = (chapter) => {
  chapterToDelete.value = chapter
  showDeleteConfirm.value = true
}
</script>

<style scoped>
/* 页面容器样式 */
.page-container {
  padding: 20px;
  height: calc(100vh - 40px);
  display: flex;
  flex-direction: column;
  position: relative;
}

.top-back-btn {
  position: absolute;
  left: 20px;
  top: 20px;
  padding: 8px 15px;
  background-color: #f5f5f5; /* 改为灰色背景 */
  color: #666; /* 改为深灰色文字 */
  border: 1px solid #ddd; /* 改为浅灰色边框 */
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.3s;
  z-index: 1;
}

.top-back-btn:hover {
  background-color: #e6f2ff;
}

/* 页面头部样式 */
.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  padding-bottom: 15px;
  border-bottom: 1px solid #eee;
  margin-top: 40px; /* 为返回按钮留出空间 */
}

/* 创建按钮样式 */
.create-btn {
  background-color: #3498db;
  color: white;
  padding: 10px 20px;
  border-radius: 4px;
  border: none;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 5px;
}

/* 编辑布局样式 */
.editor-layout {
  display: flex;
  flex: 1;
  gap: 20px;
  height: calc(100% - 60px);
}

/* 章节列表样式 */
.chapter-list {
  width: 300px;
  border-right: 1px solid #eee;
  display: flex;
  flex-direction: column;
}

/* 搜索框样式 */
.search-box {
  padding: 10px;
  border-bottom: 1px solid #eee;
}

.search-box input {
  width: 100%;
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

/* 列表容器样式 */
.list-container {
  flex: 1;
  overflow-y: auto;
}

/* 章节项样式 */
.chapter-item {
  padding: 15px;
  border-bottom: 1px solid #f0f0f0;
  cursor: pointer;
  transition: background-color 0.2s;
  display: flex;
  flex-direction: column;
  position: relative;
}

/* 章节项悬停效果 */
.chapter-item:hover {
  background-color: #f8f9fa;
}

/* 活动章节样式 */
.chapter-item.active {
  background-color: #e3f2fd;
  border-left: 3px solid #3498db;
}

/* 章节标题样式 */
.chapter-title {
  font-weight: 500;
  margin-bottom: 5px;
  padding-right: 20px;
}

/* 章节元信息样式 */
.chapter-meta {
  display: flex;
  font-size: 12px;
  color: #666;
  gap: 10px;
}

/* 状态文本样式 */
.status {
  text-transform: capitalize;
}

/* 不同状态的颜色 */
.status.已发布 {
  color: #2ecc71;
}

.status.草稿 {
  color: #f39c12;
}

.status.审核中 {
  color: #3498db;
}

.status.封禁 {
  color: #e74c3c;
}

/* 删除按钮样式 */
.delete-btn {
  position: absolute;
  right: 10px;
  top: 10px;
  background: none;
  border: none;
  color: #999;
  cursor: pointer;
  font-size: 16px;
  width: 24px;
  height: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
}



/* 编辑区域样式 */
.editor-area {
  flex: 1;
  display: flex;
  flex-direction: column;
  background-color: #fff;
  border-radius: 4px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
}

/* 空编辑区提示样式 */
.empty-editor {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #999;
}

/* 编辑容器样式 */
.editor-container {
  display: flex;
  flex-direction: column;
  height: 100%;
}

/* 编辑头部样式 */
.editor-header {
  padding: 15px;
  border-bottom: 1px solid #eee;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

/* 章节标题输入框样式 */
.chapter-title-input {
  flex: 1;
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 16px;
}

/* 编辑工具栏样式 */
.editor-toolbar {
  display: flex;
  gap: 10px;
}

/* 保存按钮样式 */
.save-btn, .status-btn {
  padding: 8px 15px;
  border-radius: 4px;
  cursor: pointer;
}

.save-btn {
  background-color: #3498db;
  color: white;
  border: none;
}

/* 禁用状态保存按钮 */
.save-btn:disabled {
  background-color: #bdc3c7;
  cursor: not-allowed;
}

/* 状态按钮基础样式 */
.status-btn {
  background-color: #f8f9fa;
  border: 1px solid #ddd;
}

/* 禁用状态按钮 */
.status-btn:disabled {
  cursor: not-allowed;
  opacity: 0.7;
}

/* 不同状态按钮样式 */
.status-btn.已发布 {
  background-color: #e8f5e9;
  border-color: #c8e6c9;
  color: #2e7d32;
}

.status-btn.审核中 {
  background-color: #e3f2fd;
  border-color: #bbdefb;
  color: #0d47a1;
}

.status-btn.封禁 {
  background-color: #ffebee;
  border-color: #ffcdd2;
  color: #c62828;
}

/* 内容编辑区样式 */
.content-editor {
  flex: 1;
  padding: 15px;
  border: none;
  resize: none;
  font-family: inherit;
  font-size: 14px;
  line-height: 1.6;
}

/* 禁用状态编辑区 */
.content-editor:disabled {
  background-color: #f5f5f5;
  color: #999;
}

/* 编辑底部样式 */
.editor-footer {
  padding: 10px 15px;
  border-top: 1px solid #eee;
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 13px;
  color: #666;
}

/* 价格设置区域样式 */
.price-setting {
  display: flex;
  align-items: center;
  gap: 10px;
}

.price-setting input {
  width: 60px;
  padding: 5px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

/* 禁用状态价格输入 */
.price-setting input:disabled {
  background-color: #f5f5f5;
}

/* 最终价格显示样式 */
.final-price {
  font-weight: bold;
  color: #333;
}

/* 收费设置区域样式 */
.charge-setting {
  display: flex;
  align-items: center;
  gap: 5px;
}

.charge-setting select {
  padding: 5px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

/* 禁用状态收费选择 */
.charge-setting select:disabled {
  background-color: #f5f5f5;
}

/* 删除确认对话框样式 */
.confirm-dialog {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.dialog-content {
  background-color: white;
  padding: 20px;
  border-radius: 8px;
  width: 400px;
  max-width: 90%;
}

.dialog-content h3 {
  margin-top: 0;
  color: #333;
}

/* 对话框操作按钮区域 */
.dialog-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
}

/* 对话框取消按钮样式 */
.cancel-btn, .confirm-btn {
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
}

.cancel-btn {
  background-color: #f5f5f5;
  border: 1px solid #ddd;
}

/* 对话框确认按钮样式 */
.confirm-btn {
  background-color: #e74c3c;
  color: white;
  border: none;
}

.confirm-btn:hover {
  background-color: #c0392b;
}
</style>