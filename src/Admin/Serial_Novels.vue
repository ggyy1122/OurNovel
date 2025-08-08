<template>
  <!-- 加载中提示 -->
  <div v-if="loading" class="loading-indicator">
      <div class="spinner"></div>
      <span>加载中...</span>
  </div>
  <div v-else>
    <div class="filter-container">
    <!-- 小说分类选择栏 -->
    <div class="category-selector">
    <!-- 分类选择触发按钮 -->
      <div class="category-trigger" @click="toggleDropdown">
        <span>分类</span>
        <span class="dropdown-icon">{{ isOpen ? '▲' : '▼' }}</span>
        <span  class="selected-category-label">{{ selectedCategory }}</span>
      </div>

      <!-- 下拉分类列表 -->
      <ul v-show="isOpen" class="dropdown-list">
        <li 
          v-for="category in categories" 
          :key="category"
          @click="selectCategory(category)"
        >
          {{ category }}
        </li>
      </ul>
    </div>
  
    <!-- 根据小说名查找 -->
    <div class="search-container">
      <input
        type="text"
        v-model="searchText"
        :placeholder="defaultPrompt"
        @focus="clearPlaceholder"
        @blur="restorePlaceholder"
        class="search-input"
      />
      <button @click="handleSearch" class="search-button">
        查找
      </button>
    </div>
  </div>

  <div class="novel-table-container">
    <table class="novel-table">
      <thead>
        <tr>
          <th><input type="checkbox" v-model="selectAll" @change="toggleSelectAll" /></th>
          <th>小说名</th>
          <th>作者</th>
          <th>操作</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="novel in pagedNovels" :key="novel.novelId">
          <td>
            <input type="checkbox" v-model="selectedIds" :value="novel.novelId" />
          </td>
          <td><router-link
            :to="{path:`/Admin/Admin_Layout/novel_managent/novelInfo`,query:{id:novel.novelId,text:novel.authorName}}"
            class="novel-link"
          >{{ novel.novelName }}</router-link></td>
          <td>{{ novel.authorName }}</td>
          <td>
              <button 
                class="reject-btn" 
                @click="rejectNovel(novel.novelId)"
              >封禁</button>
          </td>
        </tr>
      </tbody>
    </table>
    
        <div v-if="selectedIds.length" class="batch-actions">
          <button @click="deleteSelected">批量删除</button>
          <button @click="moveSelected">批量移动至</button>
        </div>
         <!-- 分页控件 -->
      <div class="pagination">
        <button @click="prevPage" :disabled="currentPage === 1">上一页</button>
        <span v-for="page in totalPages" :key="page">
          <button 
            @click="goToPage(page)" 
            :class="{active: currentPage === page}"
          >{{ page }}</button>
        </span>
        <button @click="nextPage" :disabled="currentPage === totalPages">下一页</button>
      </div>
  </div>
  </div>
  
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, computed } from 'vue'



//----------------------------------------------------------------------------------------------------------------
//通过api获取小说数据

import { getAllNovels } from '@/API/Novel_API'
import { getAuthor } from '@/API/Author_API'
import {getAllCategories} from '@/API/Category_API'
import{getNovelsByCategory} from '@/API/NovelCategory_API'
import { current_state } from '@/stores/index'
import { storeToRefs } from 'pinia'

const currentState = current_state()
const { id: managerID } = storeToRefs(currentState)


const novels=ref([]) // 用于存储小说的名和作者名
const getNovelsData=async () => {
  loading.value = true // 开始加载
  try {
    const response = await getAllNovels()
    novels.value = response.filter(novel => novel.status === '连载').map(novel => ({
      novelId: novel.novelId,
      novelName: novel.novelName,
      authorId: novel.authorId,
      authorName:'未知'
    }))
    categoryNovels.value=novels.value
    //获取所有分类
    const categoriesResponse = await getAllCategories()
    categories.value = categoriesResponse.map(cat => cat.categoryName)
    categories.value.unshift('全部') // 添加“全部”选项
    // 获取作者信息
    for (const novel of novels.value) {
      const authorResponse = await getAuthor(novel.authorId)
      if (authorResponse) {
        novel.authorName = authorResponse.authorName || '未知'
      }
    }
  } catch (error) {
    console.error('获取小说数据失败:', error)
    alert('加载小说数据失败，请检查网络或权限。')
  } finally {
    loading.value = false
  }
}

//----------------------------------------------------------------------------------------------------------------
//顶端分类栏的控制
const isOpen = ref(false);
const selectedCategory = ref('全部');

//小说分类名称
const categories = ref([]);
const categoryNovels= ref([]);

const toggleDropdown = () => {
  isOpen.value = !isOpen.value;
};

const selectCategory=async (category) => {
  loading.value = true // 开始加载
  try{
      selectedCategory.value = category;
  if(selectedCategory.value==='全部')
    categoryNovels.value=novels.value
  else{
    const response=await getNovelsByCategory(category)
    categoryNovels.value=response.filter(novel => novel.status === '连载').map(novel => ({
      novelId: novel.novelId,
      novelName: novel.novelName,
      authorId: novel.authorId,
      authorName:'未知'
    }))
     for (const novel of categoryNovels.value) {
      const authorResponse = await getAuthor(novel.authorId)
      if (authorResponse) {
        novel.authorName = authorResponse.authorName || '未知'
      }
    }
  }
  isOpen.value = false;
 
  console.log('已选择分类:', category);
  // 获取作者信息
  } catch (error) {
    console.error('获取小说数据失败:', error)
    alert('加载小说数据失败，请检查网络或权限。')
  } finally {
    loading.value = false
  }
  // 这里可以触发分类筛选逻辑
};
//----------------------------------------------------------------------------------------------------------------

//按小说名查找的逻辑
const searchText = ref('');
const defaultPrompt = '请输入小说名';
const isFocused = ref(false);
const showNovels = ref([]);

const clearPlaceholder = () => {
  isFocused.value = true;
};

const restorePlaceholder = () => {
  if (!searchText.value) {
    isFocused.value = false;
  }
};

const handleSearch = () => {
  if (searchText.value.trim()) {
    // 搜索 categoryNovels 中包含输入内容的小说
    showNovels.value = categoryNovels.value.filter(novel =>
      novel.novelName.includes(searchText.value.trim())
    );
    // 如果没有结果可提示
    if (showNovels.value.length === 0) {
      alert('未找到相关小说');
    }else{
      categoryNovels.value = showNovels.value; // 更新显示的小说列表
      console.log('搜索结果:', showNovels.value);
    }
  } else {
    alert('请输入搜索内容');
  }
};
//----------------------------------------------------------------------------------------------------------------
//分页显示
const currentPage = ref(1)
const pageSize = 5
const totalPages = computed(() => Math.ceil(categoryNovels.value.length / pageSize))
const pagedNovels = computed(() => {//本页显示的章节范围
  const start = (currentPage.value - 1) * pageSize
  return categoryNovels.value.slice(start, start + pageSize)
})
function goToPage(page) {//进入某页
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page
  }
}
function prevPage() {//下一页
  if (currentPage.value > 1) currentPage.value--
}
function nextPage() {//上一页
  if (currentPage.value < totalPages.value) currentPage.value++
}
//----------------------------------------------------------------------------------------------------------------
//从数据库获取信息时的加载中动画
const loading = ref(false)  // 加载状态

//----------------------------------------------------------------------------------------------------------------
//小说审核

import { reviewNovel } from '@/API/Novel_API'

function rejectNovel(id) {
  loading.value = true // 开始加载
  reviewNovel(id, '封禁', managerID.value,'不通过').then(response => {
    if (response.success) {
       // 移除已审核小说
      categoryNovels.value = categoryNovels.value.filter(novel => novel.novelId !== id)
      novels.value = novels.value.filter(novel => novel.novelId !== id)
    } else {
      alert('审核失败：' + response.message)
    }
  }).catch(error => {
    console.error('审核失败:', error)
    alert('审核失败，请稍后再试。')
  }).finally(() => {
    loading.value = false
  })
}
//----------------------------------------------------------------------------------------------------------------

const selectedIds = ref([])
const selectAll = ref(false)




function handleClickOutside(e) {
  // 如果有三点菜单弹出，且点击的不是菜单本身，则关闭菜单
  if (activeMenu.value !== null) {
    // 判断是否点击在菜单或三个点上
    const menus = document.querySelectorAll('.action-menu, .action-dots')
    let isMenu = false
    menus.forEach(menu => {
      if (menu.contains(e.target)) isMenu = true
    })
    if (!isMenu) activeMenu.value = null
  }
}

onMounted(() => {
  document.addEventListener('click', handleClickOutside)
  getNovelsData() // 页面加载时调用 store 方法
})
onBeforeUnmount(() => {
  document.removeEventListener('click', handleClickOutside)
})


function toggleSelectAll() {//全选
  if (selectAll.value) {
    selectedIds.value = novels.value.map(n => n.id)
  } else {
    selectedIds.value = []
  }
}

//----------------------------------------------------------------------------------------------------------------
//三点菜单的操作
const activeMenu = ref(null)



function deleteSelected() {
  novels.value = novels.value.filter(n => !selectedIds.value.includes(n.id))
  selectedIds.value = []
  selectAll.value = false
}

function moveSelected() {
  alert('将选中的小说批量移动至其他分组（示例）')
}



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
.novel-link{
  color:#000;
  text-decoration: none;
  cursor:pointer;
  transition: color 0.3s ease;
}
.novel-link:hover {
  color: #42b983;
}
.novel-table-container {
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
  padding: 24px;
  margin-left:145px;
  margin-right:143px;
  margin-top:0px;
  max-width: 1150px;
}
.novel-table {
  width: 100%;
  border-collapse: collapse;
}
.novel-table th, .novel-table td {
  padding: 12px 8px;
  border-bottom: 1px solid #eee;
  text-align: left;
}
.novel-table tr{
  transition: background-color 0.2s;
}
.novel-table tr:hover {
  background-color: #E3E3E3;
}
.novel-table th {
  background: #dde3ee;
}

.batch-actions {
  margin-top: 16px;
  text-align: right;
}
.batch-actions button {
  margin-left: 12px;
  padding: 6px 18px;
  background: #42b983;
  color: #fff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
.batch-actions button:hover {
  background: #2c3e50;
}

.filter-container {
  display: flex;
  align-items: center; /* 垂直居中 */
  gap: 32px; /* 两个元素之间的间距 */
  width: 100%;
  min-width: 600px; /* 可根据需要调整 */
  margin-top:20px;
  margin-bottom: 0px;
}


.category-selector {
  position: relative;
  display: inline-block;
  font-family: 'Microsoft YaHei', sans-serif;
  margin: 10px auto;
  flex-shrink: 0; /* 防止被压缩 */
}

.category-trigger {
  padding: 8px 16px;
  background-color: #f5f5f5;
  border: 1px solid #ddd;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all 0.3s;
}

.category-trigger:hover {
  background-color: #e9e9e9;
}

.dropdown-icon {
  font-size: 12px;
}

.dropdown-list {
  position: absolute;
  top: 100%;
  left: 0;
  width: 150px;
  margin-top: 4px;
  padding: 8px 0;
  background-color: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  list-style: none;
  z-index: 10;
}

.dropdown-list li {
  padding: 8px 16px;
  cursor: pointer;
}

.dropdown-list li:hover {
  background-color: #42b983;
}

.search-container {
  display: flex;
  width: 100%;
  max-width: 500px;
  margin: 10px auto;
  flex-grow: 1;
}

.search-input {
  flex: 1;
  padding: 10px 15px;
  border: 1px solid #ddd;
  border-radius: 4px 0 0 4px;
  font-size: 16px;
  outline: none;
  transition: border-color 0.3s;
}

.search-input:focus {
  border-color: #409eff;
}

.search-input::placeholder {
  color: #999;
}

.search-button {
  padding: 10px 20px;
  background-color: #409eff;
  color: white;
  border: none;
  border-radius: 0 4px 4px 0;
  cursor: pointer;
  font-size: 16px;
  transition: background-color 0.3s;
}

.search-button:hover {
  background-color: #66b1ff;
}

.search-button:active {
  background-color: #3a8ee6;
}
.selected-category-label {
  margin-left: 12px;
  padding: 2px 10px;
  background: #42b983;
  color: #fff;
  border-radius: 12px;
  font-weight: bold;
  font-size: 14px;
  box-shadow: 0 1px 4px rgba(66,185,131,0.12);
}
.pagination {
  display: flex;
  gap: 8px;
  margin-top: 18px;
  justify-content: center;
  align-items: center;
}
.pagination button {
  padding: 4px 10px;
  border: 1px solid #42b983;
  background: #fff;
  color: #42b983;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}
.pagination button.active {
  background: #42b983;
  color: #fff;
  font-weight: bold;
}
.pagination button:disabled {
  background: #eee;
  color: #aaa;
  cursor: not-allowed;
  border: 1px solid #ccc;
}
.reject-btn {
  background: #ff4d4f;
  color: #fff;
  border: none;
  border-radius: 4px;
  padding: 6px 14px;
  cursor: pointer;
  font-weight: bold;
  transition: background 0.2s;
}
.reject-btn:hover {
  background: #d9363e;
}
</style>