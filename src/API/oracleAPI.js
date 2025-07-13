import request from '@/API/index'

// 示例 API 请求方法
export function fetchOracleData(params) {
    return request({
        url: '/api/Reader', // 替换为你的实际 API 地址
        method: 'get',
        params
    })
}

// 添加更多 API 方法...