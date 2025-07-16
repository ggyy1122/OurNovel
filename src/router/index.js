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
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router