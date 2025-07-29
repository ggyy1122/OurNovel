<template>
  <div class="main-layout">
    <TopNav :activeMainNav="sidebarType" />
    <div class="content-wrapper">
      <component 
        :is="sidebarComponent" 
        :activeItem="activeSidebarItem" 
      />
      <main class="main-content">
        <div class="content-container">
          <router-view v-slot="{ Component }">
            <transition name="fade" mode="out-in">
              <component :is="Component" />
            </transition>
          </router-view>
        </div>
      </main>
    </div>
  </div>
</template>

<script>
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import TopNav from '@/Reader/TopNav.vue'
import HomeSidebar from '@/Reader/HomeSidebar.vue'
import BookshelfSidebar from '@/Reader/BookshelfSidebar.vue'
import MessageCenterSidebar from '@/Reader/MessageCenterSidebar.vue'

export default {
  name: 'MainLayout',
  components: {
    TopNav
  },
  props: {
    sidebarType: {
      type: String,
      required: true,
      validator: value => ['home', 'bookshelf','messagecenter'].includes(value)
    }
  },
  setup(props) {
    const route = useRoute()
    
   const sidebarComponent = computed(() => {
    switch(props.sidebarType) {
      case 'home':
        return HomeSidebar
      case 'bookshelf':
        return BookshelfSidebar
      case 'messagecenter':
        return MessageCenterSidebar // 需要导入这个组件
      default:
        return HomeSidebar // 默认返回首页侧边栏
    }
  })
    
    const activeSidebarItem = computed(() => {
      return route.meta.sidebarItem || ''
    })
    
    return {
      sidebarComponent,
      activeSidebarItem
    }
  }
}
</script>

<style scoped>
.main-layout {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  background-color: #f2f2f2; /* 添加浅灰色背景 */
}

.content-wrapper {
  display: flex;
  flex: 1;
  max-width: 1200px; /* 限制最大宽度 */
  margin: 0 auto; /* 居中显示 */
  width: 100%;
  padding: 0 15px; /* 两侧留白 */
}

.main-content {
  flex: 1;
  padding: 20px 0; /* 上下留白 */
  overflow-y: auto;
}

.content-container {
  background-color: white; /* 内容区白色背景 */
  border-radius: 8px; /* 圆角 */
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05); /* 轻微阴影 */
  padding: 20px; /* 内容区内边距 */
  margin-bottom: 20px; /* 底部留白 */
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

/* 侧边栏样式调整 */
.sidebar {
  width: 180px; /* 固定宽度 */
  margin-right: 20px; /* 与主内容间距 */
  background-color: #f2f2f2; /* 白色背景 */
  padding: 15px 0; /* 内边距 */
}

.sidebar li {
  padding: 10px 20px; /* 菜单项内边距 */
  margin: 5px 0; /* 菜单项间距 */
}

.sidebar li.active {
  background-color: #f0f7ff; /* 选中项浅蓝色背景 */
  border-left: 3px solid #1890ff; /* 左侧蓝色指示条 */
}

.sidebar li:hover:not(.active) {
  background-color: #f5f5f5; /* 悬停灰色背景 */
}
</style>