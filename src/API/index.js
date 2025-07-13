import axios from 'axios'

// 创建 axios 实例
const service = axios.create({
    baseURL: process.env.VUE_APP_BASE_API, // 从环境变量获取 API 地址
    timeout: 50000 // 请求超时时间
})

// 请求拦截器
service.interceptors.request.use(
    config => {
        // 在这里可以添加请求头等信息
        // 例如添加 token
        // if (store.getters.token) {
        //   config.headers['Authorization'] = `Bearer ${store.getters.token}`
        // }
        return config
    },
    error => {
        console.log(error)
        return Promise.reject(error)
    }
)

// 响应拦截器
service.interceptors.response.use(
    response => {
        // 对响应数据做处理
        return response.data
    },
    error => {
        console.log('Error:', error.response)
        return Promise.reject(error)
    }
)

export default service