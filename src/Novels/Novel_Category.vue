<template>
  <div class="novel-category-wrapper">
    <img :src="backgroundImages[currentBgIndex]" alt="background" class="background-image" />
    <div class="novel-category-container">
      <div class="category-filter">
        <!-- 筛选部分 -->
        <div class="filter-row">
          <span class="filter-label">作品分类:</span>
          <button v-for="category in categoriesWithAll" :key="category.id"
            :class="['filter-btn', selected.category === category.categoryName ? 'active' : '']"
            @click="selectFilter('category', category.categoryName)">
            {{ category.categoryName }}
          </button>
        </div>

        <div class="filter-row">
          <span class="filter-label">作品字数:</span>
          <button v-for="option in wordCountOptions" :key="option.value"
            :class="['filter-btn', selected.wordCount === option.value ? 'active' : '']"
            @click="selectFilter('wordCount', option.value)">
            {{ option.text }}
          </button>
        </div>

        <div class="filter-row">
          <span class="filter-label">是否完结:</span>
          <button v-for="option in statusOptions" :key="option.value"
            :class="['filter-btn', selected.isFinished === option.value ? 'active' : '']"
            @click="selectFilter('isFinished', option.value)">
            {{ option.text }}
          </button>
        </div>
      </div>
      <div class="display-mode-switch">
        <span class="switch-label">显示模式:</span>
        <div class="switch-container" :class="switchActiveClass">
          <button :class="['switch-btn', displayMode === 'image' ? 'active' : '']" @click="displayMode = 'image'">
            1列
          </button>
          <button :class="['switch-btn', displayMode === 'text' ? 'active' : '']" @click="displayMode = 'text'">
            2列
          </button>
        </div>
      </div>
      <div class="novel-list" :class="{ 'text-mode': displayMode === 'text' }">
        <div v-if="loading" class="loading">加载中...</div>
        <div v-else-if="!novels || novels.length === 0" class="no-data">暂无数据</div>
        <template v-else>
          <template v-for="(novel, index) in novels" :key="novel.novelId">
            <component :is="displayMode === 'image' ? Novel_Card : Novel_Card1" :novel="novel"
              :rank="(currentPage - 1) * pageSize + index + 1" />
            <hr v-if="index < novels.length - 1 && displayMode === 'image'" class="novel-divider" />
          </template>
        </template>
      </div>
      <!-- 分页控件 -->
      <div v-if="totalItems > 0" class="pagination">
        <button :disabled="currentPage === 1" @click="changePage(currentPage - 1)" class="page-btn">
          上一页
        </button>
        <template v-for="page in visiblePages" :key="page">
          <button :class="['page-btn', currentPage === page ? 'active' : '']" @click="changePage(page)">
            {{ page }}
          </button>
        </template>
        <button :disabled="currentPage === totalPages" @click="changePage(currentPage + 1)" class="page-btn">
          下一页
        </button>
        <div class="page-jump">
          <span>跳转至</span>
          <input type="number" v-model.number="jumpPage" min="1" :max="totalPages" @keyup.enter="jumpToPage">
          <span>/ {{ totalPages }} 页</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted, onUnmounted, watch, computed } from 'vue'
import { getAllCategories } from '@/API/Category_API'
import { getFilteredNovels } from '@/API/Novel_API'
import Novel_Card from '@/Novels/Novel_Card.vue'
import Novel_Card1 from '@/Novels/SearchNovelCard.vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const displayMode = ref('image') // 'image' 或 'text'

// 属性来切换开关状态
const switchActiveClass = computed(() => ({
  'text-active': displayMode.value === 'text'
}))
// 分页相关状态
const currentPage = ref(1)
const pageSize = computed(() => displayMode.value === 'image' ? 10 : 20) // 10条，20条
const jumpPage = ref(1)
const totalItems = ref(0)

// 分类数据
const categories = ref([])
const novels = ref([])
const loading = ref(false)

// 添加全部选项后的分类数据
const categoriesWithAll = computed(() => [
  { id: 0, categoryName: '全部' },
  ...categories.value
])

// 筛选选项
const wordCountOptions = [
  { text: '全部', value: '' },
  { text: '1万以下', value: '10k' },
  { text: '1万~2万', value: '20k' },
  { text: '2万~3万', value: '30k' },
  { text: '3万以上', value: 'gt30k' }
]

const statusOptions = [
  { text: '全部', value: '' },
  { text: '已完结', value: '完结' },
  { text: '连载中', value: '连载' }
]

// 当前选中的筛选条件
const selected = reactive({
  category: route.query.categoryName || '全部', // 默认选中"全部"
  wordCount: '',
  isFinished: ''
})

// 计算属性
const totalPages = computed(() => Math.ceil(totalItems.value / pageSize.value))
const visiblePages = computed(() => {
  const maxVisible = 5 // 最多显示5个页码
  const half = Math.floor(maxVisible / 2)
  let start = Math.max(1, currentPage.value - half)
  let end = Math.min(totalPages.value, start + maxVisible - 1)

  if (end - start + 1 < maxVisible) {
    start = Math.max(1, end - maxVisible + 1)
  }

  return Array.from({ length: end - start + 1 }, (_, i) => start + i)
})

// 获取分类数据
async function fetchCategories() {
  try {
    const response = await getAllCategories()
    categories.value = response
  } catch (error) {
    console.error('获取分类失败:', error)
  }
}

// 获取小说数据
async function fetchNovels() {
  try {
    loading.value = true

    // 转换筛选条件为API需要的格式
    const category = selected.category === '全部' ? null : selected.category
    let minWordCount = null
    let maxWordCount = null
    let isFinished = null

    // 处理字数筛选
    switch (selected.wordCount) {
      case '10k':
        maxWordCount = 10000
        break
      case '20k':
        minWordCount = 10000
        maxWordCount = 20000
        break
      case '30k':
        minWordCount = 20000
        maxWordCount = 30000
        break
      case 'gt30k':
        minWordCount = 30000
        break
    }

    // 处理状态筛选
    if (selected.isFinished === '完结') {
      isFinished = true
    } else if (selected.isFinished === '连载') {
      isFinished = false
    }

    const response = await getFilteredNovels(
      currentPage.value,
      pageSize.value,
      category,
      minWordCount,
      maxWordCount,
      isFinished
    )

    novels.value = response.items || []
    totalItems.value = response.totalCount || 0
  } catch (error) {
    console.error('获取小说列表失败:', error)
    novels.value = []
    totalItems.value = 0
  } finally {
    loading.value = false
  }
}

// 选择筛选条件
function selectFilter(type, value) {
  selected[type] = value
  // 任何筛选条件改变都重新获取数据
  currentPage.value = 1
  jumpPage.value = 1
  fetchNovels()
}

// 分页相关方法
function changePage(page) {
  if (page < 1 || page > totalPages.value) return
  currentPage.value = page
  jumpPage.value = page
  fetchNovels()
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

function jumpToPage() {
  const page = Math.max(1, Math.min(jumpPage.value, totalPages.value))
  changePage(page)
}

// 背景图轮播相关
const backgroundImages = [
  require('@/assets/bac1.jpg'),
  require('@/assets/bac2.jpg'),
  require('@/assets/bac3.jpg'),
  require('@/assets/bac4.jpg')
]
const currentBgIndex = ref(0)

// 轮播背景图
let bgInterval

const scrollToTop = () => {
  window.scrollTo({ top: 0, behavior: 'smooth' });
};

// 初始化数据
onMounted(async () => {
  await fetchCategories()
  await fetchNovels()
  scrollToTop()
  // 启动背景轮播
  bgInterval = setInterval(() => {
    currentBgIndex.value = (currentBgIndex.value + 1) % backgroundImages.length
  }, 3500)
})

// 组件卸载时清除定时器
onUnmounted(() => {
  clearInterval(bgInterval)
})

// 监听显示模式变化，重新获取数据
watch(displayMode, () => {
  fetchNovels()
})
</script>

<style scoped>
/* 原有的样式保持不变 */
.novel-category-wrapper {
  position: relative;
  min-height: 100vh;
  overflow: hidden;
  background-color: #fdfafd;
}

.background-image {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  object-fit: cover;
  filter: blur(0.5px) brightness(0.9);
  z-index: 0;
  mask-image: linear-gradient(to bottom, black 30%, rgba(0, 0, 0, 0.7) 60%, transparent 100%);
  -webkit-mask-image: linear-gradient(to bottom, black 30%, rgba(0, 0, 0, 0.7) 60%, transparent 100%);
  transition: opacity 1s ease-in-out;
}

.novel-category-container {
  max-width: 1100px;
  margin: 0 auto;
  padding: 20px;
  position: relative;
  border-radius: 8px;
}


.category-filter {
  background: rgba(244, 242, 242, 0.6);
  border-radius: 16px;
  padding: 20px;
  margin-bottom: 15px;
}

.filter-row {
  display: flex;
  align-items: center;
  margin-bottom: 15px;
  flex-wrap: wrap;
}

.filter-label {
  font-weight: bold;
  font-size: 16px;
  margin-right: 15px;
  min-width: 80px;
  color: #666;
}

.filter-btn {
  background: none;
  border: none;
  color: #222;
  font-size: 14px;
  margin-right: 12px;
  margin-bottom: 8px;
  padding: 6px 14px;
  border-radius: 14px;
  cursor: pointer;
  transition: all 0.2s;
}

.filter-btn.active {
  background: #ffd100;
  color: #fff;
  font-weight: bold;
}

.filter-btn:hover {
  background: #fffbe6;
}

.novel-list.text-mode {
  width: 90%;
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  column-gap: 80px;
  margin: 10px auto;
}

.novel-list.text-mode .novel-divider {
  display: none;
}

.display-mode-switch {
  display: flex;
  align-items: center;
  margin-bottom: 15px;
  background: rgba(244, 242, 242, 0.6);
  border-radius: 16px;
  padding: 10px 20px;
}

.switch-label {
  font-weight: bold;
  font-size: 16px;
  margin-right: 15px;
  color: #666;
}

.switch-container {
  display: flex;
  border-radius: 20px;
  background: #fcfafa;
  padding: 2px;
  position: relative;
  border: 1px solid #efefef;
  transition: background 0.3s ease;
}

.switch-btn {
  position: relative;
  padding: 6px 20px;
  border: none;
  background: transparent;
  color: #666;
  font-size: 14px;
  cursor: pointer;
  z-index: 1;
  transition: all 0.3s ease;
}

.switch-btn.active {
  color: #fff;
  font-weight: bold;
}

.switch-container::after {
  content: '';
  position: absolute;
  top: 2px;
  left: 0;
  width: 50%;
  height: calc(100% - 4px);
  background: #ffd100;
  border-radius: 18px;
  transition: all 0.3s ease;
}

.switch-container.text-active::after {
  left: 50%;
}

.novel-list {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.loading,
.no-data {
  text-align: center;
  padding: 50px;
  font-size: 16px;
  color: #888;
}

.novel-divider {
  border: none;
  border-top: 1px solid #b2b6bb;
  margin: 0.1rem 0;
}

/* 分页样式 */
.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 30px;
  gap: 8px;
  flex-wrap: wrap;
  margin-bottom: 25px;
}

.page-btn {
  padding: 6px 12px;
  border: 1px solid #ddd;
  background: #fff;
  color: #333;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.2s;
}

.page-btn:hover:not(:disabled) {
  background: #f5f5f5;
}

.page-btn.active {
  background: #ffd100;
  color: #fff;
  border-color: #ffd100;
  font-weight: bold;
}

.page-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.page-jump {
  margin-left: 15px;
  display: flex;
  align-items: center;
  gap: 8px;
}

.page-jump input {
  width: 50px;
  padding: 6px;
  border: 1px solid #ddd;
  border-radius: 4px;
  text-align: center;
}
</style>