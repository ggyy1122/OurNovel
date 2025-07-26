<template>
  <div class="page-info-container">
    <div class="avatar-container">
      <img :src="store.formattedAvatarUrl" alt="用户头像" class="user-avatar" />
    </div>
    <div class="info-container">
      <div
        v-for="(key, index) in keys"
        :key="key"
        class="info-box"
        :style="{ animationDelay: (index * 0.15) + 's' }"
      >
        <div class="info-key">{{ nameMap[key] }}</div>
        <div class="info-value">
          {{
            typeof store.$state[key] === 'object'
              ? JSON.stringify(store.$state[key])
              : store.$state[key] ?? '无'
          }}
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { readerState } from '@/stores/index'

const store = readerState()
const keys = ['readerId', 'readerName', 'phone', 'gender', 'balance', 'lastLoginTime']

const nameMap = {
  readerId: '读者ID',
  readerName: '姓名',
  phone: '电话',
  gender: '性别',
  balance: '余额',
  lastLoginTime: '最近登录时间',
}
</script>

<style scoped>
.page-info-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  background: linear-gradient(120deg, #f6d365 0%, #fda085 100%);
  padding: 20px;
  box-sizing: border-box;
}


.user-avatar {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  object-fit: cover;
  box-shadow: 0 4px 12px rgba(0,0,0,0.2);
  margin: 50px auto;
  display: block;
}

.info-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  gap: 20px;
  max-width: 600px;
}

.info-box {
  background: rgb(247, 244, 213);
  border-radius: 12px;
  padding: 25px 35px;
  width: 180px;
  text-align: center;
  box-shadow: 10px 10px 30px rgba(255, 126, 95, 0.5);
  opacity: 0;
  transform: translateY(20px);
  animation: bubbleIn 0.5s forwards;
}

.info-key {
  font-weight: 700;
  font-size: 1.1em;
  color: #ff7e5f;
  margin-bottom: 8px;
}

.info-value {
  font-size: 1em;
  color: #555;
  word-break: break-word;
}

@keyframes bubbleIn {
  0% {
    opacity: 0;
    transform: translateY(20px) scale(0.8);
  }
  60% {
    opacity: 1;
    transform: translateY(-10px) scale(1.05);
  }
  100% {
    opacity: 1;
    transform: translateY(0) scale(1);
  }
}
</style>
