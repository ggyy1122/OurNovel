<template>
    <div class="novels-layout">
        <!-- 顶部导航栏 -->
        <div class="combined-nav" ref="mainHeader">
            <!-- 背景图片容器 -->
            <div class="nav-bg">
                <img src="@/assets/index_flower_left.png" class="bg-left" alt="左装饰">
                <img src="@/assets/index_flower_right.png" class="bg-right" alt="右装饰">
            </div>
            <!-- 顶部内容 -->
            <div class="nav-content">
                <div class="logo">
                    <img src="@/assets/logo.png" alt="TJ小说网" class="img" />
                    <h1>TJ小说网</h1>
                    <img src="@/assets/slogan.png" alt="匠心打磨好作品" class="img1" />
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
            </div>
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
        </div>

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
        <!--我的主页-->
        <div class="fixed-button home-btn" @click="openMyHomePage">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24" height="24">
                <path fill="currentColor"
                    d="M192 413.952V896h640V413.952L512 147.328zM139.52 374.4l352-293.312a32 32 0 0 1 40.96 0l352 293.312A32 32 0 0 1 896 398.976V928a32 32 0 0 1-32 32H160a32 32 0 0 1-32-32V398.976a32 32 0 0 1 11.52-24.576">
                </path>
            </svg>
            <span>主页</span>
        </div>
        <!-- 回到顶部按钮 -->
        <div class="fixed-button top-btn" @click="scrollToTop">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24" height="24">
                <path fill="currentColor"
                    d="M160 832h704a32 32 0 1 1 0 64H160a32 32 0 1 1 0-64zm384-578.304V704h-64V247.296L237.248 490.048 192 444.8 508.8 128l316.8 316.8-45.312 45.248z">
                </path>
            </svg>
            <span>顶部</span>
        </div>
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

// 回到顶部功能
const scrollToTop = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
};

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
    /* background-color: #f7d8f4; */
}

.combined-nav {
    position: relative;
    background: #fdfafd;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
    z-index: 10;
    padding: 0 75px;
}

.nav-bg {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    display: flex;
    justify-content: space-between;
    pointer-events: none;
    z-index: 0;
}

.bg-left {
    height: 100%;
    width: auto;
    object-fit: contain;
}

.bg-right {
    height: 60%;
    width: auto;
}

.nav-content {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 20px 0;
    z-index: 1;
}

.header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 20px 75px;
    background-color: #fff;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
    z-index: 10;
}

.logo {
    display: flex;
    align-items: center;
}

.img {
    width: 50px;
    height: 50px;
    margin-left: 40px;
    margin-right: 10px;
    z-index: 10;
}

.img1 {
    width: 170px;
    height: 45px;
    margin-left: 50px;
}

.logo h1 {
    font-size: 24px;
    margin: 0;
}

.search-bar {
    display: flex;
    align-items: center;
    width: 100%;
    max-width: 400px;
    margin: 0 auto;
    height: 38px;
    margin-right: 180px;
}

.search-bar input {
    flex: 1;
    padding: 0 18px;
    height: 34px;
    border: 2px solid #ffd100;
    border-right: none;
    border-radius: 24px 0 0 24px;
    outline: none;
    font-size: 16px;
    background: #fff;
}

.search-bar button {
    height: 38px;
    padding: 0 32px;
    background-color: #ffd100;
    color: #222;
    font-weight: bold;
    border: none;
    border-radius: 0 24px 24px 0;
    cursor: pointer;
    font-size: 16px;
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
}

.nav-menu {
    padding: 20px 75px;
    border-bottom: 1px solid #eee;
    z-index: -1;
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
    width: 90%;
    background: #fff;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
    z-index: 100;
    border-bottom: 1px solid #eee;
    padding: 5px 75px;
    margin: 0 auto;
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
    background: #fdfafd;
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

/* 回到顶部按钮样式 */
.fixed-button {
    position: fixed;
    right: 50px;
    bottom: 220px;
    display: flex;
    align-items: center;
    justify-content: center;
    width: 48px;
    height: 48px;
    background-color: #fff;
    border-radius: 50%;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    cursor: pointer;
    transition: all 0.3s ease;
    z-index: 999;
}

/* 主页按钮 */
.fixed-button.home-btn {
    bottom: 220px;
}

/* 回到顶部按钮 */
.fixed-button.top-btn {
    bottom: 160px;
}

.fixed-button:hover {
    background-color: #f5f5f5;
    transform: translateY(-2px);
}

.fixed-button svg {
    width: 24px;
    height: 24px;
}

/* 在较大屏幕上显示文字 */
@media (min-width: 768px) {
    .fixed-button {
        width: auto;
        padding: 0 16px;
        border-radius: 24px;
    }

    .fixed-button span {
        margin-left: 8px;
        font-size: 14px;
    }
}
</style>