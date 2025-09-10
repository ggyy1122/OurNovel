<template>
    <div class="author-home">
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
        <div class="header-card">
            <div class="header-main">
                <div class="avatar-badge">
                    <div class="avatar-frame"></div>
                    <img class="avatar" :src="avatarUrl" alt="作者头像" />
                </div>
                <div class="main-info">
                    <div class="author-row">
                        <h2 class="author-name">{{ author.authorName }}</h2>
                        <img class="author-tag-img" src="@/assets/tag_author_master.png" alt="大师作家图片">
                    </div>
                    <div class="nums-row">
                        <span>
                            本站作品
                            <span class="num">{{ authorNovelCount.novelCount }}</span>
                        </span>
                        <span class="sep">|</span>
                        <span>
                            累计字数
                            <span class="num">{{ authorWordCount.totalWordCount }}</span>
                        </span>
                        <span class="sep">|</span>
                        <span>
                            创作天数
                            <span class="num">{{ authorRegisterDays.registerDays }}</span>
                        </span>
                    </div>
                    <div class="intro">
                        <span>
                            联系方式：
                            <span class="intro-content">{{ maskedPhone }}</span>
                        </span>
                        <br />
                        <span>
                            简介：
                            <span class="intro-content">{{ author.introduction || 'Ta还有点神秘哦~' }}</span>
                        </span>
                    </div>
                </div>
            </div>
            <div class="join-date">
                · {{ author.registerTime ? joinDateStr : '加入时间未知' }} ·
            </div>
            <div class="header-bg"></div>
        </div>
        <!-- 下方选项卡 -->
        <div class="card-tabs">
            <div class="tabs">
                <span class="tab active">作品</span>
            </div>
            <div class="results-container">
                <div v-if="novels.length > 0" class="novel-results">
                    <SearchNovelCard v-for="novel in novels" :key="novel.novelId" :novel="novel" />
                </div>
                <p v-else class="placeholder-works-tag">暂无作品，快去催更吧！</p>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import {
    getAuthor, getAuthorNovelCount, getAuthorTotalWordCount,
    getAuthorRegisterDays, getAuthorNovels
} from '@/API/Author_API'
import { current_state, readerState } from '@/stores/index';
import SearchNovelCard from '@/Novels/SearchNovelCard.vue'

const route = useRoute()
const router = useRouter()
const authorId = route.params.authorId
const state = current_state();
const reader_state = readerState()
const showDropdown = ref(false);
const novels = ref([])

// 作者信息
const author = ref({
    authorId: 0,
    authorName: '',
    password: '',
    earning: 0,
    phone: null,
    avatarUrl: null,
    registerTime: null,
    introduction: null
})

// 统计数据
const authorNovelCount = ref({ novelCount: 0 })
const authorWordCount = ref({ totalWordCount: 0 })
const authorRegisterDays = ref({ registerDays: 0 })

// 获取作者信息和统计数据
const fetchAuthorData = async () => {
    try {
        const [authorData, novelCount, wordCount, registerDays] = await Promise.all([
            getAuthor(authorId),
            getAuthorNovelCount(authorId),
            getAuthorTotalWordCount(authorId),
            getAuthorRegisterDays(authorId)
        ])
        author.value = authorData
        authorNovelCount.value = novelCount
        authorWordCount.value = wordCount
        authorRegisterDays.value = registerDays
    } catch (error) {
        console.error('获取作者数据失败:', error)
    }
}

const fetchNovels = async () => {
    try {
        const response = await getAuthorNovels(authorId)
        novels.value = response || []
    } catch (error) {
        console.error('获取作者作品失败:', error)
    }
}

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
    fetchAuthorData(),
        fetchNovels()
})

// 格式化手机号
const maskedPhone = computed(() => {
    if (!author.value.phone) return '未公开'
    return author.value.phone.replace(/(\d{3})\d{4}(\d{4})/, '$1****$2')
})

// 格式化头像
const avatarUrl = computed(() =>
    'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' +
    (author.value.avatarUrl || 'e165315c-da2b-42c9-b3cf-c0457d168634.jpg')
)


// 根据注册天数计算加入日期
const joinDateStr = computed(() => {
    const days = authorRegisterDays.value.registerDays
    if (days > 0) {
        const joinDate = new Date()
        joinDate.setDate(joinDate.getDate() - days)
        return `${joinDate.getFullYear()}年${joinDate.getMonth() + 1}月${joinDate.getDate()}日加入TJ小说网`
    }
    return '加入时间未知'
})
</script>

<style scoped>
.author-home {
    max-width: 1200px;
    margin: 0 auto;
    padding: 20px;
}

.header-card {
    position: relative;
    background: linear-gradient(90deg, #ffe7ba 0%, #f6f1e8 100%);
    border-radius: 12px;
    box-shadow: 0 2px 8px #e7e7e7;
    margin: 0 auto 32px auto;
    padding: 32px 40px 24px 32px;
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
}

.avatar-badge {
    position: relative;
    width: 110px;
    height: 110px;
}

.avatar-frame {
    position: absolute;
    width: 100%;
    height: 100%;
    background: url('@/assets/master_frame.png') no-repeat center;
    background-size: contain;
    z-index: 1;
}

.avatar {
    position: relative;
    width: 60%;
    height: 60%;
    border-radius: 50%;
    object-fit: cover;
    z-index: 2;
    top: 20px;
    left: 20px;
}

.author-tag-img {
    height: 30px;
    width: auto;
    border-radius: 16px;
    margin-left: 6px;
}


.main-info {
    flex: 1;
}

.author-row {
    display: flex;
    align-items: center;
    margin-bottom: 2px;
    margin-top: -45px;
}

.author-name {
    font-size: 26px;
    font-weight: 700;
    margin-right: 14px;
    color: #451d03;
}

.author-tag {
    background: #dbe7fa;
    color: #385eb0;
    font-size: 16px;
    border-radius: 16px;
    padding: 3px 16px;
    font-weight: 600;
    margin-left: 6px;
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
    color: #533b12;
    margin: 0 3px;
}

.sep {
    margin: 0 18px;
    color: #d6b27d;
    font-size: 18px;
}

.intro {
    font-weight: 700;
    font-size: 15px;
    color: #533b12;
    margin: 0 3px;
    margin-top: 10px;
    margin-bottom: -10px;
}

.intro-content {
    color: #333;
    font-size: 15px;
    font-weight: 400;
}

.join-date {
    position: absolute;
    top: 16px;
    right: 36px;
    font-size: 16px;
    color: #9c864e;
    z-index: 2;
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
    color: #ed2c2c;
    font-weight: 500;
}

.tab.active::after {
    content: '';
    display: block;
    width: 38px;
    height: 4px;
    border-radius: 3px;
    background: #ed2c2c;
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
    font-size: 18px;
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
    font-weight: bold;
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