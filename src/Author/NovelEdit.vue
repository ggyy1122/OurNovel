<template>
  <div class="edit-page">
    <!-- 返回按钮 -->
    <button type="button" class="back-btn " @click="goBack">
      返回书籍
    </button>
    
    <!-- 页面标题 -->
    <h2>小说信息</h2>
    
    <!-- 表单区域 -->
    <form class="settings-form">
      <!-- 第一表单区块 -->
      <div class="form-section">
        <!-- 小说名输入 -->
        <div class="form-group">
          <div class="form-row">
            <label>小说名</label>
            <div class="input-wrapper">
              <input 
                type="text" 
                v-model="novel.novel_name" 
                :disabled="!editMode"
                :class="{ 'disabled-input': !editMode }"
              >
            </div>
          </div>
        </div>
        
        <!-- 分类展示 -->
        <div class="form-group">
          <div class="form-row">
            <label>分类</label>
            <div class="input-wrapper">
              <input 
                type="text" 
                v-model="novel.categories" 
                disabled
                class="disabled-input"
              >
            </div>
          </div>
        </div>
        
        <!-- 状态选择 -->
        <div class="form-group">
          <div class="form-row">
            <label>状态</label>
            <div class="status-options">
              <label v-for="status in filteredStatusOptions" :key="status.value">
                <input 
                  type="radio" 
                  v-model="novel.status" 
                  :value="status.value"
                  :disabled="!editMode"
                >
                <span :class="{ 'disabled-text': !editMode }">{{ status.label }}</span>
              </label>
            </div>
          </div>
        </div>
      </div>
      
      <!-- 第二表单区块 -->
      <div class="form-section">
        <!-- 简介输入 -->
        <div class="form-group">
          <div class="form-row">
            <label>简介</label>
            <div class="input-wrapper">
              <textarea 
                rows="6" 
                v-model="novel.introduction" 
                :disabled="!editMode"
                :class="{ 'disabled-input': !editMode }"
              ></textarea>
              <!-- 字数统计 -->
              <div class="word-count">{{ novel.introduction.length }}/500</div>
            </div>
          </div>
        </div>
      </div>

      <!-- 封面上传区块 -->
      <div class="form-group">
        <div class="form-row">  
          <label>封面</label>
          <div class="upload-container">
            <!-- 左侧上传区域 -->
            <div class="upload-left">
              <div class="avatar-upload">
                <div class="avatar-preview">
                  <img :src="novel.cover_url" class="novel-cover" @error="handleImageError">
                </div>
                
                <!-- 上传按钮 -->
                <div class="upload-controls">
                  <button 
                    type="button" 
                    class="upload-btn" 
                    @click="triggerFileInput"
                    :disabled="!editMode"
                    :class="{ 'disabled-btn': !editMode }"
                  >
                    更换封面
                  </button>
                  <!-- 隐藏的文件输入 -->
                  <input 
                    type="file" 
                    ref="fileInput"
                    accept="image/*" 
                    @change="handleFileChange"
                    style="display: none;"
                  >
                </div>
              </div>
            </div>
            
            <!-- 右侧注意事项 -->
            <div class="upload-right">
              <div class="upload-notice">
                <h3>注意</h3>
                <ul class="notice-list">
                  <li>请确保上传的图片及其字体等不侵犯任何人权益，如有侵权须自行承担赔偿等责任！</li>
                  <li>上传600x800像素、不超过5M的JPG/JPEG图片；</li>
                  <li>作品封面应显示作品名和笔名；</li>
                  <li>严禁上传色情、暴力、广告宣传或不适合公众观赏的图片，一经发现即做禁书处理；</li>
                  <li>封面上传后，将在2个工作日内审核完成。</li>
                </ul>
              </div>
            </div>
          </div>
        </div>
      </div>
      
      <!-- 表单操作按钮 -->
      <div class="form-actions">
        <!-- 非编辑模式下显示编辑按钮 -->
        <template v-if="!editMode">
          <button type="button" class="edit-btn" @click="enterEditMode">
            修改信息
          </button>
        </template>
        <!-- 编辑模式下显示取消和保存按钮 -->
        <template v-if="editMode">
          <button type="button" class="cancel-btn" @click="cancelChanges">
            取消
          </button>
          <button type="submit" class="submit-btn" @click.prevent="saveNovel">
            保存更改
          </button>
        </template>
      </div>
    </form>
  </div>
</template>

<script setup>
// 导入小说状态管理
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useNovel } from '@/stores/CurrentNovel'
// 解构需要的状态和方法
const {
  novel,
  editMode,
  statusOptions,
  fileInput,
  enterEditMode,
  cancelChanges,
  triggerFileInput,
  handleFileChange,
  handleImageError,
  saveNovel
} = useNovel()
//返回上一级
const router = useRouter()  // 先获取实例
const goBack = () => {
  router.go(-1)              // 再调用方法
}

// 只显示连载和完结两个状态选项
const filteredStatusOptions = computed(() => {
  return statusOptions.filter(option => 
    option.value === '连载' || option.value === '完结'
  )
})
</script>

<style scoped>
/* 页面容器样式 */
.edit-page {
  padding: 30px;
  max-width: 1000px;
  margin: 0 auto;
  position: relative;
}

/* 按钮基础样式 */
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

.top-back-btn:hover {
  background-color: #e0e0e0;
}

/* 页面标题样式 */
h2 {
  margin-bottom: 25px;
  color: #2c3e50;
  font-size: 24px;
  text-align: left;
}

/* 表单整体样式 */
.settings-form {
  background: white;
  padding: 30px;
  border-radius: 5px;
  box-shadow: 0 4px 15px rgba(0,0,0,0.08);
}

/* 表单区块样式 */
.form-section {
  margin-bottom: 30px;
  padding-bottom: 20px;
  border-bottom: 1px solid #f0f0f0;
}

/* 最后一个区块不需要下边框 */
.form-section:last-child {
  border-bottom: none;
}

/* 表单行布局 */
.form-row {
  display: flex;
  align-items: flex-start;
  margin-bottom: 15px;
}

/* 标签样式 */
.form-row label {
  min-width: 100px;
  font-weight: 600;
  color: #34495e;
  padding-top: 10px;
}

/* 输入框容器 */
.input-wrapper {
  margin-top: 6px;
  flex: 1;
}

/* 禁用状态输入框样式 */
.disabled-input {
  background: transparent;
  border: none;
  padding: 8px 0;
  width: 100%;
  color: #333;
  font-size: 14px;
}

/* 正常输入框样式 */
input:not(.disabled-input),
textarea:not(.disabled-input) {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 14px;
  transition: all 0.3s;
}

/* 输入框聚焦效果 */
input:not(.disabled-input):focus,
textarea:not(.disabled-input):focus {
  border-color: #3498db;
  outline: none;
  box-shadow: 0 0 0 2px rgba(52, 152, 219, 0.2);
}

/* 文本区域样式 */
textarea {
  resize: vertical;
  min-height: 120px;
}

/* 字数统计样式 */
.word-count {
  text-align: right;
  font-size: 12px;
  color: #95a5a6;
  margin-top: 5px;
}

/* 上传容器布局 */
.upload-container {
  display: flex;
  gap: 20px;
}

/* 左侧上传区域 */
.upload-left {
  flex: 1;
}

/* 右侧上传区域 */
.upload-right {
  flex: 1;
}

/* 封面上传区域样式 */
.avatar-upload {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

/* 封面预览区域 */
.avatar-preview {
  width: 240px;
  height: 320px;
  border-radius: 6px;
  overflow: hidden;
  background-color: #f5f5f5;
}

/* 封面图片样式 */
.novel-cover {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

/* 上传按钮样式 */
.upload-btn {
  padding: 8px 15px;
  background: #f8f9fa;
  border: 1px solid #ddd;
  border-radius: 4px;
  color: #3498db;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.3s;
}

/* 上传按钮悬停效果 */
.upload-btn:hover:not(:disabled) {
  background: #e6f2ff;
}

/* 禁用按钮样式 */
.disabled-btn {
  border: 1px solid #ccc;
  color: #ccc;
  cursor: not-allowed;
}

/* 状态选项布局 */
.status-options {
  display: flex;
  gap: 20px;
}

/* 状态选项标签样式 */
.status-options label {
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
  padding: 8px 12px;
  border-radius: 4px;
  transition: all 0.2s;
}

/* 状态选项悬停效果 */
.status-options label:hover {
  background-color: #f5f5f5;
}

/* 禁用状态文本样式 */
.disabled-text {
  color: #333;
}

/* 右侧注意事项区域样式 */
.upload-notice {
  background-color: #f8f9fa;
  border-radius: 6px;
  padding: 20px;
  height: 80%;
}

/* 注意事项标题 */
.upload-notice h3 {
  font-size: 16px;
  color: #2c3e50;
  margin-bottom: 15px;
  padding-bottom: 10px;
  border-bottom: 1px solid #eee;
}

/* 注意事项列表 */
.notice-list {
  list-style-type: none;
  margin-bottom: 15px;
}

/* 注意事项列表项 */
.notice-list li {
  position: relative;
  padding-left: 15px;
  margin-bottom: 10px;
  font-size: 13px;
  line-height: 1.5;
  color: #555;
}

/* 列表项前导符号 */
.notice-list li:before {
  content: "•";
  position: absolute;
  left: 0;
  color: #3498db;
}

/* 表单操作按钮区域 */
.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 15px;
  margin-top: 50px;
}

/* 编辑按钮样式 */
.edit-btn {
  padding: 10px 20px;
  background-color: #3498db;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.3s;
}

/* 编辑按钮悬停效果 */
.edit-btn:hover {
  background-color: #2980b9;
}

/* 取消按钮样式 */
.cancel-btn {
  padding: 10px 20px;
  background-color: #f5f5f5;
  color: #7f8c8d;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.3s;
}

/* 取消按钮悬停效果 */
.cancel-btn:hover {
  background-color: #e0e0e0;
}

/* 提交按钮样式 */
.submit-btn {
  padding: 10px 20px;
  background-color: #2ecc71;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.3s;
}

/* 提交按钮悬停效果 */
.submit-btn:hover {
  background-color: #27ae60;
}
</style>