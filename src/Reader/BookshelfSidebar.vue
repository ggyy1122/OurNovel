<template>
  <aside class="sidebar">
    <ul>
      <li :class="{ active: activeItem === 'books' }">
        <router-link to="/BookShelf">
          <span class="menu-icon books-icon"></span>
          <span class="menu-text">我的书籍</span>
        </router-link>
      </li>
      <li :class="{ active: activeItem === 'recommend' }">
        <router-link to="/BookShelf/Recommend">
          <span class="menu-icon recommend-icon"></span>
          <span class="menu-text">我的推荐</span>
        </router-link>
      </li>
       <li :class="{ active: activeItem === 'collect' }">
        <router-link to="/BookShelf/Collect">
          <span class="menu-icon collect-icon"></span>
          <span class="menu-text">我的收藏</span>
        </router-link>
      </li>
      <li :class="{ active: activeItem === 'history' }">
        <router-link to="/BookShelf/History">
          <span class="menu-icon history-icon"></span>
          <span class="menu-text">阅读历史</span>
        </router-link>
      </li>
    </ul>
  </aside>
</template>

<script>
export default {
  name: 'BookshelfSidebar',
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
  background-image: url('~@/assets/icons/books.png');
}
.recommend-icon {
  background-image: url('~@/assets/icons/recommend.png');
}
.collect-icon {
  background-image: url('~@/assets/icons/collect.png');
}
.history-icon {
  background-image: url('~@/assets/icons/history.png');
}

.sidebar li.active .books-icon {
  background-image: url('~@/assets/icons/books-red.png');
}
.sidebar li.active .recommend-icon {
  background-image: url('~@/assets/icons/recommend-red.png');
}
.sidebar li.active .collect-icon {
  background-image: url('~@/assets/icons/collect-red.png');
}
.sidebar li.active .history-icon {
  background-image: url('~@/assets/icons/history-red.png');
}
</style>