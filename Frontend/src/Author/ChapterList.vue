<template>
  <!-- 章节管理页面容器 -->
  <div class="chapterlist-page">
    <!-- 返回按钮 -->
    <button type="button" class="back-btn" @click="goBack">
      返回书籍
    </button>
    
    <!-- 页面头部 -->
    <div class="page-header">
      <h2>{{ novel.novel_name }} - 章节管理</h2>
      <!-- 创建新章节按钮 -->
      <button @click="createNewChapter(novel.novel_id)" class="create-btn">
        <span>+</span> 写新章节
      </button>
    </div>
    
    <!-- 加载状态显示 -->
    <div v-if="chaptersStore.isLoading" class="loading">加载中...</div>
    <div v-else-if="chaptersStore.error" class="error">{{ chaptersStore.error }}</div>
    
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
            :class="{ active: chaptersStore.activeChapterId === chapter.chapter_id }"
            @click="selectChapter(chapter)"
          >
            <!-- 章节信息 -->
            <div class="chapter-title">第{{chapter.chapter_id}}章 {{ chapter.title }}</div>
            <div class="chapter-meta">
              <span class="word-count">{{ chapter.word_count }}字</span>
              <span class="status" :class="chapter.status">
                {{ chapter.status === '首次审核' ? '审核中' : getStatusText(chapter.status) }}
              </span>
              <span class="price">¥{{ (chapter.calculated_price/100).toFixed(2) }}</span>
              <span class="publish-time" v-if="chapter.status === '已发布'">
                {{ formatDate(chapter.publish_time) }}
              </span>
              <!-- 添加查看审核记录按钮 -->
              <button 
                class="view-logs-btn"
                @click.stop="viewAuditLogs(chapter)"
              >
                审核记录
              </button>
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
            <input 
              type="text" 
              class="chapter-title-input" 
              v-model="activeChapter.title" 
              placeholder="输入章节标题"
            >
            <!-- 编辑工具栏 -->
            <div class="editor-toolbar">
              <!-- 保存按钮 -->
              <button 
                class="save-btn" 
                @click="saveChapter" 
                :disabled=" activeChapter.status === '已发布' || isTitleEmpty"
              >
                保存
              </button>
              <!-- 状态切换按钮 -->
              <button 
                class="status-btn" 
                :class="activeChapter.status"
                @click="toggleChapterStatus"
                :disabled="!canChangeStatus || isTitleEmpty"
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
            @keydown.enter="handleEnterKey"
            @input="updateWordCount"
          ></textarea>
          <!-- 编辑底部 -->
          <div class="editor-footer">
            <!-- 字数统计 -->
            <div class="word-count">字数: {{ activeChapter.word_count }}</div>
            <!-- 价格设置 -->
            <div class="price-setting">
              <label>每千字价格（虚拟币）: </label>
              <input 
                type="number" 
                v-model="activeChapter.price_per_kilo" 
                min="0" 
                step="1"
                @input="updateCalculatedPrice"
                @change="validatePrice"
              >
              <span class="final-price">最终价格: {{ Math.round(activeChapter.calculated_price) }}虚拟币（合人民币¥{{ (activeChapter.calculated_price/100).toFixed(2) }}）</span>
            </div>
            <!-- 收费设置 -->
            <div class="charge-setting">
              <label>是否收费:</label>
              <select 
                v-model="activeChapter.is_charged" 
              >
                <option value="是">是</option>
                <option value="否">否</option>
              </select>
            </div>
          </div>
        </div>
        <!-- 无选中章节 -->
        <div v-else class="empty-editor">
          <p>请从左侧选择章节或创建新章节</p>
        </div>
      </div>
    </div>

    <!-- 删除确认对话框 -->
    <div v-if="showDeleteConfirm" class="delete-dialog">
      <div class="dialog-content">
        <h3>确认删除章节？</h3>
        <p>确定要删除章节 "{{ chapterToDelete?.title }}" 吗？此操作无法撤销。</p>
        <div class="dialog-actions">
          <button @click="showDeleteConfirm = false" class="dialog-btn cancel">取消</button>
          <button @click="chaptersStore.deletethisChapter(chaptersStore.novelId, chapterToDelete.chapter_id)" class="dialog-btn confirm">确认删除</button>
        </div>
      </div>
    </div>

    <!-- 审核记录对话框 -->
    <div v-if="showLogsDialog" class="logs-dialog">
      <div class="dialog-content">
        <div class="dialog-header">
          <h3>章节审核记录</h3>
          <button class="close-btn" @click="showLogsDialog = false">×</button>
        </div>
        
        <div v-if="loadingLogs" class="loading-state">
          <i class="el-icon-loading"></i> 加载中...
        </div>
        
        <div v-else-if="managementLogs.length === 0" class="empty-state">
          暂无审核记录
        </div>
        
        <div v-else class="logs-container">
          <div class="log-item" v-for="log in managementLogs.slice(0, 5)" :key="log.ManagementId">
            <div class="log-time">{{ log.time }}</div>
            <div class="log-manager">管理员: {{ log.managerName }}</div>
            <div class="log-result">处理结果: {{ log.result }}</div>
          </div>
          
          <div v-if="managementLogs.length > 5" class="logs-tip">
            仅显示最近5条记录（共{{ managementLogs.length }}条）
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router'
import { useNovel } from '@/stores/CurrentNovel'
import { ChaptersStore } from '@/stores/Chapters'
import { storeToRefs } from 'pinia'
import { getChapterManagementLogs } from '@/API/ChapterManagement_API'

// 获取小说和章节相关状态和方法
const { novel } = useNovel()
const chaptersStore = ChaptersStore()
const { 
  activeChapter,
  showDeleteConfirm,
  chapterToDelete,
  searchQuery,
  canChangeStatus,
} = storeToRefs(chaptersStore);
const filteredChapters = computed(() => chaptersStore.filteredChapters);

const {
  createNewChapter,
  toggleChapterStatus,
  updateWordCount,
  updateCalculatedPrice,
  getStatusText,
  getStatusButtonText,
  formatDate,
  saveChapter
} = chaptersStore

// 审核记录相关状态
const managementLogs = ref([])
const loadingLogs = ref(false)
const showLogsDialog = ref(false)

// 返回上一级
const router = useRouter()

// 添加计算属性检查标题是否为空
const isTitleEmpty = computed(() => {
  return !activeChapter.value?.title?.trim();
});

onMounted(async () => {
  try {
    await chaptersStore.initialize(novel.value.novel_id)
  } catch (error) {
    console.error('初始化章节存储失败:', error)
    router.push('/error') // 跳转到错误页面
  }
})
//定价有效确认
const validatePrice = () => {
  if (activeChapter.value) {
    activeChapter.value.price_per_kilo = Math.round(activeChapter.value.price_per_kilo);
    if (activeChapter.value.price_per_kilo < 0) {
      activeChapter.value.price_per_kilo = 0;
    }
    updateCalculatedPrice();
  }
};

const goBack = () => {
  router.go(-1)
}

// 换行空格
const handleEnterKey = (event) => {
  if (event.key === 'Enter') {
    event.preventDefault();    
    if (!activeChapter.value.content) {
      activeChapter.value.content = '';
    }
    
    const cursorPos = event.target.selectionStart;
    const textBefore = activeChapter.value.content.substring(0, cursorPos);
    const textAfter = activeChapter.value.content.substring(cursorPos);
    
    activeChapter.value.content = textBefore + '\n　　' + textAfter;
    setTimeout(() => {
      event.target.selectionStart = cursorPos + 3;
      event.target.selectionEnd = cursorPos + 3;
    }, 0);
  }
};

// 选择章节
const selectChapter = (chapter) => {
  console.log('点击章节:', chapter) 
  chaptersStore.activeChapterId = chapter.chapter_id
}

// 确认删除章节
const confirmDeleteChapter = (chapter) => {
  console.log("传入的 chapter 参数:", chapter);
  
  try {
    if (!chapter?.chapter_id) {
      throw new Error("无效章节数据: " + JSON.stringify(chapter));
    }
    // 设置待删除章节
    chapterToDelete.value = chapter;
    showDeleteConfirm.value = true;
  } catch (error) {
    console.error("删除准备失败:", error);
  } finally {
    console.groupEnd();
  }
};

// 获取章节管理日志
const fetchManagementLogs = async (chapterId) => {
  try {
    loadingLogs.value = true
    const res = await getChapterManagementLogs(chapterId, novel.value.novel_id)
    
    // 关键修改：即使HTTP状态码为404，只要业务逻辑返回success:false且message为'未找到相关管理日志'
    if (res.status === 404 && res.data && res.data.success === false && res.data.message === '未找到相关管理日志') {
      managementLogs.value = []
      showLogsDialog.value = true
    } 
    else if (res.success) {
      managementLogs.value = res.data
      showLogsDialog.value = true
    }
    
  } catch (error) {
    console.error('获取管理记录失败:', error)
    managementLogs.value = []
    showLogsDialog.value = true
  } finally {
    loadingLogs.value = false
  }
}

// 查看审核记录
const viewAuditLogs = (chapter) => {
  if (chapter.chapter_id) {
    fetchManagementLogs(chapter.chapter_id)
  }
}
</script>

<style scoped>
/* 页面容器样式 */
.chapterlist-page {
  padding: 20px;
  height: calc(100vh - 40px);
  display: flex;
  flex-direction: column;
  position: relative;
}

/* 返回按钮 */
.back-btn {
  position: absolute;
  left: 20px;
  top: 20px;
  padding: 8px 16px;
  background-color: #f0f0f0; 
  color: #333; 
  text-decoration: none;
  border: 1px solid #ddd;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.3s;
  z-index: 1;
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
  background-color: #e3f2fd ;
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
  flex-wrap: wrap;
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
.status.首次审核 ,
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

/* 查看审核记录按钮样式 */
.view-logs-btn {
  background: none;
  border: none;
  color: #3498db;
  cursor: pointer;
  font-size: 12px;
  padding: 0;
  text-decoration: underline;
}

.view-logs-btn:hover {
  color: #2980b9;
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
.status-btn.首次审核,
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
  line-height: 2;
  text-indent: 2em;
  white-space: pre-wrap;
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
.dialog-btn.cancel {
  background-color: #f0f0f0;
  color: #333;
  border: 1px solid #ddd;
}

/* 确认按钮样式 */
.dialog-btn.confirm {
  background-color: #e74c3c;
  color: white;
  border: none;
}

/* 审核记录对话框样式 */
.logs-dialog {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1001;
}

.logs-dialog .dialog-content {
  background-color: white;
  width: 600px;
  max-height: 70vh;
  border-radius: 8px;
  padding: 20px;
  display: flex;
  flex-direction: column;
}

.dialog-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.close-btn {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  color: #999;
}

.logs-container {
  overflow-y: auto;
  flex: 1;
}

.log-item {
  padding: 12px 0;
  border-bottom: 1px solid #eee;
}

.log-item:last-child {
  border-bottom: none;
}

.log-time {
  font-weight: bold;
  color: #333;
  margin-bottom: 5px;
}

.log-manager {
  color: #666;
  margin-bottom: 5px;
}

.log-result {
  color: #444;
}

.loading-state, .empty-state {
  text-align: center;
  padding: 30px;
  color: #999;
}
</style>