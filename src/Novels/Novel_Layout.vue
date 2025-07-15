<template>
    <div class="novels-layout">
        <!-- 顶部导航栏 -->
        <header class="header" ref="mainHeader">
            <div class="logo">
                <img src="@/assets/logo.png" alt="TJ小说网" />
                <h1>TJ小说网</h1>
                <p>匠心打磨好作品</p>
            </div>
            <div class="search-bar">
                <input type="text" placeholder="请输入书名/作者/主角" v-model="searchQuery" />
                <button @click="handleSearch">搜索</button>
            </div>
            <div class="user-actions" @click="logout">
                <button class="login-btn">
                    <span class="login-icon">
                        <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
                            <circle cx="12" cy="8" r="4" stroke="#222" stroke-width="2" />
                            <ellipse cx="12" cy="17" rx="7" ry="4" stroke="#222" stroke-width="2" />
                        </svg>
                    </span>
                    立即登录
                </button>
            </div>
        </header>

        <!-- 导航菜单 -->
        <nav class="nav-menu" ref="mainNav" v-if="showNavMenu">
            <ul>
                <li>
                    <router-link to="/Novels/Novel_Layout/home" active-class="active-link">首页</router-link>
                </li>
                <li>
                    <router-link to="/Novels/Novel_Layout/category" active-class="active-link">分类</router-link>
                </li>
                <li>
                    <router-link to="/Novels/Novel_Layout/rank" active-class="active-link">排行榜</router-link>
                </li>
            </ul>
        </nav>

        <!-- 悬浮导航栏 -->
        <div v-if="showNavMenu && showStickyNav" class="sticky-nav">
            <div class="sticky-header-content">
                <div class="logo">
                    <img src="@/assets/logo.png" alt="七猫中文网" />
                    <h1>七猫中文网</h1>
                </div>
                <ul>
                    <li>
                        <router-link to="/Novels/Novel_Layout/home" active-class="active-link">首页</router-link>
                    </li>
                    <li>
                        <router-link to="/Novels/Novel_Layout/category" active-class="active-link">分类</router-link>
                    </li>
                    <li>
                        <router-link to="/Novels/Novel_Layout/rank" active-class="active-link">排行榜</router-link>
                    </li>
                </ul>
                <div class="search-bar">
                    <input type="text" placeholder="请输入书名/作者/主角" v-model="searchQuery" />
                    <button @click="handleSearch">搜索</button>
                </div>
                <div class="user-actions" @click="logout">
                    <button class="login-btn">
                        <span class="login-icon">
                            <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
                                <circle cx="12" cy="8" r="4" stroke="#222" stroke-width="2" />
                                <ellipse cx="12" cy="17" rx="7" ry="4" stroke="#222" stroke-width="2" />
                            </svg>
                        </span>
                        立即登录
                    </button>
                </div>
            </div>
        </div>

        <!-- 主要内容区域 -->
        <main class="main-content">
            <router-view></router-view>
        </main>

        <!-- 底部信息 -->
        <footer class="footer">
            <div class="container">
                <a href="https://github.com/ggyy1122/OurNovel/" target="_blank" class="github-link">
                    <i class="fa fa-github"></i>
                    <span> 欢迎访问我们的 GitHub仓库</span>
                </a>
            </div>
        </footer>
    </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()
const searchQuery = ref('')
const showStickyNav = ref(false)
const mainHeader = ref(null)
const mainNav = ref(null)


const showNavMenu = computed(() => {
    return !route.meta.hideNav;
})
const handleSearch = () => {
    console.log('搜索关键词:', searchQuery.value)
}

const logout = () => {
    localStorage.removeItem('isLoggedIn')
    router.push('/L_R/login')
}

// 监听滚动，显示悬浮导航栏
const handleScroll = () => {
    const headerHeight = mainHeader.value?.offsetHeight || 0
    const navHeight = mainNav.value?.offsetHeight || 0
    const threshold = headerHeight + navHeight
    showStickyNav.value = window.scrollY > threshold
}

onMounted(() => {
    window.addEventListener('scroll', handleScroll)
})

onUnmounted(() => {
    window.removeEventListener('scroll', handleScroll)
})
</script>

<style scoped>
.novels-layout {
    display: flex;
    flex-direction: column;
    min-height: 100vh;
    position: relative;
}

.header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px 20px;
    background-color: #fff;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
    z-index: 10;
}

.logo {
    display: flex;
    align-items: center;
}

.logo img {
    width: 50px;
    height: 50px;
    margin-left: 30px;
    margin-right: 10px;
}

.logo h1 {
    font-size: 24px;
    margin: 0;
}

.logo p {
    font-size: 18px;
    color: #ff4d4f;
    margin: 0;
    margin-left: 16px;
}

.search-bar {
    display: flex;
    align-items: center;
    width: 100%;
    max-width: 400px;
    margin: 0 auto;
    height: 40px;
    margin-right: 70px;
}

.search-bar input {
    flex: 1;
    padding: 0 18px;
    height: 36px;
    border: 2px solid #ffd100;
    border-right: none;
    border-radius: 24px 0 0 24px;
    outline: none;
    font-size: 18px;
    background: #fff;
}

.search-bar button {
    height: 40px;
    padding: 0 32px;
    background-color: #ffd100;
    color: #222;
    font-weight: bold;
    border: none;
    border-radius: 0 24px 24px 0;
    cursor: pointer;
    font-size: 18px;
    box-shadow: none;
    transition: background 0.2s;
}

.search-bar button:hover {
    background-color: #ffea80;
}

.user-actions .login-btn {
    height: 40px;
    display: flex;
    align-items: center;
    padding: 8px 24px;
    border: 2px solid #ffd100;
    border-radius: 999px;
    background: #fff;
    color: #222;
    font-size: 18px;
    font-weight: bold;
    cursor: pointer;
    transition: border-color 0.2s;
    outline: none;
}

.user-actions .login-btn:hover {
    border-color: #ffea80;
    color: #e09c13;
}

.login-icon {
    display: flex;
    align-items: center;
    margin-right: 8px;
}

.nav-menu {
    background-color: #fff;
    padding: 10px 20px;
    border-bottom: 1px solid #eee;
    z-index: 9;
}

.nav-menu ul {
    list-style: none;
    padding: 0;
    margin: 0;
    display: flex;
    justify-content: flex-start;
    padding-left: 40px;
}

.nav-menu li {
    margin-right: 45px;
    position: relative;
}

.nav-menu a {
    text-decoration: none;
    color: #222;
    font-weight: bold;
    font-size: 20px;
    padding: 0 8px;
    transition: color 0.2s, border 0.2s;
    border-bottom: 0;
}

.nav-menu a:hover {
    color: #e47f0d;
}

.nav-menu .active-link {
    font-size: 22px;
    color: #222;
    font-weight: bold;
    position: relative;
}

.nav-menu .active-link::after {
    content: '';
    display: block;
    margin: 0 auto;
    width: 32px;
    height: 5px;
    background: #ffd100;
    border-radius: 2px;
    position: absolute;
    left: 50%;
    transform: translateX(-50%);
    bottom: -10px;
}

/* 悬浮导航栏样式 */
.sticky-nav {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    background: #fff;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
    z-index: 100;
    border-bottom: 1px solid #eee;
    padding: 0 10px;
}

.sticky-nav ul {
    display: flex;
    justify-content: flex-start;
    align-items: center;
    list-style: none;
    margin: 0;
    padding: 0 0 0 30px;
    height: 56px;
}

.sticky-nav li {
    margin-right: 45px;
    position: relative;
}

.sticky-nav a {
    text-decoration: none;
    color: #222;
    font-weight: bold;
    font-size: 20px;
    padding: 0 8px;
    transition: color 0.2s, border 0.2s;
    border-bottom: 0;
}

.sticky-nav a:hover {
    color: #e47f0d;
}

.sticky-nav .active-link {
    font-size: 22px;
    color: #222;
    font-weight: bold;
    position: relative;
}

.sticky-nav .active-link::after {
    content: '';
    display: block;
    margin: 0 auto;
    width: 32px;
    height: 5px;
    background: #ffd100;
    border-radius: 2px;
    position: absolute;
    left: 50%;
    transform: translateX(-50%);
    bottom: -10px;
}

.main-content {
    flex: 1;
    padding: 20px;
    min-height: 300px;
    overflow: auto;
}

.footer {
    text-align: center;
    color: #222;
    padding: 10px;
    background: #fff;
    border-top: 1px solid #eee;
    position: fixed;
    left: 0;
    bottom: 0;
    width: 100%;
    z-index: 99;
}

.github-link {
    display: flex;
    align-items: center;
    justify-content: center;
    color: #666;
    text-decoration: none;
    font-size: 16px;
    transition: color 0.2s;
}

.github-link:hover {
    color: #222;
}

.fa-github {
    font-size: 24px;
    margin-right: 8px;
}

.sticky-header-content {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 6px 0 0 0;
    background: #fff;
}

.sticky-header-content .logo img {
    width: 36px;
    height: 36px;
    margin-right: 6px;
    margin-left: 30px;
}

.sticky-header-content .logo h1 {
    font-size: 20px;
    margin: 0;
}

.sticky-header-content .search-bar {
    display: flex;
    align-items: center;
    max-width: 320px;
    height: 32px;
    margin: 0 32px;
}

.sticky-header-content .search-bar input {
    flex: 1;
    padding: 0 12px;
    height: 28px;
    border: 2px solid #ffd100;
    border-right: none;
    border-radius: 16px 0 0 16px;
    outline: none;
    font-size: 15px;
    background: #fff;
}

.sticky-header-content .search-bar button {
    height: 32px;
    padding: 0 18px;
    background-color: #ffd100;
    color: #222;
    font-weight: bold;
    border: none;
    border-radius: 0 16px 16px 0;
    cursor: pointer;
    font-size: 15px;
    box-shadow: none;
}

.sticky-header-content .user-actions .login-btn {
    height: 32px;
    display: flex;
    align-items: center;
    padding: 4px 16px;
    border: 2px solid #ffd100;
    border-radius: 999px;
    background: #fff;
    color: #222;
    font-size: 15px;
    font-weight: bold;
    cursor: pointer;
}

.sticky-header-content {
    margin-right: 70px;
}
</style>