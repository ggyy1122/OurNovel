<template>
  <button @click="goback" class="back-button">
    <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
      <path d="m15 18-6-6 6-6" />
    </svg>
    返回
  </button>
  <!-- 美观的书籍信息展示 -->
  <div class="book-display-card">
    <div class="book-content">
      <!-- 左侧图片区域 -->
      <div class="book-image-section">
        <div class="image-wrapper">
          <img :src="selectNovelState.formattedcoverUrl || defaultCoverImage" :alt="selectNovelState.novelName"
            class="book-cover" @error="handleImageError" />
        </div>
      </div>
      <!-- 中间信息区域 -->
      <div class="book-info-section">
        <!-- 标题 -->
        <h1 class="book-title">{{ selectNovelState.novelName }}</h1>
        <div class="tag-row"> <!-- 新增的包裹容器 -->
          <!-- 分数显示（与标题同行） -->
          <div class="score-badge" v-if="selectNovelState.score > 0">
            {{ selectNovelState.score.toFixed(1) }} ★ <!-- 假设分数是数字，保留1位小数 -->

          </div>
          <div :class="['status-badge', getStatusClass(selectNovelState.status)]">
            <div class="status-dot"></div>
            {{ selectNovelState.status }}
          </div>
          <!-- 分类徽章（每个分类独立徽章） -->
          <template v-if="categories.length > 0">
            <div v-for="category in categories" :key="category" class="category-badge">
              {{ category }}
            </div>
          </template>
        </div>
        <!-- 作者信息 -->
        <div class="author-name" v-if="selectNovelState.authorName">
          作者：{{ selectNovelState.authorName }}</div>
        <!-- 统计信息 -->
        <div class="stats-row">
          <span class="stat-item">
            <span class="stat-value">{{ selectNovelState.totalWordCount }}</span>
            <span class="stat-unit">字</span>
          </span>
          <span class="stat-item">
            <span class="stat-value">{{ selectNovelState.recommendCount }}</span>
            <span class="stat-unit">推荐</span>
          </span>
          <span class="stat-item">
            <span class="stat-value">{{ selectNovelState.collectedCount }}</span>
            <span class="stat-unit">收藏</span>
          </span>
        </div>
        <!-- 最新章节信息 -->
        <h1 class="newest-chapter">最新章节第....章 2025年xx月xx日</h1>

        <!-- 新增的蓝色按钮组 -->
        <div class="action-buttons">
          <button class="blue-border-btn" @click="handleRead">
            开始阅读
          </button>
          <button class="blue-border-btn" :class="{ 'is-collected': isCollected }" @click="toggleCollect">
            {{ isCollected ? '已收藏' : '收藏作品' }}
          </button>
          <button class="blue-border-btn">
            推荐作品
          </button>
          <button class="blue-border-btn">
            打赏作品
          </button>
        </div>
      </div>
      <!-- 右侧作者卡片区域 -->
      <div class="author-card-section">
        <div class="author-card">
          <img :src="selectNovelState.formattedauthorAvatarUrl || defaultAuthorAvatar" class="author-avatar" />
          <h2>{{ selectNovelState.authorName }}</h2>
          <p class="author-brief">{{ '作者简介:暂无作者简介' }}</p>
          <div class="author-data-cards">
            <div class="data-card">
              <div class="data-value">5</div>
              <div class="data-label">作品总数</div>
            </div>
            <div class="data-card">
              <div class="data-value">1777</div>
              <div class="data-label">累计字数</div>
            </div>
            <div class="data-card">
              <div class="data-value">3465</div>
              <div class="data-label">创作天数</div>
            </div>
          </div>
        </div>
      </div>
    </div>

  </div>


  <!-- 导航栏展示 -->
  <div class="novel-container">
    <nav class="nav-menu" ref="mainNav">
      <ul>
        <li>
          <router-link to="/Novels/Novel_Info/home" active-class="active-link">
            作品介绍
          </router-link>
        </li>
        <li>
          <router-link to="/Novels/Novel_Info/chapter" active-class="active-link">
            作品目录
          </router-link>
        </li>
        <li>
          <router-link to="/Novels/Novel_Info/comment" active-class="active-link">
            评论
          </router-link>
        </li>
      </ul>
    </nav>

    <main class="main-content">
      <router-view></router-view>
    </main>
  </div>

</template>

<script setup>
import { ref, onMounted, watch, computed } from 'vue';
import { useRouter } from 'vue-router';
import { SelectNovel_State, readerState } from '@/stores/index';
import { getCategoriesByNovel } from '@/API/NovelCategory_API';
import { addOrUpdateCollect, deleteCollect } from '@/API/Collect_API';
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
import { getChapter } from '@/API/Chapter_API';
const selectNovelState = SelectNovel_State();      //小说对象
const ReaderState = readerState();                   //当前读者对象
const categories = ref([]);                          //分类数组
const isLoadingCategories = ref(false);            //是否在加载

//是否被收藏
const isCollected = computed(() => {
  //当前小说ID
  const currentNovelId = selectNovelState.novelId;
  // 检查是否存在于收藏列表
  return ReaderState.favoriteBooks.some(item =>
    item.novel?.novelId === currentNovelId ||
    item.novelId === currentNovelId
  )
})
const router = useRouter();
const defaultCoverImage = "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='200' height='280' viewBox='0 0 200 280'%3E%3Crect width='200' height='280' fill='%23f3f4f6' rx='8'/%3E%3Ctext x='100' y='140' font-family='Arial' font-size='16' fill='%236b7280' text-anchor='middle'%3E书籍封面%3C/text%3E%3C/svg%3E";// 默认封面图片
const defaultAuthorAvatar = "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='200' height='280' viewBox='0 0 200 280'%3E%3Crect width='200' height='280' fill='%23f3f4f6' rx='8'/%3E%3Ctext x='100' y='140' font-family='Arial' font-size='16' fill='%236b7280' text-anchor='middle'%3E作者头像%3C/text%3E%3C/svg%3E";// 默认作者头像
//返回
function goback() {
  router.push('/Novels/Novel_Layout/category');
}
//处理图片错误
function handleImageError(event) {
  event.target.src = defaultCoverImage;
}
function getStatusClass(status) {
  switch (status) {
    case '连载': return 'status-running';
    case '完结': return 'status-completed';
    default: return 'status-running';
  }

}
// 获取分类数据
const fetchCategories = async () => {
  try {
    isLoadingCategories.value = true
    console.log('ID:', selectNovelState.novelId) // 调试
    const response = await getCategoriesByNovel(selectNovelState.novelId)
    categories.value = Array.isArray(response)
      ? response.map(item => item.categoryName)
      : [];
    console.log('最终分类数据:', categories.value) // 调试
  } catch (error) {
    console.error('获取分类失败:', error)
    categories.value = []
  } finally {
    isLoadingCategories.value = false
  }
}
// 组件挂载时获取数据
onMounted(() => {
  fetchCategories()
})

// 如果novelId可能变化，添加监听
watch(() => selectNovelState.novelId, (newVal) => {
  if (newVal) fetchCategories()
})
/*收藏逻辑*/
const toggleCollect = async () => {
  const currentNovelId = selectNovelState.novelId;
  const currentReaderId = ReaderState.readerId;
  try {
    if (isCollected.value) {
      // 取消收藏
      await deleteCollect(currentNovelId, currentReaderId);
      ReaderState.favoriteBooks = ReaderState.favoriteBooks.filter(item =>
        item.novel?.novelId !== currentNovelId &&
        item.novelId !== currentNovelId

      );
      toast("取消收藏", {
        "type": "success",
        "dangerouslyHTMLString": true
      })

    } else {
      // 添加收藏
      await addOrUpdateCollect(currentNovelId, ReaderState.readerId, 'no');
      ReaderState.favoriteBooks.push({
        novelId: currentNovelId,
        novel: selectNovelState, // 保存完整作品信息
        currentReaderId,
        isPublic: "no",
        collectTime: new Date().toISOString()
      });
      toast("收藏成功", {
        "type": "success",
        "dangerouslyHTMLString": true
      })

    }
  } catch (error) {
    console.error('收藏操作失败:', error);
    toast("收藏失败", {
      "type": "error",
      "dangerouslyHTMLString": true
    })
  }
};
//开始阅读
async function handleRead() {
  try {
    const response = await getChapter(selectNovelState.novelId, 1);
    selectNovelState.resetChapter(
      response.chapterId,
      response.title,
      response.content,
      response.wordCount,
      response.pricePerKilo,
      response.calculatedPrice,
      response.isCharged,
      response.publishTime,
      response.status
    );
    router.push('/Novels/reader');
  } catch (error) {
    toast("章节加载失败：该章节不存在！", {
      "type": "warning",
      "dangerouslyHTMLString": true
    })
  }
}
</script>

<style scoped>
/* 返回按钮样式 */
.back-button {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 8px 16px;
  margin-bottom: 20px;
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  color: #475569;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.back-button:hover {
  background: #e2e8f0;
  color: #334155;
}

/* 书籍展示卡片 */
.book-display-card {
  max-width: 1200px;
  margin: 0 auto 30px;
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  overflow: hidden;
  border: 1px solid #f1f5f9;
}

.book-content {
  display: flex;
  flex-wrap: wrap;
}

/* 左侧图片区域 */
.book-image-section {
  flex: 0 0 220px;
  background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%);
  padding: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.image-wrapper {
  position: relative;
  transition: transform 0.3s ease;
}

.image-wrapper:hover {
  transform: scale(1.03);
}

.image-wrapper::before {
  content: '';
  position: absolute;
  inset: -3px;
  background: linear-gradient(135deg, #3b82f6, #8b5cf6);
  border-radius: 12px;
  opacity: 0.2;
  filter: blur(6px);
  transition: opacity 0.3s ease;
  z-index: 0;
}

.image-wrapper:hover::before {
  opacity: 0.4;
}

.book-cover {
  position: relative;
  width: 180px;
  height: 240px;
  object-fit: cover;
  border-radius: 8px;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.15);
  border: 1px solid rgba(255, 255, 255, 0.2);
  z-index: 1;
}

/* 右侧信息区域 */
.book-info-section {
  flex: 1 1 400px;
  padding: 40px;
  min-width: 400px;
}

/* 标题行布局*/
.tag-row {
  display: flex;
  align-items: center;
  /* 确保垂直居中 */
  gap: 12px;
  margin-bottom: 12px;
  /* 统一控制与下方内容的间距 */
}

/* 标题样式修正 */
.book-title {
  font-size: 24px;
  font-weight: 700;
  color: #1e293b;
  margin: 0;
  /* 移除所有margin */
  line-height: 1.3;
  display: inline-flex;
  /* 确保参与flex布局的对齐 */
  align-items: center;
  /* 垂直居中 */
}

/* 分数徽章样式 */
.score-badge {
  background: #fff9c4;
  color: #ff8f00;
  padding: 4px 8px;
  border-radius: 12px;
  font-weight: bold;
  font-size: 16px;
  display: inline-flex;
  align-items: center;
  height: 100%;
  /* 确保高度与标题一致 */
}

/* 状态徽章样式修正 */
.status-badge {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 6px 14px;
  border-radius: 20px;
  font-size: 14px;
  font-weight: 600;
  margin: 0;
  height: 100%;
  /* 确保高度与标题一致 */
}

.status-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
}

/* 状态颜色样式保持不变 */
.status-running {
  background-color: #dcfce7;
  color: #166534;
}

.status-running .status-dot {
  background-color: #22c55e;
}

.status-completed {
  background-color: #dbeafe;
  color: #1e40af;
}

.status-completed .status-dot {
  background-color: #3b82f6;
}



/* 分类徽章样式 */
.category-badge {
  display: inline-flex;
  align-items: center;
  background: #e0f2fe;
  /* 浅蓝色背景 */
  color: #0369a1;
  /* 深蓝色文字 */
  padding: 4px 12px;
  border-radius: 16px;
  font-size: 14px;
  font-weight: 600;
  margin-right: 8px;
  height: 100%;
  transition: all 0.2s ease;
}

.category-badge:hover {
  background: #bae6fd;
  /* 悬停时加深颜色 */
  transform: translateY(-1px);
}

/* 统计行基础样式 */
.stats-row {
  display: flex;
  gap: 25px;
  margin-top: 10px;
  font-family: -apple-system, sans-serif;
}

/* 数字部分 */
.stat-value {
  font-size: 15px;
  /* 比原文字大1px */
  font-weight: 600;
  /* 半粗体 */
  color: #1e293b;
  /* 深灰色（接近黑） */
}

/* 单位部分 */
.stat-unit {
  font-size: 13px;
  /* 比数字小2px */
  font-weight: 400;
  /* 正常粗细 */
  color: #64748b;
  /* 中灰色 */
  margin-left: 2px;
  /* 与数字微间距 */
}

.newest-chapter {
  color: #94a3b8;
  /* 浅灰色文字 */
  font-size: 15px;
  /* 适中字号 */
  font-weight: 500;
  /* 中等字重 */
  letter-spacing: 0.3px;
  /* 轻微字距 */
  margin: 8px 0 12px 0;
  /* 上下间距 */
  padding: 6px 0;
  /* 内边距 */
  display: flex;
  align-items: center;
  gap: 8px;
}

/* 新增按钮组样式 */
.action-buttons {
  display: flex;
  gap: 12px;
  margin-top: 20px;
  flex-wrap: wrap;
}

.blue-border-btn {
  /* 基础样式 */
  background: transparent;
  border: 2px solid #1890ff;
  color: #1890ff;
  padding: 8px 16px;
  border-radius: 6px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s ease;

  /* 图标样式 */
  display: inline-flex;
  align-items: center;
  gap: 6px;
}

/* 悬停效果 */
.blue-border-btn:hover {
  background: rgba(24, 144, 255, 0.08);
  border-color: #40a9ff;
  color: #40a9ff;
  transform: translateY(-1px);
  box-shadow: 0 2px 8px rgba(24, 144, 255, 0.15);
}

/* 点击效果 */
.blue-border-btn:active {
  transform: translateY(0);
  border-color: #096dd9;
  color: #096dd9;
}

/* 新增的已收藏状态 */
.blue-border-btn.is-collected {
  border-color: #d9d9d9;
  color: #8c8c8c;
  background: #f5f5f5;
}

.blue-border-btn.is-collected:hover {
  background: #f0f0f0;
  border-color: #bfbfbf;
  color: #595959;
}

/* 响应式设计 */
@media (max-width: 768px) {
  .action-buttons {
    flex-direction: column;
    gap: 8px;
  }

  .blue-border-btn {
    width: 100%;
    justify-content: center;
  }
}

.author-card-section {
  flex: 0 0 300px;
  padding: 40px 20px 40px 0;
  display: flex;
  align-items: center;
}

.author-card {
  background: #f5f7fa;
  border-radius: 16px;
  box-shadow: 0 2px 8px rgba(36, 37, 38, 0.04);
  padding: 20px;
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.author-avatar {
  width: 72px;
  height: 72px;
  border-radius: 50%;
  background: #e5e7eb;
  margin-bottom: 12px;
  object-fit: cover;
  border: 1px solid #e0e0e0;
}

.author-card h2 {
  margin: 8px 0 0 0;
  /* 调整标题边距 */
  font-size: 20px;
  font-weight: 600;
  color: #333;
  text-align: center;
}

.author-name {
  font-family: 'Helvetica Neue', sans-serif;
  letter-spacing: 0.5px;
  color: #374151;
  background: transparent;
  padding-left: 0;
  border-left: none;
  font-weight: 600;
}

.author-brief {
  font-size: 13px;
  color: #64748b;
  margin: 0;
  text-align: center;
  line-height: 1.4;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  overflow: hidden;
  max-width: 90%;
  /* 防止溢出 */
}

.author-data-cards {
  display: flex;
  justify-content: space-between;
  margin-top: 20px;
  width: 90%;
  background: white;
  border-radius: 12px;
  padding: 16px 16px 16px 16px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.data-card {
  flex: 1;
  text-align: center;
  position: relative;
}

.data-card:not(:last-child)::after {
  content: "";
  position: absolute;
  right: 0;
  top: 50%;
  transform: translateY(-50%);
  height: 40px;
  width: 1px;
  background: #f1f5f9;
}

.data-value {
  font-size: 24px;
  font-weight: 700;
  color: #1e293b;
  margin-bottom: 4px;
}


.data-label {
  font-size: 14px;
  color: #64748b;
}

.introduction-text {
  line-height: 1.6;
  text-align: justify;
}

.novel-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.nav-menu ul {
  display: flex;
  list-style: none;
  padding: 0;
  margin: 0 0 20px 0;
  border-bottom: 1px solid #e0e0e0;
}

.nav-menu li {
  margin-right: 30px;
}

.nav-menu a {
  text-decoration: none;
  color: #333;
  font-size: 18px;
  padding: 10px 0;
  display: inline-block;
  position: relative;
}

.nav-menu a.active-link {
  color: #1890ff;
  font-weight: bold;
}

.nav-menu a.active-link:after {
  content: '';
  position: absolute;
  bottom: -1px;
  left: 0;
  right: 0;
  height: 2px;
  background-color: #1890ff;
}

.chapter-count {
  font-size: 14px;
  color: #888;
  margin-left: 5px;
  font-weight: normal;
}

.main-content {
  padding: 20px 0;
}
</style>