<template>
  <div class="introduction-section">
    <strong class="intro-label">简介：</strong>
    <p>{{ selectNovelState.introduction }}</p>
    
    <div v-if="firstChapter" class="chapter-content">
      <h3>第一章</h3>
      <div class="content">{{ firstChapter }}</div>
      
      <!-- 添加的继续阅读按钮 -->
      <div class="continue-reading-container">
        <button class="continue-reading-btn" @click="goToNextChapter">
          继续阅读 >
        </button>
      </div>
    </div>
    <div v-else>
      正在加载第一章内容...
    </div>
  </div>
</template>

<script setup>
import { SelectNovel_State,readerState  } from '@/stores/index';
import {getChapter} from '@/API/Chapter_API.js';
import { watch,ref } from 'vue';
import { useRouter} from 'vue-router';
import {addOrUpdateRecentReading } from '@/API/Reader_API'

const selectNovelState = SelectNovel_State();      //小说对象
const ReaderState=readerState();                   //当前读者对象
const firstChapter = ref('') ;                     // 第一章内容
const  router =useRouter()
// 继续阅读按钮点击事件
const goToNextChapter = async () => {
  if (firstChapter.value) {
    try {
      // 添加或更新阅读记录
      await addOrUpdateRecentReading(
        ReaderState.readerId,  // 读者ID
        selectNovelState.novelId    // 小说ID
      );
    } catch (historyError) {
      console.error("记录阅读历史失败:", historyError);
      // 这里可以选择不提示用户，因为阅读历史记录失败不影响主要功能
    }
    
    // 跳转到阅读页面
    router.push('/Novels/reader');
  }
};
//获取第一章的函数
const fetchChapter=async(chapterId)=>{
  try {
    const response = await getChapter(selectNovelState.novelId,chapterId)
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
    firstChapter.value=response.content
  } catch (error) {
    console.error('获取章节失败', error)
    
  }
}

// 监听 novelId 变化,变化时加载第一章数据
watch(
  () => selectNovelState.novelId,
  async (newNovelId) => {
    if (newNovelId) {
      try {
        await fetchChapter(1)
        console.log('小说章节数据更新完成！')
      } catch (error) {
        console.error('数据加载失败:', error)
      }
    }
  },
  { immediate: true } // 替代 onMounted，首次加载时自动执行
)
</script>

<style scoped>
.introduction-section {
  margin: 40px 0;
  padding: 15px;
  background-color:  #edf1f1ff;
  border-radius: 5px;
}

.intro-label {
  font-size: 18px;
  color: #333;
}

.chapter-content {
  margin-top: 20px;
  padding: 15px;
  background-color: #eceae3ff;
  border: 1px solid #eee;
  border-radius: 5px;
}

.chapter-content h3 {
  color: #444;
  margin-bottom: 10px;
}

.content {
  white-space: pre-line; /* 保留换行符 */
  line-height: 1.6;
}

/* 继续阅读按钮样式 */
.continue-reading-container {
  text-align: center;
  margin-top: 20px;
}

.continue-reading-btn {
  background-color: white;
  color: #333;
  border: 1px solid #ddd;
  border-radius: 20px; /* 圆角胶囊形状 */
  padding: 10px 25px;
  font-size: 16px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.continue-reading-btn:hover {
  background-color: #f5f5f5;
  border-color: #ccc;
}

@media (max-width: 768px) {
  .introduction-section {
    width: 95%;
    max-width: none;
  }
}
</style>