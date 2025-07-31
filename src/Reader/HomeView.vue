<template>
  <div class="home-view">
    <!-- 用户信息栏 -->
    <div class="user-profile">
      <div class="profile-header">
       <div class="avatar">
          <img :src="reader_state.formattedAvatarUrl || defaultAvatar" alt="用户头像" class="user-avatar">
        </div>
        <div class="user-info">

         
          <div class="name-and-buttons">
    <h2 class="username">{{ reader_state.readerName }}</h2>
    <div class="action-buttons">
      <button class="edit-btn"  @click="goToSelfInformation">编辑资料</button>
      <button class="page-btn">个人主页</button>
    </div>
  </div>
          <div class="user-level">
            <span class="level">Lv1</span>
            <span class="user-type">普通用户</span>
          </div>
          <div class="security-level">
            <span>积分级别</span>
            <div class="progress-bar">
              <div class="progress" style="width: 30%"></div>
            </div>
            <span>30/100</span>
          </div>
          <div class="social-stats">
            <div class="stat-item">
              <span>性别</span>
              <strong>{{reader_state.gender}}</strong>
            </div>
            <div class="stat-item">
              <span>余额</span>
              <strong>{{reader_state.balance}}</strong>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- 数据卡片区域 -->
    <div class="data-cards">
      <div class="card">
        <div class="card-content">
          <div class="card-title">账户余额</div>
          <div class="card-value">{{reader_state.balance}}虚拟币</div>
          <button class="card-button">充值</button>
        </div>
      </div>
      
      <div class="card">
        <div class="card-content">
          <div class="card-title">我的推荐</div>
          <div class="card-value">{{ reader_state.recommendBooksCount }}本推荐</div>
          <button class="card-button">立即查看</button>
        </div>
      </div>
      
      <div class="card">
        <div class="card-content">
          <div class="card-title">我的书架</div>
          <div class="card-value">{{ reader_state.favoriteBooksCount }}本藏书</div>
          <button class="card-button">立即查看</button>
        </div>
      </div>
      
      <div class="card">
        <div class="card-content">
          <div class="card-title">最近阅读</div>
          <div class="card-value">0 本最近阅读</div>
          <button class="card-button">立即查看</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { readerState } from '@/stores/index'
import { watch } from 'vue'
import { useRouter } from 'vue-router'
const reader_state = readerState();  //当前用户对象
console.log(reader_state.balance)
const router = useRouter()

const goToSelfInformation = () => {
  router.push('/UserHome/self-information') // 跳转到个人信息页
}
watch(
  () => reader_state.balance,
  (newVal) => {
    console.log('余额变化:', newVal) // 如果修改balance时打印日志，则证明是响应式
  }
)
</script>

<style scoped>
.home-view {
  padding: 0; /* 移除内边距 */
  background: transparent; /* 改为透明背景 */
}

.user-profile {
  background-color: white;
  border-radius: 0; /* 移除圆角 */
  padding: 20px;
  margin-bottom: 20px;
  box-shadow: none; /* 移除阴影 */
}

.profile-header {
  display: flex;
  align-items: flex-start;
}
/*头像样式*/ 
.avatar {
  width: 90px;
  height: 90px;
  border-radius: 50%;
  overflow: hidden;
  margin-right: 20px;
}

.avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.user-info {
  flex: 1;
}

.name-and-buttons {
  display: flex;
  align-items: center;
  justify-content: space-between; /* 左右分散对齐 */
  width: 100%; /* 确保容器撑满父元素 */
}

.username {
  margin: 0; /* 移除默认外边距 */
  font-size: 20px;
}

.action-buttons {
  display: flex;
  gap: 8px; /* 按钮之间的间距 */
}


.edit-btn {
  background-color: #f0f0f0;
  color: #333;
}

.page-btn {
  background-color: #ffd700;
  color: #333;
}
.edit-btn, .page-btn {
  padding: 4px 12px;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  border: none;
  transition: all 0.2s ease; /* 添加过渡效果 */
  position: relative; /* 为点击动画做准备 */
}

/* 按压效果 */
.edit-btn:active, .page-btn:active {
  transform: scale(0.98); /* 点击时轻微缩小 */
  box-shadow: 0 0 5px rgba(0, 0, 0, 0.2); /* 点击时阴影 */
}

/* 悬停效果 */
.edit-btn:hover {
  background-color: #e0e0e0; /* 比原色深10% */
}
.page-btn:hover {
  background-color: #e6c200; /* 比原色深10% */
}

.user-level {
  margin-bottom: 15px;
}

.level {
  background-color: #ffe58f;
  color: #874d00;
  padding: 2px 8px;
  border-radius: 10px;
  font-size: 12px;
  margin-right: 10px;
}

.user-type {
  color: #666;
  font-size: 14px;
}

.security-level {
  display: flex;
  align-items: center;
  margin-bottom: 15px;
  font-size: 14px;
  color: #666;
}

.progress-bar {
  flex: 1;
  height: 6px;
  background-color: #f0f0f0;
  border-radius: 3px;
  margin: 0 10px;
}

.progress {
  height: 100%;
  background-color: #1890ff;
  border-radius: 3px;
}

.social-stats {
  display: flex;
}

.stat-item {
  margin-right: 30px;
  text-align: center;
}

.stat-item span {
  display: block;
  color: #999;
  font-size: 12px;
}

.stat-item strong {
  font-size: 16px;
  color: #333;
}

.data-cards {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 15px;
  background: transparent; /* 透明背景 */
}

.card {
  background-color: white;
  border-radius: 8px;
  padding: 15px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
}

.card-content {
  text-align: center;
}

.card-title {
  font-size: 16px;
  color: #333;
  margin-bottom: 10px;
}

.card-value {
  font-size: 18px;
  color: #ff2e4d;
  margin-bottom: 15px;
}

.card-button {
  background-color: #1890ff;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.card-button:hover {
  background-color: #40a9ff;
}
</style>