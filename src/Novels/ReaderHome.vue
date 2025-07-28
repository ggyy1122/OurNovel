<template>
    <div class="reader-home">
        <header class="header" ref="mainHeader">
            <div class="logo">
                <img src="@/assets/logo.png" alt="TJ小说网" />
                <h1>TJ小说网</h1>
                <p>匠心打磨好作品</p>
            </div>
            <span class="return" @click="handle_return"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024"
                    height="20" width="20" style="vertical-align: -4px;">
                    <path fill="currentColor" d="M512 128 128 447.936V896h255.936V640H640v256h255.936V447.936z"></path>
                </svg> 返回</span>
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
                        <a href="#" @click.prevent="goToInf" class="dropdown-item">
                            <i class="fa fa-book mr-2"></i> 我的书架
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
        <div class="header-card" :style="{ 'background-image': 'url(' + backgroundUrl + ')' }">
            <div class="header-main">
                <div class="avatar-badge">
                    <img class="avatar" :src="avatarUrl" alt="读者头像" />
                </div>
                <div class="main-info">
                    <div class="reader-row">
                        <h2 class="reader-name">{{ reader.readerName }}</h2>
                        <span class="reader-gender">{{ genderText }}</span>
                    </div>
                    <div class="nums-row">
                        <span>
                            收藏数
                            <span class="num">{{ reader.isCollectVisible === '是' ? collects.length : '隐藏' }}</span>
                        </span>
                        <span class="sep">|</span>
                        <span>
                            推荐数
                            <span class="num">{{ reader.isRecommendVisible === '是' ? recommends.length : '隐藏' }}</span>
                        </span>
                    </div>
                    <div class="intro">
                        <span>
                            联系方式：
                            <span class="intro-content">{{ maskedPhone }}</span>
                        </span>
                    </div>
                </div>
            </div>
            <div class="header-bg"></div>
        </div>
        <!-- 下方选项卡 -->
        <div class="card-tabs">
            <div class="tabs">
                <span class="tab" :class="{ active: activeTab === 'collects' }" @click="switchTab('collects')">{{
                    reader.gender === '男' ? '他' : '她' }}的收藏</span>
                <span class="tab" :class="{ active: activeTab === 'recommends' }" @click="switchTab('recommends')">{{
                    reader.gender === '男' ? '他' : '她' }}的推荐</span>
            </div>
            <div class="results-container">
                <!-- 收藏内容 -->
                <div v-if="activeTab === 'collects'">
                    <div v-if="reader.isCollectVisible === '否'" class="placeholder-works-tag">
                        已隐藏收藏
                    </div>
                    <div v-else-if="collects.length > 0" class="novel-results">
                        <SearchNovelCard v-for="novel in collects" :key="novel.novel.novelId" :novel="novel.novel" />
                    </div>
                    <p v-else class="placeholder-works-tag">暂无收藏作品</p>
                </div>
                <!-- 推荐内容 -->
                <div v-if="activeTab === 'recommends'">
                    <div v-if="reader.isRecommendVisible === '否'" class="placeholder-works-tag">
                        已隐藏推荐
                    </div>
                    <div v-else-if="recommends.length > 0" class="novel-results">
                        <SearchNovelCard v-for="novel in recommends" :key="novel.novel.novelId" :novel="novel.novel" />
                    </div>
                    <p v-else class="placeholder-works-tag">暂无推荐作品</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getReader } from '@/API/Reader_API'
import { getCollectsByReader } from '@/API/Collect_API'
import { getRecommendsByReader } from '@/API/Recommend_API'
import { current_state, readerState } from '@/stores/index';
import SearchNovelCard from '@/Novels/SearchNovelCard.vue'

const route = useRoute()
const router = useRouter()
const readerId = route.params.readerId
const state = current_state();
const reader_state = readerState()
const showDropdown = ref(false);
const activeTab = ref('collects')

// 读者信息
const reader = ref({
    readerId: 0,
    readerName: '',
    password: '',
    phone: null,
    gender: null,
    balance: 0,
    avatarUrl: null,
    backgroundUrl: null,
    isCollectVisible: '是',
    isRecommendVisible: '是'
})

// 收藏和推荐数据
const collects = ref([])
const recommends = ref([])

// 获取读者信息
const fetchReaderData = async () => {
    try {
        const response = await getReader(readerId)
        reader.value = response
    } catch (error) {
        console.error('获取读者数据失败:', error)
    }
}

// 获取收藏作品
const fetchCollects = async () => {
    try {
        const response = await getCollectsByReader(readerId)
        collects.value = response || []
    } catch (error) {
        console.error('获取收藏作品失败:', error)
    }
}

// 获取推荐作品
const fetchRecommends = async () => {
    try {
        const response = await getRecommendsByReader(readerId)
        recommends.value = response || []
    } catch (error) {
        console.error('获取推荐作品失败:', error)
    }
}

function switchTab(tab) {
    activeTab.value = tab
}

function goToLogin() {
    router.push('/L_R/login');
}
function goToInf() {
    router.push('/Novels/ReaderInfomation');
}
function goToRecharge() {
    router.push('/Novels/Novel_Recharge');
}
function handle_return() {
    router.back();
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
    fetchReaderData()
    fetchCollects()
    fetchRecommends()
})

// 格式化手机号
const maskedPhone = computed(() => {
    if (!reader.value.phone) return '未公开'
    return reader.value.phone.replace(/(\d{3})\d{4}(\d{4})/, '$1****$2')
})

// 格式化头像
const avatarUrl = computed(() =>
    'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' +
    (reader.value.avatarUrl || '07850080-e498-47a4-8d3a-fd94fb47e561.jpg')
)
// 格式化背景图
const backgroundUrl = computed(() =>
    'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' +
    (reader.value.backgroundUrl || '3234ee21-39dc-400a-a811-693b420587a3.png')
)

// 性别显示文本
const genderText = computed(() => {
    return reader.value.gender === '男' ? '♂' : '♀'
})
</script>

<style scoped>
.reader-home {
    max-width: 1200px;
    margin: 0 auto;
    padding: 20px;
}

.header-card {
    position: relative;
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
    border-radius: 12px;
    box-shadow: 0 2px 8px #e7e7e7;
    margin: 0 auto 32px auto;
    padding: 32px 40px 24px 32px;
    height: 120px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    overflow: hidden;
}

.header-main {
    display: flex;
    align-items: center;
    z-index: 2;
    position: relative;
    background-color: rgba(255, 255, 255, 0.3);
    padding: 15px;
    border-radius: 8px;
    max-width: 80%;
    margin-left: 20px;
}

.avatar-badge {
    position: relative;
    width: 110px;
    height: 110px;
}

.avatar {
    position: relative;
    width: 80px;
    height: 80px;
    border-radius: 50%;
    object-fit: cover;
    z-index: 2;
    border: 3px solid #90caf9;
}

.reader-gender {
    margin-left: 10px;
    font-size: 24px;
    font-weight: bold;
    color: #1976d2;
}

.main-info {
    flex: 1;
}

.reader-row {
    display: flex;
    align-items: center;
    margin-bottom: 2px;
    margin-top: -45px;
}

.reader-name {
    font-size: 26px;
    font-weight: 700;
    margin-right: 14px;
    color: #0d47a1;
}

.nums-row {
    margin-bottom: 12px;
    font-size: 15px;
    color: #333;
    display: flex;
    align-items: center;
}

.num {
    font-weight: 700;
    font-size: 22px;
    color: #1565c0;
    margin: 0 3px;
}

.sep {
    margin: 0 18px;
    color: #90caf9;
    font-size: 18px;
}

.intro {
    font-weight: 700;
    font-size: 15px;
    color: #0d47a1;
    margin: 0 3px;
    margin-top: 10px;
    margin-bottom: -10px;
}

.intro-content {
    color: #333;
    font-size: 15px;
    font-weight: 400;
}

.header-bg {
    position: absolute;
    right: 0;
    bottom: 0;
    width: 340px;
    height: 120px;
    opacity: 0.18;
    pointer-events: none;
    z-index: 1;
}

.card-tabs {
    margin: 0 auto;
    background: #fff;
    border-radius: 12px;
    box-shadow: 0 2px 8px #e7e7e7;
    padding: 0 0 24px 0;
}

.tabs {
    display: flex;
    align-items: center;
    border-bottom: 1.5px solid #f2f2f2;
    padding-left: 30px;
    background: #fff;
    font-size: 20px;
    height: 54px;
}

.tab {
    margin-right: 34px;
    padding: 0 2px;
    color: #888;
    cursor: pointer;
    position: relative;
    height: 50px;
    display: flex;
    align-items: center;
}

.tab.active {
    color: #1976d2;
    font-weight: 500;
}

.tab.active::after {
    content: '';
    display: block;
    width: 38px;
    height: 4px;
    border-radius: 3px;
    background: #1976d2;
    position: absolute;
    left: 50%;
    bottom: -10px;
    transform: translateX(-50%);
}

.placeholder-works-tag {
    font-size: 18px;
    color: #aaa;
    text-align: center;
    margin-top: 60px;
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

.logo img {
    width: 36px;
    height: 36px;
    margin-right: 10px;
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

.return {
    font-size: 16px;
    color: #383737;
    cursor: pointer;
    margin-left: 650px;
}

.return:hover {
    color: #e67d06;
}

.results-container {
    margin-top: 30px;
}

.novel-results {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
    gap: 20px;
}
</style>