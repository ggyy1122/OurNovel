<template>
  <div class="create-page">
    <!-- 页面标题和返回按钮 -->
    <div class="page-header">
      <router-link to="/author/novels" class="back-btn">
        <span>返回列表</span>
      </router-link>
    </div>
    <h2>创建新书</h2>
    
    <!-- 表单区域 -->
    <form class="book-form" @submit.prevent="submitForm">
      <!-- 书名输入 -->
      <div class="form-group" :class="{ 'error': errors.novel_name }">
        <label>书名 <span class="required">*</span></label>
        <input 
          type="text" 
          v-model="form.novel_name" 
          placeholder="请输入书名"
          @blur="validateField('novel_name')"
        >
        <span class="error-message" v-if="errors.novel_name">{{ errors.novel_name }}</span>
      </div>
      
      <!-- 分类选择 -->
      <div class="form-group" :class="{ 'error': errors.category }">
        <label>分类 <span class="required">*</span></label>
        <div class="multi-select-container">
          <div class="selected-tags">
            <span class="tag" v-for="(cat, index) in form.category" :key="index">
              {{ cat }}
              <button type="button" class="remove-tag" @click="removeCategory(index)">
                <i class="fas fa-times"></i>
              </button>
            </span>
          </div>
          <select 
            v-model="selectedCategory" 
            @change="addCategory"
            @blur="validateField('category')"
            class="multi-select"
          >
            <option value="" disabled>请选择分类（可多选）</option>
            <option 
              v-for="category in availableCategories" 
              :key="category" 
              :value="category"
              :disabled="form.category.includes(category)"
            >
              {{ category }}
            </option>
          </select>
          <span class="error-message" v-if="errors.category">{{ errors.category }}</span>
        </div>
      </div>
      
      <!-- 简介输入 -->
      <div class="form-group" :class="{ 'error': errors.introduction }">
        <label>简介 <span class="required">*</span></label>
        <textarea 
          rows="5" 
          v-model="form.introduction" 
          placeholder="请输入作品简介"
          @blur="validateField('introduction')"
        ></textarea>
        <span class="error-message" v-if="errors.introduction">{{ errors.introduction }}</span>
        <!-- 字数统计 -->
        <div class="word-count">{{ form.introduction.length }}/500</div>
      </div>
      
      <!-- 封面上传组 -->
      <div class="form-group" :class="{ 'error': errors.cover }">
        <label>封面 <span class="required">*</span></label>
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
                  <button type="button" class="remove-btn" @click.stop="removeImage">
                    <i class="fas fa-times"></i>
                  </button>
                </div>
              </div>
              <!-- 文件输入 -->
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
        <span class="error-message" v-if="errors.cover">{{ errors.cover }}</span>
      </div>
      
      <!-- 表单操作按钮 -->
      <div class="form-actions">
        <button type="submit" class="submit-btn" :disabled="isSubmitting">
          <span v-if="!isSubmitting">创建</span>
          <span v-else class="loading">提交中...</span>
        </button>
      </div>
    </form>
  </div>
</template>

<script>
// 导入方法状态
import { getAllCategories } from '@/API/Category_API'
import { addNovelCategory} from '@/API/NovelCategory_API'
import { AuthorcreateNovel } from '@/API/Novel_API'
import { uploadNovelCover } from '@/API/Novel_Cover_API'
import { authorStore } from '@/stores/CurrentAuthor'
import { novelsStore} from '@/stores/Novels'

export default {
  data() {
    return {
      categories: [],
      selectedCategory: '',
      form: {
        novel_name: '',
        category: [],
        introduction: '',
        cover: null
      },
      previewImage: null,
      errors: {
        novel_name: '',
        category: '',
        introduction: '',
        cover: ''
      },
      isSubmitting: false,
      loading: false
    }
  },
  computed: {
    // 计算可选的分类
    availableCategories() {
      return this.categories.filter(cat => !this.form.category.includes(cat))
    }
  },
  async created() {
    await this.fetchCategories()
  },
  methods: {
    // 获取分类数据
    async fetchCategories() {
      this.loading = true
      try {
        const res = await getAllCategories()
        this.categories = (res || [])
          .map(item => item?.categoryName)
          .filter(Boolean)
        if (!this.categories.length) throw new Error('无分类数据')
      } catch (e) {
        this.categories = ['玄幻', '都市', '科幻']
        console.error('获取分类失败:', e)
      } finally {
        this.loading = false
      }
    },
    // 表单相关操作
    addCategory() {
      if (this.selectedCategory && !this.form.category.includes(this.selectedCategory)) {
        this.form.category.push(this.selectedCategory)
        this.selectedCategory = ''
        this.errors.category = ''
      }
    },
    removeCategory(index) {
      this.form.category.splice(index, 1)
      this.validateField('category')
    },
    triggerFileInput() {
      this.$refs.fileInput.click()
    },
    handleFileChange(e) {
      const file = e.target.files[0]
      if (file) this.validateImage(file)
    },
    handleDrop(e) {
      e.preventDefault()
      const file = e.dataTransfer.files[0]
      if (file) this.validateImage(file)
    },
    validateImage(file) {
      const validTypes = ['image/jpeg', 'image/png']
      if (!validTypes.includes(file.type)) {
        this.errors.cover = '只支持JPG/PNG格式图片'
        return
      }
      
      const maxSize = 5 * 1024 * 1024
      if (file.size > maxSize) {
        this.errors.cover = '图片大小不能超过5MB'
        return
      }
      
      const reader = new FileReader()
      reader.onload = (e) => {
        this.previewImage = e.target.result
        this.form.cover = file
        this.errors.cover = ''
      }
      reader.readAsDataURL(file)
    },
    removeImage() {
      this.previewImage = null
      this.form.cover = null
      this.$refs.fileInput.value = ''
    },
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
        if (this.form.category.length === 0) {
          this.errors.category = '请至少选择一个分类'
        } else {
          this.errors.category = ''
        }
      }
      
      if (field === 'introduction') {
        if (!this.form.introduction.trim()) {
          this.errors.introduction = '简介不能为空'
        } else if (this.form.introduction.length > 500) {
          this.errors.introduction = '简介不能超过500字'
        } else {
          this.errors.introduction = ''
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
    // 验证表单
    validateForm() {
      this.validateField('novel_name')
      this.validateField('category')
      this.validateField('introduction')
      this.validateField('cover')
      
      return !Object.values(this.errors).some(error => error)
    },
    // 提交表单  
    async submitForm() {
      if (this.validateForm()) {
        this.isSubmitting = true
        
        try {
          const novelData = {
            authorId: authorStore.currentAuthor?.author_id,
            novelName: this.form.novel_name,
            introduction: this.form.introduction,
          }
          console.log(novelData)

          // 创建小说并获取小说ID
          const createdNovel = await AuthorcreateNovel(novelData);
          const novelId = createdNovel.novelId; 
          
          if (!novelId) {
            throw new Error('未能获取小说ID');
          }

          if (this.form.category.length > 0) {
            try {
              // 添加分类API
              await Promise.all(
                this.form.category.map(categoryName => 
                  addNovelCategory(novelId, categoryName)
                )
              );
              console.log('分类添加成功');
            } catch (categoryError) {
              console.error('添加分类失败:', categoryError);
              
            }
          }

          if (this.form.cover) {
            try {
              const coverUrl = await this.uploadNovelCover(novelId);
              console.log('封面上传成功:', coverUrl);
            } catch (uploadError) {
              console.error('封面上传失败:', uploadError);
            }
          }
          await novelsStore.fetchNovels();
          window.location.href = '/author/novels';
          await novelsStore.fetchNovels();
          alert('小说创建成功！')
        } catch (error) {
          console.error('创建小说失败:', error)
          alert('创建小说失败: ' + (error.response?.data?.message || error.message || '请重试'))
        } finally {
          this.isSubmitting = false
        }
      }
    },
    // 提交封面
    async uploadNovelCover(novelId) {
      if (!this.form.cover) {
        throw new Error('请先上传封面图片');
      }

      if (!novelId) {
        throw new Error('缺少小说ID');
      }
      
      try {
        const coverUrl = await uploadNovelCover(novelId, this.form.cover);
        console.log("创建封面"+coverUrl)
        return coverUrl;
      } catch (error) {
        console.error('封面上传失败:', error);
        throw error;
      }
    },
   
  }
}
</script>

<style scoped>

@import url('https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css');
/* 页面容器样式 */
.create-page {
  padding: 20px 30px;
  max-width: 1000px;
  margin: 0 auto;
}

/* 页面头部样式 */
.page-header {
  display: flex;
  align-items: center;
  margin-bottom: 25px;
  position: relative;
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

/* 页面标题样式 */
h2 {
  margin: 0;
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

/* 输入框样式 */
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
  height: 100%; 
  box-sizing: border-box; 
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
  margin-top: 30px;
  padding-top: 20px;
  border-top: 1px solid #eee;
}

/* 提交按钮样式 */
.submit-btn {
  padding: 10px 25px;
  border-radius: 6px;
  text-decoration: none;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.2s;
  background-color: #2ecc71;
  color: white;
  border: none;
}

.submit-btn:hover:not(:disabled) {
  background-color: #27ae60;
}

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

/* 多选分类容器样式 */
.multi-select-container {
  display: flex;
  flex-direction: column;
}

/* 已选标签容器 */
.selected-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-bottom: 8px;
}

/* 标签样式 */
.tag {
  display: inline-flex;
  align-items: center;
  background-color: #e8f4ff;
  color: #3498db;
  padding: 4px 12px;
  border-radius: 16px;
  font-size: 13px;
}

/* 移除标签按钮 */
.remove-tag {
  background: none;
  border: none;
  color: #3498db;
  margin-left: 6px;
  cursor: pointer;
  padding: 0;
  font-size: 12px;
}

.remove-tag:hover {
  color: #e74c3c;
}

/* 多选下拉框样式 */
.multi-select {
  width: 100%;
}
</style>