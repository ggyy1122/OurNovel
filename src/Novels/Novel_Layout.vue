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
                <input type="text" placeholder="输入小说名/作者名/读者名" v-model="searchQuery" @keyup.enter="handleSearch" />
                <button @click="handleSearch">搜索</button>
            </div>
            <div class="user-actions">
                <!-- 未登录状态 -->
                <button v-if="!state.isloggedin" class="login-btn" @click="goToLogin">
                    <span class="login-icon">
                        <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
                            <circle cx="12" cy="8" r="4" stroke="#222" stroke-width="2" />
                            <ellipse cx="12" cy="17" rx="7" ry="4" stroke="#222" stroke-width="2" />
                        </svg>
                    </span>
                    立即登录
                </button>
                <!-- 已登录状态 -->
                <div v-else class="user-dropdown" @mouseenter="showDropdown = true" @mouseleave="showDropdown = false">
                    <img :src="reader_state.formattedAvatarUrl" alt="用户头像" class="user-avatar" />
                    <div v-if="showDropdown" class="dropdown-menu">
                        <div class="user-info">
                            <p>用户名：{{ reader_state.readerName }}</p>
                            <p>Lv1</p>
                        </div>
                        <div class="dropdown-divider"></div>
                        <a href="#" @click.prevent="openMyHomePage" class="dropdown-item">
                            <i class="fa fa-home mr-2"></i> 我的主页</a>
                        <a href="#" @click.prevent="goToRecharge" class="dropdown-item">
                            <i class="fa fa-yen mr-2"></i> 去充值
                        </a>
                        <a href="#" @click.prevent="logout" class="dropdown-item">
                            <i class="fa fa-sign-out mr-2"></i> 退出
                        </a>
                    </div>
                </div>
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
                    <h1>TJ中文网</h1>
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
                    <input type="text" placeholder="输入小说名/作者名/读者名" v-model="searchQuery" @keyup.enter="handleSearch" />
                    <button @click="handleSearch">搜索</button>
                </div>
                <div class="user-actions">
                    <!-- 未登录状态显示登录按钮 -->
                    <button v-if="!state.isloggedin" class="login-btn" @click="goToLogin">
                        <span class="login-icon">
                            <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
                                <circle cx="12" cy="8" r="4" stroke="#222" stroke-width="2" />
                                <ellipse cx="12" cy="17" rx="7" ry="4" stroke="#222" stroke-width="2" />
                            </svg>
                        </span>
                        立即登录
                    </button>
                    <!-- 已登录状态显示用户头像和下拉菜单 -->
                    <div v-else class="user-dropdown" @mouseenter="showDropdown = true"
                        @mouseleave="showDropdown = false">
                        <img :src="reader_state.formattedAvatarUrl" alt="用户头像" class="user-avatar" />
                        <div v-if="showDropdown" class="dropdown-menu">
                            <div class="user-info">
                                <p>用户名：{{ reader_state.readerName }}</p>
                                <p>Lv1</p>
                            </div>
                            <div class="dropdown-divider"></div>
                            <a href="#" @click.prevent="openMyHomePage" class="dropdown-item">
                                <i class="fa fa-home mr-2"></i> 我的主页
                            </a>
                            <a href="#" @click.prevent="goToRecharge" class="dropdown-item">
                                <i class="fa fa-yen mr-2"></i> 去充值
                            </a>
                            <a href="#" @click.prevent="logout" class="dropdown-item">
                                <i class="fa fa-sign-out mr-2"></i> 退出
                            </a>
                        </div>
                    </div>
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
import { current_state, readerState } from '@/stores/index'
const state = current_state()
const reader_state = readerState()

const router = useRouter()
const route = useRoute()
const searchQuery = ref('')
const showStickyNav = ref(false)
const showDropdown = ref(false)
const mainHeader = ref(null)
const mainNav = ref(null)


const showNavMenu = computed(() => {
    return !route.meta.hideNav;
})
function goToLogin() {
    router.push('/L_R/login');
}

function goToRecharge() {
    router.push('/Novels/Novel_Recharge');
}
function logout() {
    state.clearUserInfo();
    localStorage.removeItem('token');
    localStorage.removeItem('name');
    localStorage.removeItem('id');
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('name');
    sessionStorage.removeItem('id');
    router.push('/L_R/login');
}
const handleSearch = () => {
    if (searchQuery.value.trim()) {
        router.push({
            path: '/Novels/Search',
            query: {
                q: searchQuery.value,
                type: 'novel'
            }
        })
    }
}
//打开主页的方法
function openMyHomePage() {
    // 打开新窗口，跳转到用户主页
    window.open(`/UserHome`, '_blank');
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
    margin-right: 180px;
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

.user-dropdown {
    position: relative;
    cursor: pointer;
    margin-right: 120px;
}

.user-avatar {
    width: 36px;
    height: 36px;
    border-radius: 50%;
    object-fit: cover;
    border: 2px solid #ffd100;
}

.dropdown-menu {
    position: absolute;
    right: 0;
    top: 100%;
    width: 180px;
    background: linear-gradient(to bottom, #f4dfa5 0%, #f3f3f0 100%);
    border-radius: 8px;
    z-index: 100;
    padding: 10px 0;
    border: 1px solid rgba(0, 0, 0, 0.05);
    overflow: hidden;
}

.user-info {
    padding: 10px 15px;
    border-bottom: 1px solid #eee;
}

.user-info p:first-child {
    font-weight: bold;
    margin-bottom: 5px;
    color: #000;
}

.user-info p:last-child {
    font-size: 12px;
    color: #888;
}

.dropdown-divider {
    height: 1px;
    background: #eee;
    margin: 5px 0;
}

.dropdown-item {
    display: block;
    padding: 8px 15px;
    color: #333;
    text-decoration: none;
    font-size: 14px;
}

.dropdown-item:hover {
    background: #f5f5f5;
    color: #f7b604;
}

.dropdown-item i {
    width: 20px;
    text-align: center;
}

.sticky-header-content .user-dropdown {
    margin-right: 20px;
}

.sticky-header-content .user-avatar {
    width: 30px;
    height: 30px;
}

.sticky-header-content .dropdown-menu {
    top: 100%;
    right: -10px;
}
</style>