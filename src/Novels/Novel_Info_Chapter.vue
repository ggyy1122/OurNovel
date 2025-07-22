<template>
  <div class="chapter-list">
    <h2>章节目录</h2>
    <p>当前小说ID：{{ selectNovelState.novelId }}</p>

    <!-- 章节列表 -->
    <ul v-if="chapterList.length > 0">
      <transition-group name="chapter-fade" tag="ul">
        <li
          v-for="(chapter, index) in paginatedChapters"
          :key="chapter.chapterId"
          @click="selectChapter(chapter)"
          class="chapter-item"
        >
          <div class="chapter-info">
            <span class="chapter-number">第{{ index + 1 }}章</span>
            <span class="chapter-title">{{ chapter.title }}</span>
            <span v-if="chapter.isCharged === '是'" class="charged">（收费）</span>
            <span v-else class="free">（免费）</span>
          </div>
        </li>
      </transition-group>
    </ul>

    <!-- 如果章节列表为空，显示提示信息 -->
    <p v-else>作者还在努力敲字中，感谢您的关注~</p>

    <!-- 分页组件 -->
    <div v-if="chapterList.length > 0" class="pagination-container">
      <button 
        class="prev"
        @click="changePage(currentPage - 1)"
        :disabled="currentPage === 1"
      >
        🡄
      </button>

      <span class="page-info">当前：{{ currentPage }}页 / 共{{ totalPages }}页</span>

      <button 
        class="next"
        @click="changePage(currentPage + 1)"
        :disabled="currentPage === totalPages"
      >
        🡆
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { SelectNovel_State } from '@/stores/index';  
import { getChaptersByNovel } from '@/API/Chapter_API'; 

const selectNovelState = SelectNovel_State();  
const chapterList = ref([]);
const currentPage = ref(1);
const itemsPerPage = ref(5);  // 每页显示5个章节

// 计算分页后的章节
const paginatedChapters = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage.value;
  const end = currentPage.value * itemsPerPage.value;
  return chapterList.value.slice(start, end);
});

// 计算总页数
const totalPages = computed(() => {
  return Math.ceil(chapterList.value.length / itemsPerPage.value);
});

// 页面加载时获取章节数据
onMounted(async () => {
  try {
    const novelId = selectNovelState.novelId;  
    const response = await getChaptersByNovel(novelId);  
    chapterList.value = response || [];  
    console.log('章节数据:', response);  
  } catch (error) {
    console.error('获取章节失败:', error);
    chapterList.value = [];  
  }
});

// 页码变更
function changePage(page) {
  if (page < 1 || page > totalPages.value) return;  // 确保页码在有效范围内
  currentPage.value = page;
}

// 选中章节的逻辑
function selectChapter(chapter) {
  selectNovelState.setSelectedChapter(chapter); 
  console.log('已选择章节：', chapter);
}
</script>

<style scoped>
.chapter-list {
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 8px;
}

.chapter-item {
  cursor: pointer;
  padding: 15px;
  border-bottom: 1px solid #e0e0e0;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #fff;
  transition: background-color 0.3s ease;
}

.chapter-item:hover {
  background-color: #f0f0f0;
}

.chapter-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
}

.chapter-number {
  font-size: 14px;
  color: #555;
}

.chapter-title {
  font-size: 16px;
  font-weight: bold;
  color: #333;
  margin-left: 10px;
}

.charged {
  color: red;
  font-size: 14px;
}

.free {
  color: green;
  font-size: 14px;
}

p {
  font-size: 16px;
  color: #888;
  text-align: center;
  margin-top: 30px;
}

h3 {
  font-size: 18px;
  color: #333;
  margin-top: 20px;
}

.page-info {
  font-size: 14px;
  color: #555;
  margin: 0 15px;
}

.pagination-container {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 20px;
}

.pagination-container button {
  background-color: #4CAF50;
  color: white;
  border: none;
  padding: 5px 10px;
  margin: 0 10px;
  cursor: pointer;
  border-radius: 4px;
  transition: background-color 0.3s;
  font-size: 18px;
}

.pagination-container button:hover {
  background-color: #45a049;
}

.pagination-container button:disabled {
  background-color: #dcdcdc;
  cursor: not-allowed;
}

.pagination-container span {
  font-size: 16px;
  color: #555;
}
</style>
