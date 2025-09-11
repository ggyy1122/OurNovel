<template>
  <!-- 主容器 -->
  <div class="author-container">
    <!-- 固定头部 -->
    <header class="author-header">
      <h1 class="author-title">TJ小说作者管理系统</h1>
    </header>
    
    <!-- 内容 -->
    <div class="author-layout">
      <!-- 侧边栏 -->
      <aside class="author-sidebar">
        <!-- 作者信息展示 -->
        <div class="author-profile" v-if="author.currentAuthor">      
          <img :src="author.currentAuthor.avatar_url || require('@/assets/default-avatar.jpeg')" class="user-avatar">
          <span class="author-name">{{ author.currentAuthor.author_name || '未加载' }}</span>
        </div>
        <div v-else class="loading-profile">加载中...</div>
        
        <!-- 导航菜单 -->
        <nav class="sidebar-menu">
          <router-link to="/author/novels" class="menu-item">
            <i class="fas fa-book menu-icon"></i>
            <span>作品管理</span>
          </router-link>
          <router-link to="/author/stats" class="menu-item">
            <i class="fas fa-chart-line menu-icon"></i>
            <span>数据统计</span>
          </router-link>
          <router-link to="/author/income" class="menu-item">
            <i class="fas fa-coins menu-icon"></i>
            <span>收益中心</span>
          </router-link>
          <router-link to="/author/settings" class="menu-item">
            <i class="fas fa-cog menu-icon"></i>
            <span>个人设置</span>
          </router-link>

          <a href="#" class="menu-item logout-item" @click.prevent="logout">
            <i class="fas fa-sign-out-alt menu-icon"></i>
            <span>退出登录</span>
          </a>
        </nav>
      </aside>
      
      <!-- 主内容区域 -->
      <main class="author-main">
        <router-view></router-view>
      </main>
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { authorStore as author } from '@/stores/CurrentAuthor'
import { current_state } from '@/stores/index'

const router = useRouter()

// 加载作者数据
onMounted(async () => {
  if (current_state().isloggedin) {
    await author.fetchAuthorData()
  }
})

//退出，清空信息
const logout = () => {
  if (confirm('确定要退出登录吗？')) {
    current_state().clearUserInfo()
    author.clearCurrentAuthor()
    localStorage.removeItem('token')
    router.push('/L_R/Login')
  }
}
</script>

<style scoped>

/* 主容器设置 */
.author-container {
  display: flex;
  flex-direction: column;
  height: 100vh;
  overflow: hidden; 
}

/* 固定头部 */
.author-header {
  background-color: #2c3e50;
  color: white;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  height: 60px;
  display: flex;
  align-items: center;
  padding: 0 30px;
  position: sticky;
  top: 0;
  z-index: 100;
  flex-shrink: 0; 
}

.author-title {
  font-size: 20px;
  font-weight: 500;
}

/* 主布局 */
.author-layout {
  display: flex;
  flex: 1;  
  overflow: hidden; 
}

/* 固定侧边栏 */
.author-sidebar {
  width: 220px;
  background-color: white;
  border-right: 1px solid #eaeaea;
  display: flex;
  flex-direction: column;
  padding: 20px 0;
  flex-shrink: 0; 
  overflow-y: auto; 
}

/* 作者信息展示 */
.author-profile {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 0 20px 20px;
  border-bottom: 1px solid #eaeaea;
  margin-bottom: 20px;
}

.user-avatar {
  width: 80px;
  height: 80px;
  border-radius: 50%;            
  object-fit: cover;             
  border: 3px solid #e6f2ff;
  margin-bottom: 15px;
}

.author-name {
  font-size: 16px;
  font-weight: 500;
  color: #333;
}

/* 菜单栏样式 */
.sidebar-menu {
  display: flex;
  flex-direction: column;
  padding: 0 15px;
}

/* 菜单项 */
.menu-item {
  display: flex;
  align-items: center;
  padding: 12px 15px;
  color: #333;
  text-decoration: none;
  border-radius: 6px;
  margin-bottom: 5px;
  transition: all 0.2s ease;
}

.menu-item:hover {
  background-color: #e6f2ff;
  color: #3498db;
}

.menu-item.router-link-exact-active {
  background-color: #e6f2ff;
  color: #3498db;
  font-weight: 500;
}

/* 图形 */
.menu-icon {
  width: 24px;
  text-align: center;
  margin-right: 12px;
  font-size: 16px;
}

/* 登出栏样式 */
.logout-item {
  margin-top: 20px;
  border-top: 1px solid #eaeaea;
  padding-top: 15px;
}

.logout-item:hover {
  background-color: #fdedec;
  color: #e74c3c;
}

.logout-item .menu-icon {
  color: #e74c3c;
}

/* 主内容区域 */
.author-main {
  flex: 1;                       
  padding: 30px;
  background-color: #f5f7fa;
  overflow-y: auto;
}

.loading-profile {
  padding: 20px;
  text-align: center;
  color: #999;
}
</style>