import { createRouter, createWebHistory } from 'vue-router'
import Admin_Login from '@/Admin/Admin_Login.vue'
import Admin_Layout from '@/Admin/Admin_Layout.vue'
import Dash_board from '@/Admin/Dash_Board.vue'
import Novels_Board from '@/Admin/Novels_Board.vue'
import Home from '@/views/Home_test.vue'

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
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

// 添加路由守卫
router.beforeEach((to) => {
    const isLoggedIn = localStorage.getItem('isLoggedIn')

    // 如果需要认证但未登录，重定向到登录页
    if (to.meta.requiresAuth && !isLoggedIn) {
        return '/Admin/Admin_Login'
    }

    // 如果已经登录但访问登录页，重定向到仪表盘
    if (to.path === '/Admin/Admin_Login' && isLoggedIn) {
        return '/Admin/Admin_Login/dashboard'
    }
})

export default router