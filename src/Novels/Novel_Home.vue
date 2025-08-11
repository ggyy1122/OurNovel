<template>
    <div class="novel-home">
        <!-- 头部banner轮播 -->
        <div class="carousel-container">
            <div class="carousel" @mouseenter="pauseAutoPlay" @mouseleave="resumeAutoPlay">
                <div class="carousel-inner" :style="{ transform: `translateX(-${currentIndex * 100}%)` }">
                    <div class="carousel-item" v-for="(item, index) in carouselItems" :key="index">
                        <img :src="item.image" :alt="item.title" class="carousel-image" />
                        <div class="carousel-caption">
                            <h3>{{ item.title }}</h3>
                            <p>{{ item.description }}</p>
                        </div>
                    </div>
                </div>
                <button class="carousel-control prev" @click="prevSlide">‹</button>
                <button class="carousel-control next" @click="nextSlide">›</button>
                <div class="carousel-indicators">
                    <button v-for="(item, index) in carouselItems" :key="index"
                        :class="{ active: currentIndex === index }" @click="goToSlide(index)"></button>
                </div>
            </div>
        </div>

        <div class="divider">
            <span>专栏 | 公告</span>
        </div>

        <!-- 专栏|公告 -->
        <div class="main-banner-section">
            <div class="banner-carousel">
                <div class="carousel-imgs">
                    <div v-for="(novel, idx) in carouselNovels" :key="novel.novelId" class="carousel-img-item"
                        :class="{ active: idx === currentBanner }" @click="goToNovel(novel.novelId)">
                        <img :src="novel.coverUrl" class="banner-img" :alt="novel.novelName" />
                    </div>
                </div>
                <div class="carousel-titles">
                    <div v-for="(novel, idx) in carouselNovels" :key="novel.novelId" class="carousel-title-item"
                        :class="{ active: idx === currentBanner }" @click="setBanner(idx)">
                        {{ novel.novelName }}
                    </div>
                </div>
            </div>
            <div class="banner-announcement">
                <div class="announcement-title">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="24" height="24">
                        <path fill="currentColor"
                            d="M3 9v6h4l5 5V4L7 9H3zm13.5 3c0-1.77-1.02-3.29-2.5-4.03v8.05c1.48-.73 2.5-2.25 2.5-4.02zM14 3.23v2.06c2.89.86 5 3.54 5 6.71s-2.11 5.85-5 6.71v2.06c4.01-.91 7-4.49 7-8.77s-2.99-7.86-7-8.77z" />
                    </svg>
                    公告
                </div>
                <ul class="announcement-list">
                    <li v-for="(item, idx) in announcements" :key="idx">
                        <a :href="item.link" target="_blank" :class="item.type">{{ item.text }}</a>
                        <div v-if="idx < announcements.length - 1" style="border-bottom:1px solid #aaa; margin:5px 0;">
                        </div>
                    </li>
                </ul>
            </div>
        </div>

        <div class="divider">
            <span>年度征文</span>
        </div>
        <div class="single-image-container">
            <a href="https://activity.zongheng.com/activity/zhengwen/detail/384?forceMode=1" target="_blank"
                class="image-link">
                <img src="@/assets/logo2.png" alt="征文" class="single-image" />
            </a>
        </div>

        <div class="divider">
            <span>专题：TJ小说网“历史区”征文</span>
        </div>
        <div class="history-novels-container">
            <div class="intro-text">
                主打 <strong>穿越历史</strong> 题材，<strong>奇思妙想</strong> 与 <strong>史实交融</strong>，书写别样 <strong>时空传奇</strong>。
            </div>
            <ul class="novel-list">
                <li v-for="book in books" :key="book.novelId" class="novel-item">
                    <div class="cover-wrapper">
                        <img :src="'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' + book.cover"
                            class="novel-cover1" @click="handleNovelClick(book)" />
                    </div>
                    <div class="novel-info">
                        <h4 class="novel-title1" @click="handleNovelClick(book)">{{ book.title }}</h4>
                        <p>作者：<span class="novel-author1" @click="goAuthorHome1(book.authorId)">{{ book.author }}</span>
                        </p>
                        <p class="novel-category">{{ book.category }}</p>
                    </div>
                </li>
            </ul>
        </div>

        <div class="divider">
            <span>大神风采</span>
        </div>
        <!-- 作者展示 -->
        <div class="authors-container">
            <div class="author-card" v-for="(author, idx) in authors" :key="author.authorId"
                :class="['card-bg-' + (idx % 3)]">
                <div class="author-avatar">
                    <img :src="'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' + (author.avatarUrl || '07850080-e498-47a4-8d3a-fd94fb47e561.jpg')"
                        :alt="author.authorName" class="avatar-img" @click="goAuthorHome(author)" />
                </div>
                <h3 class="author-name" @click="goAuthorHome(author)">{{ author.authorName }}</h3>
                <p class="author-join-date">{{ author.registerTime }}加入TJ</p>
                <p class="author-bio">{{ author.introduction }}</p>
            </div>
        </div>

        <div class="divider">
            <span>编辑精选</span>
        </div>
        <!-- 编辑精选局中局轮播 -->
        <div class="novel-carousel-container" :style="bgStyle">
            <button class="novel-carousel-control prev" @click="prevNovel">‹</button>
            <div class="novel-carousel-wrapper" @mouseenter="pauseNovelAutoPlay" @mouseleave="resumeNovelAutoPlay">
                <div class="novel-carousel-track" :style="trackStyle">
                    <div v-for="(novel, idx) in visibleNovels" :key="novel.novelId" class="novel-carousel-item"
                        :class="{ active: idx === centerIndex }" @mouseenter="hoverIndex = idx"
                        @mouseleave="hoverIndex = null" @click="handleNovelClick(novel)">
                        <img :src="'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' + (novel.coverUrl || '2e66b6aa-0b46-4391-9e99-15f4cc5c112e.jpg')"
                            :alt="novel.novelName" class="novel-cover" />
                        <p class="novel-title">{{ novel.novelName }}</p>
                        <transition name="fade">
                            <div v-if="hoverIndex === idx" class="novel-intro">
                                {{ novel.introduction
                                    ? novel.introduction.slice(0, 30) + (novel.introduction.length > 30 ? ". . ." : "")
                                    : '暂无简介' }}
                            </div>
                        </transition>
                    </div>
                </div>
            </div>
            <button class="novel-carousel-control next" @click="nextNovel">›</button>
        </div>

        <div class="divider">
            <span>最近更新</span>
        </div>

        <table class="recent-update-table">
            <thead>
                <tr>
                    <th>书名</th>
                    <th>章节</th>
                    <th>作者</th>
                    <th>更新时间</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="update in recentUpdates" :key="update.title + update.chapter">
                    <td>《{{ update.title }}》</td>
                    <td>{{ update.chapter }}</td>
                    <td @click="goAuthorHome1(update.authorId)" class="novel-author1">{{ update.author }}</td>
                    <td>{{ update.time }}</td>
                </tr>
            </tbody>
        </table>
        <!-- 底部信息区域 -->
        <footer class="zh-footer">
            <div class="zh-footer-partner">
                <div class="zh-footer-title">合作伙伴</div>
                <div class="zh-footer-partner-links">
                    （按姓氏首字母排序）<br />
                    董女士 ｜ 官先生 ｜ 关女士 <br />
                    李女士 ｜ 乐女士 ｜ 于先生 <br />
                    张女士 ｜ 郑先生 ｜ 周先生 ｜ 朱女士
                </div>
            </div>
            <div class="zh-footer-main">
                <div class="zh-footer-block">
                    <div class="zh-footer-block-title">出版合作联系</div>
                    <div class="zh-footer-block-row">
                        <span>版权合作：关女士</span> <span>guan@hanhai.com</span><br />
                        <span>剧本杀合作：朱女士</span> <span>zhu@hanhai.com</span>
                    </div>
                    <div class="zh-footer-block-row">
                        <span>有声合作：官先生</span> <span>guan@hanhai.com</span><br />
                        <span>广告合作：周先生</span> <span>zhou@hanhai.com</span>
                    </div>
                </div>
                <div class="zh-footer-block">
                    <div class="zh-footer-block-title">客服</div>
                    <div class="zh-footer-block-row">
                        <span>QQ：3492457067 (9:00-22:00)</span>
                    </div>
                    <div class="zh-footer-block-row">
                        <span>邮箱：2351289@tongji.edu.cn</span>
                    </div>
                </div>
                <div class="zh-footer-block">
                    <div class="zh-footer-block-title">违法和不良信息举报</div>
                    <div class="zh-footer-block-row">
                        <span>电话：4008765544</span>
                    </div>
                    <div class="zh-footer-block-row">
                        <span>邮箱：jubao@hanhai.com</span>
                    </div>
                    <a href="https://www.zongheng.com/#" target="_blank" class="image-link">
                        <img src="https://revo.zongheng.com/comm/2024/db1faa95.png" class="zh-footer-logo"
                            alt="瀚海文学网" />
                    </a>
                </div>
            </div>
            <div class="zh-footer-links">
                作者投稿、商务合作、关于瀚海、友情链接、联系我们、诚聘英才、法律声明、帮助中心、隐私政策、社区规范、发展历程、投诉指引、用户协议
            </div>
            <div class="zh-footer-divider"></div>
            <div class="zh-footer-copy">
                新出发沪零字第330056号｜统一社会信用代码91310104765234891D｜沪公网安备31010402020156号｜公安部网络违法犯罪举报网站｜网上有害信息举报专区<br />
                沪ICP证100589号｜沪ICP备12007632号｜沪网文〔2023〕2156-089号
            </div>
            <div class="zh-footer-copy">
                Copyright©www.hanhai.com All Rights Reserved 版权所有 上海瀚海网络科技有限公司
            </div>
            <div class="zh-footer-copy">
                瀚海文学网,提供玄幻小说,都市小说,言情小说等免费小说阅读。作者发布小说作品时,请遵守国家互联网信息管理办法规定。<br />
                本站所收录小说作品、社区话题、书库评论均属其个人行为,不代表本站立场。
            </div>
            <div class="zh-footer-badges">
                <a href="https://www.bjjubao.org.cn/" target="_blank" class="image-link">
                    <img src="https://revo.zongheng.com/comm/2024/e9875cc7.png" alt="上海市违法和不良信息举报" />
                </a>
                <a href="https://www.12377.cn/" target="_blank" class="image-link">
                    <img src="https://revo.zongheng.com/comm/2024/04dce8a2.png" alt="中央网信办违法和不良信息举报中心" />
                </a>
            </div>
        </footer>
    </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted, watch } from 'vue'
import { getAuthor } from '@/API/Author_API'
import { getNovel } from '@/API/Novel_API'
import { SelectNovel_State } from '@/stores/index'
import { useRouter } from 'vue-router'
import { getChapter, getChapterLogs } from '@/API/Chapter_API'
import { getNovelsByCategory } from '@/API/NovelCategory_API'

const router = useRouter()
const selectNovelState = SelectNovel_State()

// Banner数据
const carouselItems = ref([
    { image: require('@/assets/1.jpg'), title: 'TJ中文网', description: '匠心打磨好作品' },
    { image: require('@/assets/2.jpg'), title: '热门小说推荐', description: '最新签约作品' },
    { image: require('@/assets/3.jpeg'), title: '作家专区', description: '点击进入 >' },
    { image: require('@/assets/4.jpg'), title: 'TJ中文网', description: '匠心打磨好作品' }
])

const authors = ref([])
const novels = ref([])


const fetchAuthors = async () => {
    try {
        const authorIds = [62, 201, 123]
        const authorPromises = authorIds.map(id => getAuthor(id))
        authors.value = await Promise.all(authorPromises)
    } catch (error) {
        console.error('获取作者数据失败:', error)
    }
}

const fetchNovels = async () => {
    try {
        const novelIds = [166, 167, 168, 169, 170, 222, 263, 183, 462]
        const novelPromises = novelIds.map(id => getNovel(id))
        novels.value = await Promise.all(novelPromises)
    } catch (error) {
        console.error('获取小说数据失败:', error)
    }
}

const handleNovelClick = async (novel) => {
    try {
        console.log('点击小说:', novel)
        // 获取作者信息
        const response = await getAuthor(novel.authorId)
        // 更新store中的小说信息
        selectNovelState.resetNovel(
            novel.novelId,
            novel.authorId,
            novel.novelName,
            novel.introduction,
            novel.createTime,
            novel.coverUrl,
            novel.score,
            novel.totalWordCount,
            novel.recommendCount,
            novel.collectedCount,
            novel.status,
            novel.totalPrice,
            response.authorName,
            response.phone,
            response.avatarUrl,
            response.registerTime,
            response.introduction
        )
        // 跳转到作品主页
        router.push('/Novels/Novel_Info/home')
    } catch (error) {
        console.error('处理失败:', error)
    }
}

const goAuthorHome = (author) => {
    router.push(`/author/${author.authorId}`);
};

const goAuthorHome1 = (authorId) => {
    router.push(`/author/${authorId}`);
};

// 局中局设置
const VISIBLE_COUNT = 7
const centerIndex = Math.floor(VISIBLE_COUNT / 2)
const ITEM_WIDTH = 120 + 16
const novelCurrent = ref(0)
const hoverIndex = ref(null)

// 只显示5个，局中局循环，居中放大
const visibleNovels = computed(() => {
    const total = novels.value.length
    if (total === 0) return []
    const result = []
    for (let i = -centerIndex; i <= centerIndex; i++) {
        let idx = (novelCurrent.value + i + total) % total
        result.push(novels.value[idx])
    }
    return result
})

// 让track局中局居中
const trackStyle = computed(() => ({
    transform: `translateX(calc(${-(centerIndex) * ITEM_WIDTH}px))`,
    transition: 'transform 0.5s cubic-bezier(.4,0,.2,1)'
}))

// 自动轮播
let novelAutoPlayTimer = null
const autoPlayDelay = 3000
const startNovelAutoPlay = () => {
    stopNovelAutoPlay()
    novelAutoPlayTimer = setInterval(nextNovel, autoPlayDelay)
}
const stopNovelAutoPlay = () => {
    if (novelAutoPlayTimer) clearInterval(novelAutoPlayTimer)
    novelAutoPlayTimer = null
}
const pauseNovelAutoPlay = stopNovelAutoPlay
const resumeNovelAutoPlay = startNovelAutoPlay

function nextNovel() {
    novelCurrent.value = (novelCurrent.value + 1) % novels.value.length
}
function prevNovel() {
    novelCurrent.value = (novelCurrent.value - 1 + novels.value.length) % novels.value.length
}

// 主色虚化背景
function getDominantColor() {
    const colors = [
        'linear-gradient(to bottom, #cfdbe6 30%, #ffffff 100%)',
        'linear-gradient(to bottom, #e0d4c8 30%, #ffffff 100%)',
        'linear-gradient(to bottom, #d8e0e5 30%, #ffffff 100%)',
        'linear-gradient(to bottom, #e6d8d0 30%, #ffffff 100%)',
        'linear-gradient(to bottom, #d4e6d0 30%, #ffffff 100%)',
        'linear-gradient(to bottom, #e0d0e6 30%, #ffffff 100%)'
    ]
    // 根据当前小说索引选择颜色
    const colorIndex = novelCurrent.value % colors.length
    return { background: colors[colorIndex] }
}
const bgStyle = computed(() => {
    return getDominantColor()
})

// Banner区轮播
const currentIndex = ref(0)
const autoPlayInterval = ref(null)
const startAutoPlay = () => {
    autoPlayInterval.value = setInterval(nextSlide, 4000)
}
const pauseAutoPlay = () => {
    if (autoPlayInterval.value) {
        clearInterval(autoPlayInterval.value)
        autoPlayInterval.value = null
    }
}
const resumeAutoPlay = () => {
    if (!autoPlayInterval.value) startAutoPlay()
}
const nextSlide = () => {
    currentIndex.value = (currentIndex.value + 1) % carouselItems.value.length
}
const prevSlide = () => {
    currentIndex.value = (currentIndex.value - 1 + carouselItems.value.length) % carouselItems.value.length
}
const goToSlide = (index) => {
    currentIndex.value = index
}
//历史征文


const books = ref([])

async function fetchHistoryNovels() {
    const novels = await getNovelsByCategory('历史')
    if (!novels || novels.length === 0) {
        books.value = []
        return
    }

    const filtered = novels.filter(n => n.status === '连载' || n.status === '完结').slice(0, 5)

    const detailedBooks = await Promise.all(
        filtered.map(async (novel) => {
            try {
                const detail = await getNovel(novel.novelId)
                let authorName = '未知作者'

                try {
                    const author = await getAuthor(detail.authorId)
                    authorName = author.authorName || '未知作者'
                } catch (error) {
                    console.warn('获取作者失败:', error)
                }

                return {
                    novelId: novel.novelId,
                    authorId: detail.authorId,
                    novelName: detail.novelName,
                    introduction: detail.introduction,
                    createTime: detail.createTime,
                    coverUrl: detail.coverUrl,
                    score: detail.score,
                    totalWordCount: detail.totalWordCount,
                    recommendCount: detail.recommendCount,
                    collectedCount: detail.collectedCount,
                    totalPrice: detail.totalPrice,


                    title: detail.novelName,
                    author: authorName,
                    status: detail.status,
                    cover: detail.coverUrl,
                    intro: detail.introduction
                }
            } catch {
                return {
                    novelId: novel.novelId,
                    authorId: novel.authorId || '',
                    novelName: novel.title || '',
                    introduction: novel.introduction || '',
                    createTime: novel.createTime || '',
                    coverUrl: novel.coverImg || '',
                    score: novel.score || 0,
                    totalWordCount: novel.totalWordCount || 0,
                    recommendCount: novel.recommendCount || 0,
                    collectedCount: novel.collectedCount || 0,
                    totalPrice: novel.totalPrice || 0,
                    title: novel.title,
                    author: '未知作者',
                    status: novel.status,
                    cover: novel.coverImg,
                    intro: ''
                }
            }
        })
    )

    books.value = detailedBooks
}

//更新

const recentUpdates = ref([])

async function fetchRecentUpdates() {
    const logsRes = await getChapterLogs()
    if (!logsRes || !logsRes.data) return

    const sortedLogs = logsRes.data
        .sort((a, b) => new Date(b.time) - new Date(a.time))
        .slice(0, 6)

    const updates = await Promise.all(sortedLogs.map(async log => {
        try {
            const novel = await getNovel(log.novelId)
            const chapter = await getChapter(log.novelId, log.chapterId)
            const author = await getAuthor(novel.authorId)
            return {
                title: novel.novelName || '未知小说',
                authorId: novel.authorId,
                author: author.authorName || '未知作者',
                chapter: `第${chapter.chapterId}章 ${chapter.title}`,
                time: log.time
            }
        } catch {
            return null
        }
    }))

    recentUpdates.value = updates.filter(Boolean)
}

const scrollToTop = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
};

const carouselNovels = [
    { novelId: 170, novelName: '如意姑娘的', coverUrl: require('@/assets/side1.jpg') },
    { novelId: 170, novelName: '写给鼹鼠先生的情', coverUrl: require('@/assets/side2.jpg') },
    { novelId: 170, novelName: '问九卿', coverUrl: require('@/assets/side3.jpg') },
    { novelId: 170, novelName: '昭娇', coverUrl: require('@/assets/side4.jpg') },
    { novelId: 170, novelName: '岁时来仪', coverUrl: require('@/assets/side5.jpg') }
]

const currentBanner = ref(2) // 默认显示第三个

let timer = null

function setBanner(idx) {
    currentBanner.value = idx
}

const goToNovel = async (novelId) => {
    try {
        const response = await getNovel(novelId)
        handleNovelClick(response)
    } catch (error) {
        console.error('处理失败:', error)
    }
}

const announcements = [
    { text: '[资讯] 书写抗战精神作品联展', link: 'https://mp.weixin.qq.com/s/4VeBev9GGxihH5MNVevfSg?mpshare=1&scene=1&srcid=0801zDxRpMpTTwRpb8djZYXr&sharer_shareinfo=467c009cbe86e604c7fb12947fa1170b&sharer_shareinfo_first=467c009cbe86e604c7fb12947fa1170b#wechat_redirect', type: 'news' },
    { text: '[公告] 《听说你喜欢我》原著', link: 'https://www.hongxiu.com/book/3756981504436501', type: 'notice' },
    { text: '[资讯] 25年绿书签行动来啦', link: 'https://mp.weixin.qq.com/s/c1G3OQ6-lWh5qwQ-sejJcg', type: 'news' },
    { text: '[公告] 25年作家福利已上线', link: 'https://write.qq.com/portal/college/editordetail?gender=2&typeid=75457244950928251&idx=75460605762836001', type: 'notice' },
    { text: '[公告] “风起国潮”二期征文', link: 'https://write.qq.com/portal/dashboard/actarticleDetail?id=665', type: 'notice' },
    { text: '[公告] 红袖大神段寻新书来袭', link: 'https://www.hongxiu.com/book/32553967803686009', type: 'notice' }
]

onMounted(async () => {
    startAutoPlay()
    startNovelAutoPlay()
    fetchAuthors()
    fetchNovels()
    scrollToTop()
    await fetchHistoryNovels()
    fetchRecentUpdates()
    timer = setInterval(() => {
        currentBanner.value = (currentBanner.value + 1) % carouselNovels.length
    }, 3500)
})

onUnmounted(() => {
    pauseAutoPlay()
    stopNovelAutoPlay()
    clearInterval(timer)
})
watch(novelCurrent, startNovelAutoPlay)
</script>

<style scoped>
.novel-home {
    width: 100%;
    margin: 0 auto;
}

.carousel-container {
    width: 100%;
    overflow: hidden;
    position: relative;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.carousel {
    position: relative;
    width: 100%;
}

.carousel-inner {
    display: flex;
    transition: transform 0.5s ease;
    height: 100%;
}

.carousel-item {
    min-width: 100%;
    position: relative;
}

.carousel-image {
    width: 100%;
    height: 100%;
    object-fit: cover;
}

.carousel-caption {
    position: absolute;
    bottom: 40px;
    left: 40px;
    color: white;
    text-shadow: 1px 1px 3px rgba(0, 0, 0, 0.8);
    max-width: 50%;
}

.carousel-caption h3 {
    font-size: 2rem;
    margin-bottom: 10px;
}

.carousel-caption p {
    font-size: 1.2rem;
}

.carousel-control {
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    background: rgba(0, 0, 0, 0.5);
    color: white;
    border: none;
    font-size: 2rem;
    width: 40px;
    height: 40px;
    border-radius: 50%;
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 10;
    transition: background 0.3s;
}

.carousel-control:hover {
    background: rgba(0, 0, 0, 0.8);
}

.carousel-control.prev {
    left: 20px;
}

.carousel-control.next {
    right: 20px;
}

.carousel-indicators {
    position: absolute;
    bottom: 20px;
    left: 50%;
    transform: translateX(-50%);
    display: flex;
    gap: 10px;
}

.carousel-indicators button {
    width: 12px;
    height: 12px;
    border-radius: 50%;
    border: none;
    background-color: rgba(255, 255, 255, 0.5);
    cursor: pointer;
    transition: background-color 0.3s;
}

.carousel-indicators button.active {
    background-color: #ffcc00;
}

.authors-container {
    display: flex;
    justify-content: space-around;
    margin: 30px 0;
    flex-wrap: wrap;
}

.author-card {
    width: 300px;
    background-color: #fffdf3;
    border-radius: 8px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    padding: 20px;
    margin-bottom: 20px;
    text-align: center;
}

.author-avatar {
    width: 100px;
    height: 100px;
    border-radius: 50%;
    overflow: hidden;
    margin: 0 auto 15px;
    border: 3px solid #f0f0f0;
    cursor: pointer;
    transition: transform 0.3s ease;
}

.author-avatar:hover {
    transform: scale(1.10);
}

.avatar-img {
    width: 100%;
    height: 100%;
    object-fit: cover;
}

.author-name {
    font-size: 1.5rem;
    margin-bottom: 10px;
    color: #333;
    cursor: pointer;
    transition: color 0.3s ease;
}

.author-name:hover {
    transform: scale(1.10);
    color: #f0940a;
}

.author-join-date {
    font-size: 0.9rem;
    color: #666;
    margin-bottom: 10px;
}

.author-bio {
    font-size: 1rem;
    color: #444;
    margin-bottom: 15px;
    line-height: 1.4;
}

.card-bg-0 {
    background: linear-gradient(to top, #f4f1d3, #fcfbfa);
}

.card-bg-1 {
    background: linear-gradient(to top, #d9f7d7, #fafcfd);
}

.card-bg-2 {
    background: linear-gradient(to top, #dde5f4, #f9fafc);
}

.single-image-container {
    width: 100%;
    margin-top: 30px;
    text-align: center;
}

.single-image {
    max-width: 100%;
    height: auto;
    border-radius: 8px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.image-link {
    display: inline-block;
    text-decoration: none;
}

.image-link:hover {
    transform: scale(1.02);
    transition: transform 0.3s ease;
}

/* 编辑精选轮播样式*/
.novel-carousel-container {
    width: 100%;
    margin: 0px auto;
    padding: 20px 0;
    border-radius: 10px;
    box-sizing: border-box;
    display: flex;
    align-items: center;
    position: relative;
    overflow: hidden;
    transition: background 0.8s ease;
}

.novel-carousel-wrapper {
    flex: 1;
    overflow: hidden;
    padding: 0 79%;
    display: flex;
    justify-content: center;
}

.novel-carousel-track {
    display: flex;
    flex-direction: row;
    align-items: flex-end;
    gap: 25px;
    transition: transform 0.5s cubic-bezier(.4, 0, .2, 1);
    will-change: transform;
    justify-content: center;
}

.novel-carousel-item {
    width: 130px;
    min-width: 0;
    flex-shrink: 0;
    transition: all 0.4s cubic-bezier(.4, 0, .2, 1);
    transform: scale(0.85);
    z-index: 1;
    border-radius: 5px;
    position: relative;
    background: transparent;
    box-shadow: none;
}

.novel-carousel-item.active {
    transform: scale(1.10);
    opacity: 1;
    filter: none;
    z-index: 2;
    box-shadow: 0 8px 36px 0 rgba(100, 120, 140, 0.12);
}

.novel-carousel-item .novel-cover {
    width: 100%;
    object-fit: cover;
    border-radius: 5px;
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.08);
    background: #fff;
    transition: box-shadow .3s;
}

.novel-carousel-item.active .novel-cover {
    box-shadow: 0 8px 36px 0 rgba(100, 120, 140, 0.12);
    border: 2px solid #fff;
}

.novel-title {
    margin-top: 12px;
    font-size: 1.00rem;
    color: #222;
    text-align: center;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    letter-spacing: 1px;
    font-weight: 500;
}

.novel-intro {
    position: absolute;
    height: 120px;
    left: 0;
    right: 0;
    bottom: 75px;
    background: linear-gradient(to top, rgb(14, 13, 13), rgba(121, 119, 119, 0.4));
    color: #fff;
    font-size: 0.90rem;
    padding: 10px 8px 8px 8px;
    border-radius: 0 0 8px 8px;
    z-index: 9;
    text-align: center;
    white-space: pre-line;
    pointer-events: none;
    transition: opacity .4s;
}

.novel-carousel-item .novel-intro {
    opacity: 1;
}

.novel-carousel-control {
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    width: 36px;
    height: 36px;
    background: rgba(40, 40, 40, 0.18);
    border: none;
    border-radius: 50%;
    font-size: 1.5rem;
    color: #444;
    cursor: pointer;
    z-index: 20;
    display: flex;
    align-items: center;
    justify-content: center;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.07);
    transition: background 0.3s, color 0.2s;
}

.novel-carousel-control:hover {
    background: rgba(70, 70, 70, 0.28);
    color: #111;
}

.novel-carousel-control.prev {
    left: 8px;
}

.novel-carousel-control.next {
    right: 8px;
}

.fade-enter-active,
.fade-leave-active {
    transition: opacity 0.4s;
}

.fade-enter,
.fade-leave-to {
    opacity: 0;
}

.divider {
    margin-top: 40px;
    margin-bottom: 20px;
    display: flex;
    align-items: center;
    justify-content: center;
    color: #333;
    font-size: 24px;
}

.divider::before,
.divider::after {
    content: '';
    flex: 1;
    height: 1px;
    background-color: #e5e5e5;
}

.divider span {
    padding: 0 15px;
}

.main-banner-section {
    display: flex;
    width: 90%;
    margin-top: 24px;
    height: 320px;
    margin: 0 auto;
}

.banner-carousel {
    flex: 2;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: flex-start;
    position: relative;
}

.carousel-imgs {
    position: relative;
    width: 100%;
    height: 260px;
    overflow: hidden;
}

.carousel-img-item {
    position: absolute;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    opacity: 0;
    transition: opacity .5s;
    cursor: pointer;
}

.carousel-img-item.active {
    opacity: 1;
    z-index: 2;
}

.banner-img {
    width: 100%;
    height: 260px;
    object-fit: cover;
    border-radius: 8px;
}

.carousel-titles {
    display: flex;
    width: 100%;
    margin-top: 0;
}

.carousel-title-item {
    flex: 1;
    background: rgba(40, 40, 40, 0.5);
    color: #fff;
    font-size: 20px;
    text-align: center;
    padding: 12px 0;
    cursor: pointer;
    transition: background .2s, color .2s;
    border-right: 1px solid #fff;
}

.carousel-title-item:last-child {
    border-right: none;
}

.carousel-title-item.active {
    background: #ff4d4f;
    color: #fff;
}

.banner-announcement {
    flex: 1;
    background: #fff;
    margin-left: 32px;
    padding: 5px 16px;
    border-radius: 8px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
    min-width: 320px;
    max-width: 340px;
}

.announcement-title {
    font-size: 26px;
    font-weight: bold;
    margin-bottom: 5px;
    display: flex;
    align-items: center;
    gap: 4px;
}

.announcement-list {
    list-style: none;
    padding: 0;
    margin: 0;
}

.announcement-list li {
    margin-bottom: 12px;
}

.announcement-list a {
    color: #ff4d4f;
    font-size: 18px;
    text-decoration: none;
    transition: color .2s;
}

.announcement-list a:hover {
    color: #e47f0d;
}

.announcement-list a.notice {
    color: #222;
}

.announcement-list a.news {
    color: #ff4d4f;
}

@media (max-width: 800px) {
    .novel-carousel-container {
        min-height: 270px;
        padding: 12px 0;
    }

    .novel-carousel-item,
    .novel-carousel-item.active {
        width: 90px;
    }

    .novel-carousel-item .novel-cover,
    .novel-carousel-item.active .novel-cover {
        height: 110px;
    }

    .novel-intro {
        font-size: 0.8rem;
        bottom: 8px;
        padding: 6px 4px 4px 4px;
    }
}

.zh-footer {
    background: #f7f8fa;
    margin-top: 64px;
    padding: 36px 0 0 0;
    font-size: 16px;
    color: #333;
    border-top: 1px solid #ebebeb;
}

.zh-footer-partner {
    max-width: 1400px;
    margin: 0 auto;
    padding: 0 40px 18px 40px;
}

.zh-footer-title {
    font-weight: bold;
    font-size: 22px;
    margin-bottom: 16px;
}

.zh-footer-partner-links {
    font-size: 16px;
    color: #666;
    line-height: 2;
    word-break: break-all;
}

.zh-footer-main {
    display: flex;
    max-width: 1400px;
    margin: 0 auto;
    padding: 28px 40px 0 40px;
    justify-content: space-between;
    gap: 48px;
}

.zh-footer-block {
    flex: 1;
    min-width: 240px;
}

.zh-footer-block-title {
    font-weight: bold;
    font-size: 20px;
    margin-bottom: 14px;
}

.zh-footer-block-row {
    color: #5c5c5c;
    line-height: 2.1;
    font-size: 15px;
    display: flex;
    flex-wrap: wrap;
    gap: 12px 20px;
}

.zh-footer-logo {
    margin-top: 12px;
    width: 64px;
    height: 64px;
    display: block;
}

.zh-footer-links {
    text-align: center;
    margin: 24px 0 8px 0;
    color: #888;
    font-size: 16px;
    letter-spacing: 0.5px;
    word-break: break-all;
}

.zh-footer-divider {
    border: none;
    border-top: 1px solid #e2e2e2;
    margin: 10px 0;
    width: 96%;
    margin-left: 2%;
}

.zh-footer-copy {
    text-align: center;
    color: #aaa;
    font-size: 14px;
    line-height: 2;
}

.zh-footer-badges {
    display: flex;
    justify-content: center;
    gap: 36px;
    margin: 18px 0 0 0;
}

.zh-footer-badges img {
    height: 48px;
    background: transparent;
}

@media (max-width: 1200px) {
    .zh-footer-main {
        flex-direction: column;
        gap: 24px;
        padding: 20px 10px 0 10px;
    }

    .zh-footer-partner,
    .zh-footer-main {
        padding-left: 10px;
        padding-right: 10px;
    }
}

.history-novels-container {
    padding: 20px;
    background-color: #fff;
}

.novel-list {
    display: flex;
    flex-wrap: wrap;
    gap: 20px;
    list-style: none;
    padding: 0;
    margin: 0;
}

.novel-item {
    width: 180px;
    background: #f9f9f9;
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
    transition: transform 0.2s ease;
    cursor: pointer;
}

.novel-item:hover {
    transform: translateY(-4px);
}

.cover-wrapper {
    width: 100%;
    height: 240px;
    overflow: hidden;
}

.novel-cover1 {
    width: 100%;
    height: 100%;
    object-fit: cover;
    display: block;
}

.novel-cover1:hover {
    transform: scale(1.05);
    transition: transform 0.3s ease;
}

.novel-info {
    padding: 10px;
    text-align: left;
}

.novel-title1 {
    font-size: 16px;
    font-weight: bold;
    margin: 0;
    color: #333;
}

.novel-title1:hover {
    color: #f0940a;
    cursor: pointer;
}

.novel-author1 {
    font-size: 15px;
    color: #777;
    margin-top: 6px;
}

.novel-author1:hover {
    color: #f0940a;
    cursor: pointer;
}

.novel-category {
    font-size: 12px;
    color: #aaa;
    margin-top: 4px;
}

.intro-text {
    margin: 10px 0 16px;
    font-size: 18px;
    line-height: 1.6;
    color: #444;
    background: #f9f9f9;
    padding: 10px 12px;
    border-left: 4px solid #c39762;
    border-radius: 4px;
}

.recent-update-table {
    width: 100%;
    border-collapse: collapse;
    font-size: 14px;
    /* 和全页面匹配的字体大小 */
}

.recent-update-table th,
.recent-update-table td {
    padding: 8px 12px;
    text-align: left;
    border-bottom: 1px solid #eaeaea;
}

.recent-update-table th {
    background-color: #f5f5f5;
    /* 表头淡灰色背景 */
    font-weight: bold;
}

.recent-update-table tr:hover {
    background-color: #fafafa;
    /* 鼠标悬停行高亮 */
}
</style>