<template>
    <div class="dynamic-login-container">
        <!-- 超慢速浮动书本背景 -->
        <div class="floating-books">
            <div v-for="i in 8" :key="i" class="book" :style="getBookStyle(i)"></div>
        </div>
        <!-- 主要内容 -->
        <div class="content-wrapper">
            <div class="app-brand">
                <h1 class="app-name">TJ小说网</h1>
                <p class="app-slogan">畅游文字海洋，品味精彩人生</p>
            </div>
            <div class="login-section">
                <button class="dynamic-login-btn" @click="goToLogin">
                    <span>开始阅读之旅</span>
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none"
                        stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                        <path d="M5 12h14M12 5l7 7-7 7" />
                    </svg>
                </button>
            </div>
        </div>
        <!-- 页脚 -->
        <footer class="footer">
            <p>百万小说资源 · 每日更新 · 纯净阅读体验</p>
        </footer>
    </div>
</template>

<script setup>
import { useRouter } from 'vue-router'

const router = useRouter()

function goToLogin() {
    router.push('/L_R/login')
}

function getBookStyle(index) {
    return {
        top: `${Math.random() * 100}%`,
        left: `${Math.random() * 100}%`,
        width: `${Math.random() * 80 + 40}px`,
        height: `${Math.random() * 30 + 20}px`,
        transform: `rotate(${Math.random() * 60 - 30}deg)`,
        animationDelay: `${index * 2}s`,
        backgroundColor: `hsl(${Math.random() * 60 + 200}, 50%, 80%)`
    }
}
</script>

<style scoped>
.dynamic-login-container {
    position: relative;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100vh;
    background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
    font-family: 'Noto Sans SC', sans-serif;
    overflow: hidden;
}

.content-wrapper {
    position: relative;
    z-index: 2;
    text-align: center;
}

.app-brand {
    margin-bottom: 3rem;
    animation: fadeInUp 2s ease-out;
}

.app-name {
    font-size: 3rem;
    color: #2c3e50;
    margin-bottom: 0.5rem;
    font-weight: 700;
    text-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.app-slogan {
    font-size: 1.2rem;
    color: #7f8c8d;
    font-weight: 300;
    animation: fadeIn 2.5s ease-out;
}

.login-section {
    position: relative;
    margin-top: 2rem;
}

.dynamic-login-btn {
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 1.2rem 2.5rem;
    background: linear-gradient(to right, #3498db, #2980b9);
    color: white;
    border: none;
    border-radius: 50px;
    font-size: 1.2rem;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.6s ease;
    box-shadow: 0 4px 20px rgba(41, 128, 185, 0.3);
    z-index: 3;
    overflow: hidden;
    animation: subtlePulse 8s infinite ease-in-out;
}

.dynamic-login-btn:hover {
    transform: translateY(-3px);
    box-shadow: 0 8px 25px rgba(41, 128, 185, 0.4);
    background: linear-gradient(to right, #2980b9, #3498db);
}

.dynamic-login-btn::before {
    content: '';
    position: absolute;
    top: 0;
    left: -100%;
    width: 100%;
    height: 100%;
    background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
    transition: 1.5s;
}

.dynamic-login-btn:hover::before {
    left: 100%;
}

.dynamic-login-btn svg {
    margin-left: 0.8rem;
    transition: transform 0.6s ease;
}

.dynamic-login-btn:hover svg {
    transform: translateX(5px);
}

.floating-books {
    position: absolute;
    width: 100%;
    height: 100%;
    pointer-events: none;
}

.book {
    position: absolute;
    background: #e0e5ec;
    border-radius: 2px;
    box-shadow: 3px 3px 6px rgba(0, 0, 0, 0.1);
    opacity: 0.5;
    animation: ultraSlowFloat 90s infinite ease-in-out;

}

/* 页脚 */
.footer {
    position: absolute;
    bottom: 2rem;
    color: #7f8c8d;
    font-size: 0.9rem;
    animation: fadeIn 3s ease-out;
}

/* 动画定义 */
@keyframes fadeIn {
    from {
        opacity: 0;
    }

    to {
        opacity: 1;
    }
}

@keyframes fadeInUp {
    from {
        opacity: 0;
        transform: translateY(20px);
    }

    to {
        opacity: 1;
        transform: translateY(0);
    }
}

@keyframes subtlePulse {

    0%,
    100% {
        transform: scale(1);
        box-shadow: 0 4px 20px rgba(41, 128, 185, 0.3);
    }

    50% {
        transform: scale(1.01);
        box-shadow: 0 6px 25px rgba(41, 128, 185, 0.4);
    }
}

@keyframes ultraSlowFloat {

    0%,
    100% {
        transform: translateY(0) rotate(0deg);
    }

    50% {
        transform: translateY(-5px) rotate(1deg);

    }
}
</style>