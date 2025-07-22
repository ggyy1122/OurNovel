<template>
    <div class="reader-bg" :style="readerBgStyle">
        <div class="guide-modal" v-if="showGuide">
            <div class="guide-content">
                <h3>快捷键指南</h3>
                <div class="shortcut-item">
                    <span class="key">F11</span>
                    <span>全屏模式</span>
                </div>
                <div class="shortcut-item">
                    <span class="key">↑ ↓</span>
                    <span>上下移动</span>
                </div>
                <div class="shortcut-item">
                    <span class="key">← →</span>
                    <span>换章</span>
                </div>
                <button class="confirm-btn" @click="showGuide = false">知道了</button>
            </div>
        </div>
        <div class="settings-modal" v-if="showSettings">
            <div class="settings-content">
                <h3>阅读设置</h3>
                <div class="setting-group">
                    <h4>阅读背景：</h4>
                    <div class="bg-options">
                        <button v-for="option in bgOptions" :key="option.value"
                            :style="{ backgroundColor: option.value }" @click="updateBgColor(option.value)"
                            :class="{ active: bgColor === option.value }">
                            {{ option.name }}
                        </button>
                    </div>
                </div>
                <div class="setting-group">
                    <h4>字体大小：</h4>
                    <div class="font-size-control">
                        <button @click="updateFontSize(-1)">A-</button>
                        <span>{{ fontSize }}</span>
                        <button @click="updateFontSize(1)">A+</button>
                    </div>
                </div>
                <div class="setting-group">
                    <h4>正文字体：</h4>
                    <div class="font-family-options">
                        <button v-for="option in fontOptions" :key="option.value"
                            @click="updateFontFamily(option.value)" :class="{ active: fontFamily === option.value }">
                            {{ option.name }}
                        </button>
                    </div>
                </div>
                <button class="close-btn" @click="showSettings = false">关闭</button>
            </div>
        </div>
        <div class="reader-main">
            <div class="left-menu">
                <button class="back-btn" @click="handleBack">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="M896 192H128v128h768zm0 256H384v128h512zm0 256H128v128h768zM320 384 128 512l192 128z">
                        </path>
                    </svg>
                    返回
                </button>
                <button class="menu-item guide" @click="handleGuideClick">
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
            <div class="reader-card" :style="{ backgroundColor: cardBgColor }">
                <div class="chapter-header" :style="{ color: textColor }">
                    <h2 class="chapter-title">第{{ selectNovelState.chapterId }}章 {{ selectNovelState.cha_title }}</h2>
                    <div class="meta-row">
                        <span>书名：{{ selectNovelState.novelName }}</span>
                        <span>作者：{{ selectNovelState.authorName }}</span>
                        <span>本章字数：{{ selectNovelState.cha_wordCount }}</span>
                        <span>更新时间：{{ selectNovelState.cha_publishTime }}</span>
                    </div>
                    <div class="chapter-divider"></div>
                </div>
                <div class="novel-content"
                    :style="{ fontSize: fontSize + 'px', fontFamily: fontFamily, color: textColor }">
                    {{ selectNovelState.cha_content }}
                </div>
                <div class="chapter-nav-buttons">
                    <button class="nav-button prev" @click="changeChapter(-1)">上一章</button>
                    <button class="nav-button next" @click="changeChapter(1)">下一章</button>
                </div>
            </div>
            <div class="right-menu">
                <div class="menu-item"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24"
                        height="24">
                        <path fill="currentColor"
                            d="M640 384v256H384V384zm64 0h192v256H704zm-64 512H384V704h256zm64 0V704h192v192zm-64-768v192H384V128zm64 0h192v192H704zM320 384v256H128V384zm0 512H128V704h192zm0-768v192H128V128z">
                        </path>
                    </svg> 目录</div>
                <div class="menu-item" @click="showSettings = !showSettings"><svg xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="M600.704 64a32 32 0 0 1 30.464 22.208l35.2 109.376c14.784 7.232 28.928 15.36 42.432 24.512l112.384-24.192a32 32 0 0 1 34.432 15.36L944.32 364.8a32 32 0 0 1-4.032 37.504l-77.12 85.12a357.12 357.12 0 0 1 0 49.024l77.12 85.248a32 32 0 0 1 4.032 37.504l-88.704 153.6a32 32 0 0 1-34.432 15.296L708.8 803.904c-13.44 9.088-27.648 17.28-42.368 24.512l-35.264 109.376A32 32 0 0 1 600.704 960H423.296a32 32 0 0 1-30.464-22.208L357.696 828.48a351.616 351.616 0 0 1-42.56-24.64l-112.32 24.256a32 32 0 0 1-34.432-15.36L79.68 659.2a32 32 0 0 1 4.032-37.504l77.12-85.248a357.12 357.12 0 0 1 0-48.896l-77.12-85.248A32 32 0 0 1 79.68 364.8l88.704-153.6a32 32 0 0 1 34.432-15.296l112.32 24.256c13.568-9.152 27.776-17.408 42.56-24.64l35.2-109.312A32 32 0 0 1 423.232 64H600.64zm-23.424 64H446.72l-36.352 113.088-24.512 11.968a294.113 294.113 0 0 0-34.816 20.096l-22.656 15.36-116.224-25.088-65.28 113.152 79.68 88.192-1.92 27.136a293.12 293.12 0 0 0 0 40.192l1.92 27.136-79.808 88.192 65.344 113.152 116.224-25.024 22.656 15.296a294.113 294.113 0 0 0 34.816 20.096l24.512 11.968L446.72 896h130.688l36.48-113.152 24.448-11.904a288.282 288.282 0 0 0 34.752-20.096l22.592-15.296 116.288 25.024 65.28-113.152-79.744-88.192 1.92-27.136a293.12 293.12 0 0 0 0-40.256l-1.92-27.136 79.808-88.128-65.344-113.152-116.288 24.96-22.592-15.232a287.616 287.616 0 0 0-34.752-20.096l-24.448-11.904L577.344 128zM512 320a192 192 0 1 1 0 384 192 192 0 0 1 0-384m0 64a128 128 0 1 0 0 256 128 128 0 0 0 0-256">
                        </path>
                    </svg> 阅读设置</div>
                <div class="menu-item" @click="handleAddShelf" :class="{ 'in-shelf': isFavorite }"
                    :disabled="isFavorite"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24"
                        height="24">
                        <path fill="currentColor"
                            d="M192 128v768h640V128zm-32-64h704a32 32 0 0 1 32 32v832a32 32 0 0 1-32 32H160a32 32 0 0 1-32-32V96a32 32 0 0 1 32-32">
                        </path>
                        <path fill="currentColor"
                            d="M672 128h64v768h-64zM96 192h128q32 0 32 32t-32 32H96q-32 0-32-32t32-32m0 192h128q32 0 32 32t-32 32H96q-32 0-32-32t32-32m0 192h128q32 0 32 32t-32 32H96q-32 0-32-32t32-32m0 192h128q32 0 32 32t-32 32H96q-32 0-32-32t32-32">
                        </path>
                    </svg> {{ isFavorite ? '已在书架' : '加入书架' }}</div>
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
import { SelectNovel_State, readerState } from '@/stores/index';
import { useRouter } from 'vue-router';
import { ref, computed } from 'vue';
import { onMounted, onUnmounted } from 'vue';
import { getChapter } from '@/API/Chapter_API';
import { addOrUpdateCollect } from '@/API/Collect_API'
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";

const showSettings = ref(false);
const fontSize = ref(18);
const fontFamily = ref('system');
const bgColor = ref('#e5e5e5');

const bgOptions = [
    { value: '#e5e5e5', name: '默认' },
    { value: '#f8f4e6', name: '护眼' },
    { value: '#e6f0f9', name: '淡蓝' },
    { value: '#f9e6e6', name: '淡粉' },
    { value: '#e8f9e6', name: '淡绿' },
    { value: '#1a1a1a', name: '深色' }
];

const fontOptions = [
    { value: 'system', name: '系统' },
    { value: 'SimSun', name: '宋体' },
    { value: 'KaiTi', name: '楷体' }
];
const readerBgStyle = ref({
    backgroundColor: bgColor.value
});

const updateFontSize = (change) => {
    fontSize.value = Math.max(12, Math.min(30, fontSize.value + change));
};

const updateBgColor = (color) => {
    bgColor.value = color;
    readerBgStyle.value.backgroundColor = color;
};

const updateFontFamily = (family) => {
    fontFamily.value = family;
};
const cardBgColor = computed(() => {
    if (bgColor.value === '#1a1a1a') return '#2a2a2a';
    const hex = bgColor.value.replace('#', '');
    if (hex.length === 6) {
        const r = Math.round((parseInt(hex.slice(0, 2), 16) + 255) / 2);
        const g = Math.round((parseInt(hex.slice(2, 4), 16) + 255) / 2);
        const b = Math.round((parseInt(hex.slice(4, 6), 16) + 255) / 2);
        return `rgb(${r},${g},${b})`;
    }
    return bgColor.value;
});
const textColor = computed(() => {
    return bgColor.value === '#1a1a1a' ? '#f0f0f0' : '#222';
});
const showGuide = ref(false);
const handleGuideClick = () => {
    showGuide.value = true;
};

const selectNovelState = SelectNovel_State();
const reader_state = readerState();
const router = useRouter();
const isFavorite = ref(reader_state.isFavorite(selectNovelState.novelId));
async function handleAddShelf() {
    if (isFavorite.value) return;
    const readerId = reader_state.readerId;
    const response = await addOrUpdateCollect(selectNovelState.novelId, readerId, 'yes');
    if (response) {
        isFavorite.value = true;
        reader_state.favoriteBooks.push({
            novelId: selectNovelState.novelId,
            novel: selectNovelState, // 保存完整作品信息
            readerId,
            isPublic: "yes",
            collectTime: new Date().toISOString()
        });
        toast("加入书架成功！", {
            "type": "success",
            "dangerouslyHTMLString": true
        })
    } else {
        toast("加入书架失败：" + response.message, {
            "type": "error",
            "dangerouslyHTMLString": true
        })
    }
}
async function changeChapter(num) {
    try {
        const response = await getChapter(selectNovelState.novelId, selectNovelState.chapterId + num);
        selectNovelState.resetChapter(
            response.chapterId,
            response.title,
            response.content,
            response.wordCount,
            response.pricePerKilo,
            response.calculatedPrice,
            response.isCharged,
            response.publishTime,
            response.status
        );
        router.push('/Novels/reader');
    } catch (error) {
        toast("章节加载失败：该章节不存在!", {
            "type": "warning",
            "dangerouslyHTMLString": true
        })
    }
}
const handleKeyDown = (e) => {
    if (showGuide.value) return;
    switch (e.key) {
        case 'ArrowLeft':
            changeChapter(-1); // 上一章
            break;
        case 'ArrowRight':
            changeChapter(1); // 下一章
            break;
    }
};
const handleBack = () => {
    router.go(-1);
};
const scrollToTop = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
};
onMounted(() => {
    window.addEventListener('keydown', handleKeyDown);
});

onUnmounted(() => {
    window.removeEventListener('keydown', handleKeyDown);
});
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
    left: 110px;
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

.menu-item.guide:hover {
    background: #ffd100;
    color: #eb640a;
}

.reader-card {
    box-shadow: 0 2px 24px rgba(60, 117, 221, 0.08);
    border-radius: 12px;
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
    background: transparent;
    margin: 16px 0;
    border-bottom: 1px dashed currentColor;
    opacity: 0.6;
}

.novel-content {
    font-size: 18px;
    color: #222;
    line-height: 2.1;
    margin-top: 16px;
    text-indent: 2em;
    white-space: pre-line !important;
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
    color: #f27908;
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

.settings-modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

.settings-content {
    background: white;
    padding: 24px;
    border-radius: 12px;
    width: 400px;
    max-width: 90%;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
}

.settings-content h3 {
    margin-top: 0;
    text-align: center;
    color: #131212;
}

.setting-group {
    margin-bottom: 20px;
}

.setting-group h4 {
    margin-bottom: 10px;
    color: #666;
}

.bg-options,
.font-family-options {
    display: flex;
    flex-wrap: wrap;
    gap: 8px;
}

.bg-options button {
    width: 60px;
    height: 30px;
    color: #f10d0d;
    border-radius: 4px;
    border: 1px solid #ddd;
    cursor: pointer;
}

.font-family-options button,
.font-size-control button {
    padding: 6px 12px;
    border-radius: 4px;
    border: 1px solid #ddd;
    background: white;
    cursor: pointer;
}

.font-size-control {
    display: flex;
    align-items: center;
    gap: 12px;
}

.font-size-control span {
    min-width: 30px;
    text-align: center;
}

button.active {
    border-color: #ffd100;
    background-color: #ffd100;
    color: #222;
}

.close-btn {
    display: block;
    margin: 20px auto 0;
    padding: 8px 24px;
    background: #ffd100;
    border: none;
    border-radius: 20px;
    cursor: pointer;
}

.guide-modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 2000;
}

.guide-content {
    background: white;
    padding: 24px;
    border-radius: 12px;
    width: 300px;
    text-align: center;
}

.guide-content h3 {
    margin-top: 0;
    margin-bottom: 20px;
    color: #333;
}

.shortcut-item {
    font-size: 16px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px 0;
    border-bottom: 1px solid #eee;
}

.key {
    background: #f5f5f5;
    padding: 4px 12px;
    border-radius: 4px;
    font-family: monospace;
    color: #333;
}

.confirm-btn {
    margin-top: 20px;
    padding: 8px 24px;
    background: #ffd100;
    border: none;
    border-radius: 20px;
    cursor: pointer;
    font-weight: bold;
}

.menu-item.in-shelf {
    background: #f0f0f0;
    color: #999;
    cursor: default;
}

.menu-item.in-shelf:hover {
    background: #f0f0f0;
    color: #999;
}

.menu-item:disabled {
    opacity: 1;
}

.chapter-nav-buttons {
    display: flex;
    justify-content: center;
    gap: 160px;
    margin: 40px auto 20px;
    width: 100%;
    max-width: 600px;
}

.nav-button {
    padding: 10px 30px;
    border: 1px solid #000;
    border-radius: 6px;
    font-size: 16px;
    cursor: pointer;
    transition: all 0.2s;
    flex: 0 0 auto;
}

.nav-button.prev {
    background-color: #f5f5f5;
    color: #333;
    border-color: #000;
}

.nav-button.next {
    background-color: #ffd100;
    color: #222;
    border-color: #000;
}

.nav-button.prev:hover {
    background-color: #e0e0e0;
    border-color: #000;
}

.nav-button.next:hover {
    background-color: #f0c000;
    border-color: #000;
}

@media (max-width: 768px) {
    .chapter-nav-buttons {
        gap: 20px;
        margin: 30px auto 15px;
    }

    .nav-button {
        padding: 8px 20px;
        font-size: 14px;
    }
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