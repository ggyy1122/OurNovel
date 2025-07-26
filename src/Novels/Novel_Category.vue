<template>
  <div class="novel-category-container">
    <div class="category-filter">
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
    <div class="novel-list">
      <div v-if="loading" class="loading">加载中...</div>
      <div v-else-if="!filteredNovels || filteredNovels.length === 0" class="no-data">暂无数据</div>
      <template v-else>
        <template v-for="(novel, index) in filteredNovels" :key="novel.novelId">
          <Novel_Card :novel="novel" :rank="index + 1" />
          <hr v-if="index < filteredNovels.length - 1" class="novel-divider" />
        </template>
      </template>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted, watch } from 'vue'
import { getAllCategories } from '@/API/Category_API'
import { getNovelsByCategory } from '@/API/NovelCategory_API'
import { getAllNovels } from '@/API/Novel_API'
import Novel_Card from '@/Novels/Novel_Card.vue'

// 分类数据
const categories = ref([])
const novels = ref([])
const loading = ref(false)

// 添加全部选项后的分类数据
const categoriesWithAll = ref([
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
  category: '全部', // 默认选中"全部"
  wordCount: '',
  isFinished: ''
})

// 获取分类数据
async function fetchCategories() {
  try {
    const response = await getAllCategories()
    categories.value = response
    categoriesWithAll.value = [
      { id: 0, categoryName: '全部' },
      ...response
    ]
  } catch (error) {
    console.error('获取分类失败:', error)
  }
}

// 获取小说数据
async function fetchNovels() {
  try {
    loading.value = true
    const response = await getAllNovels()
    novels.value = response
  } catch (error) {
    console.error('获取小说列表失败:', error)
  } finally {
    loading.value = false
  }
}

// 根据分类筛选小说 
async function filterByCategory() {
  if (!selected.category || selected.category === '全部') {
    return novels.value
  }
  try {
    loading.value = true
    const response = await getNovelsByCategory(selected.category)
    return Array.isArray(response) ? response : []
  } catch (error) {
    console.error('获取分类小说失败:', error)
    return []
  } finally {
    loading.value = false
  }
}

// 综合筛选结果
const filteredNovels = ref([])
async function applyFilters() {
  try {
    loading.value = true
    let result = await filterByCategory()
    if (!result) {
      filteredNovels.value = []
      return
    }
    if (selected.wordCount) {
      result = result.filter(novel => {
        const wordCount = novel.totalWordCount || 0
        switch (selected.wordCount) {
          case '10k': return wordCount < 10000
          case '20k': return wordCount >= 10000 && wordCount < 20000
          case '30k': return wordCount >= 20000 && wordCount < 30000
          case 'gt30k': return wordCount >= 30000
          default: return true
        }
      })
    }
    if (selected.isFinished) {
      result = result.filter(novel => novel.status === selected.isFinished)
    }
    filteredNovels.value = result || []
  } catch (error) {
    console.error('筛选小说失败:', error)
    filteredNovels.value = []
  } finally {
    loading.value = false
  }
}

// 选择筛选条件
function selectFilter(type, value) {
  if (type === 'category' && selected[type] === value) {
    return
  }
  selected[type] = value
  applyFilters()
}

// 初始化数据
onMounted(async () => {
  await fetchCategories()
  await fetchNovels()
  applyFilters()
})

// 监听novels变化
watch(novels, applyFilters)
</script>

<style scoped>
.novel-category-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.category-filter {
  background: #fafafa;
  border-radius: 16px;
  padding: 20px;
  margin-bottom: 24px;
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
</style>