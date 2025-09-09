<template>
  <div v-if="loading" class="loading-indicator">
      <div class="spinner"></div>
      <span>加载中...</span>
  </div>
  <div v-else>
    <div class="category-manager">
      <ul class="category-list">
        <li v-for="cat in categories" :key="cat.id">
          <div class="category-item"> <!-- 添加一个外层容器 -->
            <div class="border-style-8">
              <div class="content">
                <span>{{ cat.categoryName }}</span>
              </div>
            </div>
            <div class="icon-container"> <!-- 修改类名并添加样式 -->
              <i class="fas fa-pen edit-icon" title="编辑" @click="showModal=true;index=2;name=cat.categoryName"></i>
              <i class="fas fa-minus minus-icon" title="删除" @click="name=cat.categoryName;deleteMyCategory()"></i>
            </div>
          </div>
        </li>
      </ul>
      <!-- 可扩展：添加新分类按钮等 -->
       <button class="btn-animate btn-animate__surround" @click="showModal=true;index=1">
        <span>添加新分类</span>
      </button>

        <!-- result的输入框 -->
    <div v-if="showModal" class="modal-overlay">
      <div class="modal-content">
        <h3>请输入分类名称</h3>
        <input 
          v-model="inputValue" 
          @keyup.enter="confirmInput"
          placeholder="请输入内容"
          class="modal-input"
        >
        <div class="modal-buttons">
          <button @click="confirmInput" class="confirm-btn">确定</button>
          <button @click="cancelInput" class="cancel-btn">取消</button>
        </div>
      </div>
    </div>
    </div>
  </div>

</template>

<script setup>
import { ref,onMounted } from 'vue';
import{getAllCategories,createCategory,renameCategory,deleteCategory} from '@/API/Category_API'
import{getNovelsByCategory} from '@/API/NovelCategory_API'

const loading = ref(false)  // 加载状态
const categories=ref([])
const getCategoryData=async () => {
  loading.value = true
  try {
    const response = await getAllCategories()
    if(response){
      categories.value  = response
    }
  } catch (error) {
    console.error('获取分类数据失败:', error)
  } finally {
    loading.value = false
  }
}



onMounted(() => {
  getCategoryData() // 页面加载时调用 store 方法
})

const index=ref(0)
const showModal = ref(false)//result的输入框
const inputValue = ref('');
const result = ref('');
const name=ref('');

const isRepeat = (newName) => {
  // 去除首尾空格并转为小写进行比较（可选，根据需求）
  return categories.value.some(category => 
    category.categoryName === newName
  );
};


const confirmInput = () => {
  if (!inputValue.value.trim()) {
    alert('请输入有效的内容');
  }
  else if(isRepeat(inputValue.value.trim())){
    alert('分类名已存在，请重新输入');
    inputValue.value = '';
  } 
  else{
    result.value = inputValue.value.trim();
    inputValue.value = '';
    showModal.value = false;
    if(index.value===1){
      addCategory();
    }
    else if(index.value===2){
      editCategory();
    }
  }
};



const cancelInput = () => {
  showModal.value = false;
  inputValue.value = '';
};




//调用api对分类进行操作
const addCategory =async () => {
  // 添加分类的逻辑
  loading.value = true // 开始加载
  try {
      const newData={
        categoryName: result.value
      }
      await createCategory(newData);
      getCategoryData();
    } catch (error) {
      console.error('添加分类失败:', error);
      alert('添加分类失败，请重试。');
    }finally {
    loading.value = false // 结束加载
  }

};

const editCategory = async() => {
  // 编辑分类的逻辑
  loading.value = true // 开始加载
  try {
      await renameCategory(name.value,result.value);
      getCategoryData();
    } catch (error) {
      console.error('编辑分类失败:', error);
      alert('编辑分类失败，请重试。');
    }finally {
    loading.value = false // 结束加载
  }
};

const deleteMyCategory = async() => {
  // 删除分类的逻辑
  loading.value = true // 开始加载
  try {
      const response=await getNovelsByCategory(name.value);
      if(response && response.length>0){
        alert('该分类下存在小说，无法删除');
        return;
      }
      await deleteCategory(name.value);
      getCategoryData();
    } catch (error) {
      console.error('删除分类失败:', error);
      alert('删除分类失败，请重试。');
    }finally {
    loading.value = false // 结束加载
  }
};


</script>

<style scoped>
/* 加载动画样式 */
.loading-indicator {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  margin: 20px 0;
  color: #666;
  font-size: 16px;
}

.spinner {
  width: 24px;
  height: 24px;
  border: 3px solid #ccc;
  border-top-color: #486482;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

.category-manager {
  max-width: 800px;
  margin: 30px auto;
  padding: 20px;
  background: #eee;
  border-radius: 8px;
  box-shadow: 0 2px 8px #eee;
}
.category-list {
  display: flex;
  flex-wrap: wrap;
  gap: 24px; /* 增大间距 */
  padding: 0;
  margin: 0;
  list-style: none;
  justify-content: flex-start;
  margin-left:30px;
  margin-top:20px;
}
.category-list li {
  background: transparent; /* 确保列表项背景透明 */
}
@keyframes rotate {
  100% {
    transform: rotate(1turn);
  }
}

.border-style-8 {
  position: relative;
  overflow: hidden;
  /* 可以在这里添加一些基础样式，如边框半径 */

  border-radius: 5px; /* 圆角 */

  padding: 5px 24px; /* 上下左右内边距一致，上下10px，左右24px */
  min-width: 130px;
  height: 44px; /* 细长效果，固定高度 */
  flex: 0 0 calc(20% - 16px); /* 一行五个分类，减去间距 */
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: flex-end; /* 右对齐 */
  margin-bottom: 5px;
  margin-right:10px;
  transition: transform 0.2s cubic-bezier(.4,0,.2,1); /* 添加平滑过渡 */
  z-index: 1;
}
.border-style-8:hover {
  transform: scale(1.06); /* 鼠标悬停时放大 */
  background-image:linear-gradient(135deg,#FFD26F 15%,#3677FF 100%)
}

.border-style-8::before {
  content: "";
  position: absolute;
  left: -50%;
  top: -50%;
  width: 200%;
  height: 200%;
  border-radius: 5px;
  background-image: conic-gradient(transparent, #f90, transparent 30%);
  background-position: 0 0;
  background-repeat: no-repeat;
  animation: rotate 2s linear infinite;
  z-index: -2;
}

.border-style-8::after {
  content: "";
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: calc(100% - 10px);
  height: calc(100% - 10px);
  z-index: -1;
  background-image:linear-gradient(135deg,#edcb82 15%,#668de2 100%);
  border-radius: 5px;
}

.border-style-8 > .content {
  padding: 10px;
}

.border-style-8 > .content span {
  font-size: 18px;
  font-weight: bold;

  color: #333;
}

button {
  background: #409eff;
  color: #fff;
  border: none;
  border-radius: 4px;
  padding: 4px 12px;
  cursor: pointer;
}
button:hover {
  background: #66b1ff;
}

.category-item {
  display: flex; /* 水平排列 */
  align-items: center; /* 垂直居中 */
  gap: 0px; /* 设置两个元素之间的间距 */
}

.icon-container {
  display: flex;
  flex-direction: column; /* 竖直排列 */
  gap: 8px; /* 图标之间的垂直间距 */
  margin-right:30px;
}

.edit-icon {
  font-size: 15px; /* 图标大小 */
  color: #666; /* 图标颜色 */
  transition: color 0.3s ease;
  cursor: pointer;
}

.edit-icon:hover {
  color: #409eff; /* 悬停颜色 */
}

.minus-icon {
  font-size: 15px; /* 图标大小 */
  color: #666; /* 图标颜色 */
  transition: color 0.3s ease;
  cursor: pointer;
}

.minus-icon:hover {
  color: #ff4d4f; /* 悬停颜色 */
}


.btn-animate {
  margin-left:600px;
  margin-top:30px;
  margin-bottom:30px;
  position:relative;
  width: 130px;
  height: 40px;
  line-height: 40px;
  border: none;
  border-radius: 5px;
  background-image:radial-gradient(#Fff 10%,#3677FF 120%);
  color: #fff;
  text-align: center;
}

.btn-animate__surround::before, 
.btn-animate__surround::after {
  content: '';
  position: absolute;
  right: 0;
  top: 0;
  background: #027efb;
  transition: all 0.3s ease;
}

.btn-animate__surround::before {
  height: 0%;
  width: 2px;
}

.btn-animate__surround::after {
  width: 0%;
  height: 2px;
}

.btn-animate__surround:hover {
  background: transparent;
}

.btn-animate__surround:hover::before {
  height: 100%;
}

.btn-animate__surround:hover::after {
  width: 100%;
}

.btn-animate__surround > span {
  display: block;
  color:rgb(22, 18, 12);
  font-size:15px;
  font-weight: 600;
}

.btn-animate__surround > span:hover {
  color: #027efb;
}

.btn-animate__surround > span:hover::before {
  height: 100%;
}

.btn-animate__surround > span:hover::after {
  width: 100%;
}

.btn-animate__surround > span::before, 
.btn-animate__surround > span::after {
  content: '';
  position: absolute;
  left: 0;
  bottom: 0;
  background: #027efb;
  transition: all 0.3s ease;
}

.btn-animate__surround > span::before {
  width: 2px;
  height: 0%;
}

.btn-animate__surround > span::after {
  width: 0%;
  height: 2px;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  padding: 20px;
  border-radius: 8px;
  width: 300px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.modal-input {
  width: 100%;
  padding: 8px;
  margin: 15px 0;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.modal-buttons {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

.confirm-btn {
  background-color: #42b983;
  color: white;
}

.cancel-btn {
  background-color: #f0f0f0;
}

</style>
