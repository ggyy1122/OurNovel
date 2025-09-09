<template>
  <div class="category-manager">
    <!-- 加载状态 -->
    <div v-if="loading" class="loading-wrapper">
      <div class="loading-spinner">
        <div class="spinner"></div>
        <span>加载中...</span>
      </div>
    </div>

    <div v-else>
      <!-- 标题移出内容框 -->
      <h2 class="section-title">分类管理</h2>
      
      <!-- 内容区域 -->
      <div class="content-wrapper">
        <!-- 分类列表 -->
        <div class="category-list">
          <div v-for="cat in categories" :key="cat.id" class="category-card">
            <div class="category-info">
              <span class="category-name">{{ cat.categoryName }}</span>
            </div>
            <div class="category-actions">
              <button 
                @click="showModal=true;index=2;name=cat.categoryName"
                class="action-button edit-button"
              >
                <svg class="icon" viewBox="0 0 24 24">
                  <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z"/>
                </svg>
              </button>
              <button 
                @click="name=cat.categoryName;deleteMyCategory()"
                class="action-button delete-button"
              >
                <svg class="icon" viewBox="0 0 24 24">
                  <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z"/>
                </svg>
              </button>
            </div>
          </div>
        </div>

        <!-- 添加按钮 -->
        <button 
          @click="showModal=true;index=1" 
          class="add-category-button"
        >
          <svg class="add-icon" viewBox="0 0 24 24">
            <path d="M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z"/>
          </svg>
          <span>添加分类</span>
        </button>

        <!-- 模态框 -->
        <div v-if="showModal" class="modal-overlay" @click.self="cancelInput">
          <div class="modal-container">
            <div class="modal-header">
              <h3 class="modal-title">分类名称</h3>
              <button @click="cancelInput" class="modal-close-button">
                <svg viewBox="0 0 24 24">
                  <path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/>
                </svg>
              </button>
            </div>
            <div class="modal-body">
              <input
                v-model="inputValue"
                @keyup.enter="confirmInput"
                placeholder="请输入分类名称"
                class="modal-input"
                autofocus
              >
            </div>
            <div class="modal-footer">
              <button @click="cancelInput" class="modal-button cancel-button">取消</button>
              <button @click="confirmInput" class="modal-button confirm-button">确认</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>


<script setup>
import { ref, onMounted } from 'vue';
import { getAllCategories, createCategory, renameCategory, deleteCategory } from '@/API/Category_API';
import { getNovelsByCategory } from '@/API/NovelCategory_API';

const loading = ref(false);
const categories = ref([]);
const getCategoryData = async () => {
  loading.value = true;
  try {
    const response = await getAllCategories();
    if (response) {
      categories.value = response;
    }
  } catch (error) {
    console.error('获取分类数据失败:', error);
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  getCategoryData();
});

const index = ref(0);
const showModal = ref(false);
const inputValue = ref('');
const result = ref('');
const name = ref('');

const isRepeat = (newName) => {
  return categories.value.some(category => 
    category.categoryName === newName
  );
};

const confirmInput = () => {
  if (!inputValue.value.trim()) {
    alert('请输入有效的内容');
  } else if (isRepeat(inputValue.value.trim())) {
    alert('分类名已存在，请重新输入');
    inputValue.value = '';
  } else {
    result.value = inputValue.value.trim();
    inputValue.value = '';
    showModal.value = false;
    if (index.value === 1) {
      addCategory();
    } else if (index.value === 2) {
      editCategory();
    }
  }
};

const cancelInput = () => {
  showModal.value = false;
  inputValue.value = '';
};

const addCategory = async () => {
  loading.value = true;
  try {
    const newData = {
      categoryName: result.value
    };
    await createCategory(newData);
    getCategoryData();
  } catch (error) {
    console.error('添加分类失败:', error);
    alert('添加分类失败，请重试。');
  } finally {
    loading.value = false;
  }
};

const editCategory = async () => {
  loading.value = true;
  try {
    await renameCategory(name.value, result.value);
    getCategoryData();
  } catch (error) {
    console.error('编辑分类失败:', error);
    alert('编辑分类失败，请重试。');
  } finally {
    loading.value = false;
  }
};

const deleteMyCategory = async () => {
  loading.value = true;
  try {
    const response = await getNovelsByCategory(name.value);
    if (response && response.length > 0) {
      alert('该分类下存在小说，无法删除');
      return;
    }
    await deleteCategory(name.value);
    getCategoryData();
  } catch (error) {
    console.error('删除分类失败:', error);
    alert('删除分类失败，请重试。');
  } finally {
    loading.value = false;
  }
};
</script>


<style scoped>
/* 基础样式 */
.category-manager {
  font-family: 'Segoe UI', 'PingFang SC', sans-serif;
  max-width: 1000px;
  margin: 0 auto;
  padding: 2rem;
}

/* 标题样式 */
.section-title {
  font-size: 1.5rem;
  color: #333;
  margin-bottom: 1rem;
  font-weight: 600;
  padding-left: 0.5rem;
}

/* 加载动画 */
.loading-wrapper {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 300px;
}

.loading-spinner {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.spinner {
  width: 50px;
  height: 50px;
  border: 4px solid rgba(0, 123, 255, 0.2);
  border-radius: 50%;
  border-top-color: #007bff;
  animation: spin 1s ease-in-out infinite;
  margin-bottom: 1rem;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

/* 内容区域 */
.content-wrapper {
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.05);
  padding: 2rem;
}

/* 分类列表 */
.category-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 1rem;
  margin-bottom: 2rem;
}

.category-card {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: #f9f9f9;
  border-radius: 8px;
  padding: 1rem 1.5rem;
  transition: all 0.3s ease;
  border: 1px solid #eee;
}

.category-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
  border-color: #007bff;
}

.category-name {
  font-size: 1rem;
  color: #333;
  font-weight: 500;
}

.category-actions {
  display: flex;
  gap: 0.5rem;
}

.action-button {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  border: none;
  cursor: pointer;
  transition: all 0.2s;
  background: transparent;
}

.action-button:hover {
  transform: scale(1.1);
}

.edit-button:hover {
  background: rgba(0, 123, 255, 0.1);
}

.delete-button:hover {
  background: rgba(220, 53, 69, 0.1);
}

.icon {
  width: 18px;
  height: 18px;
  fill: #666;
}

.edit-button .icon {
  fill: #486482;
}

.delete-button .icon {
  fill: #90555f;
}

/* 添加按钮 */
.add-category-button {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  width: 100%;
  padding: 0.75rem;
  background: #486482;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s;
}

.add-category-button:hover {
  background: #486482;
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(0, 123, 255, 0.2);
}

.add-icon {
  width: 20px;
  height: 20px;
  fill: white;
}

/* 模态框 */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  backdrop-filter: blur(4px);
}

.modal-container {
  background: white;
  border-radius: 12px;
  width: 90%;
  max-width: 400px;
  overflow: hidden;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.2);
  animation: modalFadeIn 0.3s ease-out;
}

@keyframes modalFadeIn {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem 1.5rem 1rem;
  border-bottom: 1px solid #eee;
}

.modal-title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #333;
  margin: 0;
}

.modal-close-button {
  background: none;
  border: none;
  cursor: pointer;
  padding: 0.25rem;
  border-radius: 50%;
  transition: background 0.2s;
}

.modal-close-button:hover {
  background: #f5f5f5;
}

.modal-close-button svg {
  width: 20px;
  height: 20px;
  fill: #666;
}

.modal-body {
  padding: 1.5rem;
}

.modal-input {
  width: 100%;
  padding: 0.75rem 1rem;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
  transition: border 0.3s;
}

.modal-input:focus {
  outline: none;
  border-color:#486482;
  box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.2);
}

.modal-footer {
  display: flex;
  justify-content: flex-end;
  padding: 1rem 1.5rem;
  border-top: 1px solid #eee;
  gap: 0.75rem;
}

.modal-button {
  padding: 0.5rem 1.25rem;
  border-radius: 6px;
  font-size: 0.9rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
}

.cancel-button {
  background: #f8f9fa;
  color: #333;
  border: 1px solid #ddd;
}

.cancel-button:hover {
  background: #e9ecef;
}

.confirm-button {
  background: #486482;
  color: white;
  border: 1px solid #486482;
}

.confirm-button:hover {
  background:#3a5169;
  border-color: #486482;
}
</style>