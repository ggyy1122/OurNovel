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
            <img 
              :src="item.novel.coverUrl || defaultCover" 
              alt="封面" 
              class="cover"
            />
            <div class="collect-info">
              <h3 class="novel-name">{{ item.novel.novelName }}</h3>
              <p class="novel-author">作者：{{ item.novel.authorName }}</p>
              <p class="novel-intro" :title="item.novel.introduction">{{ item.novel.introduction }}</p>
              <div class="meta">
                <span>状态：<strong>{{ item.novel.status }}</strong></span>
                <span>评分：<strong>{{ item.novel.score }}</strong></span>
                <span>收藏数：<strong>{{ item.novel.collectedCount }}</strong></span>
              </div>
            </div>
            <div v-if="selectedNovelId === item.novel.novelId" class="overlay" @click.stop>
              <button class="overlay-button" @click="viewDetail(item)">查看详情</button>
              <button class="overlay-button" @click="cancelCollect(item.novel.novelId)">取消收藏</button>
            </div>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'
import { readerState } from '@/stores/index'
import { getCollectsByReader, deleteCollect } from '@/API/Collect_API'

const store = readerState()

const collects = ref([])
const loading = ref(false)
const error = ref(null)
const defaultCover = '/default-cover.png' 

const selectedNovelId = ref(null)
import { getAuthor } from '@/API/Author_API'

async function fetchCollects() {
  loading.value = true
  error.value = null
  try {
    if (!store.readerId) {
      throw new Error('未检测到登录读者ID')
    }
    const response = await getCollectsByReader(store.readerId)
    if (response && response.length > 0) {
      // 并行获取作者名并挂载
      const updatedCollects = await Promise.all(
        response.map(async (item) => {
          if (item.novel && item.novel.authorId) {
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
          return item
        })
      )
      collects.value = updatedCollects
    } else {
      collects.value = []
    }
  } catch (err) {
    error.value = err.message || '获取收藏失败'
    collects.value = []
  } finally {
    loading.value = false
  }
}


// 取消收藏
async function cancelCollect(novelId) {
  try {
    if (!store.readerId) {
      throw new Error('未检测到登录读者ID')
    }
    await deleteCollect(novelId, store.readerId)
    selectedNovelId.value = null
    await fetchCollects()
  } catch (err) {
    alert('取消收藏失败：' + (err.message || '请稍后再试'))
  }
}

function toggleSelect(novelId) {
  if (selectedNovelId.value === novelId) {
    selectedNovelId.value = null
  } else {
    selectedNovelId.value = novelId
  }
}

// 查看详情（暂时仅弹窗）
function viewDetail(item) {
  alert('查看详情，小说ID：' + item.novel.novelId)
}

// 点击卡片外区域，取消选中
function handleClickOutside(event) {
  if (!event.target.closest('.collect-item')) {
    selectedNovelId.value = null
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
.page-back-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  background: linear-gradient(120deg, #f6d365 0%, #fda085 100%);
  padding: 20px;
  box-sizing: border-box;
}

.collects-container {
  max-width: 900px;
  margin: 0 auto;
  padding: 16px;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  color: #333;
}

h2 {
  margin-bottom: 24px;
  font-weight: 700;
  font-size: 24px;
  text-align: center;
  color: #222;
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
  color:  #f8d302f5;
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

/* 覆盖层按钮样式 */
.overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  backdrop-filter: blur(4px);
  background-color: rgba(0,0,0,0.3);
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
