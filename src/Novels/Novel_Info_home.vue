<template>
  <div class="introduction-section">
    <strong class="intro-label">简介：</strong>
    <p>{{ selectNovelState.introduction }}</p>

    <div v-if="firstChapter" class="chapter-content">
      <h3>第一章</h3>
      <div class="content">{{ firstChapter }}</div>

      <!-- 添加的继续阅读按钮 -->
      <div class="continue-reading-container">
        <button class="continue-reading-btn" @click="handleContinueReading">
          继续阅读 >
        </button>
      </div>
    </div>
    <div v-else-if="loadingChapter">
      正在加载第一章内容...
    </div>
    <div v-else>
      无法加载第一章内容
    </div>
  </div>
  <!-- 购买弹窗 -->
  <teleport to="body">
    <!-- 单章购买弹窗 -->
    <div v-if="showChapterPurchaseDialog" class="purchase-dialog-overlay">
      <div class="purchase-dialog">
        <div class="dialog-header"
          style="display: flex; justify-content: center; align-items: center; position: relative;">
          <h3 style="margin: 0;">购买第{{ firstChapterData.chapterId }}章</h3>
          <button class="da_close-btn" @click="showChapterPurchaseDialog = false"
            style="position: absolute; right: 20px;">&times;</button>
        </div>
        <div class="purchase-content">
          <p>本章节价格为 {{ firstChapterData.calculatedPrice }}币</p>
        </div>
        <button class="confirm-reward-btn" @click="purchaseChapterHandler">
          确认购买
        </button>
      </div>
    </div>

    <!-- 单章购买余额不足弹窗 -->
    <div v-if="showChapterInsufficientDialog" class="purchase-insufficient-overlay">
      <div class="purchase-insufficient-dialog">
        <div class="purchase-dialog-header">
          <h3>章节购买</h3>
          <button class="purchase-close-btn" @click="showChapterInsufficientDialog = false">&times;</button>
        </div>
        <div class="purchase-insufficient-content">
          <p class="purchase-insufficient-message">账户余额不足，无法购买本章节</p>
          <div class="purchase-amount-info">
            <span>章节价格：{{ firstChapterData.calculatedPrice }}币</span>
            <span>当前余额：{{ readerStore.balance }}币</span>
          </div>
          <div class="purchase-action-buttons">
            <button class="purchase-recharge-btn" @click="goToRecharge">立即充值</button>
            <button class="purchase-cancel-btn" @click="showChapterInsufficientDialog = false">取消</button>
          </div>
        </div>
      </div>
    </div>
  </teleport>
</template>

<script setup>
import { SelectNovel_State, readerState } from '@/stores/index';
import { getChapter } from '@/API/Chapter_API.js';
import { watch, ref } from 'vue';
import { useRouter } from 'vue-router';
import { addOrUpdateRecentReading } from '@/API/Reader_API'
import { checkPurchase, purchaseChapter } from '@/API/Purchase_API';
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

const selectNovelState = SelectNovel_State();      //小说对象
const readerStore = readerState();                 //当前读者对象
const firstChapter = ref('');                     // 第一章内容（用于显示）
const firstChapterData = ref(null);               // 第一章完整数据
const loadingChapter = ref(false);                // 加载状态
const router = useRouter()

// 购买相关状态
const showChapterPurchaseDialog = ref(false)
const showChapterInsufficientDialog = ref(false)

// 处理继续阅读按钮点击
const handleContinueReading = async () => {
  if (!firstChapterData.value) return;
  // 如果是收费章节且未购买，显示购买弹窗
  if (firstChapterData.value.status === '已发布' && firstChapterData.value.isCharged === '是' && !firstChapterData.value.hasPurchased) {
    showChapterPurchaseDialog.value = true;
    return;
  }
  // 正常情况直接跳转阅读
  await goToReading();
};

// 跳转到阅读页面
const goToReading = async () => {
  try {
    // 将第一章数据设置到store中
    selectNovelState.resetChapter(
      firstChapterData.value.chapterId,
      firstChapterData.value.title,
      firstChapterData.value.content,
      firstChapterData.value.wordCount,
      firstChapterData.value.pricePerKilo,
      firstChapterData.value.calculatedPrice,
      firstChapterData.value.isCharged,
      firstChapterData.value.publishTime,
      firstChapterData.value.status
    );
    // 添加或更新阅读记录
    try {
      await addOrUpdateRecentReading(
        readerStore.readerId,  // 读者ID
        selectNovelState.novelId,    // 小说ID
        1
      );
    } catch (historyError) {
      console.error("记录阅读历史失败:", historyError);
    }
    // 跳转到阅读页面
    router.push('/Novels/reader');
  } catch (error) {
    console.error("跳转阅读页面失败:", error);
    toast.error("跳转失败，请稍后再试", { autoClose: 2000 });
  }
};

// 购买章节
const purchaseChapterHandler = async () => {
  try {
    if (readerStore.balance < firstChapterData.value.calculatedPrice) {
      showChapterPurchaseDialog.value = false;
      showChapterInsufficientDialog.value = true;
      return;
    }
    const response = await purchaseChapter({
      readerId: readerStore.readerId,
      novelId: firstChapterData.value.novelId,
      chapterId: firstChapterData.value.chapterId
    });
    if (response.success) {
      // 更新购买状态
      firstChapterData.value.hasPurchased = true;
      readerStore.balance -= firstChapterData.value.calculatedPrice;
      showChapterPurchaseDialog.value = false;
      toast.success("购买成功！", { autoClose: 1500 });
      await new Promise(resolve => setTimeout(resolve, 1500));
      // 购买成功后跳转到阅读
      await goToReading();
    } else {
      toast.error("购买失败: " + (response.message || "未知错误"), { autoClose: 2000 });
    }
  } catch (error) {
    console.error('购买章节失败:', error);
    if (error.response && error.response.status === 400) {
      showChapterPurchaseDialog.value = false;
      showChapterInsufficientDialog.value = true;
    } else {
      toast.error("购买失败: " + (error.message || "未知错误"), { autoClose: 2000 });
    }
  }
};

// 去充值
const goToRecharge = () => {
  showChapterPurchaseDialog.value = false;
  showChapterInsufficientDialog.value = false;
  router.push('/Novels/Novel_Recharge');
};

//获取第一章的函数
const fetchChapter = async (chapterId) => {
  loadingChapter.value = true;
  try {
    const response = await getChapter(selectNovelState.novelId, chapterId)
    firstChapterData.value = response; // 存储完整的响应数据
    if (response.status === '首次审核' || response.status === '草稿') {
      firstChapter.value = '';
      return;
    } else if (response.status === '审核中') {
      firstChapter.value = '第1章正在审核中，请稍后再试'
      return;
    } else if (response.status === '封禁') {
      firstChapter.value = '第1章已封禁，无法查看内容'
      return;
    }
    // 检查购买状态
    if (response.isCharged === '是') {
      try {
        const purchaseStatus = await checkPurchase(
          readerStore.readerId,
          selectNovelState.novelId,
          chapterId
        );
        firstChapterData.value.hasPurchased = purchaseStatus?.hasPurchased || false;
        if (!firstChapterData.value.hasPurchased) {
          firstChapter.value = '第1章为付费章节，您尚未购买，无法查看'
          return;
        }
      } catch (error) {
        console.error('检查购买状态失败:', error);
        firstChapter.value = '第1章为付费章节，无法验证购买状态'
        return;
      }
    } else {
      // 免费章节标记为已购买
      firstChapterData.value.hasPurchased = true;
    }
    // 正常情况显示内容
    firstChapter.value = response.content;
  } catch (error) {
    console.error('获取章节失败', error);
    firstChapter.value = '';
    firstChapterData.value = null;
  } finally {
    loadingChapter.value = false;
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
  { immediate: true }
)
</script>

<style scoped>
.introduction-section {
  margin: 40px 0;
  padding: 15px;
  background-color: #edf1f1ff;
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
  white-space: pre-line;
  line-height: 1.6;
}

.continue-reading-container {
  text-align: center;
  margin-top: 20px;
}

.continue-reading-btn {
  background-color: white;
  color: #333;
  border: 1px solid #ddd;
  border-radius: 20px;
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

.purchase-dialog-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.7);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

.purchase-dialog {
  background-color: white;
  border-radius: 12px;
  width: 90%;
  max-width: 400px;
  padding: 25px;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.3);
  animation: fadeIn 0.3s ease;
  z-index: 10000;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }

  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.purchase-content {
  text-align: center;
  margin: 25px 0;
  font-size: 18px;
  color: #333;
  padding: 0 20px;
}

.purchase-content p {
  margin: 15px 0;
  font-weight: bold;
  font-size: 20px;
  color: #f56c6c;
}

.confirm-reward-btn {
  width: 100%;
  padding: 12px;
  background-color: #ff6b6b;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 16px;
  font-weight: bold;
  cursor: pointer;
  transition: background-color 0.2s;
  margin-top: 15px;
}

.confirm-reward-btn:hover {
  background-color: #ff5252;
}

.da_close-btn {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  color: #999;
  transition: color 0.2s;
}

.da_close-btn:hover {
  color: #666;
}

.purchase-insufficient-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.7);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 10001;
}

.purchase-insufficient-dialog {
  background-color: white;
  border-radius: 12px;
  width: 90%;
  max-width: 400px;
  padding: 20px;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.2);
  animation: purchaseFadeIn 0.3s ease;
}

@keyframes purchaseFadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }

  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.purchase-dialog-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 15px;
  padding-bottom: 10px;
  border-bottom: 1px solid #eee;
}

.purchase-dialog-header h3 {
  margin: 0;
  font-size: 18px;
}

.purchase-close-btn {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  color: #999;
}

.purchase-insufficient-content {
  padding: 10px 0;
}

.purchase-insufficient-message {
  color: #f56c6c;
  font-size: 16px;
  text-align: center;
  margin: 10px 0 20px;
}

.purchase-amount-info {
  display: flex;
  flex-direction: column;
  gap: 10px;
  margin: 20px 0;
  padding: 15px;
  background: #f9f9f9;
  border-radius: 8px;
}

.purchase-amount-info span {
  display: flex;
  justify-content: space-between;
  font-size: 15px;
}

.purchase-action-buttons {
  display: flex;
  gap: 15px;
  margin-top: 20px;
}

.purchase-recharge-btn {
  flex: 1;
  padding: 12px;
  background-color: #f56c6c;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 16px;
  cursor: pointer;
  transition: background 0.2s;
}

.purchase-recharge-btn:hover {
  background-color: #e65c5c;
}

.purchase-cancel-btn {
  flex: 1;
  padding: 12px;
  background-color: #f0f0f0;
  color: #666;
  border: none;
  border-radius: 6px;
  font-size: 16px;
  cursor: pointer;
  transition: background 0.2s;
}

.purchase-cancel-btn:hover {
  background-color: #e0e0e0;
}
</style>