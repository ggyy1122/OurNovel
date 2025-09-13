<template>
  <div class="home-view">
    <div class="page-back-container">
      <div class="collects-container">
        <div class="header-bar">
          <h2>阅读历史</h2>
          <button @click="toggleBatchDelete" class="batch-delete-btn">
            {{ batchDeleteMode ? '取消删除' : '删除管理' }}
          </button>
          <button v-if="batchDeleteMode" @click="toggleSelectAll" class="select-all-btn">
            {{ allSelected ? '取消全选' : '全选' }}
          </button>
          <button v-if="batchDeleteMode" @click="deleteSelected" :disabled="selectedIds.size === 0"
            class="batch-delete-confirm-btn">
            删除所选
          </button>
        </div>

        <div v-if="loading" class="loading">加载中...</div>
        <div v-else-if="error" class="error">{{ error }}</div>
        <div v-else>
          <div v-if="histories.length === 0" class="empty">暂无历史记录</div>
          <ul class="collect-list">
            <li v-for="item in histories" :key="item.readingId" class="collect-item"
              @click="handle_NovelInfro(item, $event)">

              <input v-if="batchDeleteMode" type="checkbox" :value="item.novel.novelId"
                :checked="selectedIds.has(item.novel.novelId)"
                @change="toggleSelect(item.novel.novelId, $event.target.checked)" class="select-checkbox" />
              <div class="chapter-hint">
                最近阅读第{{ item.chapterId }}章
              </div>
              <img :src="item.novel.fullCoverUrl" alt="封面" class="cover" />
              <div class="collect-info">
                <h3 class="novel-name">{{ item.novel.novelName }}</h3>
                <p class="novel-author">作者：{{ item.novel.authorName || '未知作者' }}</p>
                <p class="novel-introduction">{{ item.novel.introduction || '暂无简介' }}</p>
                <div class="meta">
                  <span>状态：<strong>{{ item.novel.status }}</strong></span>
                  <span>评分：<strong>{{ item.novel.score }}</strong></span>
                  <span>上次阅读时间：<strong>{{ formatTime(item.recentReadingTime) }}</strong></span>
                </div>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { readerState } from '@/stores'
import { getRecentReadingsByReaderId, deleteRecentReading } from '@/API/Reader_API'
import { getAuthor } from '@/API/Author_API'
import { useRouter } from 'vue-router'
import { SelectNovel_State } from '@/stores/index'
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

const selectNovelState = SelectNovel_State()
const router = useRouter()
const store = readerState()
const histories = ref([])
const loading = ref(false)
const error = ref(null)

// 新增状态变量
const batchDeleteMode = ref(false)
const selectedIds = ref(new Set())

const ossPrefix = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/'
const defaultCover = ossPrefix + 'e165315c-da2b-42c9-b3cf-c0457d168634.jpg'

function getFileNameFromUrl(url) {
  if (!url) return ''
  try {
    const parsedUrl = new URL(url)
    const paths = parsedUrl.pathname.split('/')
    return paths[paths.length - 1] || ''
  } catch {
    const parts = url.split('/')
    return parts[parts.length - 1] || ''
  }
}

function formatCover(coverUrl) {
  const fileName = getFileNameFromUrl(coverUrl)
  return fileName ? ossPrefix + fileName : defaultCover
}

function formatTime(timeStr) {
  const d = new Date(timeStr)
  return d.toLocaleString()
}

async function fetchHistory() {
  loading.value = true
  error.value = null

  try {
    const res = await getRecentReadingsByReaderId(store.readerId)
    const processed = res || []

    for (const item of processed) {
      item.novel.fullCoverUrl = formatCover(item.novel.coverUrl)
      if (item.novel?.authorId) {
        try {
          const author = await getAuthor(item.novel.authorId)
          item.novel.authorName = author.authorName || '未知作者'
        } catch {
          item.novel.authorName = '未知作者'
        }
      }
    }

    histories.value = processed
    store.updateReadHistory(processed)
  } catch (e) {
    console.error('获取历史记录失败', e)
    error.value = '获取历史记录失败'
    histories.value = []
    store.updateReadHistory([])
  } finally {
    loading.value = false
  }
}

// 切换删除管理模式
function toggleBatchDelete() {
  batchDeleteMode.value = !batchDeleteMode.value
  if (!batchDeleteMode.value) {
    selectedIds.value.clear()
  }
}

// 选中/取消选中单个
function toggleSelect(novelId, checked) {
  if (checked) selectedIds.value.add(novelId)
  else selectedIds.value.delete(novelId)
}

// 全选状态计算
const allSelected = computed(() => histories.value.length > 0 && selectedIds.value.size === histories.value.length)

// 切换全选/取消全选
function toggleSelectAll() {
  if (allSelected.value) {
    selectedIds.value.clear()
  } else {
    selectedIds.value = new Set(histories.value.map(item => item.novel.novelId))
  }
}

// 批量删除选中项
async function deleteSelected() {
  if (selectedIds.value.size === 0) return
  if (!confirm(`确定删除选中的 ${selectedIds.value.size} 条记录吗？`)) return
  try {
    for (const novelId of selectedIds.value) {
      await deleteRecentReading(store.readerId, novelId)
    }
    histories.value = histories.value.filter(item => !selectedIds.value.has(item.novel.novelId))
    store.updateReadHistory(histories.value)
    selectedIds.value.clear()
    batchDeleteMode.value = false
  } catch (e) {
    toast.error('批量删除失败，请重试')
  }
}

// 作品主页
async function handle_NovelInfro(item, event) {
  if (event && event.target.classList.contains('select-checkbox')) {
    return;
  }
  if (batchDeleteMode.value || (event && event.target.classList.contains('select-checkbox'))) {
    return
  }
  try {
    const response = await getAuthor(item.novel.authorId);
    selectNovelState.resetNovel(
      item.novel.novelId,
      item.novel.authorId,
      item.novel.novelName,
      item.novel.introduction,
      item.novel.createTime,
      item.novel.coverUrl,
      item.novel.score,
      item.novel.totalWordCount,
      item.novel.recommendCount,
      item.novel.collectedCount,
      item.novel.status,
      item.novel.totalPrice,
      response.authorName,
      response.phone,
      response.avatarUrl,
      response.registerTime,
      response.introduction
    );
  } catch (error) {
    console.error('处理失败:', error);
  }
  router.push('/Novels/Novel_Info/home');
}

onMounted(fetchHistory)
</script>

<style scoped>
.home-view {
  padding: 0;
}



.collects-container {
  max-width: 900px;
  margin: 0 auto;
  padding: 16px;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  color: #333;
}

.header-bar {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 16px;
}

.batch-delete-btn,
.select-all-btn,
.batch-delete-confirm-btn {
  padding: 6px 12px;
  font-size: 14px;
  border-radius: 4px;
  border: none;
  cursor: pointer;
  background-color: #409EFF;
  color: white;
  transition: background-color 0.3s;
}

.batch-delete-btn:hover,
.select-all-btn:hover,
.batch-delete-confirm-btn:hover {
  background-color: #66b1ff;
}

.batch-delete-confirm-btn:disabled {
  background-color: #a0cfff;
  cursor: not-allowed;
}

.loading,
.error,
.empty {
  text-align: center;
  font-size: 18px;
  margin: 20px 0;
}

.error {
  color: #d9534f;
}

.collect-list {
  list-style: none;
  padding: 0;
  display: flex;
  flex-wrap: wrap;
  gap: 24px;
  justify-content: flex-start;
}

.collect-item {
  position: relative;
  background: #fff;
  box-shadow: 0 2px 6px rgb(0 0 0 / 0.1);
  border-radius: 10px;
  overflow: hidden;
  width: 280px;
  display: flex;
  flex-direction: column;
  cursor: pointer;
  transition: transform 0.3s ease, box-shadow 0.3s ease, filter 0.3s ease;
}

.collect-item:hover {
  transform: translateY(-6px);
  box-shadow: 0 8px 20px rgb(0 0 0 / 0.15);
}

.select-checkbox {
  position: absolute;
  top: 12px;
  left: 12px;
  width: 20px;
  height: 20px;
  cursor: pointer;
  z-index: 20;
}

.chapter-hint {
  position: absolute;
  top: 15px;
  left: 15px;
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 14px;
  font-weight: bold;
  z-index: 5;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
  background-color: #409EFF;
  color: white;
}

.cover {
  width: 100%;
  height: 160px;
  object-fit: cover;
  border-bottom: 1px solid #eee;
}

.collect-info {
  padding: 16px;
  flex-grow: 1;
  display: flex;
  flex-direction: column;
}

.novel-name {
  font-size: 20px;
  margin: 0 0 10px;
  color: #f8d302f5;
  font-weight: 700;
  line-height: 1.2;
}

.novel-introduction {
  font-size: 14px;
  color: #555;
  flex-grow: 1;
  overflow: hidden;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  line-clamp: 3;
  -webkit-box-orient: vertical;
  text-overflow: ellipsis;
  margin-bottom: 12px;
}

.meta {
  font-size: 13px;
  color: #666;
  display: flex;
  justify-content: space-between;
  gap: 8px;
}

.meta span strong {
  font-size: 16px;
  color: #83718b;
}

.delete-btn {
  position: absolute;
  top: 8px;
  right: 8px;
  background-color: #d9534f;
  border: none;
  color: white;
  padding: 4px 8px;
  font-size: 12px;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.delete-btn:hover {
  background-color: #c9302c;
}
</style>
