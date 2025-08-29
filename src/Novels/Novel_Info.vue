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
        <h1 class="newest-chapter" v-if="hasChapter">
          最新章节第{{ chapterId }}章 {{ formattedDate }}
        </h1>
        <!-- 新增的蓝色按钮组 -->
        <div class="action-buttons">
          <button class="blue-border-btn" @click="handleRead">
            开始阅读
          </button>
          <button class="blue-border-btn" :class="{ 'is-collected': isCollected }" @click="toggleCollect">
            {{ isCollected ? '已收藏' : '收藏作品' }}
          </button>
          <button class="blue-border-btn" :class="{ 'is-recommended': isRecommended }" @click="handleRecommend">
            {{ isRecommended ? '已推荐' : '推荐作品' }}
          </button>
          <button class="blue-border-btn" @click="showRewardDialog = true">
            打赏作品
          </button>
        </div>
      </div>
      <!-- 右侧作者卡片区域 -->
      <div class="author-card-section">
        <div class="author-card">
          <img :src="selectNovelState.formattedauthorAvatarUrl || defaultAuthorAvatar" class="author-avatar"
            @click.stop="goAuthorHome" />
          <h2 @click.stop="goAuthorHome">{{ selectNovelState.authorName }}</h2>
          <p class="author-brief">简介:{{ selectNovelState.a_introduction || '暂无作者简介' }}</p>
          <div class="author-data-cards">
            <div class="data-card">
              <div class="data-value">{{ authorNovelCount }}</div>
              <div class="data-label">作品总数</div>
            </div>
            <div class="data-card">
              <div class="data-value">{{ authorWordCount }}</div>
              <div class="data-label">累计字数</div>
            </div>
            <div class="data-card">

              <div class="data-value">{{ authorRegisterDays }}</div>
              <div class="data-label">创作天数</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- 导航栏展示 -->
  <div class="novel-container">
    <!-- 修改导航栏部分 -->
    <nav class="nav-menu" ref="mainNav">
      <ul>
        <li>
          <a :class="{ 'active-link': $route.path.endsWith('/home') }"
            @click.prevent="switchTab('/Novels/Novel_Info/home')" href="#">作品介绍</a>
        </li>
        <li>
          <a :class="{ 'active-link': $route.path.endsWith('/chapter') }"
            @click.prevent="switchTab('/Novels/Novel_Info/chapter')" href="#">作品目录</a>
        </li>
        <li>
          <a :class="{ 'active-link': $route.path.endsWith('/comment') }"
            @click.prevent="switchTab('/Novels/Novel_Info/comment')" href="#">评论</a>
        </li>
      </ul>
    </nav>
    <main class="main-content">
      <router-view></router-view>
    </main>
  </div>
  <!-- 改成 div 弹窗（样式上模拟 dialog 就行） -->
  <div v-if="showRecommendDialog" class="recommend-dialog-overlay">
    <div class="recommend-dialog">
      <div class="dialog-content">
        <h3>{{ isRecommended ? '推荐状态' : '推荐原因' }}</h3>
        <template v-if="!isRecommended">
          <textarea v-model="recommendReason" placeholder="这本小说太棒了，快来推荐一下吧!(可选)" class="recommend-textarea"></textarea>
        </template>
        <template v-else>
          <p class="already-recommended">您已推荐</p>
        </template>
        <div class="dialog-actions">
          <button class="cancel-btn" @click="showRecommendDialog = false">退出</button>
          <button v-if="!isRecommended" @click="submitRecommend" class="confirm-btn">
            确认推荐
          </button>
          <button v-if="isRecommended" @click="cancelRecommend" class="cancel-recommend-btn">
            取消推荐
          </button>
        </div>
      </div>
    </div>
  </div>
  <!-- 新增的固定位置按钮 -->
  <div class="fixed-buttons">
    <!-- 阅读按钮 -->
    <div class="fixed-button" @click="handleRead">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="24" height="24">
        <path fill="currentColor"
          d="M21.59 11.59h-1.34a1 1 0 0 0 0 2h1.34a1 1 0 0 0 0-2zm-18.18 0H2.07a1 1 0 0 0 0 2h1.34a1 1 0 0 0 0-2zm15.09-7h-1.34a1 1 0 0 0 0 2h1.34a1 1 0 0 0 0-2zm-12.73 0H2.07a1 1 0 0 0 0 2h1.34a1 1 0 0 0 0-2zm15.09 0h-1.34a1 1 0 0 0 0 2h1.34a1 1 0 0 0 0-2zm-12.73 0H2.07a1 1 0 0 0 0 2h1.34a1 1 0 0 0 0-2zm9.09 11H8.66a1 1 0 0 0 0 2h6.36a1 1 0 0 0 0-2zm0-7H8.66a1 1 0 0 0 0 2h6.36a1 1 0 0 0 0-2zm0-7H8.66a1 1 0 0 0 0 2h6.36a1 1 0 0 0 0-2z" />
      </svg>
      <span>阅读</span>
    </div>
    <!-- 充值按钮 -->
    <div class="fixed-button" @click="handleRecharge">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="24" height="24">
        <path fill="currentColor"
          d="M12 2a10 10 0 1 0 10 10 10 10 0 0 0-10-10zm1 14a1 1 0 0 1-2 0v-3h-2a1 1 0 0 1 0-2h3a1 1 0 0 1 1 1zm4.6-6.08a1 1 0 0 1-.2 1.4 7 7 0 0 1-9.93-1.4 1 1 0 1 1 1.6-1.2 5 5 0 0 0 7.13.8 1 1 0 0 1 1.4.2z" />
      </svg>
      <span>充值</span>
    </div>

    <!-- 评分按钮 -->
    <div class="fixed-button" @click="handleRatingClick">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="24" height="24">
        <path :fill="hasRating ? '#1890ff' : 'currentColor'"
          d="M22 9.24l-7.19-.62L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21 12 17.27 18.18 21l-1.63-7.03L22 9.24zM12 15.4l-3.76 2.27 1-4.28-3.32-2.88 4.38-.38L12 6.1l1.71 4.04 4.38.38-3.32 2.88 1 4.28L12 15.4z" />
      </svg>
      <span :style="{ color: hasRating ? '#1890ff' : '' }">
        {{ hasRating ? '已评分' : '评分' }}
      </span>
    </div>

    <!-- 回到顶部 -->
    <div class="fixed-button" @click="scrollToTop">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24" height="24">
        <path fill="currentColor"
          d="M160 832h704a32 32 0 1 1 0 64H160a32 32 0 1 1 0-64m384-578.304V704h-64V247.296L237.248 490.048 192 444.8 508.8 128l316.8 316.8-45.312 45.248z">
        </path>
      </svg> <span>顶部</span>
    </div>

  </div>
  <!-- 评分弹窗 -->
  <div v-if="showRatingDialog" class="rating-dialog-overlay">
    <div class="rating-dialog">
      <div class="dialog-header">
        <h3>{{ hasRating ? '您的评分' : '为作品评分' }}</h3>
        <button class="close-btn" @click="closeDialog">&times;</button>
      </div>

      <div class="stars-container">
        <div v-for="i in 10" :key="i" class="star" @click="selectRating(i)" @mouseover="hoverRating = i"
          @mouseleave="hoverRating = null">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="28" height="28">
            <path :fill="getStarColor(i)"
              d="M12 17.27L18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21z" />
          </svg>
        </div>
      </div>

      <div class="rating-value">
        {{ currentRating || hoverRating || 0 }} 分
      </div>

      <button v-if="!hasRating" class="submit-btn" @click="submitRating" :disabled="!currentRating">
        提交评分
      </button>
    </div>
  </div>


  <!-- 打赏弹窗 -->
  <div v-if="showRewardDialog" class="reward-dialog-overlay">
    <div class="reward-dialog">

      <div class="dialog-header"
        style="display: flex; justify-content: center; align-items: center; position: relative;">
        <h3 style="margin: 0;">打赏</h3>
        <button class="close-btn " @click="showRewardDialog = false"
          style="position: absolute; right: 20px;">&times;</button>
      </div>

      <div class="reward-options">
        <div v-for="option in rewardOptions" :key="option.value"
          :class="['reward-option', { 'selected': selectedReward === option.value }]"
          @click="selectReward(option.value)">
          <span>{{ option.label }}</span>
          <svg v-if="selectedReward === option.value" class="check-icon" viewBox="0 0 24 24">
            <path d="M20 6L9 17l-5-5" stroke="currentColor" stroke-width="2" fill="none" />
          </svg>
        </div>
      </div>

      <div class="balance-info">
        <span>账户余额 {{ accountBalance }} 币</span>
        <span>本次打赏 {{ selectedReward }} 币</span>
      </div>
      <button class="confirm-reward-btn" @click="confirmReward">
        确认打赏
      </button>
    </div>
  </div>
  <div v-if="showBalanceInsufficientDialog" class="insufficient-dialog-overlay">
    <div class="insufficient-dialog">
      <div class="dialog-header">
        <h3>打赏</h3>
        <button class="close-btn" @click="showBalanceInsufficientDialog = false">&times;</button>
      </div>
      <div class="insufficient-content">
        <p class="insufficient-message">账户余额不足</p>
        <div class="amount-info">
          <span>本次打赏 {{ selectedReward }} 币</span>
          <span>账户余额 {{ accountBalance }} 币·还差 {{ (selectedReward - accountBalance).toFixed(2) }} 币</span>
        </div>
        <div class="quick-payment">
          <button class="recharge-btn" @click="goToRecharge">去充值</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>

import { ref, watch, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { SelectNovel_State, readerState } from '@/stores/index';
import { getCategoriesByNovel } from '@/API/NovelCategory_API';
import { addOrUpdateCollect, deleteCollect } from '@/API/Collect_API';
import { getNovelWordCount, getNovelRecommendCount, getNovelCollectCount, getLatestPublishedChapter } from '@/API/Novel_API';
import { getAuthorNovelCount, getAuthorTotalWordCount, getAuthorRegisterDays } from '@/API/Author_API';
import { getChapter } from '@/API/Chapter_API';
import { addRecommend, deleteRecommend } from '@/API/Recommend_API';
import { getReaderBalance, addOrUpdateRecentReading, getLastReadChapterId } from '@/API/Reader_API';
import { rewardNovel } from '@/API/Reward_API';
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
import { getRatesByReader, addRate } from '@/API/Rate_API';
import { checkPurchase } from '@/API/Purchase_API';
const selectNovelState = SelectNovel_State();      //当前选择的小说对象
const ReaderState = readerState();                   //当前读者对象
const categories = ref([]);                          //分类数组
const isLoadingCategories = ref(false);            //是否在加载
const novelWordCount = ref(0);                       //当前小说的字数
const collectedCount = ref(0);                       //当前小说的被收藏数
const recommendCount = ref(0);                       //当前小说的被推荐数
const authorNovelCount = ref(0);                      //当前作者的创作小说数
const authorWordCount = ref(0);                      //当前作者的创作总字数
const authorRegisterDays = ref(0);                   //当前作者的创作天数
const router = useRouter();
const showRecommendDialog = ref(false);               // 是否显示推荐弹窗
const recommendReason = ref('');                     // 用户输入的推荐理由
const showRewardDialog = ref(false);                  // 是否显示打赏弹窗
const accountBalance = ref(0);                        // 账号余额
const selectedReward = ref(1);                      // 默认选中1点打赏金额
const chapterId = ref(null)
const publishTime = ref(null)
const hasChapter = ref(false)
const hasRating = ref(false);                           // 是否评分
const currentRating = ref(0);                             //当前评分
const showRatingDialog = ref(false);
const hoverRating = ref(null);


const defaultCoverImage = "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='200' height='280' viewBox='0 0 200 280'%3E%3Crect width='200' height='280' fill='%23f3f4f6' rx='8'/%3E%3Ctext x='100' y='140' font-family='Arial' font-size='16' fill='%236b7280' text-anchor='middle'%3E书籍封面%3C/text%3E%3C/svg%3E";// 默认封面图片
const defaultAuthorAvatar = "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='200' height='280' viewBox='0 0 200 280'%3E%3Crect width='200' height='280' fill='%23f3f4f6' rx='8'/%3E%3Ctext x='100' y='140' font-family='Arial' font-size='16' fill='%236b7280' text-anchor='middle'%3E作者头像%3C/text%3E%3C/svg%3E";// 默认作者头像
const rewardOptions = [
  { value: 1, label: '1币' },
  { value: 10, label: '10币' },
  { value: 50, label: '50币' },
  { value: 100, label: '100币' },
  { value: 200, label: '200币' },
  { value: 1000, label: '1千币' },
  { value: 5000, label: '5千币' },
  { value: 10000, label: '1万币' }
];


//是否被收藏
const isCollected = computed(() => {
  //当前小说ID
  const currentNovelId = selectNovelState.novelId;
  //console.log("是否收藏",currentNovelId)
  //  console.log("本地收藏",ReaderState.favoriteBooks)
  const a = ReaderState.favoriteBooks.some(item =>
    item.novelId === currentNovelId)
  // console.log("a",a)
  // 检查是否存在于收藏列表
  return a

})
//是否被推荐
const isRecommended = computed(() => {
  //当前小说ID
  const currentNovelId = selectNovelState.novelId;
  // 检查是否存在于收藏列表
  return ReaderState.recommendBooks.some(item =>
    item.novel?.novelId === currentNovelId ||
    item.novelId === currentNovelId
  )
})


//处理图片错误
function handleImageError(event) {
  event.target.src = defaultCoverImage;
}
//获取小说状态样式
function getStatusClass(status) {
  switch (status) {
    case '连载': return 'status-running';
    case '完结': return 'status-completed';
    default: return 'status-running';
  }

}
const formattedDate = computed(() => {
  if (!publishTime.value) return ''

  const date = new Date(publishTime.value)
  return `${date.getFullYear()}年${date.getMonth() + 1}月${date.getDate()}日`
})

const goAuthorHome = () => {
  router.push(`/author/${selectNovelState.authorId}`);
};
/*数据获取部分 */
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
// 获取字数的函数
const fetchWordCount = async () => {
  try {
    const response = await getNovelWordCount(selectNovelState.novelId)
    novelWordCount.value = response.data?.totalWords || response?.totalWords || 0

    console.log('最终字数:', novelWordCount.value) // 调试
  } catch (error) {
    console.error('获取字数失败:', error)
    novelWordCount.value = 0
  }
}
// 获取推荐数的函数

const fetchRecommendCount = async () => {
  try {
    const response = await getNovelRecommendCount(selectNovelState.novelId)
    recommendCount.value = response.data?.recommendCount || response?.recommendCount || 0
    console.log('最终推荐数:', recommendCount.value) // 调试
  } catch (error) {
    console.error('获取推荐数失败:', error)
    recommendCount.value = 0
  }

}
// 获取收藏数的函数
const fetchCollectedCount = async () => {
  try {
    const response = await getNovelCollectCount(selectNovelState.novelId)
    collectedCount.value = response.data?.collectCount || response?.collectCount || 0
    console.log('最终收藏数:', collectedCount.value) // 调试
  } catch (error) {
    console.error('获取收藏数失败:', error)
    collectedCount.value = 0
  }

}
// 获取作者创作书籍数的函数
const fetchAuthorNovelCount = async () => {
  try {
    const response = await getAuthorNovelCount(selectNovelState.authorId)
    authorNovelCount.value = response.data?.novelCount || response?.novelCount || 0
    console.log('最终作者创作数:', authorNovelCount.value) // 调试
  } catch (error) {
    console.error('获取作者作品数失败:', error)
    collectedCount.value = 0
  }

}
//获取作者创作总字数的函数
const fetchAuthorWordCount = async () => {
  try {
    const response = await getAuthorTotalWordCount(selectNovelState.authorId)
    authorWordCount.value = response.data?.totalWordCount || response?.totalWordCount || 0
  } catch (error) {
    console.error('获取作者创作字数失败:', error)
    authorWordCount.value = 0
  }
}
//获取作者创作天数
const fetchAuthorRegisterDays = async () => {
  try {
    const response = await getAuthorRegisterDays(selectNovelState.authorId)
    authorRegisterDays.value = response.data?.registerDays || response?.registerDays || 0
    console.log('获取作者创作天数:', authorRegisterDays.value)
  } catch (error) {
    console.error('获取作者创作天数失败:', error)
    authorRegisterDays.value = 0
  }
}
//获取账号余额的函数
const fetchReaderBalance = async () => {
  try {
    const response = await getReaderBalance(ReaderState.readerId)
    accountBalance.value = response.data?.balance || response?.balance || 0
    console.log('获取用户余额:', accountBalance.value)
  } catch (error) {
    console.error('获取用户余额失败:', error)
    accountBalance.value = 0
  }
}
//获取最新章节
const fetchNewestChapter = async () => {
  try {
    const response = await getLatestPublishedChapter(selectNovelState.novelId)
    chapterId.value = response?.chapterId || 0
    publishTime.value = response?.publishTime || ''
    hasChapter.value = true
    console.log('response:', response)
  } catch (error) {
    if (error.message !== 'Error: response') {
      console.error('获取章节信息失败:', error)
    }
    hasChapter.value = false
  }
}
// 获取评分状态
const fetchRatingStatus = async () => {
  try {
    const ratings = await getRatesByReader(ReaderState.readerId) // 假设readerId是ref
    const rating = ratings.find(r => r.novelId === selectNovelState.novelId)
    hasRating.value = !!rating
    currentRating.value = rating?.score || null
    console.log('评分状态:', hasRating.value, '评分:', currentRating.value)
  } catch (error) {
    if (error.message !== 'Error: response') {
      console.error('获取评分信息失败:', error)
    }
    hasRating.value = false
    currentRating.value = null
  }
}
// 1.监听 novelId 变化,变化时加载数据
watch(
  () => selectNovelState.novelId,
  async (newNovelId) => {
    if (newNovelId) {
      try {
        await Promise.all([
          fetchCategories(),
          fetchWordCount(),
          fetchRecommendCount(),
          fetchCollectedCount(),
          fetchAuthorNovelCount(),
          fetchAuthorWordCount(),
          fetchAuthorRegisterDays(),
          fetchNewestChapter(),
          fetchRatingStatus()
        ])
        console.log('小说详情页数据更新完成！')
      } catch (error) {
        console.error('数据加载失败:', error)
      }
    }
  },
  { immediate: true } // 替代 onMounted，首次加载时自动执行
)
// 2. 单独监听弹窗（精确控制余额刷新）
watch(
  () => showRewardDialog.value,
  async (isOpen) => {
    if (isOpen) {
      try {
        await fetchReaderBalance()
      } catch (error) {
        console.error('刷新余额失败:', error)
      }
    }
  }
)


/*按钮逻辑部分*/
//返回按钮
function goback() {
  router.back();
}
//切换标签页
function switchTab(path) {
  router.replace(path)
}
//回到顶部
const scrollToTop = () => {
  window.scrollTo({ top: 0, behavior: 'smooth' });
};
onMounted(() => {
  scrollToTop();
});
//收藏按钮的逻辑

const toggleCollect = async () => {
  const currentNovelId = selectNovelState.novelId;
  const currentReaderId = ReaderState.readerId;
  try {
    if (isCollected.value) {
      // 取消收藏
      await deleteCollect(currentNovelId, currentReaderId);
      const newFavorites = ReaderState.favoriteBooks.filter(item =>
        item.novel?.novelId !== currentNovelId &&
        item.novelId !== currentNovelId
      )
      ReaderState.updateFavoriteBooks(newFavorites)


      toast("取消收藏", {
        "type": "success",
        "dangerouslyHTMLString": true
      })
    } else {
      // 添加收藏
      await addOrUpdateCollect(currentNovelId, ReaderState.readerId, 'no');
      // 只提取需要的小说信息字段
      const novelData = {
        novelId: selectNovelState.novelId,
        authorId: selectNovelState.authorId,
        novelName: selectNovelState.novelName,
        introduction: selectNovelState.introduction,
        createTime: selectNovelState.createTime,
        coverUrl: selectNovelState.coverUrl,
        score: selectNovelState.score,
        totalWordCount: selectNovelState.totalWordCount,
        recommendCount: selectNovelState.recommendCount,
        collectedCount: selectNovelState.collectedCount,
        status: selectNovelState.status,
        originalNovelId: selectNovelState.originalNovelId,
        totalPrice: selectNovelState.totalPrice
      };
      //  更新推荐列表（安全方式）
      const newFavoriteBooks = [
        ...ReaderState.favoriteBooks, // 解构原有数组
        {
          novelId: currentNovelId,
          novel: novelData, // 保存完整作品信息
          readerId: currentReaderId,
          isPublic: "no",
          collectTime: new Date().toISOString()
        }
      ]

      //  通过 action 更新（触发同步）
      ReaderState.updateFavoriteBooks(newFavoriteBooks)

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
//开始阅读按钮的逻辑
async function handleRead() {
  try {
    // 获取用户上次阅读的章节ID，如果没有记录则返回1
    let chapterIdToRead = 1;
    try {
      const lastReadResponse = await getLastReadChapterId(
        ReaderState.readerId,
        selectNovelState.novelId
      );
      chapterIdToRead = lastReadResponse || 1;
    } catch (error) {
      console.warn("获取阅读历史失败，使用默认第1章:", error);
      chapterIdToRead = 1;
    }
    // 使用 let 声明 response，因为后面可能需要重新赋值
    let response = await getChapter(selectNovelState.novelId, chapterIdToRead);
    if (response.status === '首次审核' || response.status === '草稿') {
      toast(`暂无第${chapterIdToRead}章`, {
        "type": "info",
        "dangerouslyHTMLString": true
      });
      // 如果目标章节不存在，尝试获取第1章
      if (chapterIdToRead !== 1) {
        try {
          const firstChapterResponse = await getChapter(selectNovelState.novelId, 1);
          if (firstChapterResponse.status !== '首次审核' && firstChapterResponse.status !== '草稿') {
            chapterIdToRead = 1;
            response = firstChapterResponse;
          }
        } catch (fallbackError) {
          console.error("获取第1章也失败:", fallbackError);
          return;
        }
      } else {
        return;
      }
    }
    // 检查章节购买状态
    if (response.status === '已发布' && response.isCharged === '是') {
      const purchaseStatus = await checkPurchase(
        ReaderState.readerId,
        selectNovelState.novelId,
        chapterIdToRead
      );
      if (!purchaseStatus?.hasPurchased) {
        toast(`第${chapterIdToRead}章需要购买后才能阅读`, {
          "type": "info",
          "dangerouslyHTMLString": true
        });
        return;
      }
    }
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
    // 添加或更新阅读记录（使用实际阅读的章节ID）
    try {
      await addOrUpdateRecentReading(
        ReaderState.readerId,      // 读者ID
        selectNovelState.novelId,  // 小说ID
        chapterIdToRead            // 实际阅读的章节ID
      );
    } catch (historyError) {
      console.error("记录阅读历史失败:", historyError);
    }
    // 跳转到阅读页面
    router.push('/Novels/reader');
  } catch (error) {
    console.error("阅读失败:", error);
    toast("无法获取章节内容", {
      "type": "error",
      "dangerouslyHTMLString": true
    });
  }
}

//推荐按钮的逻辑
const handleRecommend = async () => {
  console.log('成功点击推荐按钮');
  if (isRecommended.value) {
    // 已推荐 → 显示提示

    showRecommendDialog.value = true
  } else {
    // 未推荐 → 弹出输入框
    showRecommendDialog.value = true

  }
}
//提交推荐内容
const submitRecommend = async () => {
  const currentNovelId = selectNovelState.novelId;
  const currentReaderId = ReaderState.readerId;
  const currentReason = recommendReason.value;
  try {
    // 调用推荐 API
    const response = await addRecommend(currentNovelId, currentReaderId, currentReason)
    console.log('添加推荐响应:', response)

    // 更新本地状态
    isRecommended.value = true
    showRecommendDialog.value = false

    //  更新推荐列表（安全方式）
    const newRecommendBooks = [
      ...ReaderState.recommendBooks, // 解构原有数组
      {
        novelId: selectNovelState.novelId,
        novel: selectNovelState, // 保存完整作品信息
        readerId: ReaderState.readerId,
        reason: recommendReason.value,
        recommendTime: new Date().toISOString()
      }
    ]

    // 通过 action 更新（触发同步）
    ReaderState.updateRecommendBooks(newRecommendBooks)

    // 提示成功
    toast("推荐成功！", {
      type: "success",
      dangerouslyHTMLString: true
    })
    recommendReason.value = '';

  } catch (error) {
    console.error('推荐失败:', error)
    toast("推荐失败，请重试", {
      type: "error",
      dangerouslyHTMLString: true
    })
  }
}
//取消推荐
const cancelRecommend = async () => {
  const currentNovelId = selectNovelState.novelId;
  const currentReaderId = ReaderState.readerId;
  try {
    // 取消推荐
    await deleteRecommend(currentNovelId, currentReaderId);
    const newRecommendBooks = ReaderState.recommendBooks.filter(item =>
      item.novel?.novelId !== currentNovelId &&
      item.novelId !== currentNovelId

    );
    ReaderState.updateRecommendBooks(newRecommendBooks)
    toast("取消推荐", {
      "type": "success",
      "dangerouslyHTMLString": true
    })
    showRecommendDialog.value = false;
  }
  catch (error) {
    console.error('推荐操作失败:', error);
    toast("推荐失败", {
      "type": "error",
      "dangerouslyHTMLString": true
    })
  }
}

const showBalanceInsufficientDialog = ref(false);
//选择打赏金额
const selectReward = (value) => { selectedReward.value = value; }
//确认打赏按钮
// 确认打赏按钮逻辑
const confirmReward = async () => {
  const currentNovelId = selectNovelState.novelId;
  const currentReaderId = ReaderState.readerId;
  const currentvalue = selectedReward.value;
  try {
    // 1. 检查余额是否充足
    if (accountBalance.value < selectedReward.value) {
      showBalanceInsufficientDialog.value = true; // 显示余额不足弹窗
      return;
    }

    // 2. 调用打赏API
    const response = await rewardNovel({
      readerId: currentReaderId,    // 当前用户ID
      novelId: currentNovelId, // 被打赏小说ID
      amount: currentvalue    // 打赏金额
    });
    readerState.balance -= currentvalue; // 更新余额
    // 3. 处理成功结果
    toast(`成功打赏 ${currentvalue} 币`, {
      type: "success", // 改为 success 类型
      dangerouslyHTMLString: true
    });
    console.log('打赏结果:', response.data);

    // 4. 刷新余额
    await fetchReaderBalance();
    if (typeof ReaderState.updateBalance === 'function') {
      ReaderState.updateBalance(accountBalance.value)
    } else {
      console.error('updateBalance is not a function!')
    }
    console.log("余额", ReaderState.balance)

    // 5. 关闭弹窗
    //showRewardDialog.value = false;


  } catch (error) {
    // 错误处理
    console.error('打赏失败:', error);
    toast(`打赏失败: ${error.message}`, {
      type: "error",
      dangerouslyHTMLString: true
    });
    // 失败时重新获取最新余额
    await fetchReaderBalance();
  }
};

const goToRecharge = () => {
  showBalanceInsufficientDialog.value = false;
  showRewardDialog.value = false;
  const route = router.resolve({ path: '/Novels/Novel_Recharge' });
  window.open(route.href, '_blank'); // 新窗口打开
};
const handleRecharge = () => {
  const route = router.resolve({ path: '/Novels/Novel_Recharge' });
  window.open(route.href, '_blank'); // 新窗口打开

}


// 处理评分按钮点击
const handleRatingClick = () => {
  showRatingDialog.value = true;
};

// 关闭弹窗
const closeDialog = () => {
  showRatingDialog.value = false;
  hoverRating.value = null;
};

// 选择评分
const selectRating = (rating) => {
  if (!hasRating.value) {
    currentRating.value = rating;
  }
};

// 获取星星颜色
const getStarColor = (i) => {
  if (hasRating.value) {
    return i <= currentRating.value ? '#FFD700' : '#DDD';
  }
  return (hoverRating.value && i <= hoverRating.value) ||
    (!hoverRating.value && currentRating.value && i <= currentRating.value)
    ? '#FFD700' : '#DDD';
};

// 提交评分
const submitRating = async () => {
  try {
    // 这里调用API提交评分
    await addRate(selectNovelState.novelId, ReaderState.readerId, currentRating.value);
    hasRating.value = true;
    closeDialog();
  } catch (error) {
    console.error('提交评分失败:', error);
  }
};




</script>



<style scoped>
.insufficient-dialog-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
  text-align: center;
}

.insufficient-dialog {
  background-color: white;
  border-radius: 8px;
  width: 90%;
  max-width: 400px;
  padding: 20px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.dialog-header {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 20px;
  padding-bottom: 10px;
  border-bottom: 1px solid #eee;

}

.dialog-header h3 {
  margin: 0;
  font-size: 18px;
  font-weight: bold;
  text-align: center;
}

.close-btn {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  color: #999;
}

.insufficient-content {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.insufficient-message {
  color: #f56c6c;
  font-size: 16px;
  text-align: center;
  margin: 10px 0;
}

.amount-info {
  display: flex;
  flex-direction: column;
  gap: 8px;
  font-size: 14px;
  color: #666;
  padding: 10px 0;
  border-bottom: 1px solid #eee;
}

.quick-payment {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 15px;
  padding-top: 10px;
}

.quick-payment p {
  margin: 0;
  font-size: 14px;
  color: #666;
}

.recharge-btn {
  width: 100%;
  padding: 12px;
  background-color: #f56c6c;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 16px;
  cursor: pointer;
  transition: background-color 0.2s;
}

.recharge-btn:hover {
  background-color: #e65c5c;
}

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

/* 新增的已推荐状态 */
.blue-border-btn.is-recommended {
  border-color: #d9d9d9;
  color: #8c8c8c;
  background: #f5f5f5;
}

.blue-border-btn.is-recommended:hover {
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
  cursor: pointer;
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

.author-avatar:hover {
  transform: scale(1.1);
  transition: transform 0.2s ease;
}

.author-card h2 {
  margin: 8px 0 0 0;
  font-size: 20px;
  font-weight: 600;
  color: #333;
  text-align: center;
}

.author-card h2:hover {
  color: #f0940a;
  transform: scale(1.10);
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



.recommend-dialog-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 999;
}

.recommend-dialog {
  background: #fff;
  padding: 32px 28px 26px 28px;
  border-radius: 16px;
  min-width: 320px;
  max-width: 90vw;
  box-shadow: 0 8px 32px rgba(0, 0, 0, .18);
  animation: popup-fade-in 0.22s cubic-bezier(.4, 0, .2, 1);
}

@keyframes popup-fade-in {
  from {
    opacity: 0;
    transform: scale(0.95);
  }

  to {
    opacity: 1;
    transform: scale(1);
  }
}

.dialog-content {
  display: flex;
  flex-direction: column;
  align-items: stretch;
}

h3 {
  margin: 0 0 18px 0;
  font-size: 1.3rem;
  font-weight: 600;
  text-align: center;
  color: #23292f;
}

.recommend-textarea {
  width: 100%;
  min-height: 70px;
  box-sizing: border-box;
  border-radius: 6px;
  border: 1px solid #e1e4e8;
  padding: 10px;
  font-size: 1rem;
  margin-bottom: 18px;
  resize: vertical;
  background: #f9fafb;
  transition: border-color 0.2s;
}

.recommend-textarea:focus {
  border-color: #409eff;
  outline: none;
  background: #fff;
}

.already-recommended {
  margin: 20px 0;
  color: #909399;
  text-align: center;
  font-size: 1.07rem;
}

.dialog-actions {
  display: flex;
  justify-content: center;
  /* 居中所有按钮 */
  gap: 24px;
  margin-top: 24px;
}

.cancel-btn,
.confirm-btn {
  padding: 7px 22px;
  font-size: 1rem;
  border-radius: 6px;
  border: none;
  cursor: pointer;
  min-width: 100px;
  transition: background 0.17s, color 0.17s;
}

.cancel-btn {
  background: #f5f6fa;
  color: #909399;
}

.cancel-btn:hover {
  background: #e5e8ef;
  color: #606266;
}

.confirm-btn {
  background: #409eff;
  color: #fff;
  font-weight: 500;
}

.confirm-btn:hover {
  background: #2979ff;
}

.cancel-recommend-btn {
  background: #ffeded;
  color: #e74c3c;
  border: none;
  border-radius: 6px;
  padding: 7px 22px;
  font-size: 1rem;
  min-width: 100px;
  cursor: pointer;
  margin-left: 0;
  transition: background 0.18s, color 0.18s;
}

.cancel-recommend-btn:hover {
  background: #ffcccc;
  color: #c0392b;
}

.reward-dialog-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.reward-dialog {
  background-color: white;
  border-radius: 8px;
  width: 90%;
  max-width: 400px;
  padding: 20px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.dialog-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.dialog-header h3 {
  margin: 0;
  font-size: 18px;
  font-weight: bold;
}

.close-btn {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  color: #999;
}

.reward-options {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 10px;
  margin-bottom: 20px;
}

.reward-option {
  position: relative;
  border: 1px solid #ddd;
  border-radius: 4px;
  padding: 10px;
  text-align: center;
  cursor: pointer;
  transition: all 0.2s;
  display: flex;
  flex-direction: column;
}

.reward-option:hover {
  border-color: #ff6b6b;
}

.reward-option.selected {
  background-color: #fff0f0;
  border-color: #ff6b6b;
  color: #ff6b6b;
}

.monthly-ticket {
  font-size: 12px;
  color: #666;
  margin-top: 4px;
}

.reward-option.selected .monthly-ticket {
  color: #ff6b6b;
}

.check-icon {
  position: absolute;
  top: 4px;
  right: 4px;
  width: 16px;
  height: 16px;
  color: #ff6b6b;
}

.balance-info {
  display: flex;
  justify-content: space-between;
  margin-bottom: 20px;
  font-size: 14px;
  color: #666;
}

.confirm-reward-btn {
  width: 100%;
  padding: 12px;
  background-color: #ff6b6b;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 16px;
  cursor: pointer;
  transition: background-color 0.2s, transform 0.1s;

  &:hover {
    background-color: #ff5252;
  }

  &:active {
    transform: scale(0.98);
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
  }
}



/* 新增的固定按钮样式 */
.fixed-buttons {
  position: fixed;
  right: 20px;
  bottom: 180px;
  display: flex;
  flex-direction: column;
  gap: 12px;
  z-index: 999;
}

.fixed-button {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 48px;
  height: 48px;
  background-color: #fff;
  border-radius: 50%;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  cursor: pointer;
  transition: all 0.3s ease;
}

.fixed-button:hover {
  background-color: #f5f5f5;
  transform: translateY(-2px);
}

.fixed-button svg {
  width: 24px;
  height: 24px;
}

/* 在较大屏幕上显示文字 */
@media (min-width: 768px) {
  .fixed-button {
    width: auto;
    padding: 0 16px;
    border-radius: 24px;
  }

  .fixed-button span {
    margin-left: 8px;
    font-size: 14px;
  }
}




/* 评分弹窗样式 */
.rating-dialog-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.rating-dialog {
  background-color: white;
  border-radius: 12px;
  padding: 24px;
  width: 320px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
}

.dialog-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.dialog-header h3 {
  margin: 0;
  font-size: 18px;
  color: #333;
}

.close-btn {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  color: #999;
}

.stars-container {
  display: flex;
  justify-content: center;
  gap: 8px;
  margin: 20px 0;
}

.star {
  cursor: pointer;
  transition: transform 0.2s;
}

.star:hover {
  transform: scale(1.2);
}

.rating-value {
  text-align: center;
  font-size: 18px;
  font-weight: bold;
  color: #FFD700;
  margin: 10px 0 20px;
}

.submit-btn {
  display: block;
  width: 100%;
  padding: 10px;
  background-color: #1890ff;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 16px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.submit-btn:hover {
  background-color: #40a9ff;
}

.submit-btn:disabled {
  background-color: #DDD;
  cursor: not-allowed;
}
</style>
