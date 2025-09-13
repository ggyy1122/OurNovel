<template>
  <div class="home-view">
    <div class="page-back-container">
      <div class="collects-container">
        <div class="header-row">
          <h2>我的推荐</h2>
          <div class="visibility-control">
            <label>推荐是否可见：</label>
            <select v-model="isRecommendVisible" @change="updateRecommendVisibility">
              <option value="是">是</option>
              <option value="否">否</option>
            </select>
          </div>
        </div>
        <div v-if="loading" class="loading">加载中...</div>
        <div v-else-if="error" class="error">{{ error }}</div>
        <div v-else>
          <div v-if="recommends.length === 0" class="empty">暂无推荐内容</div>
          <ul class="collect-list">
            <li v-for="item in recommends" :key="item.novelId" class="collect-item"
              :class="{ selected: selectedNovelId === item.novel.novelId }" @click="toggleSelect(item.novel.novelId)">
              <img :src="item.novel.fullCoverUrl" alt="封面" class="cover" />
              <div class="collect-info">
                <h3 class="novel-name">{{ item.novel.novelName }}</h3>
                <p class="novel-author">作者：{{ item.novel.authorName }}</p>
                <div class="meta">
                  <span>状态：<strong>{{ item.novel.status }}</strong></span>
                  <span>评分：<strong>{{ item.novel.score }}</strong></span>
                  <span>推荐数：<strong>{{ item.novel.recommendCount }}</strong></span>
                </div>
                <p class="recommend-reason">推荐理由：“{{ item.reason }}”</p>
              </div>
              <div v-if="selectedNovelId === item.novel.novelId" class="overlay" @click.stop>
                <button class="overlay-button" @click="viewDetail(item)">查看详情</button>
                <button class="overlay-button" @click="cancelRecommend(item.novel.novelId)">取消推荐</button>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, computed } from 'vue'
import { readerState, SelectNovel_State } from '@/stores/index'
import { getRecommendsByReader, deleteRecommend } from '@/API/Recommend_API'
import { getAuthor } from '@/API/Author_API'
import { useRouter } from 'vue-router'
import { updateReader } from '@/API/Reader_API'
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

const selectNovelState = SelectNovel_State()
const router = useRouter()

const store = readerState()
const isRecommendVisible = ref(store.isRecommendVisible || '是')
const recommends = computed(() => store.recommendBooks)
const loading = ref(false)
const error = ref(null)
const selectedNovelId = ref(null)

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

const ossPrefix = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/'
const defaultCover = ossPrefix + 'e165315c-da2b-42c9-b3cf-c0457d168634.jpg'

function formatCover(coverUrl) {
  if (coverUrl && coverUrl.trim() !== '') {
    const filename = getFileNameFromUrl(coverUrl)
    if (filename) {
      return ossPrefix + filename
    }
  }
  return defaultCover
}

async function fetchRecommends() {
  loading.value = true
  error.value = null

  try {
    if (!store.readerId) throw new Error('未检测到登录读者ID')

    const response = await getRecommendsByReader(store.readerId)
    const processed = response || []

    for (const item of processed) {
      item.novel.fullCoverUrl = formatCover(item.novel.coverUrl)
      if (item.novel?.authorId) {
        try {
          const authorData = await getAuthor(item.novel.authorId)
          item.novel.authorName = authorData.authorName || '未知作者'
        } catch (e) {
          console.error(`获取作者失败，小说ID ${item.novel.novelId}，作者ID ${item.novel.authorId}`, e)
          item.novel.authorName = '未知作者'
        }
      } else {
        item.novel.authorName = '未知作者'
      }
    }
    store.updateRecommendBooks(processed)

  } catch (err) {
    console.error('获取推荐失败：', err)
    error.value = err.message || '获取推荐失败'
    store.updateRecommendBooks([])
  } finally {
    loading.value = false
  }
}

async function cancelRecommend(novelId) {
  try {
    if (!store.readerId) {
      throw new Error('未检测到登录读者ID')
    }
    await deleteRecommend(novelId, store.readerId)
    selectedNovelId.value = null
    await fetchRecommends()
  } catch (err) {
    toast.error('取消推荐失败：' + (err.message || '请稍后再试'))
  }
}

function toggleSelect(novelId) {
  if (selectedNovelId.value === novelId) {
    selectedNovelId.value = null
  } else {
    selectedNovelId.value = novelId
  }
}

// 查看详情
async function viewDetail(item) {
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

function handleClickOutside(event) {
  if (!event.target.closest('.collect-item')) {
    selectedNovelId.value = null
  }
}

// 更新推荐可见性
async function updateRecommendVisibility() {
  try {
    const updateData = {
      readerId: store.readerId,
      readerName: store.readerName,
      password: store.password,
      phone: store.phone,
      gender: store.gender,
      isCollectVisible: store.isCollectVisible,
      isRecommendVisible: isRecommendVisible.value,
      balance: store.balance,
      avatarUrl: store.avatarUrl,
      backgroundUrl: store.backgroundUrl,
    }

    await updateReader(store.readerId, updateData)
    store.isRecommendVisible = isRecommendVisible.value
    toast.success('推荐可见性已更新')
  } catch (error) {
    console.error('更新推荐可见性失败:', error)
    toast.error('更新失败，请稍后再试')
  }
}

onMounted(() => {
  isRecommendVisible.value = store.isRecommendVisible || '是'
  fetchRecommends()
  window.addEventListener('click', handleClickOutside)
})

onBeforeUnmount(() => {
  window.removeEventListener('click', handleClickOutside)
})
</script>

<style scoped>
.home-view {
  padding: 0;
}

.header-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.visibility-control {
  display: flex;
  align-items: center;
  gap: 10px;
}

.visibility-control label {
  font-weight: 600;
  font-size: 18px;
  margin-bottom: 0;
}

.visibility-control select {
  padding: 4px 6px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 18px;
}

.collects-container {
  max-width: 900px;
  margin: 0 auto;
  padding: 16px;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  color: #333;
}

.cover {
  width: 100%;
  height: 140px;
  object-fit: cover;
  border-bottom: 1px solid #eee;
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

.collect-item.selected {
  pointer-events: none;
}

.collect-item.selected .overlay {
  pointer-events: auto;
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

.meta {
  font-size: 13px;
  color: #666;
  display: flex;
  justify-content: space-between;
  gap: 8px;
  margin-bottom: 8px;
}

.meta span strong {
  font-size: 16px;
  color: #83718b;
}

.recommend-reason {
  font-size: 14px;
  color: #444;
  font-style: italic;
  line-height: 1.4;
  word-break: break-word;
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
</style>
