<template>
  <div v-if="loading" class="loading-indicator">
      <div class="spinner"></div>
      <span>加载中...</span>
  </div>
  <div v-else>
     
    <div class="action-buttons">
      <!-- 返回按钮 -->
      <button @click="goBack" class="back-link">
         返回列表
      </button>
      <!-- 删除小说按钮 -->
    </div>
    

  <div class="novel-detail-container">
    <div class="novel-info">
      <!-- 封面在左侧 -->
      <img :src="novel.coverUrl" class="novel-cover" alt="小说封面" />
      
      <!-- 右侧信息区域 -->
      <div class="novel-meta">
        <h2>{{ novel.novelName }}</h2>
        <p class="novel-author">作者：{{ novel.authorName }}</p>
        <div class="novel-status-categories">
          <span class="novel-status-chip">{{ novel.status }}</span>
          <span v-for="cat in categories" :key="cat" class="novel-category-chip">{{ cat }}</span>
        </div>
        <p>创建于:{{ novel.createTime }}</p>
        <p>总字数:{{ novel.totalWordCount }}</p>
        <!-- 选择章节按钮 -->
        
      </div>
    </div>

    <!-- 小说描述 -->
    <div class="novel-desc">
      <h3>小说简介</h3>
      <p class="novel-desc-detail">{{ novel.introduction }}</p>
    </div>   

    <!-- 章节列表 (点击按钮后显示) -->
     <button 
        @click="goToChapterSelection" 
        class="chapter-select-btn"
      >
        选择章节 ▼
      </button>
    <div v-if="showChapters" class="chapter-list">
      <h3>章节列表</h3>
      <ul class="chapter-list-detail">
        <li 
          v-for="chapter in pagedChapters" 
          :key="chapter.chapterId"
          @click="selectChapter(chapter)"
        >
          <div class="chapter-content" @click="selectChapter(chapter)">
            <span class="chapter-id-title">
              <strong>{{ chapter.chapterId }}. {{ chapter.title }}</strong>
            </span>
            <span class="chapter-right">
              <span class="chapter-publish">{{ chapter.publishTime }}</span>
              <span :class="['chapter-status', chapter.status === '已发布' ? 'status-published' : 'status-other']">
                {{ chapter.status }}
              </span>
            </span>
          </div>
        
          <!-- 三点按钮 -->
          <div class="chapter-actions">
            <button class="action-btn" @click.stop="(toggleActionMenu(chapter.chapterId),chapterIndex=chapter.chapterId)">
              <span>⋮</span>
            </button>
            
            <!-- 操作菜单 -->
            <div 
              v-if="activeMenu === chapter.chapterId"
              class="action-menu"
              v-click-outside="closeAllMenus"
            >
              <button
                @click.stop="
                  chapter.status === '首次审核'
                    ? (index=0,showModal=true)
                    : chapter.status === '审核中'
                    ? (index=0,showModal=true)
                    : chapter.status === '已发布'
                    ? (index=1,showModal=true)
                    : chapter.status === '封禁'
                    ? (index=0,showModal=true)
                    : null
                "
              >
                {{
                  chapter.status === '首次审核' ? chapterOp[0].firstReview :
                  chapter.status === '审核中' ? chapterOp[0].review :
                  chapter.status === '已发布' ? chapterOp[0].published :
                  chapter.status === '封禁' ? chapterOp[0].ban :
                  null
                }}
              </button>
              <button
                @click.stop="
                  chapter.status === '首次审核'
                    ?(index=1,showModal=true)
                    : chapter.status === '审核中'
                    ? (index=1,showModal=true)
                    : chapter.status === '已发布'
                    ? confirmDelete()
                    : chapter.status === '封禁'
                    ? confirmDelete()
                    :null
                " 
              >
                {{
                  chapter.status === '首次审核' ? chapterOp[1].firstReview :
                  chapter.status === '审核中' ? chapterOp[1].review :
                  chapter.status === '已发布' ? chapterOp[1].published :
                  chapter.status === '封禁' ? chapterOp[1].ban :
                  null
                }}
              </button>
            </div>
          </div>
        </li>
      </ul>
        <!-- 分页控件 -->
      <div class="pagination">
        <button @click="prevPage" :disabled="currentPage === 1">上一页</button>
        <span v-for="page in totalPages" :key="page">
          <button 
            @click="goToPage(page)" 
            :class="{active: currentPage === page}"
          >{{ page }}</button>
        </span>
        <button @click="nextPage" :disabled="currentPage === totalPages">下一页</button>
      </div>
    </div>
    <!-- result的输入框 -->
    <div v-if="showModal" class="modal-overlay">
      <div class="modal-content">
        <h3>请输入审核备注</h3>
        <input 
          v-model="inputValue" 
          @keyup.enter="confirmInput"
          placeholder="请输入内容"
          class="modal-input"
        >
        <div class="modal-buttons">
          <button @click="confirmInput" class="confirm-btn">确定</button>
          <button @click="cancelInput" class="cancel-btn">取消</button>
        </div>
      </div>
    </div>
  </div>
  </div>
  
</template>

<script setup>
import { ref,onMounted,computed } from 'vue'
import { useRoute,useRouter } from 'vue-router'

// 引入路由和idStore
const route = useRoute()


//----------------------------------------------------------------------------------------------------------------
//通过API由novelId获取单个小说的具体数据
import { getNovel } from '@/API/Novel_API'
import { getChaptersByNovel,deleteChapter } from '@/API/Chapter_API'
import{getCategoriesByNovel} from '@/API/NovelCategory_API'
import { managerState } from '@/stores/index'
import { storeToRefs } from 'pinia'
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

const currentState = managerState()
const { id: managerID } = storeToRefs(currentState)

const novel = ref({
  novelName: '',       // 字符串类型默认值
  authorName: '',
  coverUrl: '',
  introduction: '',
  createTime: ''
})
const chapters = ref([])//获取该小说的章节数据
const categories=ref([])//获取该小说的分类数据
const getNovelData=async () => {
  loading.value = true // 开始加载
  try {
    const response = await getNovel(route.query.id)
    if (response) {
      novel.value = {
        novelName: response.novelName,
        authorName:route.query.text || '未知',
        coverUrl:getFullCoverUrl(response.coverUrl),
        introduction: response.introduction || '暂无简介',
        createTime: response.createTime || '未知',
        totalWordCount: response.totalWordCount ,
        status: response.status,
      }
    }
    const chapterResponse = await getChaptersByNovel(route.query.id)
    if (chapterResponse) {
      chapters.value = chapterResponse.filter(chapter => chapter.status != '草稿').map(chapter => ({
        chapterId: chapter.chapterId,
        title: chapter.title,
        publishTime: chapter.publishTime,
        status: chapter.status,
        content: chapter.content
      }))
    }
    const categoryResponse=await getCategoriesByNovel(route.query.id)
    if(categoryResponse){
      categories.value=categoryResponse.map(cat=>cat.categoryName)
    }
  } catch (error) {
    console.error('获取小说数据失败:', error)
    toast.error('加载小说数据失败，请检查网络或权限。')
  } finally {
    loading.value = false
  }
}
//----------------------------------------------------------------------------------------------------------------
import {reviewChapter} from '@/API/Chapter_API'
//章节操作
const chapterOp=[
  {
    firstReview:'审核通过',
    review:'审核通过',
    published:'封禁',
    ban:'解封',
  },
  {
    firstReview:'审核不通过',
    review:'审核不通过',
    published:'删除',
    ban:'删除',
  }
]
const statuses= [
  '已发布' , '封禁' 
]
const chapterIndex=ref(0)
const index=ref(0)//状态
const showModal = ref(false)//result的输入框
const inputValue = ref('');
const result = ref('');

const confirmInput = () => {
  if (inputValue.value.trim()) {
    result.value = inputValue.value;
    showModal.value = false;
    inputValue.value = '';
    executeReview(); // 传入当前章节ID
  }
  else {
    toast.info('请输入有效的内容');
  }
};

const cancelInput = () => {
  showModal.value = false;
  inputValue.value = '';
};

//审核
function executeReview() {
  loading.value = true // 开始加载
  reviewChapter(route.query.id, chapterIndex.value, statuses[index.value], managerID.value, result.value)
    .then(() => {
      console.log('章节审核成功')
      closeAllMenus()
      chapters.value = chapters.value.map(chapter => {
        if (chapter.chapterId === chapterIndex.value) {
          return { ...chapter, status: statuses[index.value] }
        }
        return chapter
      })
    })
    .catch(err => {
      console.error('章节审核失败:', err)
    })
    .finally(() => {
      loading.value = false
    })
}




//----------------------------------------------------------------------------------------------------------------
//加载中动画
const loading = ref(false)  // 加载状态
//----------------------------------------------------------------------------------------------------------------
//加载数据
onMounted(() => {
  getNovelData() 
})
//----------------------------------------------------------------------------------------------------------------
//返回上一页
const router = useRouter();
const goBack = () => {
  router.go(-1); // 返回上一页
  // 或者指定路由：router.push('/reported-comments-list')
};
//----------------------------------------------------------------------------------------------------------------
 // 统一的封面URL处理方法
const getFullCoverUrl=(partialPath)=> {
    if (!partialPath) {
      // 随机默认封面
      return 'https://picsum.photos/300/400?random=' + Math.floor(Math.random() * 1000)
    }
    if (partialPath.startsWith('http')) return partialPath
    const ossBase = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/'
    return `${ossBase}${partialPath.replace(/^\//, '')}`
  }
//----------------------------------------------------------------------------------------------------------------
//分页显示
const currentPage = ref(1)
const pageSize = 5
const totalPages = computed(() => Math.ceil(chapters.value.length / pageSize))
const pagedChapters = computed(() => {//本页显示的章节范围
  const start = (currentPage.value - 1) * pageSize
  return chapters.value.slice(start, start + pageSize)
})
function goToPage(page) {//进入某页
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page
  }
}
function prevPage() {//下一页
  if (currentPage.value > 1) currentPage.value--
}
function nextPage() {//上一页
  if (currentPage.value < totalPages.value) currentPage.value++
}
//----------------------------------------------------------------------------------------------------------------
const confirmDelete = () => {
  if(confirm('确定要删除该章节吗？此操作不可恢复！')) {
    // 执行删除逻辑
    console.log('删除小说操作');
    deleteChapter(route.query.id, chapterIndex.value) .then(() => {
      console.log('章节删除成功')
      closeAllMenus()
      chapters.value = chapters.value.filter(chapter => chapter.chapterId !== chapterIndex.value)
    })
    .catch(err => {
      console.error('章节删除失败:', err)
    })
    .finally(() => {
      loading.value = false
    })

  }
};

// 点击外部关闭菜单的指令
const vClickOutside = {
  beforeMount(el, binding) {
    el.clickOutsideEvent = function(event) {
      if (!(el === event.target || el.contains(event.target))) {
        binding.value();
      }
    };
    document.addEventListener('click', el.clickOutsideEvent);
  },
  unmounted(el) {
    document.removeEventListener('click', el.clickOutsideEvent);
  }
};

const activeMenu = ref(0);

const toggleActionMenu = (chapterId) => {
  activeMenu.value = activeMenu.value===chapterId?null:chapterId;
};

const closeAllMenus = () => {
  activeMenu.value = null;
};


const showChapters = ref(false);

// 跳转章节选择
const goToChapterSelection = () => {
  showChapters.value = !showChapters.value;
};

// 选择章节
const selectChapter = (chapter) => {
  console.log('选择章节:', chapter);
  // 实际应该跳转到阅读页面
  // router.push({ name: 'ChapterRead', params: { chapterId: chapter.id } });
    router.push({
    name: 'ChapterContent',
    params: {
      novel_id: route.query.id,
      chapter_id: chapter.chapterId
    },
    query: {
      title: chapter.title,
      content: chapter.content
    }
  })
};
</script>

<style scoped>
/* 加载动画样式 */
.loading-indicator {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  margin: 20px 0;
  color: #666;
  font-size: 16px;
}

.spinner {
  width: 24px;
  height: 24px;
  border: 3px solid #ccc;
  border-top-color: #486482;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}
.novel-detail-container {
  max-width: 900px;
  margin: 0 auto;
  padding: 20px;
}

.novel-info {
  display: flex;
  gap: 30px;
  margin-bottom: 20px;
}

.novel-cover {
  width: 180px;
  height: 240px;
  object-fit: cover;
  border-radius: 4px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.novel-meta {
  flex: 1;
}

.novel-meta h2 {
  margin: 0 0 10px 0;
  color: #333;
}

.novel-author {
  margin: 0 0 15px 0;
  color: #666;
}

.chapter-select-btn {
  padding: 8px 16px;
  background-color: #42b983;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.chapter-select-btn:hover {
  background-color: #3aa876;
}

.novel-desc {
  line-height: 1.6;
  margin-top:10px;
  margin-left:0px;
  padding: 10px;
  background: #f8f8f8;
  border-radius: 15px;
}

.novel-desc h3 {
  margin-left: 20px;
  margin-bottom:5px;
  color: #333;
  font-size: 21px;
}
.novel-desc-detail {
  color: #444;
  margin-top:0px;
  margin-left: 20px;
  margin-right:20px;
  margin-bottom: 20px;
}

.chapter-list {
  margin: 30px 0;
  border-top: 1px solid #eee;
  padding-top: 20px;
}

.chapter-list h3 {
  margin-bottom: 15px;
}

.chapter-list-detail {
  list-style: none;
  padding: 0;
  display: block;
}

.chapter-list-detail li {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 8px;
  position: relative;
}

.chapter-content {
  flex: 1;
  display: flex;
  justify-content: space-between;
  align-items: center;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  padding: 12px 16px;
  border: 1px solid #eee;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.2s;
}
.chapter-content:hover {
  background-color: #f5f5f5;
  border: 1px solid #42b983;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
}

.chapter-id-title {
  color: #2c3e50;
  font-weight: 500;
  font-size: 16px;
}

.chapter-right {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-start;
  gap: 12px;
}

.chapter-publish {
  color: #888;
  font-size: 14px;
}

.chapter-status {
  padding: 2px 10px;
  border-radius: 12px;
  font-size: 14px;
  font-weight: bold;
  margin-left: 8px;
}

.status-published {
  background: #e6f9f0;
  color: #42b983;
  border: 1px solid #42b983;
}

.status-other {
  background: #ffeaea;
  color: #e74c3c;
  border: 1px solid #e74c3c;
}

.chapter-actions {
  position: relative;
}

.action-btn {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 20px;
  color: #666;
  padding: 0 8px;
  display: flex;
  align-items: center;
  height: 100%;
}

.action-btn:hover {
  color: #333;
}

.action-menu {
  position: absolute;
  right: 0;
  top: 100%;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
  z-index: 10;
  min-width: 100px;
}

.action-menu button {
  display: block;
  width: 100%;
  padding: 8px 12px;
  text-align: left;
  background: none;
  border: none;
  cursor: pointer;
  color: #333;
}

.action-menu button:hover {
  background-color: #78deb0;
}



.action-buttons {
  margin-left:150px;
  margin-bottom: 20px;
  display: flex;/*元素水平 */
  align-items: center;/*垂直居中 */
  gap: 780px; /* 控制两个按钮之间的间距 */
  margin-top: 30px;
}

.delete-btn {
  padding: 8px 16px;/*内边距 */
  background-color: #ef5454; /* 浅红色背景 */
  color: #fff; /* 黑色文字 */
  border: none;
  border-radius: 4px;
  cursor: pointer;/* 鼠标悬停时变为手型指针 */
  font-size: 14px;
  font-weight: 600;
  transition: background-color 0.3s;/* 背景色变化0.3秒过渡 */
}

.delete-btn:hover {
  background-color: #d42626; /* 深红色悬停状态 */
}

/* 调整原有返回链接样式使其更像按钮 */
.back-link {
  display: inline-block;/* 设为块级元素，允许设置padding */
  padding: 8px 16px;
  background-color: #fff;
  color: #000;
  text-decoration: none;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  font-weight: 600;
  transition: background-color 0.3s;
}

.back-link:hover {
  background-color: #42b983;
  text-decoration: none;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  padding: 20px;
  border-radius: 8px;
  width: 300px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.modal-input {
  width: 100%;
  padding: 8px;
  margin: 15px 0;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.modal-buttons {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

.confirm-btn {
  background-color: #42b983;
  color: white;
}

.cancel-btn {
  background-color: #f0f0f0;
}

.novel-status-categories {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 8px;
  margin-top: 2px;
  flex-wrap: wrap;
}
.novel-status-chip {
  background: #e6f9f0;
  color: #42b983;
  border: 1px solid #42b983;
  border-radius: 12px;
  padding: 2px 14px;
  font-size: 15px;
  font-weight: bold;
  margin-right: 12px;
}
.novel-category-chip {
  background: #dde3ee;
  color: #486482;
  border: 1px solid #bfcbe3;
  border-radius: 12px;
  padding: 2px 12px;
  font-size: 15px;
  font-weight: 500;
  margin-right: 6px;
}
.novel-category-chip:last-child {
  margin-right: 0;
}

.pagination {
  display: flex;
  gap: 8px;
  margin-top: 18px;
  justify-content: center;
  align-items: center;
}
.pagination button {
  padding: 4px 10px;
  border: 1px solid #42b983;
  background: #fff;
  color: #42b983;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}
.pagination button.active {
  background: #42b983;
  color: #fff;
  font-weight: bold;
}
.pagination button:disabled {
  background: #eee;
  color: #aaa;
  cursor: not-allowed;
  border: 1px solid #ccc;
}
</style>