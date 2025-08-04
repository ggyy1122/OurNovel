<template>
  <div class="page-back-container">
    <div class="collects-container">
      <h2>我的收藏</h2>
      <div v-if="loading" class="loading">加载中...</div>
      <div v-else-if="error" class="error">{{ error }}</div>
      <div v-else>
        <div v-if="collects.length === 0" class="empty">暂无收藏内容</div>
        <ul class="collect-list">
          <li
            v-for="item in collects"
            :key="item.novelId"
            class="collect-item"
            :class="{ selected: selectedNovelId === item.novel.novelId }"
            @click="toggleSelect(item.novel.novelId)"
          >
            <img :src="item.novel.fullCoverUrl" alt="封面" class="cover" />
            <div class="collect-info">
              <h3 class="novel-name">{{ item.novel.novelName }}</h3>
              <p class="novel-author">作者：{{ item.novel.authorName }}</p>
              <p class="novel-intro" :title="item.novel.introduction">
                {{ item.novel.introduction }}
              </p>
              <div class="meta">
                <span>状态：<strong>{{ item.novel.status }}</strong></span>
                <span>评分：<strong>{{ item.novel.score }}</strong></span>
                <span>收藏数：<strong>{{ item.novel.collectedCount }}</strong></span>
              </div>
            </div>
            <div
              v-if="selectedNovelId === item.novel.novelId"
              class="overlay"
              @click.stop
            >
              <button class="overlay-button" @click="viewDetail(item)">查看详情</button>
              <button class="overlay-button" @click="updateCollectPublic(item.novel.novelId)">是否公开</button>
              <button class="overlay-button" @click="cancelCollect(item.novel.novelId)">取消收藏</button>
            </div>
          </li>
        </ul>
        <div v-if="isPublicDialogVisible" class="dialog-mask">
    <div class="dialog-box">
      <h3>设置收藏状态</h3>
      <p>请选择该收藏是否公开：</p>
      <div class="dialog-buttons">
        <button @click="confirmPublic('yes')">公开</button>
        <button @click="confirmPublic('no')">私密</button>
      </div>
    </div>
  </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount,computed } from 'vue'
import { readerState } from '@/stores/index'
import { getCollectsByReader, deleteCollect,addOrUpdateCollect } from '@/API/Collect_API'
import { getAuthor } from '@/API/Author_API'

const store = readerState()

const collects = computed(() => store.favoriteBooks)
const loading = ref(false)
const error = ref(null)
const selectedNovelId = ref(null)

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
  if (coverUrl && coverUrl.trim() !== '') {
    const filename = getFileNameFromUrl(coverUrl)
    if (filename) {
      return ossPrefix + filename
    }
  }
  return defaultCover
}

async function fetchCollects() {
  loading.value = true
  error.value = null
  try {
    if (!store.readerId) throw new Error('未检测到登录读者ID')
    const response = await getCollectsByReader(store.readerId)
    if (response && response.length > 0) {
      const updatedCollects = await Promise.all(
        response.map(async (item) => {
          if (item.novel && item.novel.authorId) {
            item.novel.fullCoverUrl = formatCover(item.novel.coverUrl)
            try {
              const authorData = await getAuthor(item.novel.authorId)
              item.novel.authorName = authorData.authorName || '未知作者'
            } catch {
              item.novel.authorName = '未知作者'
            }
          } else {
            item.novel.authorName = '未知作者'
          }
          return item
        })
      )
      store.updateFavoriteBooks(updatedCollects)
    } else {
      store.updateFavoriteBooks([])
    }
  } catch (err) {
    error.value = err.message || '获取收藏失败'
    store.updateFavoriteBooks([])
  } finally {
    loading.value = false
  }
}

async function cancelCollect(novelId) {
  try {
    if (!store.readerId) throw new Error('未检测到登录读者ID')
    await deleteCollect(novelId, store.readerId)
    selectedNovelId.value = null
    await fetchCollects()
  } catch (err) {
    alert('取消收藏失败：' + (err.message || '请稍后再试'))
  }
}

function toggleSelect(novelId) {
  selectedNovelId.value = selectedNovelId.value === novelId ? null : novelId
}

function viewDetail(item) {
  alert('查看详情，小说ID：' + item.novel.novelId)
}

function handleClickOutside(event) {
  if (!event.target.closest('.collect-item')) {
    selectedNovelId.value = null
  }
}

const isPublicDialogVisible = ref(false)
const pendingNovelId = ref(null)

function updateCollectPublic(novelId) {
  if (!store.readerId) {
    alert('未检测到登录读者ID')
    return
  }
  pendingNovelId.value = novelId
  isPublicDialogVisible.value = true
}

async function confirmPublic(choice) {
  try {
    await addOrUpdateCollect(pendingNovelId.value, store.readerId, choice)
    alert('设置成功：该收藏已设为 ' + (choice === 'yes' ? '公开' : '私密'))
    await fetchCollects()
  } catch (err) {
    alert('设置公开失败：' + (err.message || '请稍后再试'))
  } finally {
    isPublicDialogVisible.value = false
    pendingNovelId.value = null
  }
}



onMounted(() => {
  fetchCollects()
  window.addEventListener('click', handleClickOutside)
})

onBeforeUnmount(() => {
  window.removeEventListener('click', handleClickOutside)
})
</script>


<style scoped>

.collects-container {
  max-width: 900px;
  margin: 0 auto;
  padding: 16px;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  color: #333;
}

.loading, .error, .empty {
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

.collect-item.selected {
  pointer-events: none;
}

.collect-item.selected .overlay {
  pointer-events: auto;
}

.cover {
  width: 100%;
  height: 140px;
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

.novel-intro {
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

.overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  backdrop-filter: blur(4px);
  background-color: rgba(0, 0, 0, 0.3);
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 12px;
  pointer-events: auto;
  border-radius: 10px;
  padding: 0 10px;
  z-index: 10;
}

.overlay-button {
  padding: 12px 24px;
  background: #fff;
  color: #333;
  border: none;
  border-radius: 6px;
  font-weight: 600;
  cursor: pointer;
  width: 160px;
  transition: background 0.3s ease, transform 0.2s ease;
  user-select: none;
}

.overlay-button:hover {
  background: #f0f0f0;
  transform: scale(1.05);
}

.dialog-mask {
  position: fixed;
  top: 0; left: 0;
  right: 0; bottom: 0;
  background-color: rgba(0, 0, 0, 0.4);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 999;
}

.dialog-box {
  background: #fff;
  padding: 20px 30px;
  border-radius: 8px;
  text-align: center;
  width: 300px;
}

.dialog-buttons button {
  margin: 10px;
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.dialog-buttons button:first-child {
  background-color: #409EFF;
  color: white;
}

.dialog-buttons button:last-child {
  background-color: #E6A23C;
  color: white;
}

</style>
