<template>
  <!-- 页面容器 -->
  <div class="page-container">
    <!-- 轮播器部分 -->
    <div class="carousel-container" @mouseenter="isHovering = true" @mouseleave="isHovering = false">
      <div class="carousel">
        <div class="carousel-inner" :style="{ transform: `translateX(-${currentSlide * 100}%)` }">
          <div class="carousel-item" v-for="(item, index) in carouselItems" :key="index"
            :class="{ active: currentSlide === index }">
            <div class="carousel-caption" v-if="item.caption">
              <h3>{{ item.caption.title }}</h3>
              <p>{{ item.caption.description }}</p>
            </div>
          </div>
        </div>
        <button class="carousel-control prev" @click="prevSlide">‹</button>
        <button class="carousel-control next" @click="nextSlide">›</button>
        <div class="carousel-indicators">
          <span v-for="(item, index) in carouselItems" :key="index" :class="{ active: currentSlide === index }"
            @click="goToSlide(index)"></span>
        </div>
      </div>
    </div>

    <!-- 小说列表部分 -->
    <div class="page-header">
      <h2>我的小说</h2>
      <!-- 导航到创建小说界面 -->
      <router-link to="/author/novels/create" class="create-btn">
        <span>+</span> 创建小说
      </router-link>
    </div>
    <!-- 小说网格布局 -->
    <div class="novel-grid">
      <!-- 每个小说卡片 -->
      <div v-for="novel in novels" :key="novel.novel_id" class="novel-card">
        <router-link :to="'/author/novels/' + novel.novel_id" class="novel-link">
          <div class="status-badge" :class="novel.status">{{ novel.status }}</div>
          <div class="novel-cover">
          </div>
          <div class="novel-info">
            <h3 class="novel-title">{{ novel.novel_name }}</h3>
            <p class="meta">
              <span>{{ novel.total_word_count }}字</span>
              <span>{{ novel.chapter_count }}章</span>
            </p>
          </div>
        </router-link>
      </div>
    </div>
  </div>
</template>

<script>
// 导入轮播图逻辑hook
import { useCarousel } from '@/utils/carousel'
// 导入小说数据store
import { novelsStore } from '@/stores/Novels'
// 导入轮播图数据
import { carouselItems } from '@/stores/CarouselData'

export default {
  setup() {
    // 使用轮播图hook获取相关状态和方法
    const {
      currentSlide,  // 当前幻灯片索引
      isHovering,    // 是否悬停在轮播图上
      prevSlide,     // 上一张方法
      nextSlide,     // 下一张方法
      goToSlide      // 跳转到指定幻灯片方法
    } = useCarousel(carouselItems)

    return {
      carouselItems,  // 轮播图数据
      currentSlide,   // 当前幻灯片索引
      isHovering,     // 悬停状态
      prevSlide,      // 上一张方法
      nextSlide,      // 下一张方法
      goToSlide,      // 跳转方法
      novels: novelsStore.novels  // 小说列表数据
    }
  }
}
</script>
<style scoped>
/* 轮播器样式 */
.carousel-container {
  margin-bottom: 40px;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  position: relative;
}

/* 轮播器主体 */
.carousel {
  position: relative;
  width: 100%;
  height: 300px;
  overflow: hidden;
}

/* 轮播内容容器 */
.carousel-inner {
  display: flex;
  transition: transform 0.6s ease-in-out;
  height: 100%;
  width: 100%;
}

/* 单个轮播项 */
.carousel-item {
  min-width: 100%;
  height: 100%;
  flex-shrink: 0;
  position: relative;
  opacity: 0;
  transition: opacity 0.6s ease-in-out;
}

/* 当前激活的轮播项 */
.carousel-item.active {
  opacity: 1;
}

/* 轮播图片样式 */
.carousel-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
}

/* 轮播文字说明区域 */
.carousel-caption {
  position: absolute;
  bottom: 20px;
  left: 20px;
  right: 20px;
  background: rgba(0, 0, 0, 0.5);
  color: white;
  padding: 15px;
  border-radius: 4px;
  max-width: 500px;
}

/* 轮播标题 */
.carousel-caption h3 {
  margin: 0 0 8px 0;
  font-size: 1.5rem;
}

/* 轮播描述 */
.carousel-caption p {
  margin: 0;
  font-size: 1rem;
}

/* 轮播控制按钮 */
.carousel-control {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  background: rgba(0, 0, 0, 0.3);
  color: white;
  border: none;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  font-size: 20px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 10;
  transition: all 0.3s;
  opacity: 0;
}

/* 鼠标悬停时显示控制按钮 */
.carousel:hover .carousel-control {
  opacity: 1;
}

/* 控制按钮悬停效果 */
.carousel-control:hover {
  background: rgba(0, 0, 0, 0.6);
}

/* 上一页按钮位置 */
.prev {
  left: 20px;
}

/* 下一页按钮位置 */
.next {
  right: 20px;
}

/* 轮播指示器 */
.carousel-indicators {
  position: absolute;
  bottom: 20px;
  left: 50%;
  transform: translateX(-50%);
  display: flex;
  gap: 8px;
  z-index: 10;
}

/* 单个指示点 */
.carousel-indicators span {
  width: 12px;
  height: 12px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.5);
  cursor: pointer;
  transition: all 0.3s;
}

/* 当前激活的指示点 */
.carousel-indicators span.active {
  background: white;
  transform: scale(1.2);
}

/* 指示点悬停效果 */
.carousel-indicators span:hover {
  background: rgba(255, 255, 255, 0.8);
}

/* 页面基础样式 */
.page-container {
  padding: 30px;
  max-width: 1200px;
  margin: 0 auto;
  font-family: 'Microsoft YaHei', sans-serif;
}

/* 页面头部样式 */
.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
}

/* 页面标题 */
.page-header h2 {
  font-size: 1.8rem;
  color: #2c3e50;
  margin: 0;
}

/* 创建按钮样式 */
.create-btn {
  background-color: #3498db;
  color: white;
  padding: 10px 20px;
  border-radius: 6px;
  text-decoration: none;
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: 500;
  transition: all 0.3s;
}

/* 创建按钮悬停效果 */
.create-btn:hover {
  background-color: #2980b9;
  transform: translateY(-2px);
}

/* 小说网格布局 */
.novel-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
  gap: 25px;
}

/* 小说卡片样式 */
.novel-card {
  background: transparent;
  position: relative;
  transition: all 0.3s;
}

/* 小说链接样式 */
.novel-link {
  text-decoration: none;
  color: inherit;
}

/* 小说卡片悬停效果 */
.novel-card:hover {
  transform: translateY(-3px);
}

/* 状态标签样式 */
.status-badge {
  position: absolute;
  top: -8px;
  left: -8px;
  padding: 4px 8px;
  border-radius: 0 0 4px 0;
  font-size: 12px;
  font-weight: bold;
  color: white;
  z-index: 2;
  border-radius: 10%;
  text-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
}

/* 连载中状态样式 */
.status-badge.连载中 {
  background-color: #3498db;
}

/* 已完结状态样式 */
.status-badge.已完结 {
  background-color: #2ecc71;
}

/* 审核中状态样式 */
.status-badge.审核中 {
  background-color: #f39c12;
}

/* 小说封面样式 */
.novel-cover {
  width: 100%;
  height: 220px;
  position: relative;
  overflow: hidden;
  border-radius: 6px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

/* 封面图片样式 */
.cover-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s;
}

/* 封面图片悬停效果 */
.novel-card:hover .cover-image {
  transform: scale(1.05);
}

/* 小说信息区域 */
.novel-info {
  padding: 12px 0;
  text-align: center;
}

/* 小说标题样式 */
.novel-title {
  color: #2c3e50;
  font-size: 16px;
  font-weight: bold;
  margin: 10px 0 5px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  text-decoration: none;
}

/* 元信息样式 */
.meta {
  display: flex;
  justify-content: space-around;
  font-size: 13px;
  color: #7f8c8d;
}
</style>