<template>
  <div class="page-container">
    <!-- 左侧栏 -->
    <div class="left-sidebar">
      <button @click="goback">返回</button>
      <nav class="nav-menu">
        <ul>
          <li
            v-for="item in navItems"
            :key="item.key"
            :class="{ active: currentTab === item.key }"
            @click="currentTab = item.key"
          >
            {{ item.label }}
          </li>
        </ul>
      </nav>
    </div>

    <!-- 右侧内容区 -->
    <div class="right-content">
      <!-- 根据 currentTab 动态加载对应组件 -->
      <component
        :is="currentComponent"
        v-if="currentComponent"
      />

      <!-- 其他栏目提示 -->
      <div v-else>
        <p>栏目“{{ currentTab }}”内容待开发...</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'

import PersonalInfo from './PersonalInfo.vue'
import PersonalEdit from './PersonalEdit.vue'
import PersonalCollection from './PersonalCollection.vue'
import PersonalRecommend from './PersonalRecommend.vue'
import PersonalReport from './PersonalReport.vue'

const router = useRouter()
function goback() {
  router.push('/Novels/Novel_Layout/home')
}

const navItems = [
  { key: 'personalInfo', label: '个人信息' },
  { key: 'collectioninfo', label: '我的收藏' },
  { key: 'recommendations', label: '我的推荐' },
  { key: 'history', label: '历史浏览记录' },
  { key: 'inbox', label: '收件箱' },
  { key: 'editInfo', label: '信息修改' },
]

const currentTab = ref('personalInfo')
const currentComponent = computed(() => {
  if (currentTab.value === 'personalInfo') return PersonalInfo
  if (currentTab.value === 'editInfo') return PersonalEdit
  if (currentTab.value === 'collectioninfo') return PersonalCollection
  if (currentTab.value === 'recommendations') return PersonalRecommend
  if (currentTab.value === 'inbox') return PersonalReport
  return null
})
</script>

<style scoped>
.page-container {
  display: flex;
  min-height: 100vh;
  background-image: linear-gradient(to top, #f3e7e9 0%, #e3eeff 99%, #e3eeff 100%);
  padding: 20px;
  box-sizing: border-box;
  gap: 30px;
}

/* 左侧栏 */
.left-sidebar {
  width: 200px;
  display: flex;
  flex-direction: column;
  align-items: center;
}

/* 返回按钮 */
button {
  width: 100%;
  padding: 10px 0;
  margin-top:20px;
  margin-bottom: 50px;
  background-color: #f8d302f5;
  border: none;
  border-radius: 6px;
  color: white;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.3s;
  user-select: none;
}
button:hover {
  background-color: #feb47b;
}

/* 导航菜单 */
.nav-menu {
  width: 100%;
}

.nav-menu ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.nav-menu li {
  padding: 12px 15px;
  cursor: pointer;
  border-radius: 6px;
  text-align: center;
  color: #5d5c52f5;
  font-weight: 600;
  margin-bottom: 10px;
  user-select: none;
  transition: background-color 0.2s;
}

.nav-menu li:hover {
  background-color: #ffd9ca;
}

.nav-menu li.active {
  background-color:  #f8d302f5;
  color: white;
  font-weight: 700;
}

/* 右侧内容 */
.right-content {
  flex: 1;
  padding: 30px;
  max-height: 90vh;   /*滚动相关*/
  overflow-y: auto;
}

</style>
