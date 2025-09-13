import { defineStore } from 'pinia'

export const current_state = defineStore('state', {
    state: () => ({
        value: 0,   //0:读者, 1:作者, 2:管理员
        roles: ['读者', '作者', '管理员'],
        name: '',  //名称
        id: 0,   //ID
        isloggedin: false,  //是否已经登录
    }),
    persist: true,  //持久化存储
    getters: {
        Role: (state) => (num) => state.roles[(state.value + num) % 3]
    },
    actions: {
        reset(num) {
            this.value = num
        },
        add(num) {
            this.value = (this.value + num) % 3
        },
        setUserInfo(name, id) {   //登录，设置用户信息
            this.name = name;
            this.id = id;
            this.isloggedin = true;
        },
        clearUserInfo() {         //登出，清除用户信息
            this.name = '';
            this.id = '';
            this.isloggedin = false;
        }
    }
})

//作者信息
export const authorState = defineStore('author', {
    state: () => ({
        id: 0
    }),
    persist: true,  //持久化存储
    actions: {
        setAuthorId(id) {
            this.id = id || 0;
        }
    }
})

//管理员信息
export const managerState = defineStore('manager', {
    state: () => ({
        id: 0
    }),
    persist: true,  //持久化存储
    actions: {
        setManagerId(id) {
            this.id = id || 0;
        }
    }
})

export const readerState = defineStore('reader', {
    state: () => ({
        //读者10个属性
        readerId: 0,
        readerName: "",
        password: "",
        phone: "",
        gender: "",
        balance: 0,
        avatarUrl: "",      //头像不用这个，用下面的formattedAvatarUrl,加了前缀
        backgroundUrl: "",  //背景图不用这个，用下面的formattedBackgroundUrl,加了前缀
        isCollectVisible: '是',
        isRecommendVisible: '是',
        //扩展属性
        lastLoginTime: null, // 最近登录时间
        isloggedin: false,    // 是否已登录，貌似没用
        readHistory: [],      // 阅读历史，比如写主页时放历史阅读记录书籍的id(或整个对象)，随你
        favoriteBooks: [],    // 收藏书籍
        recommendBooks: [],    // 推荐书籍
        //你还想记录什么属性可以加
        true_password: ""  // 真实密码，修改密码时用于验证旧密码
    }),
    persist: true,  //持久化存储
    getters: {
        formattedAvatarUrl: (state) => {
            const prefix = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/';  //前缀
            if (state.avatarUrl) {
                return prefix + state.avatarUrl;
            }
            return prefix + 'e165315c-da2b-42c9-b3cf-c0457d168634.jpg';  // 默认头像
        },
        formattedBackgroundUrl: (state) => {
            const prefix = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/';  //前缀
            if (state.backgroundUrl) {
                return prefix + state.backgroundUrl;
            }
            return prefix + '415116fc-fb23-48f8-96c9-12a99de76e7d.jpg';  // 默认背景图
        },
        recommendBooksCount: (state) => state.recommendBooks.length,
        favoriteBooksCount: (state) => state.favoriteBooks.length,
        readHistoryCount: (state) => state.readHistory.length,
    },
    actions: {
        initializeReader(id, name, password, phone, gender, balance, avatarUrl, backgroundUrl, isCollectVisible, isRecommendVisible, favoriteBooks, recommendBooks, readHistory, true_password) {
            this.readerId = id || 0;
            this.readerName = name || "";
            this.password = password || "";
            this.phone = phone || "";
            this.gender = gender || "";
            this.balance = balance || 0;
            this.avatarUrl = avatarUrl || "";
            this.backgroundUrl = backgroundUrl || "";
            this.isCollectVisible = isCollectVisible || '是';
            this.isRecommendVisible = isRecommendVisible || '是';

            this.favoriteBooks = favoriteBooks || []; // 初始化收藏书籍
            this.recommendBooks = recommendBooks || []; // 初始化推荐书籍
            this.readHistory = readHistory || []; // 初始化阅读历史
            this.true_password = true_password || "";
            this.isloggedin = true;
            this.lastLoginTime = new Date().toISOString(); // 设置最近登录时间
        },
        // 登出时重置读者信息
        resetReaderInfo() {
            this.readerId = 0;
            this.readerName = "";
            this.password = "";
            this.phone = "";
            this.gender = "";
            this.balance = 0;
            this.avatarUrl = "";
            this.backgroundUrl = "";
            this.isCollectVisible = null;
            this.isRecommendVisible = null;
            this.lastLoginTime = null;
            this.isloggedin = false;
            this.readHistory = [];
            this.favoriteBooks = [];
            this.recommendBooks = [];
            this.true_password = "";
        },
        isFavorite(novelId) {
            return this.favoriteBooks.some(item =>
                item.novelId === novelId ||
                (item.novel && item.novel.novelId === novelId)
            );
        },
        // 修改余额时同步到其他标签页
        updateBalance(newBalance) {
            this.balance = newBalance
            localStorage.setItem('reader_balance', newBalance) // 显式存储
            window.dispatchEvent(new Event('storage')) // 触发事件
        },
        //同步推荐数组
        updateRecommendBooks(newBooks) {
            this.recommendBooks = newBooks
            localStorage.setItem('reader_recommendBooks', JSON.stringify(newBooks))
            window.dispatchEvent(new Event('storage')) // 触发同步
        },
        //同步收藏数组
        updateFavoriteBooks(newBooks) {
            this.favoriteBooks = newBooks
            localStorage.setItem('reader_favoriteBooks', JSON.stringify(newBooks))
            window.dispatchEvent(new Event('storage')) // 触发同步
        },
        // 同步历史记录数组
        updateReadHistory(newHistory) {
            this.readHistory = newHistory
            localStorage.setItem('reader_readHistory', JSON.stringify(newHistory))
            window.dispatchEvent(new Event('storage')) // 触发同步事件
        },
        // 初始化时添加事件监听
        initializeStore() {
            window.addEventListener('storage', (event) => {
                if (event.key === 'reader_balance') {
                    this.balance = Number(event.newValue) // 从其他标签页同步
                }
                if (event.key === 'reader_recommendBooks') {
                    this.recommendBooks = JSON.parse(event.newValue) // 同步推荐书籍
                }
                if (event.key === 'reader_favoriteBooks') {
                    this.favoriteBooks = JSON.parse(event.newValue) // 同步推荐书籍
                }
                if (event.key === 'reader_readHistory') {
                    this.readHistory = JSON.parse(event.newValue) || []
                }
            })
        }
    }
})


export const SelectNovel_State = defineStore('select_novel', {
    state: () => ({
        novelId: 0,
        authorId: 0,    // 作者ID
        novelName: "",
        introduction: "",
        createTime: "",
        coverUrl: "",    //封面不用这个，用下面的formattedcoverUrl,加了前缀
        score: 0,
        totalWordCount: 0,
        recommendCount: 0,
        collectedCount: 0,
        status: "",

        totalPrice: 0,

        selectedComment: null, // 当前选中的评论

        authorName: "",      // 作者名称
        authorPhone: "",     // 作者电话
        authorAvatarUrl: "", // 作者头像URL,头像不用这个，用下面的formattedauthorAvatarUrl,加了前缀
        registerTime: "", // 作者注册时间
        a_introduction: "", // 作者简介

        //章节
        chapterId: 1,
        cha_title: "",
        cha_content: "",
        cha_wordCount: 0,
        cha_pricePerKilo: 0,
        cha_calculatedPrice: 0,
        cha_isCharged: "",
        cha_publishTime: "",
        cha_status: ""
    }),
    persist: true,  //持久化存储
    getters: {
        formattedcoverUrl: (state) => {
            const prefix = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/';  //前缀
            if (state.coverUrl) {
                return prefix + state.coverUrl;
            }
            return prefix + 'e165315c-da2b-42c9-b3cf-c0457d168634.jpg';  // 默认头像
        },
        formattedauthorAvatarUrl: (state) => {
            const prefix = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/';  //前缀
            if (state.authorAvatarUrl) {
                return prefix + state.authorAvatarUrl;
            }
            return prefix + 'e165315c-da2b-42c9-b3cf-c0457d168634.jpg';  // 默认头像
        }
    },
    actions: {
        resetNovel(id, authorId, name, introduction, createTime, coverUrl, score, totalWordCount, recommendCount, collectedCount, status, totalPrice, authorName, authorPhone, authorAvatarUrl, registerTime, a_introduction) {
            this.novelId = id || 0;
            this.authorId = authorId || 0;
            this.novelName = name || "";
            this.introduction = introduction || "";
            this.createTime = createTime || "";
            this.coverUrl = coverUrl || "";
            this.score = score || 0;
            this.totalWordCount = totalWordCount || 0;
            this.recommendCount = recommendCount || 0;
            this.collectedCount = collectedCount || 0;
            this.status = status || "";

            this.totalPrice = totalPrice || 0;

            this.authorName = authorName || "";
            this.authorPhone = authorPhone || "";
            this.authorAvatarUrl = authorAvatarUrl || "";
            this.registerTime = registerTime || "";
            this.a_introduction = a_introduction || "";

            this.selectedComment = null;
        },
        resetChapter(chapterId, cha_title, cha_content, cha_wordCount, cha_pricePerKilo, cha_calculatedPrice, cha_isCharged, cha_publishTime, cha_status) {
            this.chapterId = chapterId || 1;
            this.cha_title = cha_title || "";
            this.cha_content = cha_content || "";
            this.cha_wordCount = cha_wordCount || 0;
            this.cha_pricePerKilo = cha_pricePerKilo || 0;
            this.cha_calculatedPrice = cha_calculatedPrice || 0;
            this.cha_isCharged = cha_isCharged || "";
            this.cha_publishTime = cha_publishTime || "";
            this.cha_status = cha_status || "";
        }
    }
});