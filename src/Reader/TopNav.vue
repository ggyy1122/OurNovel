<template>
  <header class="top-nav">
    <div class="nav-container">
      <div class="nav-brand">
        <span class="logo">TJ小说网</span>
        <span class="brand-text">个人中心</span>
      </div>
      <nav class="nav-menu">
        <ul>
          <li :class="{ active: activeMainNav === 'home' }">
            <router-link to="/UserHome">
              <span class="nav-icon">
                <svg viewBox="0 0 24 24" width="18" height="18">
                  <path fill="currentColor" d="M10 20v-6h4v6h5v-8h3L12 3 2 12h3v8z"/>
                </svg>
              </span>
              <span>我的首页</span>
            </router-link>
          </li>
          <li :class="{ active: activeMainNav === 'bookshelf' }">
            <router-link to="/BookShelf">
              <span class="nav-icon">
                <svg viewBox="0 0 24 24" width="18" height="18">
                  <path fill="currentColor" d="M4 6H2v14c0 1.1.9 2 2 2h14v-2H4V6zm16-4H8c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2zm-1 9h-4v4h-2v-4H9V9h4V5h2v4h4v2z"/>
                </svg>
              </span>
              <span>我的书架</span>
            </router-link>
          </li>
          <li :class="{ active: activeMainNav === 'messagecenter' }">
            <router-link to="/MessageCenter">
              <span class="nav-icon">
                <svg viewBox="0 0 24 24" width="18" height="18">
                  <path fill="currentColor" d="M12 22c1.1 0 2-.9 2-2h-4c0 1.1.89 2 2 2zm6-6v-5c0-3.07-1.64-5.64-4.5-6.32V4c0-.83-.67-1.5-1.5-1.5s-1.5.67-1.5 1.5v.68C7.63 5.36 6 7.92 6 11v5l-2 2v1h16v-1l-2-2z"/>
                </svg>
              </span>
              <span>消息中心</span>
            </router-link>
          </li>
        </ul>
      </nav>
    </div>
  </header>
</template>

<script>
export default {
  name: 'TopNav',
  props: {
    activeMainNav: {
      type: String,
      required: true
    }
  }
}
</script>

<style scoped>
.top-nav {
  background-color: #3c3b3b;
  color: white;
  height: 80px;
  display: flex;
  align-items: center;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.nav-container {
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
  display: flex;
  align-items: center;
  padding: 0 20px;
}

.nav-brand {
  display: flex;
  align-items: center;
  margin-right: 30px;
}

.logo {
  background-color: #ed424b;
  color: white;
  padding: 5px 10px;
  border-radius: 4px;
  font-size: 16px;
  font-weight: bold;
  margin-right: 10px;
}

.brand-text {
  font-size: 18px;
  color: white;
  font-weight: 500;
}

.nav-menu ul {
  display: flex;
  list-style: none;
  margin: 0;
  padding: 0;
}

.nav-menu li {
  margin-left: 15px;
}

.nav-menu li a {
  display: flex;
  align-items: center;
  color: white;
  text-decoration: none;
  padding: 8px 12px;
  border-radius: 4px;
  transition: all 0.3s;
  font-size: 14px;
}

.nav-menu li.active a {
  background-color: #ed424b;
}

.nav-menu li:not(.active):hover a {
  background-color: #3a3a3a;
}

.nav-icon {
  display: flex;
  align-items: center;
  margin-right: 6px;
}

.nav-icon svg {
  vertical-align: middle;
}
</style>