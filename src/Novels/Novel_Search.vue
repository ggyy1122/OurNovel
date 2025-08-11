<template>
    <div class="search-results">
        <header class="header" ref="mainHeader">
            <div class="logo">
                <img src="@/assets/logo.png" alt="TJ小说网" class="img" />
                <h1>TJ小说网</h1>
                <img src="@/assets/slogan.png" alt="匠心打磨好作品" class="img1" />
            </div>
            <span class="return" @click="handle_return"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024"
                    height="20" width="20" style="vertical-align: -4px;">
                    <path fill="currentColor" d="M512 128 128 447.936V896h255.936V640H640v256h255.936V447.936z"></path>
                </svg> 返回首页</span>
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
        </header>
        <div class="search-header">
            <div class="search-controls">
                <input type="text" placeholder="输入小说名/作者名/读者名" v-model="searchQuery" @keyup.enter="performSearch" />
                <button @click="performSearch">搜索</button>
            </div>
            <div class="filter-options">
                <div class="filter-tab" :class="{ active: filter === 'novel' }" @click="changeFilter('novel')">
                    小说
                </div>
                <div class="filter-tab" :class="{ active: filter === 'author' }" @click="changeFilter('author')">
                    作者
                </div>
                <div class="filter-tab" :class="{ active: filter === 'reader' }" @click="changeFilter('reader')">
                    读者
                </div>
            </div>
            <p class="result-count">共 <span class="count-number">{{ totalResults }}</span> 项相关的结果</p>
        </div>
        <div class="results-container">
            <div v-if="filter === 'novel' && novels.length > 0" class="novel-results">
                <SearchNovelCard v-for="novel in novels" :key="novel.novelId" :novel="novel" />
            </div>
            <div v-if="filter === 'author' && authors.length > 0" class="author-results">
                <SearchAuthorCard v-for="author in authors" :key="author.authorId" :author="author" />
            </div>
            <div v-if="filter === 'reader' && readers.length > 0" class="reader-results">
                <SearchReaderCard v-for="reader in readers" :key="reader.readerId" :reader="reader" />
            </div>
            <div v-if="noResults" class="no-results">
                <p>没有找到与 "{{ searchQuery }}" 相关的结果</p>
            </div>
            <div v-if="isLoading" class="loading">
                <p>正在搜索中...</p>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { searchNovels, searchAuthors, searchReaders } from '@/API/Search_API'
import SearchNovelCard from '@/Novels/SearchNovelCard.vue'
import SearchAuthorCard from '@/Novels/SearchAuthorCard.vue'
import SearchReaderCard from '@/Novels/SearchReaderCard.vue'
import { current_state, readerState } from '@/stores/index';

const route = useRoute()
const router = useRouter();
const searchQuery = ref(route.query.q || '')
const filter = ref('novel')
const novels = ref([])
const authors = ref([])
const readers = ref([])
const isLoading = ref(false)
const totalResults = ref(0)
const noResults = ref(false)
const state = current_state();
const reader_state = readerState();
const showDropdown = ref(false);

function goToLogin() {
    router.push('/L_R/login');
}
//打开主页的方法
function openMyHomePage() {
    // 打开新窗口，跳转到用户主页
    window.open(`/UserHome`, '_blank');
}
function goToRecharge() {
    router.push('/Novels/Novel_Recharge');
}
function handle_return() {
    router.push('/Novels/Novel_Layout/home');
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
onMounted(() => {
    if (searchQuery.value) {
        performSearch()
    }
})

const changeFilter = (newFilter) => {
    filter.value = newFilter
    performSearch()
}

const performSearch = async () => {
    if (!searchQuery.value.trim()) return
    router.replace({ query: { ...route.query, q: searchQuery.value.trim() } })
    isLoading.value = true
    noResults.value = false
    novels.value = []
    authors.value = []
    readers.value = []
    try {
        switch (filter.value) {
            case 'novel': {
                const novelResponse = await searchNovels(searchQuery.value)
                novels.value = Array.isArray(novelResponse)
                    ? novelResponse.filter(novel => novel.status === '连载' || novel.status === '完结')
                    : []
                totalResults.value = novels.value.length
                break
            }
            case 'author': {
                const authorResponse = await searchAuthors(searchQuery.value)
                authors.value = authorResponse || []
                totalResults.value = authors.value.length
                break
            }
            case 'reader': {
                const readerResponse = await searchReaders(searchQuery.value)
                readers.value = readerResponse || []
                totalResults.value = readers.value.length
                break
            }
        }
        noResults.value = totalResults.value === 0
    } catch (error) {
        console.error('Search error:', error)
        noResults.value = true
        totalResults.value = 0
    } finally {
        isLoading.value = false
    }
}
</script>

<style scoped>
.search-results {
    max-width: 1200px;
    margin: 0 auto;
    padding: 20px;
}

.search-header {
    margin-bottom: 30px;
    text-align: center;
}

.search-controls {
    display: flex;
    justify-content: center;
    margin-bottom: 20px;
}

.search-controls input {
    width: 60%;
    padding: 12px 20px;
    font-size: 16px;
    border: 2px solid #ffd100;
    border-radius: 24px 0 0 24px;
    outline: none;
}

.search-controls button {
    padding: 0 30px;
    background-color: #ffd100;
    color: #222;
    font-weight: bold;
    border: none;
    border-radius: 0 24px 24px 0;
    cursor: pointer;
    font-size: 16px;
    transition: background 0.2s;
}

.search-controls button:hover {
    background-color: #ffea80;
}

.return {
    font-size: 16px;
    color: #383737;
    cursor: pointer;
    margin-left: 650px;
}

.return:hover {
    color: #e67d06;
}

.filter-options {
    display: flex;
    justify-content: center;
    gap: 20px;
    margin-bottom: 25px;
}

.filter-tab {
    padding: 0.5rem 1rem;
    font-size: 17px;
    cursor: pointer;
    color: #333;
    position: relative;
}

.filter-tab.active {
    color: #ff5722;
}

.filter-tab.active::after {
    content: '';
    position: absolute;
    bottom: -1px;
    left: 0;
    width: 100%;
    height: 2px;
    background-color: #ff5722;
}

.result-count {
    color: #666;
    font-size: 14px;
}

.result-count .count-number {
    color: #ff0000;
    font-weight: bold;
}

.results-container {
    margin-top: 30px;
}

.novel-results,
.author-results,
.reader-results {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
    gap: 20px;
}

.no-results,
.loading {
    text-align: center;
    padding: 50px;
    color: #666;
    font-size: 18px;
}

.header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px 20px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
    z-index: 10;
    margin-bottom: 30px;
    padding-bottom: 10px;
}

.logo {
    display: flex;
    align-items: center;
}

.img {
    width: 36px;
    height: 36px;
    margin-right: 10px;
}

.img1 {
    width: 160px;
    height: 36px;
    margin-left: 50px;
}

.logo h1 {
    font-size: 20px;
    margin: 0;
}

.logo p {
    font-size: 18px;
    color: #ff4d4f;
    margin: 0;
    margin-left: 16px;
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

.user-dropdown {
    position: relative;
    cursor: pointer;
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
</style>