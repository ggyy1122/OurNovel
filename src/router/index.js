import { createRouter, createWebHistory } from 'vue-router'
import Admin_Layout from '@/Admin/Admin_Layout.vue'
import Dash_board from '@/Admin/Dash_Board.vue'
import Novels_Board from '@/Admin/Novels_Board.vue'
import Home from '@/views/Home_test.vue'
import Novel_Layout from '@/Novels/Novel_Layout.vue'
import Novel_Home from '@/Novels/Novel_Home.vue'
import Novel_Category from '@/Novels/Novel_Category.vue'
import Novel_Rank from '@/Novels/Novel_Rank.vue'
import LoginForm from '@/Login_Register/LoginForm.vue'
import RegisterForm from '@/Login_Register/RegisterForm.vue'
import L_R from '@/Login_Register/L_R.vue'
import Novel_Reader from '@/Novels/Novel_Reader.vue'
import Author_api_test from '@/API_Test/Author_Test.vue'
import Comment_api_test from '@/API_Test/Comment_Test.vue'
import ResetForm from '@/Login_Register/ResetForm.vue'
import Novel_api_test from '@/API_Test/Novel_Test.vue'
import Collect_api_test from '@/API_Test/Collect_Test.vue'
import Rate_api_test from '@/API_Test/Rate_Test.vue'
import Category_api_test from '@/API_Test/Category_Test.vue'
import NovelCategory_api_test from '@/API_Test/NovelCategory_Test.vue'
import Chapter_api_test from '@/API_Test/Chapter_Test.vue'
import Reader_api_test from '@/API_Test/Reader_Test.vue'
import Reader_imfromation from '@/Novels/Novel_ReaderInfomation.vue'
import Novel_Info from '@/Novels/Novel_Info.vue'
import Novel_Info_home from '@/Novels/Novel_Info_home.vue'
import Novel_Info_Comment from '@/Novels/Novel_Info_Comment.vue'
import Novel_Info_Chapter from '@/Novels/Novel_Info_Chapter.vue'
//作者
import AuthorLayout from '@/Author/AuthorLayout.vue'
import NovelList from '@/Author/NovelList.vue'
import CreateNovel from '@/Author/CreateNovel.vue'
import NovelDetail from '@/Author/NovelDetail.vue'
import NovelEdit from '@/Author/NovelEdit.vue'
import ChapterList from '@/Author/ChapterList.vue'
import AuthorStats from '@/Author/AuthorStats.vue'
import AuthorSettings from '@/Author/AuthorSettings.vue'


const routes = [
    {
        path: '/',
        name: 'Home',
        component: Home
    },
    {
        path: '/Comment_api_test',
        name: 'Comment_api_test',
        component: Comment_api_test
    },
    {
        path: '/Author_api_test',
        name: 'Author_api_test',
        component: Author_api_test
    },
    {
        path: '/Novel_api_test',
        name: 'Novel_api_test',
        component: Novel_api_test
    },
    {
        path: '/Collect_api_test',
        name: 'Collect_api_test',
        component: Collect_api_test
    },
    {
        path: '/Rate_api_test',
        name: 'Rate_api_test',
        component: Rate_api_test
    },
    {
        path: '/Category_api_test',
        name: 'Category_api_test',
        component: Category_api_test
    },
    {
        path: '/NovelCategory_api_test',
        name: 'NovelCategory_api_test',
        component: NovelCategory_api_test
    },
    {
        path: '/Chapter_api_test',
        name: 'Chapter_api_test',
        component: Chapter_api_test
    },
    {
        path: '/Reader_api_test',
        name: 'Reader_api_test',
        component: Reader_api_test

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
                path: 'novelsboard',
                component: Novels_Board
            }
            // 其他管理路由可以在这里添加
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
            },
            {
                path: 'reader',
                name: 'NovelReader',
                component: Novel_Reader,
                meta: { hideNav: true },
                props: (route) => ({
                    novel: route.query.novel ? JSON.parse(decodeURIComponent(route.query.novel)) : null
                })
            }
        ]
    },
    {
        path: '/Novels/ReaderInfomation',
        name: 'ReaderInfomation',
        component: Reader_imfromation,
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
                path: 'stats',
                name: 'AuthorStats',
                component: AuthorStats,
                meta: { title: '数据统计' }
            },
            {
                path: 'settings',
                name: 'AuthorSettings',
                component: AuthorSettings,
                meta: { title: '账号设置' }
            }
        ]
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router