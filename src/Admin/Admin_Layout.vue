<template>
    <div class="admin-container">
        <!-- 顶部导航栏 -->
        <header class="admin-header">
            <h2>小说阅读平台管理系统</h2>
            <button @click="logout">退出登录</button>
        </header>

        <!-- 侧边栏菜单 -->
        <aside class="admin-sidebar">
            <ul>
                <li><router-link to="/Admin/Admin_Layout/dashboard">仪表盘</router-link></li>
                <li><router-link to="/Admin/Admin_Layout/novelsboard">小说管理</router-link></li>
                <li><router-link to="/Admin/Admin_Layout/users">用户管理</router-link></li>
            </ul>
        </aside>

        <!-- 主内容区 -->
        <main class="admin-main">
            <router-view></router-view>
        </main>
    </div>
</template>

<script setup>
import { useRouter } from 'vue-router'

const router = useRouter()

const logout = () => {
    localStorage.removeItem('isLoggedIn')
    router.push('/L_R/login')
}
</script>

<style scoped>
.admin-container {
    display: grid;
    grid-template-areas:
        "header header"
        "sidebar main";
    grid-template-columns: 200px 1fr;
    grid-template-rows: 60px 1fr;
    min-height: 100vh;
}

.admin-header {
    grid-area: header;
    background: #35495e;
    color: white;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0 20px;
}

.admin-sidebar {
    grid-area: sidebar;
    background: #f0f0f0;
    padding: 20px;
}

.admin-main {
    grid-area: main;
    padding: 20px;
}

ul {
    list-style: none;
    padding: 0;
}

li {
    margin-bottom: 10px;
}

a {
    text-decoration: none;
    color: #2c3e50;
}

.router-link-active {
    font-weight: bold;
    color: #42b983;
}
</style>