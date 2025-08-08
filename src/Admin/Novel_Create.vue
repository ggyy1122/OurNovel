<template>
  <div class="novel-create-container">
    <div class="header-row">
      <h2>创建新小说</h2>
      <button @click="goBack" class="back-link">
        返回列表
      </button>
    </div>
    <!-- 基础信息表单 -->
    <div class="form-section">
      <h3>基本信息</h3>
      <div class="form-group">
        <label>小说名称：</label>
        <input v-model="novelForm.title" placeholder="请输入小说名称">
      </div>
      
      <div class="form-group">
        <label>作者：</label>
        <input v-model="novelForm.author" placeholder="请输入作者">
      </div>
      
      <div class="form-group">
        <label>简介：</label>
        <textarea v-model="novelForm.description" rows="4" placeholder="请输入简介"></textarea>
      </div>
      
      <div class="form-group">
        <label>封面：</label>
        <input 
          type="file" 
          accept="image/*"
          @change="handleCoverUpload"
          ref="coverInput"
        >
        <div v-if="coverPreview" class="cover-preview">
          <img :src="coverPreview" alt="封面预览">
        </div>
      </div>
    </div>
    
    <!-- 确认按钮 -->
    <button 
      class="confirm-btn"
      @click="confirmNovelInfo"
      :disabled="!isBasicInfoValid"
    >
      确认基本信息
    </button>
    
    <!-- 章节编辑区域 -->
    <div v-if="showChapterSection" class="chapter-section">
      <h3>添加章节</h3>
      
      <div class="form-group">
        <label>章节标题：</label>
        <input v-model="chapterForm.title" placeholder="请输入章节标题">
      </div>
      
      <div class="form-group">
        <label>章节内容：</label>
        <textarea 
          v-model="chapterForm.content" 
          rows="10" 
          placeholder="请输入章节内容"
        ></textarea>
      </div>
      
      <div class="action-buttons">
        <button @click="addChapter" :disabled="!isChapterValid">添加章节</button>
        <button @click="finishCreation">完成创建</button>
      </div>
      
      <!-- 章节列表 -->
      <div v-if="chapters.length > 0" class="chapter-list">
        <h4>已添加章节 ({{ chapters.length }}章)</h4>
        <ul>
          <li v-for="(chapter, index) in chapters" :key="index">
            {{ chapter.title }}
            <button @click="removeChapter(index)">删除</button>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import {useRouter } from 'vue-router'
//----------------------------------------------------------------------------------------------------------------
//返回列表
const router = useRouter();
const goBack = () => {
  if (window.confirm('确定要返回吗？已编辑的数据将会丢失')) {
    router.go(-1); // 返回上一页
    // 或者指定路由：router.push('/reported-comments-list')
  }
};
//----------------------------------------------------------------------------------------------------------------
// 小说表单数据
const novelForm = ref({
  title: '',
  author: '',
  description: ''
});

// 封面相关
const coverInput = ref(null);
const coverPreview = ref('');
const coverFile = ref(null);

// 章节相关
const showChapterSection = ref(false);
const chapterForm = ref({
  title: '',
  content: ''
});
const chapters = ref([]);

// 计算属性验证表单
const isBasicInfoValid = computed(() => {
  return novelForm.value.title.trim() && 
         novelForm.value.author.trim() && 
         novelForm.value.description.trim();
});

const isChapterValid = computed(() => {
  return chapterForm.value.title.trim() && 
         chapterForm.value.content.trim();
});

// 处理封面上传
const handleCoverUpload = (event) => {
  const file = event.target.files[0];
  if (file) {
    coverFile.value = file;
    const reader = new FileReader();
    reader.onload = (e) => {
      coverPreview.value = e.target.result;
    };
    reader.readAsDataURL(file);
  }
};

// 确认基本信息
const confirmNovelInfo = () => {
  showChapterSection.value = true;
};

// 添加章节
const addChapter = () => {
  chapters.value.push({
    ...chapterForm.value
  });
  // 清空表单
  chapterForm.value = {
    title: '',
    content: ''
  };
};

// 删除章节
const removeChapter = (index) => {
  chapters.value.splice(index, 1);
};

// 完成创建
const finishCreation = async () => {
  const formData = new FormData();
  
  // 添加小说基本信息
  formData.append('title', novelForm.value.title);
  formData.append('author', novelForm.value.author);
  formData.append('description', novelForm.value.description);
  
  // 添加封面文件
  if (coverFile.value) {
    formData.append('cover', coverFile.value);
  }
  
  // 添加章节
  formData.append('chapters', JSON.stringify(chapters.value));
  
  try {
    // 这里替换为实际的API调用
    // const response = await api.createNovel(formData);
    console.log('提交数据:', {
      ...novelForm.value,
      cover: coverFile.value?.name,
      chapters: chapters.value
    });
    
    alert('小说创建成功！');
    // 重置表单
    resetForm();
  } catch (error) {
    console.error('创建失败:', error);
    alert('创建失败，请重试');
  }
};

// 重置表单
const resetForm = () => {
  novelForm.value = {
    title: '',
    author: '',
    description: ''
  };
  coverPreview.value = '';
  coverFile.value = null;
  chapters.value = [];
  showChapterSection.value = false;
  if (coverInput.value) {
    coverInput.value.value = '';
  }
};
</script>

<style scoped>
.novel-create-container {
  max-width: 1000px;
  margin: 0 auto;
  padding: 20px;
}

/* 新增头部行样式 */
.header-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 18px;
}

.form-section {
  margin-bottom: 20px;
  padding: 15px;
  border: 1px solid #eee;
  border-radius: 25px;
  background-color: #f9f9f9;
}

.form-group {
  margin-bottom: 15px;
  margin-left:15px;
  margin-right:30px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.cover-preview {
  margin-top: 10px;
}

.cover-preview img {
  max-width: 200px;
  max-height: 300px;
  border: 1px solid #ddd;
}

.confirm-btn {
  background-color: #42b983;
  color: white;
  padding: 10px 15px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.confirm-btn:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.chapter-section {
  margin-top: 30px;
  padding: 15px;
  border: 1px solid #eee;
  border-radius: 5px;
}

.action-buttons {
  margin: 15px 0;
  display: flex;
  gap: 10px;
}

.chapter-list {
  margin-top: 20px;
}

.chapter-list ul {
  list-style: none;
  padding: 0;
}

.chapter-list li {
  padding: 8px;
  border-bottom: 1px solid #eee;
  display: flex;
  justify-content: space-between;
}

.back-link {
  display: inline-block;/* 设为块级元素，允许设置padding */
  padding: 8px 16px;
  background-color: #fff;
  color: #000;
  text-decoration: none;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  font-weight: 600;
  transition: background-color 0.3s;
}

.back-link:hover {
  background-color: #42b983;
  text-decoration: none;
}
</style>