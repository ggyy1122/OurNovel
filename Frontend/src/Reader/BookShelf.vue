<template>
  <div class="page-back-container">
    <section class="book-section">
      <!-- 收藏卡片 -->
      <div class="category-card">
        <div class="card-header">
          <h2>我的收藏</h2>
        </div>
        <div class="card-content">
          <template v-if="loading">
            <div class="loading">加载中...</div>
          </template>
          <template v-else>
            <div class="novel-grid">
                <div class="novel-item" v-for="item in collects" :key="item.novel.novelId">
    <div class="img-container" @click="handle_NovelInfro(item)">
      <img :src="item.novel.fullCoverUrl" :alt="item.novel.novelName"/>
    </div>
    <div class="novel-title">{{ item.novel.novelName }}</div>
  </div>
              <div v-if="collects.length === 0" class="empty">暂无收藏内容</div>
            </div>
          </template>
        </div>
      </div>

      <!-- 推荐卡片 -->
      <div class="category-card">
        <div class="card-header">
          <h2>我的推荐</h2>
        </div>
        <div class="card-content">
          <template v-if="loading">
            <div class="loading">加载中...</div>
          </template>
          <template v-else>
            <div class="novel-grid">

         <div class="novel-item" v-for="item in recommends" :key="item.novel.novelId">
           <div class="img-container" @click="handle_NovelInfro(item)">
          <img :src="item.novel.fullCoverUrl" :alt="item.novel.novelName"/>
          </div>
         <div class="novel-title">{{ item.novel.novelName }}</div>
         </div>

              <div v-if="recommends.length === 0" class="empty">暂无推荐内容</div>
            </div>
          </template>
        </div>
      </div>

      <!-- 阅读历史卡片 -->
      <div class="category-card">
        <div class="card-header">
          <h2>我的阅读历史</h2>
        </div>
        <div class="card-content">
          <template v-if="loading">
            <div class="loading">加载中...</div>
          </template>
          <template v-else>
            <div class="novel-grid">
             <div class="novel-item" v-for="item in history" :key="item.novel.novelId">
              <div class="img-container" @click="handle_NovelInfro(item)">
              <img :src="item.novel.fullCoverUrl" :alt="item.novel.novelName"/>
               </div>
              <div class="novel-title">{{ item.novel.novelName }}</div>
             </div>
              <div v-if="history.length === 0" class="empty">暂无阅读历史</div>
            </div>
          </template>
        </div>
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
import { useRouter } from 'vue-router'
import { SelectNovel_State } from '@/stores/index'

const selectNovelState = SelectNovel_State()
const router = useRouter()

const store = readerState()
const collects = ref([])
const recommends = ref([])
const history = ref([])

const loading = ref(false)
const error = ref(null)



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


// 作品主页
async function handle_NovelInfro(item) {
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



onMounted(() => {
  fetchCollects()
  fetchRecommends()
  fetchHistory()
})
</script>

<style scoped>
.page-back-container {
  padding: 20px;
}

.book-section {
  max-width: 1200px;
  margin: 0 auto;
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.category-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.card-header {
  padding: 15px 20px;
  border-bottom: 1px solid #f0f0f0;
}

.card-header h2 {
  font-size: 18px;
  color: #333;
  margin: 0;
}

.card-content {
  padding: 15px 20px;
}

.novel-grid {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  gap: 15px;
}

.novel-item {
  text-align: center;
}


.novel-title {
  margin-top: 8px;
  font-size: 13px;
  color: #333;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.loading, .empty {
  grid-column: 1 / -1;
  padding: 20px;
  text-align: center;
  color: #666;
}

/* 响应式设计 - 在小屏幕上减少每行显示数量 */
@media (max-width: 1200px) {
  .novel-grid {
    grid-template-columns: repeat(5, 1fr);
  }
}

@media (max-width: 768px) {
  .novel-grid {
    grid-template-columns: repeat(3, 1fr);
  }
}

@media (max-width: 480px) {
  .novel-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}
/* 新增图片容器样式 */
.img-container {
  position: relative;
  overflow: hidden;
  border-radius: 4px;
  width: 100%;
  height: 140px;
}

.novel-item img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
  cursor: pointer;
}

/* 点击效果 */
.novel-item img:active {
  transform: scale(0.95);
}

/* 悬停效果 */
.novel-item:hover img {
  transform: scale(1.03);
}

/* 点击时的涟漪效果 */
.img-container::after {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  width: 5px;
  height: 5px;
  background: rgba(255, 255, 255, 0.5);
  opacity: 0;
  border-radius: 100%;
  transform: scale(1, 1) translate(-50%, -50%);
  transform-origin: 50% 50%;
}

.img-container:active::after {
  animation: ripple 0.6s ease-out;
}

@keyframes ripple {
  0% {
    transform: scale(0, 0);
    opacity: 0.5;
  }
  100% {
    transform: scale(20, 20);
    opacity: 0;
  }
}
</style>