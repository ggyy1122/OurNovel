import request from '@/API/index'

/*
 * API 请求方法模板
 * @param {string} url - API 接口路径（不包含 baseURL）
 * @param {string} method - HTTP 请求方法（默认 'get'）
 * @param {object} data - 请求数据（GET 请求时自动转为 query 参数，其他请求作为 body）
 * @param {object} options - 额外配置项（可选）
 * @returns {Promise} - 返回 Promise 对象

export function apiRequest(url, method = 'get', data = {}, options = {}) {
    // 处理 GET 请求的参数（自动转为 URL 查询字符串）
    const params = method.toLowerCase() === 'get' ? data : null;
    // 处理其他请求的参数（作为请求体）
    const requestData = ['post', 'put', 'delete', 'patch'].includes(method.toLowerCase()) ? data : null;

    return request({
        url,
        method,
        params,      // GET 请求参数
        data: requestData, // 非 GET 请求参数
        ...options   // 额外配置（如 headers、timeout 等）
    });
}

// 示例：获取用户列表
export function fetchUserList(params) {
    return apiRequest('/api/users', 'get', params);
}

// 示例：创建用户
export function createUser(data) {
    return apiRequest('/api/users', 'post', data);
}

// 示例：更新用户
export function updateUser(userId, data) {
    return apiRequest(`/api/users/${userId}`, 'put', data);
}

// 示例：删除用户
export function deleteUser(userId) {
    return apiRequest(`/api/users/${userId}`, 'delete');
}
*/

// 示例 API 请求方法
export function fetchOracleData(params) {
    return request({
        url: '/api/Reader', // 替换为你的实际 API 地址
        method: 'get',
        params
    })
}

// 添加更多 API 方法...