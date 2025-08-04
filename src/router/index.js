import { createRouter, createWebHistory } from 'vue-router'
import Admin_Layout from '@/Admin/Admin_Layout.vue'
import Dash_board from '@/Admin/Dash_Board.vue'
import Novels_Board from '@/Admin/Novels_Board.vue'
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
import Novel_Info from '@/Novels/Novel_Info.vue'
import Novel_Info_home from '@/Novels/Novel_Info_home.vue'
import Novel_Info_Comment from '@/Novels/Novel_Info_Comment.vue'
import Novel_Info_Chapter from '@/Novels/Novel_Info_Chapter.vue'
import Recommend_api_test from '@/API_Test/Recommend_Test.vue'
import AuthorIncome_api_test from '@/API_Test/AuthorIncome_Test.vue'
import Likes_api_test from '@/API_Test/Likes_Test.vue'
import Manager_api_test from '@/API_Test/Manager_Test.vue'
import Purchase_api_test from '@/API_Test/Purchase_Test.vue'
import Ranking_api_test from '@/API_Test/Ranking_Test.vue'
import Report_api_test from '@/API_Test/Report_Test.vue'
import Reward_api_test from '@/API_Test/Reward_Test.vue'
import CommentReply_api_test from '@/API_Test/CommentReply_Test.vue'
import Transaction_api_test from '@/API_Test/Transaction_Test.vue'
import Recharge_api_test from '@/API_Test/Recharge_Test.vue'
import Search_api_test from '@/API_Test/Search_Test.vue'
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
import MainLayout  from '@/Reader/MainLayout.vue'
import HomeView  from '@/Reader/HomeView.vue'
import FinanceView from '@/Reader/FinanceView.vue'
import BookshelfView from '@/Reader/BookshelfView.vue'
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
        path: '/Recommend_api_test',
        name: 'Recommend_api_test',
        component: Recommend_api_test
    },
    {
        path: '/AuthorIncome_api_test',
        name: 'AuthorIncome_api_test',
        component: AuthorIncome_api_test
    },
    {
        path: '/Likes_api_test',
        name: 'Likes_api_test',
        component: Likes_api_test
    },
    {
        path: '/Manager_api_test',
        name: 'Manager_api_test',
        component: Manager_api_test
    },
    {
        path: '/Purchase_api_test',
        name: 'Purchase_api_test',
        component: Purchase_api_test
    },
    {
        path: '/Ranking_api_test',
        name: 'Ranking_api_test',
        component: Ranking_api_test
    },
    {
        path: '/Report_api_test',
        name: 'Report_api_test',
        component: Report_api_test
    },
    {
        path: '/Reward_api_test',
        name: 'Reward_api_test',
        component: Reward_api_test
    },
    {
        path: '/CommentReply_api_test',
        name: 'CommentReply_api_test',
        component: CommentReply_api_test
    },
    {
        path: '/Transaction_api_test',
        name: 'Transaction_api_test',
        component: Transaction_api_test
    },
    {
        path: '/Recharge_api_test',
        name: 'Recharge_api_test',
        component: Recharge_api_test
    },
    {
        path: '/Search_api_test',
        name: 'Search_api_test',
        component: Search_api_test
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