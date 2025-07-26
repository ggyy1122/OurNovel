<template>
  <div>
    <!-- 主内容块：章节列表 + 整本购买按钮 -->
    <div class="chapter-list">
     <!-- 标题和按钮并排对齐 -->
<div class="chapter-header-bar">
  <h2 class="chapter-title">章节目录</h2>
  <div v-if="selectNovelState.status === '完结'">
    <button 
      :disabled="hasPurchased"
      class="whole-puy-btn"
      @click="showPurchaseModal = true"
    >
      {{ hasPurchased ? '已买断' : '整本购买' }}
    </button>
  </div>
</div>




      <!-- 章节列表 -->
      <ul v-if="displayedChapters.length > 0">
        <li
          v-for="chapter in displayedChapters"
          :key="chapter.chapterId"

          @click="!isDisabled(chapter) && selectChapter(chapter)"
          :class="['chapter-item', { banned: isDisabled(chapter) }]">
          <div class="chapter-info">
            <span class="chapter-number">第{{ chapter.chapterId }}章</span>
            <span class="chapter-title">
              {{ chapter.title }}
              <span v-if="chapter.status === '封禁'" class="banned-tag">【封禁中】</span>
              <span v-else-if="chapter.status === '审核中'" class="banned-tag">【审核中】</span>
            </span>
            <span v-if="chapter.isCharged === '是'" class="charged">（收费）</span>
            <span v-else class="free">（免费）</span>
          </div>
        </li>

      </ul>

      <!-- 如果章节为空 -->
      <p v-else>作者还在努力敲字中，感谢您的关注~</p>

      <!-- 分页组件 -->
      <div v-if="totalPages > 1" class="pagination-container">
        <button 
          class="prev"
          @click="changePage(currentPage - 1)"
          :disabled="currentPage === 1"
        >
          🡄
        </button>
        <span class="page-info">当前：{{ currentPage }}页 / 共{{ totalPages }}页</span>
        <button 
          class="next"
          @click="changePage(currentPage + 1)"
          :disabled="currentPage === totalPages"
        >
          🡆
        </button>
      </div>

    </div>

    <!-- ✅ Teleport 到 body，必须在根元素外层并写在 template 内 -->
    <teleport to="body">
      <div v-if="showPurchaseModal" class="modal-overlay">
        <div class="modal">
          <div class="modal-header">
            <span>限时优惠！</span>
            <button class="close-btn" @click="showPurchaseModal = false">×</button>
          </div>
          <div class="modal-body">
            <p>本书整本价格为 <strong>￥{{ selectNovelState.totalPrice }}</strong></p>
          </div>
          <div class="modal-footer">
            <button class="confirm-btn" @click="confirmPurchase">确认购买</button>
          </div>
        </div>
      </div>
    </teleport>
  </div>
</template>

<script setup>
import { ref, onMounted, computed, nextTick } from 'vue'
import { getChaptersByNovel } from '@/API/Chapter_API'
import { getWholePurchaseStatus, purchaseWholeNovel } from '@/API/Transaction_API'
import { readerState, SelectNovel_State } from '@/stores/index'
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css' 
const readerStore = readerState()
const selectNovelState = SelectNovel_State()

const readerId = readerStore.readerId
const novelId = selectNovelState.novelId

// 章节相关
const chapterList = ref([])
const displayedChapters = ref([])
const currentPage = ref(1)
const itemsPerPage = 5


// 整本买断状态
const showPurchaseModal = ref(false)
const hasPurchased = ref(false)
//const purchaseMessage = ref('')
//const totalPrice = computed(() => selectNovelState.totalPrice ?? 0)

// 过滤掉草稿章节
const visibleChapters = computed(() =>
  chapterList.value.filter(ch => ch.status !== '草稿')
);

// 总页数
const totalPages = computed(() =>
  Math.ceil(visibleChapters.value.length / itemsPerPage) || 1
);


// 初始化加载章节
onMounted(async () => {
  try {
    const novelId = selectNovelState.novelId;
    const response = await getChaptersByNovel(novelId);
    chapterList.value = response || [];

    await updateDisplayedChapters(); // 初始化第一页
  } catch (err) {
    console.error('获取章节失败:', err);
    chapterList.value = [];
  }
});

onMounted(async () => {
  try {
    const status = await getWholePurchaseStatus(readerId, novelId)
    hasPurchased.value = status?.hasPurchased || false
  } catch (err) {
    console.error('查询买断状态失败', err)
  }
})
// 分页切换
async function changePage(page) {
  if (page < 1 || page > totalPages.value) return;
  currentPage.value = page;
  await updateDisplayedChapters();
}

// 更新显示的章节（先清空再加载）
async function updateDisplayedChapters() {
  displayedChapters.value = []; // 清空章节列表
  await nextTick(); // 等 DOM 更新后重新赋值
  const start = (currentPage.value - 1) * itemsPerPage;
  const end = currentPage.value * itemsPerPage;
  displayedChapters.value = visibleChapters.value.slice(start, end);
}

async function confirmPurchase() {
  try {
    const res = await purchaseWholeNovel({ readerId, novelId })

    if (res.success === 1) {
      hasPurchased.value = true
      showPurchaseModal.value = false
      toast.success('购买成功！', { autoClose: 2000 })
    } else {
      handleBalanceCheckAndError(res.message)
    }

  } catch (err) {
    console.error('购买失败:', err)

    // 优先检查后端返回的错误信息
    const backendMsg = err?.response?.data?.message || ''
    if (backendMsg.includes('余额不足')) {
      const balance = readerStore.balance ?? 0
     // const price = selectNovelState.totalPrice ?? 0
      toast.warning(`余额不足，当前余额 ¥${balance.toFixed(2)}`, {
        autoClose: 3000
      })
    } else {
      toast.error('❌ 发生错误，请稍后再试', { autoClose: 2000 })
    }
  }
}


function handleBalanceCheckAndError(msg) {
  const balance = readerStore.balance ?? 0
  const price = selectNovelState.totalPrice ?? 0

  if (balance < price || msg.includes('余额不足')) {
    toast.warning(`❗ 余额不足，当前余额为 ¥${balance.toFixed(2)}，需要 ¥${price.toFixed(2)}`, {
      autoClose: 3000
    })
  } else {
    toast.warning(msg || '购买失败，请重试', { autoClose: 2000 })
  }
}


// 禁止点击的章节
function isDisabled(chapter) {
  return chapter.status === '封禁' || chapter.status === '审核中';
}

// 选中章节后存入全局状态
function selectChapter(chapter) {
  selectNovelState.resetChapter(
    chapter.chapterId,
    chapter.title,
    chapter.content,
    chapter.wordCount,
    chapter.pricePerKilo,
    chapter.calculatedPrice,
    chapter.isCharged,
    chapter.publishTime,
    chapter.status
  );
}
</script>

<style scoped>
.chapter-list {
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 8px;
}

.chapter-item {
  cursor: pointer;
  padding: 15px;
  border-bottom: 1px solid #e0e0e0;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #fff;
  transition: background-color 0.3s ease;
}

.chapter-item:hover {
  background-color: #f0f0f0;
}

.chapter-item.banned {
  background-color: #e0e0e0;
  cursor: not-allowed;
}

.chapter-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
}

.chapter-number {
  font-size: 14px;
  color: #555;
}
.chapter-title {
  font-size: 16px;
  font-weight: bold;
  color: #333;
  margin-left: 10px;
  display: inline; 
  white-space: nowrap; /* 防止断行 */
}

.charged {
  color: red;
  font-size: 14px;
}

.free {
  color: green;
  font-size: 14px;
}

.banned-tag {
  color: #ff6600;
  margin-left: 10px;
  font-weight: bold;
}

p {
  font-size: 16px;
  color: #888;
  text-align: center;
  margin-top: 30px;
}


.page-info {
  font-size: 14px;
  color: #555;
  margin: 0 5px;
  text-align: center;
}


.pagination-container {
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
  margin-top: 20px;
  gap: 10px; /* 保持间距 */
}

.pagination-container button {
  background-color: #4CAF50;
  color: white;
  border: none;
  padding: 5px 10px;
  margin: 0 10px;
  cursor: pointer;
  border-radius: 4px;
  transition: background-color 0.3s;
  font-size: 18px;
}



.pagination-container button:hover {
  background-color: #45a049;
}

.pagination-container button:disabled {
  background-color: #dcdcdc;
  cursor: not-allowed;
}

.pagination-container span {
  font-size: 16px;
  color: #555;
}



.chapter-header-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 15px;
  padding: 0 10px;
  flex-wrap: wrap; /* 响应式换行 */
}




.whole-puy-btn {
  background: linear-gradient(135deg, #4a90e2, #357ABD); /* 渐变蓝色 */
  color: white;
  padding: 12px 24px;
  font-size: 16px;
  font-weight: bold;
  border: none;
  border-radius: 30px;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 4px 8px rgba(0,0,0,0.1);
}

.whole-puy-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 12px rgba(0,0,0,0.15);
}

.whole-puy-btn:disabled {
  background: #cccccc;
  color: #666;
  cursor: not-allowed;
  box-shadow: none;
}


/* 弹窗样式 */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0,0,0,0.5); /* 半透明遮罩 */
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}
.modal {
  background-color: #e7dbcb;
  border-radius: 10px;
  padding: 20px;
  width: 25vw;
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.2);
}


.modal-header {
  display: flex;
  justify-content: space-between;
  font-size: 18px;
  font-weight: bold;
}

.close-btn {
  background: transparent;
  border: none;
  font-size: 24px;
  font-weight: bold;
  color: #666;
  cursor: pointer;
  transition: color 0.2s ease;
}

.close-btn:hover {
  color: #d0021b; /* 红色高亮 */
  transform: scale(1.2); /* 微微放大 */
}


.modal-body {
  margin: 15px 0;
  font-size: 16px;
}

.confirm-btn {
  width: 100%;
  padding: 10px;
  background-color: #3f83f8;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}

</style>