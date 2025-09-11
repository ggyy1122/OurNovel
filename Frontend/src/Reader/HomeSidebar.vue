<template>
  <aside class="sidebar">
    <ul>
      <li :class="{ active: activeItem === 'home' }">
        <router-link to="/UserHome">
          <span class="menu-icon home-icon"></span>
          <span class="menu-text">首页</span>
        </router-link>
      </li>
      <li :class="{ active: activeItem === 'finance' }">
        <router-link to="/UserHome/finance">
          <span class="menu-icon finance-icon"></span>
          <span class="menu-text">账务中心</span>
        </router-link>
      </li>
    
       <li :class="{ active: activeItem === 'comment' }">
        <router-link to="/UserHome/comment">
          <span class="menu-icon comment-icon"></span>
          <span class="menu-text">我的书评</span>
        </router-link>
      </li>
      <li :class="{ active: activeItem === 'self-information' }">
        <router-link to="/UserHome/self-information">
          <span class="menu-icon information-icon"></span>
          <span class="menu-text">个人信息</span>
        </router-link>
      </li>
    </ul>
  </aside>
</template>

<script>
export default {
  name: 'HomeSidebar',
  props: {
    activeItem: {
      type: String,
      required: true
    }
  }
}
</script>
<style scoped>
.sidebar {
  width: 200px;
  background-color: #f8f4f0; /* 改为浅米色背景 */
  padding: 10px 0; /* 增加上下内边距 */
}

.sidebar ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.sidebar li {
  margin: 0;
}

.sidebar li a {
  display: flex;
  align-items: center;
  padding: 12px 20px;
  color: #333; /* 文字颜色加深 */
  text-decoration: none;
  font-size: 14px;
  transition: all 0.2s;
}

.sidebar li.active a {
  color: #ff2e4d; /* 选中项红色文字 */
  background-color: #fff0f0; /* 改为淡粉色背景 */
  font-weight: 500;
}

.sidebar li:hover:not(.active) a {
  background-color: rgba(255, 255, 255, 0.7); /* 调整悬停透明度 */
}

.menu-icon {
  width: 20px;
  height: 20px;
  margin-right: 10px;
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
}

.home-icon {
  background-image: url('~@/assets/icons/home.png');
}
.finance-icon {
  background-image: url('~@/assets/icons/finance.png');
}
.comment-icon {
  background-image: url('~@/assets/icons/comment.png');
}
.information-icon {
  background-image: url('~@/assets/icons/information.png');
}
.sidebar li.active .home-icon {
  background-image: url('~@/assets/icons/home-red.png');
}
.sidebar li.active .finance-icon {
  background-image: url('~@/assets/icons/finance-red.png');
}
.sidebar li.active .comment-icon {
  background-image: url('~@/assets/icons/comment-red.png');
}
.sidebar li.active .information-icon {
  background-image: url('~@/assets/icons/information-red.png');
}
</style>