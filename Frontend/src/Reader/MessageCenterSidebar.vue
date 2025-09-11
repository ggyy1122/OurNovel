<template>
  <aside class="sidebar">
    <ul>
      <li :class="{ active: activeItem === 'messagecenter' }">
        <router-link to="/MessageCenter">
          <span class="menu-icon books-icon"></span>
          <span class="menu-text">我的消息</span>
        </router-link>
      </li>
    </ul>
  </aside>
</template>

<script>
export default {
  name: 'MessageCenterSidebar',
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
  background-color: #f8f4f0; /* 浅米色背景 */
  padding: 10px 0;
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
  color: #333;
  text-decoration: none;
  font-size: 14px;
  transition: all 0.2s;
}

.sidebar li.active a {
  color: #ff2e4d; /* 选中项红色文字 */
  background-color: #fff0f0; /* 淡粉色背景 */
  font-weight: 500;
}

.sidebar li:hover:not(.active) a {
  background-color: rgba(255, 255, 255, 0.7);
}

.menu-icon {
  width: 20px;
  height: 20px;
  margin-right: 10px;
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
}

.books-icon {
  background-image: url('~@/assets/icons/message.png');
}
.sidebar li.active .books-icon {
  background-image: url('~@/assets/icons/message-red.png');
}
</style>