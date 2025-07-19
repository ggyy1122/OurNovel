<template>
  <!-- 创建新书页面容器 -->
  <div class="page-container">
    <!-- 页面标题 -->
    <h2>创建新书</h2>
    
    <!-- 表单区域 -->
    <form class="book-form" @submit.prevent="submitForm">
      <!-- 书名输入组 -->
      <div class="form-group" :class="{ 'error': errors.novel_name }">
        <label>书名 <span class="required">*</span></label>
        <input 
          type="text" 
          v-model="form.novel_name" 
          placeholder="请输入书名"
          @blur="validateField('novel_name')"
        >
        <!-- 错误提示 -->
        <span class="error-message" v-if="errors.novel_name">{{ errors.novel_name }}</span>
      </div>
      
      <!-- 分类选择组 -->
      <div class="form-group" :class="{ 'error': errors.category }">
        <label>分类 <span class="required">*</span></label>
        <select v-model="form.category" @change="validateField('category')">
          <option value="">选择分类</option>
          <!-- 分类选项循环 -->
          <option v-for="category in categories" :key="category" :value="category">{{ category }}</option>
        </select>
        <!-- 错误提示 -->
        <span class="error-message" v-if="errors.category">{{ errors.category }}</span>
      </div>
      
      <!-- 简介输入组 -->
      <div class="form-group" :class="{ 'error': errors.description }">
        <label>简介 <span class="required">*</span></label>
        <textarea 
          rows="5" 
          v-model="form.description" 
          placeholder="请输入作品简介"
          @blur="validateField('description')"
        ></textarea>
        <!-- 错误提示 -->
        <span class="error-message" v-if="errors.description">{{ errors.description }}</span>
        <!-- 字数统计 -->
        <div class="word-count">{{ form.description.length }}/500</div>
      </div>
      
      <!-- 封面上传组 -->
      <div class="form-group" :class="{ 'error': errors.cover }">
        <label>封面 <span class="required">*</span></label>
        <!-- 上传容器 -->
        <div class="upload-container">
          <!-- 左侧上传区域 -->
          <div class="upload-left">
            <!-- 上传区域 -->
            <div class="upload-area" @click="triggerFileInput" @dragover.prevent @drop="handleDrop">
              <!-- 无图片时的占位符 -->
              <div v-if="!previewImage" class="upload-placeholder">
                <div class="upload-icon-container">
                  <i class="fas fa-image upload-icon"></i>
                </div>
                <div class="upload-text">
                  <p class="upload-title">点击上传封面</p>
                  <p class="upload-hint">支持JPG/PNG格式，建议尺寸600×800</p>
                </div>
              </div>
              <!-- 有图片时的预览 -->
              <div v-else class="preview-container">
                <div class="preview-wrapper">
                  <img :src="previewImage" alt="封面预览" class="preview-image">
                  <!-- 删除按钮 -->
                  <button type="button" class="remove-btn" @click.stop="removeImage">
                    <i class="fas fa-times"></i>
                  </button>
                </div>
              </div>
              <!-- 隐藏的文件输入 -->
              <input 
                type="file" 
                ref="fileInput"
                accept="image/jpeg, image/png" 
                @change="handleFileChange"
              >
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
        <!-- 错误提示 -->
        <span class="error-message" v-if="errors.cover">{{ errors.cover }}</span>
      </div>
      
      <!-- 表单操作按钮 -->
      <div class="form-actions">
        <!-- 取消按钮 -->
        <router-link to="/author/novels" class="cancel-btn">取消</router-link>
        <!-- 提交按钮 -->
        <button type="submit" class="submit-btn" :disabled="isSubmitting">
          <span v-if="!isSubmitting">创建</span>
          <span v-else class="loading">提交中...</span>
        </button>
      </div>
    </form>
  </div>
</template>

<script>
export default {
  data() {
    return {
      // 分类选项
      categories: ['玄幻', '都市', '科幻', '武侠', '言情', '历史', '悬疑', '军事', '游戏', '体育'],
      // 表单数据
      form: {
        novel_name: '',
        category: '',
        description: '',
        cover: null
      },
      // 封面预览图
      previewImage: null,
      // 错误信息
      errors: {
        novel_name: '',
        category: '',
        description: '',
        cover: ''
      },
      // 提交状态
      isSubmitting: false
    }
  },
  methods: {
    // 触发文件选择
    triggerFileInput() {
      this.$refs.fileInput.click()
    },
    // 处理文件选择
    handleFileChange(e) {
      const file = e.target.files[0]
      if (file) {
        this.validateImage(file)
      }
    },
    // 处理拖放上传
    handleDrop(e) {
      e.preventDefault()
      const file = e.dataTransfer.files[0]
      if (file) {
        this.validateImage(file)
      }
    },
    // 验证图片
    validateImage(file) {
      // 检查文件类型
      const validTypes = ['image/jpeg', 'image/png']
      if (!validTypes.includes(file.type)) {
        this.errors.cover = '只支持JPG/PNG格式图片'
        return
      }
      
      // 检查文件大小 (5MB限制)
      const maxSize = 5 * 1024 * 1024
      if (file.size > maxSize) {
        this.errors.cover = '图片大小不能超过5MB'
        return
      }
      
      // 创建预览
      const reader = new FileReader()
      reader.onload = (e) => {
        this.previewImage = e.target.result
        this.form.cover = file
        this.errors.cover = ''
      }
      reader.readAsDataURL(file)
    },
    // 移除图片
    removeImage() {
      this.previewImage = null
      this.form.cover = null
      this.$refs.fileInput.value = ''
    },
    // 验证字段
    validateField(field) {
      if (field === 'novel_name') {
        if (!this.form.novel_name.trim()) {
          this.errors.novel_name = '书名不能为空'
        } else if (this.form.novel_name.length > 20) {
          this.errors.novel_name = '书名不能超过20个字符'
        } else {
          this.errors.novel_name = ''
        }
      }
      
      if (field === 'category') {
        if (!this.form.category) {
          this.errors.category = '请选择分类'
        } else {
          this.errors.category = ''
        }
      }
      
      if (field === 'description') {
        if (!this.form.description.trim()) {
          this.errors.description = '简介不能为空'
        } else if (this.form.description.length > 500) {
          this.errors.description = '简介不能超过500字'
        } else {
          this.errors.description = ''
        }
      }
      
      if (field === 'cover') {
        if (!this.form.cover) {
          this.errors.cover = '请上传封面图片'
        } else {
          this.errors.cover = ''
        }
      }
    },
    // 验证整个表单
    validateForm() {
      this.validateField('novel_name')
      this.validateField('category')
      this.validateField('description')
      this.validateField('cover')
      
      return !Object.values(this.errors).some(error => error)
    },
    // 提交表单
    submitForm() {
      if (this.validateForm()) {
        this.isSubmitting = true
        
        // 模拟API请求
        setTimeout(() => {
          console.log('提交表单:', this.form)
          this.isSubmitting = false
          this.$router.push('/author/novels')
        }, 1500)
      }
    }
  }
}
</script>

<style scoped>
/* 导入字体图标库 */
@import url('https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css');

/* 页面容器样式 */
.page-container {
  padding: 30px;
  max-width: 1000px;
  margin: 0 auto;
}

/* 页面标题样式 */
h2 {
  margin-bottom: 25px;
  color: #2c3e50;
  font-size: 24px;
  font-weight: 600;
}

/* 表单样式 */
.book-form {
  background: white;
  padding: 30px;
  border-radius: 8px;
  box-shadow: 0 2px 15px rgba(0,0,0,0.05);
}

/* 表单组样式 */
.form-group {
  margin-bottom: 25px;
  position: relative;
}

/* 标签样式 */
.form-group label {
  display: block;
  margin-bottom: 8px;
  font-weight: 600;
  color: #34495e;
}

/* 必填标记样式 */
.required {
  color: #e74c3c;
  margin-left: 4px;
}

/* 输入框通用样式 */
.form-group input,
.form-group select,
.form-group textarea {
  width: 100%;
  padding: 12px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 14px;
  transition: border-color 0.3s;
}

/* 输入框聚焦效果 */
.form-group input:focus,
.form-group select:focus,
.form-group textarea:focus {
  border-color: #3498db;
  outline: none;
  box-shadow: 0 0 0 2px rgba(52, 152, 219, 0.2);
}

/* 错误状态样式 */
.form-group.error input,
.form-group.error select,
.form-group.error textarea,
.form-group.error .upload-area {
  border-color: #e74c3c;
}

/* 错误状态聚焦效果 */
.form-group.error input:focus,
.form-group.error select:focus,
.form-group.error textarea:focus {
  box-shadow: 0 0 0 2px rgba(231, 76, 60, 0.2);
}

/* 错误提示信息样式 */
.error-message {
  display: block;
  margin-top: 8px;
  color: #e74c3c;
  font-size: 13px;
}

/* 文本区域样式 */
.form-group textarea {
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
  align-items: stretch; 
}

/* 左侧上传区域 */
.upload-left {
  flex: 1;
  min-height: 400px;
  width: 240px;
  height: 500px;
}

/* 右侧上传区域 */
.upload-right {
  flex: 1;
  min-height: 400px; 
  width: 240px;
  height: 500px;
}

/* 上传区域样式 */
.upload-area {
  border: 1px dashed #bdc3c7;
  border-radius: 6px;
  padding: 20px;
  text-align: center;
  cursor: pointer;
  transition: all 0.3s;
  position: relative;
  overflow: hidden;
  background-color: #f8f9fa;
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  box-sizing: border-box;
}

/* 右侧注意事项区域样式 */
.upload-notice {
  background-color: #f8f9fa;
  border-radius: 6px;
  padding: 20px;
  height: 100%; /* 改为100%填充父容器 */
  box-sizing: border-box; /* 确保padding不影响高度计算 */
}

/* 上传区域悬停效果 */
.upload-area:hover {
  border-color: #3498db;
  background-color: #f0f7ff;
}

/* 上传占位符样式 */
.upload-placeholder {
  display: flex;
  flex-direction: column;
  align-items: center;
  color: #7f8c8d;
}

/* 上传图标容器 */
.upload-icon-container {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  background-color: #e8f4ff;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 15px;
}

/* 上传图标样式 */
.upload-icon {
  font-size: 24px;
  color: #3498db;
}

/* 上传文字区域 */
.upload-text {
  text-align: center;
}

/* 上传标题样式 */
.upload-title {
  font-size: 16px;
  font-weight: 500;
  color: #2c3e50;
  margin-bottom: 5px;
}

/* 上传提示样式 */
.upload-hint {
  font-size: 12px;
  color: #95a5a6;
}

/* 隐藏文件输入 */
.upload-area input[type="file"] {
  display: none;
}

/* 预览容器样式 */
.preview-container {
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
}

/* 预览包装器样式 */
.preview-wrapper {
  position: relative;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #f8f9fa;
  border-radius: 4px;
  overflow: hidden;
}

/* 预览图片样式 */
.preview-image {
  max-width: 100%;
  max-height: 100%;
  object-fit: contain;
}

/* 删除按钮样式 */
.remove-btn {
  position: absolute;
  top: 10px;
  right: 10px;
  width: 28px;
  height: 28px;
  background: rgba(0,0,0,0.6);
  color: white;
  border: none;
  border-radius: 50%;
  font-size: 14px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s;
}

/* 删除按钮悬停效果 */
.remove-btn:hover {
  background: rgba(231, 76, 60, 0.8);
}

/* 右侧注意事项区域样式 */
.upload-notice {
  background-color: #f8f9fa;
  border-radius: 6px;
  padding: 20px;
  height: 100%;
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
  margin-top: 30px;
  padding-top: 20px;
  border-top: 1px solid #eee;
}

/* 取消按钮样式 */
.cancel-btn,
.submit-btn {
  padding: 10px 25px;
  border-radius: 6px;
  text-decoration: none;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.2s;
}

/* 取消按钮样式 */
.cancel-btn {
  background-color: #f1f1f1;
  color: #7f8c8d;
  border: 1px solid #ddd;
}

/* 取消按钮悬停效果 */
.cancel-btn:hover {
  background-color: #e0e0e0;
}

/* 提交按钮样式 */
.submit-btn {
  background-color: #2ecc71;
  color: white;
  border: none;
}

/* 提交按钮悬停效果 */
.submit-btn:hover:not(:disabled) {
  background-color: #27ae60;
}

/* 禁用状态提交按钮 */
.submit-btn:disabled {
  background-color: #95a5a6;
  cursor: not-allowed;
}

/* 加载动画样式 */
.loading {
  display: inline-block;
  position: relative;
  width: 20px;
  height: 20px;
}

/* 加载动画旋转效果 */
.loading:after {
  content: " ";
  display: block;
  width: 16px;
  height: 16px;
  margin: 2px;
  border-radius: 50%;
  border: 2px solid #fff;
  border-color: #fff transparent #fff transparent;
  animation: loading 1.2s linear infinite;
}

/* 加载动画关键帧 */
@keyframes loading {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

</style>