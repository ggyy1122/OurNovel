<template>
    <div class="login-bg">
        <div class="top-nav">
            <div class="logo">
                <img src="@/assets/logo.png" alt="TJ小说网" />
                <h1>TJ小说网</h1>
                <p>匠心打磨好作品</p>
            </div>
            <div class="nav-right">
                <a href="#" @click.prevent="goHome">
                    <i class="fa fa-home mr-2"></i> 返回首页
                </a>
                <span> | </span>
                <a href="#" @click.prevent="switchTo(1)">
                    <i class="fa fa-user-plus mr-2"></i> 登录注册{{state.Role(1)}}帐号
                </a>
                <span> | </span>
                <a href="#" @click.prevent="switchTo(2)">
                    <i class="fa fa-user-plus mr-2"></i> 登录注册{{state.Role(2)}}帐号
                </a>
            </div>
        </div>
        <div class="login-panel">
            <main class="main-content">
                <router-view></router-view>
            </main>
            <div class="login-right">
                <div class="right-content">
                    <h2 class="right-title">异世界 · 言情 · 玄幻</h2>
                    <div class="right-desc">尽在TJ</div>
                    <img src="@/assets/logo1.png" alt="背景图" class="right-img" />
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { useRouter } from "vue-router";
import { current_state} from '@/stores/index';
const state =current_state();
const router = useRouter();
function goHome() {
    router.push('/')
}
function switchTo(num) {
    state.add(num);
    router.push('/L_R/login');
}
</script>

<style scoped>
.main-content {
    flex: 3;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
}

.top-nav {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 8px 16px;
    margin-bottom: 20px;
    width: 100%;
}

.logo {
    display: flex;
    align-items: center;
    margin-left: 10px;
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

.nav-right {
    display: flex;
    align-items: center;
    gap: 10px;
    margin-right: 70px;
}

.nav-right .fa {
    color: #41474e;
    font-size: 20px;
}

.nav-right a {
    color: #41474e;
    text-decoration: none;
    margin-left: 20px;
    margin-right: 20px;
}

.nav-right a:hover {
    text-decoration: underline;
    color: #e69a18
}

.nav-right span {
    color: #ccc;
}

.login-bg {
    background: #f4f7fa;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    padding-top: 20px;
}

.login-panel {
    width: 900px;
    height: 600px;
    background: #fff;
    border-radius: 24px;
    display: flex;
    box-shadow: 0 10px 40px 0 rgba(60, 117, 221, 0.08);
    overflow: hidden;
}

.login-right {
    flex: 2;
    background: #f9e094;
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    border-top-right-radius: 24px;
    border-bottom-right-radius: 24px;
    overflow: hidden;
}

.right-content {
    width: 100%;
    height: 100%;
    color: #111111;
    text-align: center;
    position: relative;
    z-index: 2;
    display: flex;
    flex-direction: column;
    justify-content: center;
}

.right-title {
    font-size: 32px;
    font-weight: bold;
    margin-top: 180px;
}

.right-desc {
    font-size: 24px;
    margin-top: 20px;
}

.right-img {
    position: absolute;
    z-index: 1;
    right: 0;
    top: 0;
    width: 100%;
    height: 100%;
    object-fit: contain;
    opacity: 0.4;
}


@media (max-width: 900px) {
    .login-panel {
        flex-direction: column;
        width: 98vw;
        height: auto;
        min-height: 600px;
    }

    .login-right {
        border-radius: 0;
    }

    .right-title {
        margin-top: 80px;
    }
}
</style>