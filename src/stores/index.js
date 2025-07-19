import { defineStore } from 'pinia'

export const current_state = defineStore('state', {
    state: () => ({
        value: 0,   //0:读者, 1:作者, 2:管理员
        roles: ['读者', '作者', '管理员'],
        name: '',  //名称
        id: 0,   //ID
        isloggedin: false,  //是否已经登录
    }),
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


export const readerState = defineStore('reader', {
    state: () => ({
        //读者10个属性
        readerId: 0,
        readerName: "",
        password: "",
        phone: null,
        gender: null,
        balance: 0,
        avatarUrl: "",      //头像不用这个，用下面的formattedAvatarUrl,加了前缀
        backgroundUrl: null,
        isCollectVisible: null,
        isRecommendVisible: null,
        //扩展属性
        lastLoginTime: null, // 最近登录时间
        isloggedin: false,    // 是否已登录，貌似没用
        readHistory: [],      // 阅读历史，比如写主页时放历史阅读记录书籍的id(或整个对象)，随你
        favoriteBooks: [],    // 收藏书籍
        //你还想记录什么属性可以加
    }),
    getters: {
        formattedAvatarUrl: (state) => {
            const prefix = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/';  //前缀
            if (state.avatarUrl) {
                return prefix + state.avatarUrl;
            }
            return prefix + 'e165315c-da2b-42c9-b3cf-c0457d168634.jpg';  // 默认头像
        },
        isLoggedIn: (state) => state.readerId > 0,
        hasPhone: (state) => !!state.phone,
        hasGender: (state) => !!state.gender,
        formattedBalance: (state) => `¥${state.balance.toLocaleString()}`,
        // 获取最近阅读的5本书
        recentBooks: (state) => state.readHistory.slice(0, 5),
        // 获取收藏数量
        favoriteCount: (state) => state.favoriteBooks.length
    },
    actions: {
        // 登录时初始化读者信息
        initializeReader(id, name, avatarUrl) {
            this.readerId = id || 0;
            this.readerName = name || "";
            this.avatarUrl = avatarUrl || "";
            this.isloggedin = true;
            this.lastLoginTime = new Date().toISOString(); // 设置最近登录时间
        },
        // initializeReader(id, name, password, phone, gender, balance, avatarUrl, backgroundUrl, isCollectVisible, isRecommendVisible) {
        //     this.readerId = id || 0;
        //     this.readerName = name || "";
        //     this.password = password || "";
        //     this.phone = phone || null
        //     this.gender = gender || null;
        //     this.balance = balance || 0;
        //     this.avatarUrl = avatarUrl || "";
        //     this.backgroundUrl = backgroundUrl || null;
        //     this.isCollectVisible = isCollectVisible || null;
        //     this.isRecommendVisible = isRecommendVisible || null;
        //     this.isloggedin = true;
        //     this.lastLoginTime = new Date().toISOString(); // 设置最近登录时间
        // },

        // 登出时重置读者信息
        resetReaderInfo() {
            this.readerId = 0;
            this.readerName = "";
            this.password = "";
            this.phone = null;
            this.gender = null;
            this.balance = 0;
            this.avatarUrl = "";
            this.backgroundUrl = null;
            this.isCollectVisible = null;
            this.isRecommendVisible = null;
            this.lastLoginTime = null;
            this.isloggedin = false;
            this.readHistory = [];
            this.favoriteBooks = [];
        }
    }
})