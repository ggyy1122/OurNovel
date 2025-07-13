<template>
    <div class="novels-layout">
        <!-- 顶部导航栏 -->
        <header class="header">
            <div class="logo">
                <img src="@/assets/logo.png" alt="七猫中文网" />
                <h1>七猫中文网</h1>
                <p>匠心打磨好作品</p>
            </div>
            <div class="search-bar">
                <input type="text" placeholder="请输入书名/作者/主角" v-model="searchQuery" />
                <button @click="handleSearch">搜索</button>
            </div>
            <div class="user-actions">
                <button class="login-btn">
                    <span class="login-icon">
                        <!-- SVG 用户图标 -->
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
        <nav class="nav-menu">
            <ul>
                <li>
                    <router-link to="/" active-class="active-link">首页</router-link>
                </li>
                <li>
                    <router-link to="/categories" active-class="active-link">分类</router-link>
                </li>
                <li>
                    <router-link to="/rankings" active-class="active-link">排行榜</router-link>
                </li>
            </ul>
        </nav>

        <!-- 主要内容区域 -->
        <main class="main-content">
            <slot></slot>
        </main>

        <!-- 底部信息 -->
        <footer class="footer bg-gray-100 py-6">
            <div class="container mx-auto px-4 flex flex-col items-center">
                <a href="https://github.com/ggyy1122/OurNovel/" target="_blank"
                    class="flex items-center space-x-2 text-gray-600 hover:text-gray-900 transition-colors duration-300">
                    <i class="fa fa-github text-5xl"></i>
                    <span class="text-lg font-medium"> 欢迎访问我们的 GitHub仓库</span>
                </a>
            </div>
        </footer>
    </div>
</template>

<script setup>
import { ref } from 'vue';

// 定义响应式数据
const searchQuery = ref('');

// 定义方法
const handleSearch = () => {
    // 处理搜索逻辑
    console.log('搜索关键词:', searchQuery.value);
    // 可以在这里添加路由跳转或其他逻辑
};
</script>

<style scoped>
.novels-layout {
    display: flex;
    flex-direction: column;
    min-height: 100vh;
}

.header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px 20px;
    background-color: #fff;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.logo {
    display: flex;
    align-items: center;
}

.logo img {
    width: 50px;
    height: 50px;
    margin-left: 16px;
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
    /* 椭圆型 */
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
    color: #e09c13
}

.login-icon {
    display: flex;
    align-items: center;
    margin-right: 8px;
}

.nav-menu {
    background-color: #bbebec;
    padding: 10px 20px;
    border-bottom: 1px solid #eee;
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
    border-bottom: 3px solid #edde09;
}

.nav-menu .active-link {
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

.main-content {
    flex: 1;
    padding: 20px;
}

.footer {
    text-align: center;
    color: #222;
    padding: 10px;
}
</style>