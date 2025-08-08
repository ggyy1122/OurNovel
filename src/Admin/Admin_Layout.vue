<template>
    <div class="admin-container">
        <!-- 顶部导航栏 -->
        <header class="admin-header">
            <h2>小说阅读平台管理系统</h2>
            <button @click="logout">退出登录</button>
        </header>

       <!-- 侧边导航栏 -->
    <div class="sidebar" :class="{ 'collapsed': sidebar.isCollapsed }">
      <button @click="sidebar.toggleCollapse" class="toggle-btn">
        {{ sidebar.isCollapsed ? '>' : '<' }}
      </button>
      <div class="logo">{{ sidebar.isCollapsed ? 'L' : 'LOGO' }}</div>
      <nav><!--nav表示导航链接-->
          <router-link to="/Admin/Admin_Layout/dashboard" class="submenu-link">
            <i class="icon-dash"></i>
            <span v-if="!sidebar.isCollapsed">仪表盘</span>
          </router-link>
        <div class="menu-item-with-children">
          <div class="menu-parent" @click="toggleMenu('novel_managent')">
            <i class="icon-novel"></i>
            <span v-if="!sidebar.isCollapsed">小说管理</span>
            <span v-if="!sidebar.isCollapsed" class="arrow">{{ openMenus.includes('novel_managent') ? '▼' : '▶' }}</span>
          </div>
          <div v-if="openMenus.includes('novel_managent') && !sidebar.isCollapsed" class="submenu">
            <router-link :to="{path:'/Admin/Admin_Layout/novel_managent/ToBeReviewed_Novels'}" class="submenu-link">
            <span>待审核</span></router-link>
            <router-link :to="{path:'/Admin/Admin_Layout/novel_managent/Serial_Novels'}" class="submenu-link">
            <span>连载</span></router-link>
            <router-link :to="{path:'/Admin/Admin_Layout/novel_managent/Finish_Novels'}" class="submenu-link">
            <span>完结</span></router-link>
            <router-link :to="{path:'/Admin/Admin_Layout/novel_managent/Ban_Novels'}" class="submenu-link">
            <span>封禁</span></router-link>
          </div>
        </div>
            <router-link to="/Admin/Admin_Layout/users" class="submenu-link">
            <i class="icon-user"></i>
            <span v-if="!sidebar.isCollapsed">用户管理</span>
          </router-link>
            <router-link to="/Admin/Admin_Layout/chapterboard" class="submenu-link">
            <i class="icon-review"></i>
            <span v-if="!sidebar.isCollapsed">待审核章节管理</span>
          </router-link>
        <div class="menu-item-with-children">
          <div class="menu-parent" @click="toggleMenu('complaint_management')">
            <i class="icon-report"></i>
            <span v-if="!sidebar.isCollapsed">举报处理</span>
            <span v-if="!sidebar.isCollapsed" class="arrow">{{ openMenus.includes('complaint_management') ? '▼' : '▶' }}</span>
          </div>
          <div v-if="openMenus.includes('complaint_management') && !sidebar.isCollapsed" class="submenu">
            <router-link to="/Admin/Admin_Layout/complaint_management/unprocessed" class="submenu-link" @click="class_id = 0"><span>待处理</span></router-link>
            <router-link to="/Admin/Admin_Layout/complaint_management/processed" class="submenu-link" @click="class_id = 0"><span>已处理</span></router-link>
          </div>
        </div>
      </nav>
    </div>

        <!-- 主内容区 -->
        <main class="admin-main">
            <router-view></router-view>
        </main>
    </div>
</template>

<script setup>
import { useRouter } from 'vue-router'
import{ref} from 'vue'
const router = useRouter()

const logout = () => {
    localStorage.removeItem('isLoggedIn')
    router.push('/L_R/login')
}

//----------------------------------------------------------------------------------------------------------------
//侧边栏
import { useSidebarStore } from '@/stores/sidebar'
const sidebar = useSidebarStore()
//把控制导航栏的store引入
const openMenus = ref([]) // 用于存储当前展开的菜单
function toggleMenu(menuKey) {
  if (!sidebar.isCollapsed) {
   // 展开时正常展开/收起
      if (openMenus.value.includes(menuKey)) {
        openMenus.value = openMenus.value.filter(k => k !== menuKey)
      } else {
        openMenus.value.push(menuKey)
      }
    }
}
//----------------------------------------------------------------------------------------------------------------
</script>

<style scoped>
.admin-container {
    display: grid;
    grid-template-areas:
        "header header"
        "sidebar main";
    grid-template-columns: 250px 1fr;
    grid-template-rows: 60px 1fr;
    min-height: 100vh;
    position: relative; /* 新增：为绝对定位的sidebar提供参照 */
}

.admin-header {
    grid-area: header;
    background: #35495e;
    color: white;
    display: flex;
    gap:1200px;
    align-items: center;
    padding: 0 20px;
    height: 60px; /* 新增：确保高度一致 */
     position: fixed;      /* 新增：固定顶部 */
    top: 0;               /* 顶部对齐 */
    left: 0;
    width: 100%;          /* 宽度占满 */
    z-index: 10;          /* 保证在侧边栏之上 */
}
.admin-sidebar {
    grid-area: sidebar;
    background: #f0f0f0;
    padding: 20px;
}

.admin-main {
    grid-area: main;
    padding: 20px;
    transition: margin-left 0.3s;
    background-color: #fff;
}
.sidebar.collapsed ~ .admin-main {
    margin-right: 120px; /* 侧边栏折叠时主内容区右移宽度同步 */
}

.sidebar {/*导航栏 */
  width: 250px;
  background: #f0f0f0;
  position: fixed; 
  top: 60px;          /* 新增：紧贴顶部导航栏 */
  left: 0;
  height: calc(100vh - 60px); /* 新增：高度减去顶部导航栏 */
  transition: width 0.3s;/* 宽度变化的过渡动画 */
  overflow: hidden;/* 隐藏超出内容 */
  color: #000;/* 文字颜色 */
  z-index: 2; /* 保证在main之上但不遮挡header */
}

.sidebar.collapsed {
  width: 90px;/* 折叠后的宽度 */
}

.toggle-btn {/*折叠按钮 */
  background: #6097ce;
  color: white;
  border: none;/* 去除边框 */
  padding: 10px;/* 内边距 */
  width: 100%;/* 占满侧边栏宽度 */
  cursor: pointer;/* 鼠标悬停手势 */
  font-weight: bold;/* 加粗文字 */
}

.logo {
  font-size: 1.5rem;
  padding: 20px;
  text-align: center;/* 文字居中 */
  border-bottom: 1px solid #2c3e50;/* 底部边框 */
}

.sidebar nav {
  padding: 20px;
}

.sidebar nav a {
  color: #000;/* 链接文字颜色 */
  display: flex;/* 弹性布局（图标+文字对齐） */
  align-items: center;/* 垂直居中 */
  padding: 10px;
  text-decoration: none;/* 去除下划线 */
  border-radius: 4px;/* 圆角 */
  margin-bottom: 5px;/* 菜单项间距 */
}


.sidebar nav a i {
  margin-right: 10px;
}

/* 图标样式 - 可以使用实际图标库如Font Awesome */
.icon-novel::before { content: "📚"; }
.icon-report::before { content: "🔈"; }
.icon-user::before { content: "👤"; }
.icon-review::before { content: "✅"; }
.icon-dash::before { content: "📊"; }

.menu-item-with-children .menu-parent {
  display: flex;
  align-items: center;
  cursor: pointer;
  padding: 10px;
}
.menu-item-with-children .arrow {
  margin-left: auto;
}
.submenu {
  padding-left: 30px;
}
.submenu-link:hover {
  background: #4d82d2;
}
.submenu nav a.router-link-exact-active {
  background: #42b983;/* Vue Router 当前激活路由的高亮色 */
}
.submenu-link {
  display: block;
  color: #fff;
  padding: 6px 0;
  text-decoration: none;
}
.submenu-link.router-link-exact-active {
  background: #42b983;
}
.on {
  background: #42b983;
  color: #fff;
  font-weight: bold;
}
.off {
  background: #34495e;
  color: #fff;
}
.off:hover {
  background: #2c3e50;
}
</style>