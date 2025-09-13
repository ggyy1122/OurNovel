import { createRouter, createWebHistory } from 'vue-router'
import Admin_Layout from '@/Admin/Admin_Layout.vue'
import Dash_board from '@/Admin/Dash_Board.vue'
import Users_Board from '@/Admin/Users_Board.vue'
import Chapter_Board from '@/Admin/Chapter_Board.vue'
import ToBeReviewed_Novels from '@/Admin/ToBeReviewed_Novels.vue'
import Serial_Novels from '@/Admin/Serial_Novels.vue'
import Finish_Novels from '@/Admin/Finish_Novels.vue'
import Ban_Novels from '@/Admin/Ban_Novels.vue'
import NovelDetailView from '@/Admin/NovelDetailView.vue'
import Complaints_processed from '@/Admin/Report_processed.vue'
import Complaints_unprocessed from '@/Admin/Report_unprocessed.vue'
import Comment_detail from '@/Admin/Comment_detail.vue'
import NovelCreate from '@/Admin/Novel_Create.vue'
import CategoryManage from '@/Admin/Category_Manage.vue'

import Home from '@/views/Home_test.vue'
import Novel_Layout from '@/Novels/Novel_Layout.vue'
import Novel_Home from '@/Novels/Novel_Home.vue'
import Novel_Category from '@/Novels/Novel_Category.vue'
import Novel_Search from '@/Novels/Novel_Search.vue'
import Novel_Rank from '@/Novels/Novel_Rank.vue'
import LoginForm from '@/Login_Register/LoginForm.vue'
import RegisterForm from '@/Login_Register/RegisterForm.vue'
import L_R from '@/Login_Register/L_R.vue'
import Novel_Reader from '@/Novels/Novel_Reader.vue'
import ResetForm from '@/Login_Register/ResetForm.vue'
import Novel_Info from '@/Novels/Novel_Info.vue'
import Novel_Info_home from '@/Novels/Novel_Info_home.vue'
import Novel_Info_Comment from '@/Novels/Novel_Info_Comment.vue'
import Novel_Info_Chapter from '@/Novels/Novel_Info_Chapter.vue'
import AuthorHome from '@/Novels/AuthorHome.vue'
import Novel_Recharge from '@/Novels/Novel_Recharge.vue'
import ReaderHome from '@/Novels/ReaderHome.vue'
import ChapterComments from '@/Novels/ChapterComments.vue'
import Comment_Report from '@/Novels/Comment_Report.vue'
//作者
import AuthorLayout from '@/Author/AuthorLayout.vue'
import NovelList from '@/Author/NovelList.vue'
import CreateNovel from '@/Author/CreateNovel.vue'
import NovelDetail from '@/Author/NovelDetail.vue'
import NovelEdit from '@/Author/NovelEdit.vue'
import ChapterList from '@/Author/ChapterList.vue'
import AuthorStats from '@/Author/AuthorStats.vue'
import AuthorIncome from '@/Author/AuthorIncome.vue'
import AuthorSettings from '@/Author/AuthorSettings.vue'
import RatingList from '@/Author/RatingList.vue'
import RecomendList from '@/Author/RecomendList.vue'
import CollectList from '@/Author/CollectList.vue'
import CommentList from '@/Author/CommentList.vue'
//主页
import MainLayout from '@/Reader/MainLayout.vue'
import HomeView from '@/Reader/HomeView.vue'
import FinanceView from '@/Reader/FinanceView.vue'
import BookshelfView from '@/Reader/BookShelf.vue'
import CommentView from '@/Reader/CommentView.vue'
import MessageCenterView from '@/Reader/MessageCenterView.vue'
import RecommendView from '@/Reader/RecommendView.vue'
import CollectView from '@/Reader/CollectView.vue'
import HistoryView from '@/Reader/HistoryView.vue'
import InformationView from '@/Reader/InformationView.vue'

//充值界面


const routes = [
    {
        path: '/',
        name: 'Home',
        component: Home
    },
    {
        path: '/L_R',
        component: L_R,
        meta: { requiresAuth: true },
        children: [
            {
                path: 'login',
                name: 'Login',
                component: LoginForm
            },
            {
                path: 'register',
                name: 'Register',
                component: RegisterForm
            },
            {
                path: 'reset',
                name: 'Reset',
                component: ResetForm
            }
        ]
    },
    {
        path: '/Admin/Admin_Layout',
        component: Admin_Layout,
        meta: { requiresAuth: true },
        children: [
            {
                path: 'dashboard',
                component: Dash_board
            },
            {
                path: 'users',
                component: Users_Board
            },
            {
                path: 'chapterboard',
                name: 'ChapterBoard',
                component: Chapter_Board
            },
            {
                path: 'chapter/:novel_id/:chapter_id',
                name: 'ChapterContent',
                component: () => import('@/Admin/Chapter_Content.vue'),
                props: true
            },
            // 其他管理路由可以在这里添加
            {
                path: 'novel_managent',
                name: 'novel_managent',// 路由的唯一标识
                children: [
                    { path: 'ToBeReviewed_Novels', name: 'ToBeReviewed_Novels', component: ToBeReviewed_Novels },
                    { path: 'Serial_Novels', name: 'Serial_Novels', component: Serial_Novels },
                    { path: 'Finish_Novels', name: 'Finish_Novels', component: Finish_Novels },
                    { path: 'Ban_Novels', name: 'Ban_Novels', component: Ban_Novels },
                    { path: 'novelInfo', name: 'novel-detail', component: NovelDetailView },
                    { path: 'novel_create', name: 'novel_create', component: NovelCreate }
                ]
            },
            {
                path: 'complaint_management',
                name: 'complaint_management',
                children: [
                    { path: 'processed', name: 'processed', component: Complaints_processed },
                    { path: 'unprocessed', name: 'unprocessed', component: Complaints_unprocessed }
                ]
            },
            {
                path: 'comment_detail',
                name: 'comment_detail',
                component: Comment_detail
            },
            {
                path: 'category_manage',
                name: 'category_manage',
                component: CategoryManage
            }
        ]
    },
    {
        path: '/Novels/Novel_Layout',
        component: Novel_Layout,
        meta: { requiresAuth: true },
        children: [
            {
                path: 'home',
                name: 'Novel_Home',
                component: Novel_Home
            },
            {
                path: 'category',
                name: 'Novel_Category',
                component: Novel_Category
            },
            {
                path: 'rank',
                name: 'Novel_Rank',
                component: Novel_Rank
            }
        ]
    },
    {
        path: '/author/:authorId',
        name: 'AuthorHome',
        component: AuthorHome,
        props: true
    },
    {
        path: '/reader/:readerId',
        name: 'ReaderHome',
        component: ReaderHome,
        props: true
    },
    {
        path: '/Novels/Search',
        name: 'Search',
        component: Novel_Search,
        props: (route) => ({
            query: route.query.q,
            filter: route.query.type || 'novel'
        })
    },
    {
        path: '/chapter-comments/:novelId/:chapterId',
        name: 'ChapterComments',
        component: ChapterComments,
        props: true
    },
    {
        path: '/comment-report/:readerId/:commentId',
        name: 'CommentReport',
        component: Comment_Report,
        props: true
    },
    {
        path: '/Novels/Novel_Recharge',
        name: 'Novel_Recharge',
        component: Novel_Recharge
    },
    {
        path: '/Novels/reader',
        name: 'NovelReader',
        component: Novel_Reader
    },
    {
        path: '/Novels/Novel_Info',
        component: Novel_Info,
        meta: { requiresAuth: true },
        children: [
            {
                path: 'home',
                name: 'Novel_Info_home',
                component: Novel_Info_home
            },
            {
                path: 'chapter',
                name: 'Novel_Info_chapter',
                component: Novel_Info_Chapter
            },
            {
                path: 'comment',
                name: 'Novel_Info_comment',
                component: Novel_Info_Comment
            }
        ]
    },
    // 作者路由组
    {
        path: '/author',
        component: AuthorLayout,
        redirect: '/author/novels',
        meta: { requiresAuth: true },
        children: [
            {
                path: 'novels',
                name: 'AuthorNovelList',
                component: NovelList,
                meta: { title: '我的小说' }
            },
            {
                path: 'novels/create',
                name: 'CreateNovel',
                component: CreateNovel,
                meta: { title: '创建新小说' }
            },
            {
                path: 'novels/:id',
                name: 'NovelDetail',
                component: NovelDetail,
                props: true,
                meta: { title: '小说详情' }
            },
            {
                path: 'novels/:id/edit',
                name: 'NovelEdit',
                component: NovelEdit,
                props: true,
                meta: { title: '修改小说' }
            },
            {
                path: 'novels/:id/chapters',
                name: 'ChapterList',
                component: ChapterList,
                props: true,
                meta: { title: '章节列表' }
            },
            {
                path: 'novels/:id/ratinglist',
                name: 'RatingList',
                component: RatingList,
                props: true,
                meta: { title: '评分列表' }
            },
            {
                path: 'novels/:id/recomendlist',
                name: 'RecomendList',
                component: RecomendList,
                props: true,
                meta: { title: '推荐列表' }
            },
            {
                path: 'novels/:id/collectlist',
                name: 'CollectList',
                component: CollectList,
                props: true,
                meta: { title: '收藏列表' }
            },
            {
                path: 'novels/:id/commentlist',
                name: 'CommentList',
                component: CommentList,
                props: true,
                meta: { title: '评论列表' }
            },
            {
                path: 'stats',
                name: 'AuthorStats',
                component: AuthorStats,
                meta: { title: '数据统计' }
            },
            {
                path: 'income',
                name: 'AuthorIncome',
                component: AuthorIncome,
                meta: { title: '作者收益' }
            },
            {
                path: 'settings',
                name: 'AuthorSettings',
                component: AuthorSettings,
                meta: { title: '账号设置' }
            },
        ]
    },
    // 首页路由组
    {
        path: '/UserHome',
        component: MainLayout,
        props: { sidebarType: 'home' },
        children: [
            {
                path: '',
                name: 'home',
                component: HomeView,
                meta: { sidebarItem: 'home' }
            },
            {
                path: 'finance',
                name: 'home-finance',
                component: FinanceView,
                meta: { sidebarItem: 'finance' }
            },
            {
                path: 'comment',
                name: 'home-comment',
                component: CommentView,
                meta: { sidebarItem: 'comment' }
            },
            {
                path: 'self-information',
                name: 'home-self-information',
                component: InformationView,
                meta: { sidebarItem: 'self-information' }
            },
        ]
    },
    // 书架路由组
    {
        path: '/BookShelf',
        component: MainLayout,
        props: { sidebarType: 'bookshelf' },
        children: [
            {
                path: '',
                name: 'bookshelf',
                component: BookshelfView,
                meta: { sidebarItem: 'books' }
            },
            {
                path: 'Recommend',
                name: 'recommend',
                component: RecommendView,
                meta: { sidebarItem: 'recommend' }
            },
            {
                path: 'Collect',
                name: 'collect',
                component: CollectView,
                meta: { sidebarItem: 'collect' }
            },
            {
                path: 'History',
                name: 'history',
                component: HistoryView,
                meta: { sidebarItem: 'history' }
            }
        ]
    },
    // 消息中心路由组
    {
        path: '/MessageCenter',
        component: MainLayout,
        props: { sidebarType: 'messagecenter' },
        children: [
            {
                path: '',
                name: 'messagecenter',
                component: MessageCenterView,
                meta: { sidebarItem: 'messagecenter' }
            }
        ]
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router