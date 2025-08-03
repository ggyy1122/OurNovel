<template>
  <div class="page-back-container">
  <section class="book-section">
    <h3>我的收藏</h3>
    <div class="book-carousel">
      <button class="arrow left" @click="scrollLeft">‹</button>
      <div class="book-list" ref="carouselRef">
        <template v-if="loading">
          <div class="loading">加载中...</div>
        </template>
        <template v-else>
          <div
            class="book-item"
            v-for="item in collects"
            :key="item.novel.novelId"
          >
            <img :src="item.novel.fullCoverUrl" :alt="item.novel.novelName" />
            <div class="book-title">{{ item.novel.novelName }}</div>
          </div>
          <div v-if="collects.length === 0" class="empty">暂无收藏内容</div>
        </template>
      </div>
      <button class="arrow right" @click="scrollRight">›</button>
    </div>

    <h3>我的推荐</h3>
    <div class="book-carousel">
      <button class="arrow left" @click="scrollLeftRecommend">‹</button>
      <div class="book-list" ref="recommendRef">
        <template v-if="loading">
          <div class="loading">加载中...</div>
        </template>
        <template v-else>
          <div
            class="book-item"
            v-for="item in recommends"
            :key="item.novel.novelId"
          >
            <img :src="item.novel.fullCoverUrl" :alt="item.novel.novelName" />
            <div class="book-title">{{ item.novel.novelName }}</div>
          </div>
          <div v-if="recommends.length === 0" class="empty">暂无推荐内容</div>
        </template>
      </div>
      <button class="arrow right" @click="scrollRightRecommend">›</button>
    </div>

    <h3>我的阅读历史</h3>
    <div class="book-carousel">
      <button class="arrow left" @click="scrollLeftHistory">‹</button>
      <div class="book-list" ref="historyRef">
        <template v-if="loading">
          <div class="loading">加载中...</div>
        </template>
        <template v-else>
          <div
            class="book-item"
            v-for="item in history"
            :key="item.novel.novelId"
          >
            <img :src="item.novel.fullCoverUrl" :alt="item.novel.novelName" />
            <div class="book-title">{{ item.novel.novelName }}</div>
          </div>
          <div v-if="history.length === 0" class="empty">暂无阅读历史</div>
        </template>
      </div>
      <button class="arrow right" @click="scrollRightHistory">›</button>
    </div>
  </section>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { readerState } from '@/stores/index'
import { getCollectsByReader } from '@/API/Collect_API'
import { getRecommendsByReader } from '@/API/Recommend_API'
import { getRecentReadingsByReaderId } from '@/API/Reader_API'
import { getAuthor } from '@/API/Author_API'

const store = readerState()
const collects = ref([])
const recommends = ref([])
const history = ref([])

const loading = ref(false)
const error = ref(null)

const carouselRef = ref(null)
const recommendRef = ref(null)
const historyRef = ref(null)

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
    collects.value = await enrichNovelList(response)
  } catch (err) {
    error.value = err.message || '获取收藏失败'
    collects.value = []
  } finally {
    loading.value = false
  }
}

async function fetchRecommends() {
  loading.value = true
  error.value = null
  try {
    if (!store.readerId) throw new Error('未检测到登录读者ID')
    const response = await getRecommendsByReader(store.readerId)
    recommends.value = await enrichNovelList(response)
  } catch (err) {
    error.value = err.message || '获取推荐失败'
    recommends.value = []
  } finally {
    loading.value = false
  }
}

async function fetchHistory() {
  loading.value = true
  error.value = null
  try {
    if (!store.readerId) throw new Error('未检测到登录读者ID')
    const response = await getRecentReadingsByReaderId(store.readerId)
    history.value = await enrichNovelList(response)
  } catch (err) {
    error.value = err.message || '获取历史失败'
    history.value = []
  } finally {
    loading.value = false
  }
}

async function enrichNovelList(list = []) {
  return await Promise.all(
    list.map(async (item) => {
      item.novel.fullCoverUrl = formatCover(item.novel.coverUrl)
      try {
        const authorData = await getAuthor(item.novel.authorId)
        item.novel.authorName = authorData.authorName || '未知作者'
      } catch {
        item.novel.authorName = '未知作者'
      }
      return item
    })
  )
}

// 滚动操作
function scrollLeft() {
  carouselRef.value.scrollLeft -= 200
}
function scrollRight() {
  carouselRef.value.scrollLeft += 200
}

function scrollLeftRecommend() {
  recommendRef.value.scrollLeft -= 200
}
function scrollRightRecommend() {
  recommendRef.value.scrollLeft += 200
}

function scrollLeftHistory() {
  historyRef.value.scrollLeft -= 200
}
function scrollRightHistory() {
  historyRef.value.scrollLeft += 200
}

onMounted(() => {
  fetchCollects()
  fetchRecommends()
  fetchHistory()
})
</script>

<style scoped>
.page-back-container {
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  background-image: linear-gradient(to top, #dfe9f3 0%, white 100%);
  padding: 20px;
  box-sizing: border-box;
}

.book-section {
  margin-bottom: 30px;
}

.book-carousel {
  position: relative;
  display: flex;
  align-items: center;
  overflow: hidden;
}

.book-list {
  display: flex;
  overflow-x: auto;
  gap: 20px;
  scroll-behavior: smooth;
  padding: 10px 0;
}

.book-item {
  position: relative;
  margin: 0 10px;
  width: 100px;
  flex-shrink: 0;
  cursor: pointer;
}

.book-item img {
  width: 150px;
  height: 210px;
  object-fit: cover;
  border-radius: 6px;
  transition: transform 0.2s;
}

.book-item:hover img {
  transform: scale(1.05);
}

.book-title {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.65);
  color: #fff;
  font-size: 12px;
  text-align: center;
  padding: 4px 2px;
  opacity: 0;
  transition: opacity 0.2s;
}

.book-item:hover .book-title {
  opacity: 1;
}

.arrow {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  color: #666;
  padding: 0 10px;
  z-index: 1;
}

.arrow:hover {
  color: #000;
}
</style>
