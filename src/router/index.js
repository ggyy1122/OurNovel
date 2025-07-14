import { createRouter, createWebHistory } from 'vue-router'
import Admin_Login from '@/Admin/Admin_Login.vue'
import Admin_Layout from '@/Admin/Admin_Layout.vue'
import Dash_board from '@/Admin/Dash_Board.vue'
import Novels_Board from '@/Admin/Novels_Board.vue'
import Home from '@/views/Home_test.vue'
import Novel_Layout from '@/Novels/Novel_Layout.vue'
import Novel_Login from '@/Novels/Novel_Login.vue'
import Novel_Home from '@/Novels/Novel_Home.vue'
import Novel_Category from '@/Novels/Novel_Category.vue'
import Novel_Rank from '@/Novels/Novel_Rank.vue'

const routes = [
    {
        path: '/',
        name: 'Home',
        component: Home
    },
    {
        path: '/Admin/Admin_Login',
        name: 'Admin_Login',
        component: Admin_Login
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
        path: '/Novels/Novel_Login',
        name: 'Novel_Login',
        component: Novel_Login
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
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router