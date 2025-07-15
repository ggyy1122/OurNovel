<template>
    <div class="reader-bg">
        <div class="reader-main">
            <!-- 左侧返回/指南 -->
            <div class="left-menu">
                <button class="back-btn" @click="handleBack">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="M896 192H128v128h768zm0 256H384v128h512zm0 256H128v128h768zM320 384 128 512l192 128z">
                        </path>
                    </svg>
                    返回
                </button>
                <button class="menu-item guide" @click="handleBack">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="M512 896a384 384 0 1 0 0-768 384 384 0 0 0 0 768m0 64a448 448 0 1 1 0-896 448 448 0 0 1 0 896">
                        </path>
                        <path fill="currentColor"
                            d="M725.888 315.008C676.48 428.672 624 513.28 568.576 568.64c-55.424 55.424-139.968 107.904-253.568 157.312a12.8 12.8 0 0 1-16.896-16.832c49.536-113.728 102.016-198.272 157.312-253.632 55.36-55.296 139.904-107.776 253.632-157.312a12.8 12.8 0 0 1 16.832 16.832">
                        </path>
                    </svg>
                    指南
                </button>

            </div>
            <div class="reader-card">
                <div class="chapter-header">
                    <h2 class="chapter-title">第1章 {{ novel.title }}</h2>
                    <div class="meta-row">
                        <span>书名：{{ novel.title }}</span>
                        <span>作者：{{ novel.author }}</span>
                        <span>本章字数：{{ wordCount }}</span>
                        <span>更新时间：{{ updateTime }}</span>
                    </div>
                    <div class="chapter-divider"></div>
                </div>
                <div class="novel-content">
                    <p v-for="(para, idx) in formattedContent" :key="idx">{{ para }}</p>
                </div>
            </div>
            <div class="right-menu">
                <div class="menu-item"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24"
                        height="24">
                        <path fill="currentColor"
                            d="M640 384v256H384V384zm64 0h192v256H704zm-64 512H384V704h256zm64 0V704h192v192zm-64-768v192H384V128zm64 0h192v192H704zM320 384v256H128V384zm0 512H128V704h192zm0-768v192H128V128z">
                        </path>
                    </svg> 目录</div>
                <div class="menu-item"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24"
                        height="24">
                        <path fill="currentColor"
                            d="M600.704 64a32 32 0 0 1 30.464 22.208l35.2 109.376c14.784 7.232 28.928 15.36 42.432 24.512l112.384-24.192a32 32 0 0 1 34.432 15.36L944.32 364.8a32 32 0 0 1-4.032 37.504l-77.12 85.12a357.12 357.12 0 0 1 0 49.024l77.12 85.248a32 32 0 0 1 4.032 37.504l-88.704 153.6a32 32 0 0 1-34.432 15.296L708.8 803.904c-13.44 9.088-27.648 17.28-42.368 24.512l-35.264 109.376A32 32 0 0 1 600.704 960H423.296a32 32 0 0 1-30.464-22.208L357.696 828.48a351.616 351.616 0 0 1-42.56-24.64l-112.32 24.256a32 32 0 0 1-34.432-15.36L79.68 659.2a32 32 0 0 1 4.032-37.504l77.12-85.248a357.12 357.12 0 0 1 0-48.896l-77.12-85.248A32 32 0 0 1 79.68 364.8l88.704-153.6a32 32 0 0 1 34.432-15.296l112.32 24.256c13.568-9.152 27.776-17.408 42.56-24.64l35.2-109.312A32 32 0 0 1 423.232 64H600.64zm-23.424 64H446.72l-36.352 113.088-24.512 11.968a294.113 294.113 0 0 0-34.816 20.096l-22.656 15.36-116.224-25.088-65.28 113.152 79.68 88.192-1.92 27.136a293.12 293.12 0 0 0 0 40.192l1.92 27.136-79.808 88.192 65.344 113.152 116.224-25.024 22.656 15.296a294.113 294.113 0 0 0 34.816 20.096l24.512 11.968L446.72 896h130.688l36.48-113.152 24.448-11.904a288.282 288.282 0 0 0 34.752-20.096l22.592-15.296 116.288 25.024 65.28-113.152-79.744-88.192 1.92-27.136a293.12 293.12 0 0 0 0-40.256l-1.92-27.136 79.808-88.128-65.344-113.152-116.288 24.96-22.592-15.232a287.616 287.616 0 0 0-34.752-20.096l-24.448-11.904L577.344 128zM512 320a192 192 0 1 1 0 384 192 192 0 0 1 0-384m0 64a128 128 0 1 0 0 256 128 128 0 0 0 0-256">
                        </path>
                    </svg> 阅读设置</div>
                <div class="menu-item"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24"
                        height="24">
                        <path fill="currentColor"
                            d="M192 128v768h640V128zm-32-64h704a32 32 0 0 1 32 32v832a32 32 0 0 1-32 32H160a32 32 0 0 1-32-32V96a32 32 0 0 1 32-32">
                        </path>
                        <path fill="currentColor"
                            d="M672 128h64v768h-64zM96 192h128q32 0 32 32t-32 32H96q-32 0-32-32t32-32m0 192h128q32 0 32 32t-32 32H96q-32 0-32-32t32-32m0 192h128q32 0 32 32t-32 32H96q-32 0-32-32t32-32m0 192h128q32 0 32 32t-32 32H96q-32 0-32-32t32-32">
                        </path>
                    </svg> 加入书架</div>
                <div class="menu-item to-top" @click="scrollToTop"><svg xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="M160 832h704a32 32 0 1 1 0 64H160a32 32 0 1 1 0-64m384-578.304V704h-64V247.296L237.248 490.048 192 444.8 508.8 128l316.8 316.8-45.312 45.248z">
                        </path>
                    </svg> 回到顶部</div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { useRouter } from 'vue-router';
import { defineProps, computed } from 'vue';
const props = defineProps({
    novel: {
        type: Object,
        required: true
    }
});
const router = useRouter();
const formattedContent = computed(() => {
    return props.novel.content.split('\n').filter(p => p.trim());
});
const handleBack = () => {
    router.go(-1);
};
// 假设字数为内容长度（可按实际传递）
const wordCount = computed(() => props.novel.content.length);
// 更新时间截取（如'最新更新 第325章再用失邀请函 2025-07-13 01:00:05'提取日期）
const updateTime = computed(() => {
    const t = props.novel.update?.match(/\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}/);
    return t ? t[0] : '';
});
const scrollToTop = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
};
</script>

<style scoped>
.reader-bg {
    background: #e5e5e5;
    min-height: 100vh;
    width: 100vw;
}

.reader-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 0 36px;
    height: 64px;
    background: #fff;
    border-bottom: 1px solid #eee;
}

.logo {
    display: flex;
    align-items: center;
}

.logo img {
    width: 36px;
    height: 36px;
    margin-right: 8px;
}

.logo h1 {
    font-size: 20px;
    margin: 0;
    font-weight: bold;
}

.search-bar {
    display: flex;
    align-items: center;
    width: 400px;
    height: 32px;
}

.search-bar input {
    flex: 1;
    padding: 0 16px;
    height: 28px;
    border: 2px solid #ffd100;
    border-right: none;
    border-radius: 16px 0 0 16px;
    outline: none;
    font-size: 15px;
    background: #fff;
}

.search-bar button {
    height: 32px;
    padding: 0 18px;
    background-color: #ffd100;
    color: #222;
    font-weight: bold;
    border: none;
    border-radius: 0 16px 16px 0;
    cursor: pointer;
    font-size: 15px;
}

.user-actions .login-btn {
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

.reader-main {
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: stretch;
    width: 100%;
    margin-top: 32px;
    position: relative;
}

.left-menu {
    position: fixed;
    left: 140px;
    top: 50%;
    transform: translateY(-50%);
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100px;
    z-index: 100;
}

.back-btn {
    background: #ffd100;
    color: #222;
    border: none;
    border-radius: 8px;
    padding: 8px 0;
    font-size: 16px;
    width: 68px;
    margin-bottom: 10px;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 4px;
}
.back-btn:hover {
    background: #f27908;
    color: #fefcfc;
}

.menu-item.guide {
    background: #f5f5f5;
    color: #222;
    border-radius: 8px;
    width: 68px;
    text-align: center;
    margin-top: 6px;
    padding: 5px 0;
    font-size: 14px;
}
.menu-item-guide:hover {
    background: #ffd100;
    color: #eb640a;
}

.reader-card {
    background: #fff;
    border-radius: 12px;
    box-shadow: 0 2px 24px rgba(60, 117, 221, 0.08);
    padding: 36px 48px;
    min-width: 600px;
    max-width: 900px;
    width: 100%;
    min-height: 700px;
    margin: 0 24px;
    display: flex;
    flex-direction: column;
    align-items: stretch;
}

.chapter-header {
    margin-bottom: 24px;
    text-align: center;
}

.chapter-title {
    font-size: 28px;
    font-weight: bold;
    margin: 0 0 10px 0;
}

.meta-row {
    display: flex;
    justify-content: center;
    gap: 36px;
    font-size: 15px;
    color: #666;
    margin-bottom: 10px;
}

.chapter-divider {
    height: 1px;
    background: #eee;
    margin: 16px 0 0 0;
}

.novel-content {
    font-size: 18px;
    color: #222;
    line-height: 2.1;
    margin-top: 16px;
}

.novel-content p {
    margin-bottom: 1.2em;
    text-indent: 2em;
}

.right-menu {
    position: fixed;
    right: 90px;
    top: 50%;
    transform: translateY(-50%);
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100px;
    z-index: 100;
}

.menu-item {
    background: #fff;
    border-radius: 8px;
    color: #222;
    width: 100px;
    text-align: center;
    margin-bottom: 12px;
    padding: 10px 0;
    font-size: 16px;
    box-shadow: 0 1px 6px rgba(60, 117, 221, 0.08);
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 7px;
    transition: background 0.2s;
}

.menu-item:hover {
    background: #ffd100;
    color:#f27908;
}

.menu-item.to-top {
    margin-top: 150px;
}

.iconfont {
    font-family: "iconfont" !important;
    font-size: 20px;
    font-style: normal;
    font-weight: normal;
    display: inline-block;
    vertical-align: middle;
}

@media (max-width: 1200px) {
    .reader-card {
        min-width: 350px;
        padding: 18px 8vw;
    }

    .reader-main {
        margin-top: 16px;
    }

    .left-menu,
    .right-menu {
        width: 56px;
    }

    .menu-item,
    .menu-item.guide {
        width: 56px;
        font-size: 13px;
        padding: 7px 0;
    }
}

@media (max-width: 900px) {
    .reader-card {
        padding: 12px 3vw;
        min-width: 0;
        max-width: 98vw;
    }

    .reader-main {
        flex-direction: column;
        align-items: center;
    }

    .left-menu,
    .right-menu {
        flex-direction: row;
        margin: 0 0 12px 0;
        width: auto;
    }
}
</style>