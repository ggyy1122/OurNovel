<template>
  <div class="finance-container">
    <!-- 我的资产板块 -->
    <div class="asset-section">
       <h2 class="section-title">我的资产</h2>  
      <div class="balance-card">
        <div class="balance-info">
          <div class="balance-title">账户余额（虚拟币）</div>
          <div class="balance-amount">{{accountBalance}}</div>
        </div>
        <button class="recharge-btn" @click="showRecharge">充值</button>
      </div>
    </div>

   <div class="record-header">
        <h2 class="section-title">交易记录</h2>  <!-- 添加统一类名 -->
        <div class="record-notice">交易记录可能存在延时，仅供参考，请以实际金额变化为准</div>
      </div>
    <!-- 交易记录板块 -->
    <div class="record-section">

      <!-- 导航标签 - 添加点击事件 -->
      <div class="record-tabs">
        <div 
          class="tab-item" 
          :class="{ 'active': activeTab === 'consumption' }"
          @click="switchTab('consumption')"
        >
          消费记录
        </div>
        <div 
          class="tab-item" 
          :class="{ 'active': activeTab === 'recharge' }"
          @click="switchTab('recharge')"
        >
          充值记录
        </div>
        <div 
          class="tab-item" 
          :class="{ 'active': activeTab === 'twd' }"
          @click="switchTab('twd')"
        >
          订阅记录
        </div>
        <div 
          class="tab-item" 
          :class="{ 'active': activeTab === 'gifts' }"
          @click="switchTab('gifts')"
        >
          打赏记录
        </div>
      </div>

      <!-- 查询区域 - 根据当前标签动态变化 -->
      <div class="query-bar" v-if="activeTab === 'recharge'">
        <div class="query-title">查询充值记录</div>
        <select class="time-select" v-model="selectedTimeRange">
          <option>全部</option>
          <option>最近一年</option>
          <option>最近三个月</option>
          <option>最近一个月</option>
        </select>
      </div>

      <!-- 根据不同标签显示不同内容 -->
      <div class="record-table">
        <!-- 充值记录 -->
        <table v-if="activeTab === 'recharge'">
          <thead>
            <tr>
              <th>充值金额</th>
              <th>虚拟币数量</th>
              <th>充值时间</th>
            </tr>
          </thead>
          <!-- 修改后的表格行代码 -->
        <tbody>
           <tr v-if="paginatedRecords.length === 0" class="empty-row">
           <td colspan="3">目前暂无充值记录</td>
             </tr>
          <tr 
            v-else 
               v-for="record in paginatedRecords" 
                   :key="record.transactionId"
                   >
                <td class="amount-cell">{{ record.rechargeAmount }}</td>
                <td class="amount-cell">{{ record.virtualCoin }}</td>
                 <td>{{ formatDate(record.rechargeTime) }}</td>
        </tr>
             </tbody>
        </table>

        <!-- 交易记录 -->
        <table v-if="activeTab === 'consumption'">
          <thead>
            <tr>
              <th>交易虚拟币</th>
              <th>交易时间</th>
              <th>交易类型</th>
            </tr>
          </thead>
          <tbody>
           <tr v-if="paginatedRecords.length === 0" class="empty-row">
           <td colspan="3">目前暂无交易记录</td>
             </tr>
          <tr 
            v-else 
               v-for="record in paginatedRecords" 
                   :key="record.transactionId"
                   >
                <td class="amount-cell">{{ record.amount }}</td>
                 <td>{{ formatDate(record.time) }}</td>
                  <td>{{ record.transType }}</td>
        </tr>
             </tbody>
        </table>

        <!-- 订阅订单 -->
        <table v-if="activeTab === 'twd'">
          <thead>
            <tr>
              <th>订单虚拟币</th>
              <th>订阅小说</th>
              <th>订阅章节</th>
              <th>订阅时间</th>
            </tr>
          </thead>
         <tbody>
      <tr v-if="paginatedRecords.length === 0" class="empty-row">
        <td colspan="3">目前暂无订阅记录</td>
      </tr>
      <tr 
        v-else 
        v-for="record in paginatedRecords" 
        :key="record.transactionId"
      >
        <td class="amount-cell">{{ record.ConsumeAmount }}</td>
        <td>{{ record.novelTitle }}</td>
        <td>{{ record.ChapterId }}</td>
        <td>{{ formatDate(record.consumeTime) }}</td>
      </tr>
    </tbody>
        </table>

        <!-- 打赏记录 -->
          <!-- 打赏记录表格（添加分页功能） -->
  <table v-if="activeTab === 'gifts'">
    <thead>
      <tr>
        <th>打赏小说</th>
        <th>打赏虚拟币</th>
        <th>打赏时间</th>
      </tr>
    </thead>
    <tbody>
      <tr v-if="paginatedRecords.length === 0" class="empty-row">
        <td colspan="3">目前暂无打赏记录</td>
      </tr>
      <tr 
        v-else 
        v-for="record in paginatedRecords" 
        :key="record.transactionId"
      >
        <td>{{ record.novelTitle }}</td>
        <td class="amount-cell">{{ record.amount }}</td>
        <td>{{ formatDate(record.rewardTime) }}</td>
      </tr>
    </tbody>
  </table>

  <!-- 分页控件（保持与图片一致的简洁风格） -->
  <div class="pagination" v-if="rewardRecords.length > 10">
    <button 
      @click="prevPage" 
      :disabled="currentPage === 1"
      class="page-btn"
    >
      上一页
    </button>
    <span class="page-info">
      第 {{ currentPage }} 页 / 共 {{ totalPages }} 页
    </span>
    <button 
      @click="nextPage" 
      :disabled="currentPage === totalPages"
      class="page-btn"
    >
      下一页
    </button>
  </div>
     
      </div>


    </div>

  </div>
</template>

<script setup>
import { ref, computed , onMounted} from 'vue'
import { readerState } from '@/stores/index'
import { useRouter } from 'vue-router'
import {getReaderBalance} from '@/API/Reader_API';
import {getReaderReward,getReaderTransactions,getReaderRecharge,getReaderSubscribtion} from '@/API/Transaction_API'
const reader_state = readerState();  //当前用户对象
const accountBalance = ref(0);                        // 账号余额

// 当前激活的标签页
const activeTab = ref('consumption')
//打赏记录数据
const rewardRecords = ref([])
//交易记录数据
const transactionRecords = ref([])
// 充值记录数据
const rechargeRecords = ref([])
const selectedTimeRange = ref('全部')
//订阅记录数据
const subscribtionRecords = ref([])
const router = useRouter()

// 分页相关状态
const currentPage = ref(1)
const pageSize = 10 // 每页10条

// 修改后的分页计算属性
const paginatedRecords = computed(() => {
  const start = (currentPage.value - 1) * pageSize
  const end = start + pageSize
  
  // 根据当前标签选择数据源
  switch(activeTab.value) {
    case 'consumption':
      return transactionRecords.value.slice(start, end)
    case 'gifts':
      return rewardRecords.value.slice(start, end)
    case 'recharge':
      return filteredRechargeRecords.value.slice(start, end)
      case 'twd':
      return subscribtionRecords.value.slice(start, end)
    default:
      return []
  }
})

// 对应的总页数计算也需要修改
const totalPages = computed(() => {
  const totalRecords = {
    'consumption': transactionRecords.value.length,
    'gifts': rewardRecords.value.length,
    'recharge': filteredRechargeRecords.value.length,
    'twd': subscribtionRecords.value.length
  }[activeTab.value] || 0
  
  return Math.ceil(totalRecords / pageSize)
})
// 分页方法
const prevPage = () => {
  if (currentPage.value > 1) currentPage.value--
}
const nextPage = () => {
  if (currentPage.value < totalPages.value) currentPage.value++
}



// 切换标签页
const switchTab = (tab) => {
  activeTab.value = tab
}
// 格式化日期
const formatDate = (dateInput) => {
  // 处理空值情况
  if (!dateInput) return '无记录'
  
  let date
  
  // 处理各种时间格式
  if (typeof dateInput === 'number') {
    // 时间戳格式
    date = new Date(dateInput)
  } else if (dateInput.includes('/Date(')) {
    // 处理 "/Date(1634567890123)/" 这种格式
    const timestamp = parseInt(dateInput.match(/\/Date$(\d+)$\//)[1])
    date = new Date(timestamp)
  } else if (dateInput.includes('T')) {
    // ISO 8601 格式 "2023-10-20T12:34:56Z"
    date = new Date(dateInput)
  } else {
    // 尝试直接解析
    date = new Date(dateInput)
  }
  
  // 检查日期是否有效
  if (isNaN(date.getTime())) {
    console.error('无效的日期格式:', dateInput)
    return '日期格式错误'
  }
  
  // 格式化日期
  const options = {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
    hour12: false
  }
  
  return date.toLocaleString('zh-CN', options)
    .replace(/\//g, '-')
    .replace(/(\d{4})-(\d{2})-(\d{2})/, '$1年$2月$3日')
}



/*获取数据部分*/
// 获取打赏记录
const fetchRewardRecords = async () => {
  try {
    const response = await getReaderReward(reader_state.readerId)
    rewardRecords.value = Array.isArray(response) ? response : []
    currentPage.value = 1 // 重置到第一页
  } catch (error) {
    console.error('获取打赏记录失败:', error)
    rewardRecords.value = []
  }
}
// 获取交易数据
const fetchTransactionRecords = async () => {
  try {
    const response = await getReaderTransactions(reader_state.readerId)
    transactionRecords.value = Array.isArray(response) ? response : []
    currentPage.value = 1 // 重置到第一页
  } catch (error) {
    console.error('获取交易数据失败:', error)
    transactionRecords.value = []
  }
}
// 获取充值数据
const fetchRechargeRecords = async () => {
  try {
    const response = await getReaderRecharge(reader_state.readerId)
    rechargeRecords.value = Array.isArray(response) ? response : []
    currentPage.value = 1 // 重置到第一页
    console.log("充值数据",rechargeRecords.value)
  } catch (error) {
    console.error('获取充值数据失败:', error)
    rechargeRecords.value = []
  }
}
// 获取订阅数据
const fetchSubscribtionRecords = async () => {
  try {
    const response = await getReaderSubscribtion(reader_state.readerId)
    subscribtionRecords.value = Array.isArray(response) ? response : []
    currentPage.value = 1 // 重置到第一页
    console.log("订阅数据",subscribtionRecords.value)
  } catch (error) {
    console.error('获取订阅数据失败:', error)
    subscribtionRecords.value = []
  }
}
//获得余额
const fetchReaderBalance=async()=>{
  try {
    const response = await getReaderBalance(reader_state.readerId)
    accountBalance.value = response.data?.balance || response?.balance|| 0
   console.log('获取用户余额:', accountBalance.value)
  } catch (error) {
    console.error('获取用户余额失败:', error)
     accountBalance.value = 0
  }
}


// 根据时间范围筛选充值记录
const filteredRechargeRecords = computed(() => {
  // 这里可以根据selectedTimeRange筛选记录
  return rechargeRecords.value
})

// 显示充值界面
const showRecharge = () => {
const route = router.resolve({ path: '/Novels/Novel_Recharge' });
  window.open(route.href, '_blank'); // 新窗口打开
}

//钩子函数
onMounted(() => {
  fetchRewardRecords(),
  fetchTransactionRecords(),
  fetchRechargeRecords(),
 fetchSubscribtionRecords(),
 fetchReaderBalance()
})

</script>

<style scoped>
.finance-container {
  background-color: #f2f2f2; 
  min-height: 100vh;
  padding: 15px;
  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
}
.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 20px;
  gap: 15px;
}

.page-btn {
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  padding: 6px 12px;
  cursor: pointer;
  font-size: 14px;
}

.page-btn:disabled {
  color: #ccc;
  cursor: not-allowed;
}

.page-info {
  color: #666;
  font-size: 14px;
}
/* 我的资产板块 */
.asset-section {
  margin-bottom: 20px;
}
.section-title {
  font-size: 18px;          
  color: #333;             
  font-weight: 500;        
  margin-bottom: 15px;     
  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif; /* 系统默认字体 */
}

.balance-card {
  background: white;
  border-radius: 8px;
  padding: 15px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}
.balance-info {
  flex: 1;
}
.balance-title {
  font-size: 14px;
  color: #666;
  margin-bottom: 5px;
}
.balance-amount {
  font-size: 24px;
  font-weight: bold;
  color: #333;
  margin-bottom: 3px;
}

.recharge-btn {
  background: #f56c6c; /* 红色按钮 */
  color: white;
  border: none;
  padding: 8px 15px;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: background 0.2s, transform 0.1s; /* 平滑过渡 */
}

/* 鼠标悬停效果 */
.recharge-btn:hover {
  background: #e05b5b; /* 稍微深一点的红色 */
}

/* 点击（按下）效果 */
.recharge-btn:active {
  background: #d44a4a; /* 更深的红色 */
  transform: scale(0.98); /* 轻微缩小，模拟按下效果 */
}

/* 交易记录板块 */
.record-section {
  background: white;
  border-radius: 8px;
  padding: 15px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}
.record-header h3 {
  font-size: 16px;
  color: #333;
  margin-bottom: 5px;
  font-weight: 500;
}
.record-notice {
  font-size: 12px;
  color: #999;
  margin-bottom: 15px;
}

/* 导航标签 */
.record-tabs {
  display: flex;
  border-bottom: 1px solid #eee;
  margin-bottom: 15px;
}
.tab-item {
  padding: 8px 15px;
  font-size: 14px;
  color: #666;
  cursor: pointer;
  position: relative;
}
.tab-item.active {
  color: #333;
  font-weight: 500;
}
.tab-item.active::after {
  content: '';
  position: absolute;
  bottom: -1px;
  left: 0;
  right: 0;
  height: 2px;
  background: #1890ff;
}

/* 查询区域 */
.query-bar {
  display: flex;
  align-items: center;
  margin-bottom: 15px;
}
.query-title {
  font-size: 14px;
  color: #666;
  margin-right: 10px;
}
.time-select {
  border: 1px solid #ddd;
  border-radius: 4px;
  padding: 5px 10px;
  font-size: 14px;
  color: #333;
}

/* 表格样式 */
.record-table {
  width: 100%;
}
.record-table table {
  width: 100%;
  border-collapse: collapse;
}
.record-table th {
  text-align: left;
  padding: 10px 15px;
  font-size: 14px;
  color: #666;
  font-weight: normal;
  background: #fafafa;
  border-bottom: 1px solid #eee;
}
.record-table td {
  padding: 15px;
  font-size: 14px;
  color: #333;
  border-bottom: 1px solid #eee;
}
.empty-row td {
  text-align: center;
  color: #999;
  padding: 30px 0;
}

/* 充值对话框样式 */
.recharge-dialog {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}
.dialog-content {
  background: white;
  border-radius: 8px;
  padding: 20px;
  width: 400px;
}
.amount-options {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 10px;
  margin: 20px 0;
}
.amount-options button {
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  background: none;
  cursor: pointer;
}
.amount-options button.selected {
  border-color: #1890ff;
  background: #e6f7ff;
}
.dialog-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}
.dialog-actions button {
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
}
.cancel-btn {
  background: none;
  border: 1px solid #ddd;
}
.confirm-btn {
  background: #1890ff;
  color: white;
  border: none;
}
</style>
