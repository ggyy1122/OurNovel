import request from '@/API/index'

/**
 * 获取所有作者
 * @returns {Promise<Array<Author>>} 返回作者数组
 * @typedef {Object} Author  - 含6个元素
 * @property {number} authorId - 作者ID
 * @property {string} authorName - 作者名称
 * @property {string} password - 密码
 * @property {number} earning - 收入
 * @property {string|null} phone - 电话
 * @property {string|null} avatarUrl - 头像URL
 */
export function getAllAuthors() {
    return request({
        url: '/api/Author',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取单个作者详情
 * @param {number} authorId - 参数：作者ID
 * @returns {Promise<Author>} - 返回单个作者对象
 * @typedef {Object} Author  - 含6个元素
 */
export function getAuthor(authorId) {
    return request({
        url: `/api/Author/${authorId}`,
        method: 'get'
    })
}

/**
 * 创建作者
 * @param {Object} authorData - 作者数据
 * @typedef {Object} Author  - 含5个元素
 * @param {Object} authorData - 作者数据
 * @param {string} authorData.authorName - 作者名称
 * @param {string} authorData.password - 密码
 * @param {number} [authorData.earning=0] - 收入
 * @param {string} [authorData.phone] - 电话
 * @param {string} [authorData.avatarUrl] - 头像URL
 * @returns {Promise<Author>}
 */
export function createAuthor(authorData) {
    return request({
        url: '/api/Author',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            authorName: authorData.authorName,
            password: authorData.password,
            earning: authorData.earning || 0,
            phone: authorData.phone || null,
            avatarUrl: authorData.avatarUrl || null
        }
    })
}

/**
 * 更新作者
 * @param {number} authorId - 作者ID
 * @param {Object} updateData - 更新数据，注意包含id，即6个参数
 * @param {string} [updateData.authorName] - 作者名称
 * @param {string} [updateData.password] - 密码
 * @param {number} [updateData.earning] - 收入
 * @param {string|null} [updateData.phone] - 电话
 * @param {string|null} [updateData.avatarUrl] - 头像URL
 * @returns {Promise<Author>}
 */
export function updateAuthor(authorId, updateData) {
    return request({
        url: `/api/Author/${authorId}`,
        method: 'put',
        headers: {
            'Content-Type': 'application/json'
        },
        data: updateData
    })
}

/**
 * 删除作者
 * @param {number} authorId - 作者ID
 * @returns {Promise<void>}
 */
export function deleteAuthor(authorId) {
    return request({
        url: `/api/Author/${authorId}`,
        method: 'delete'
    })
}

/** 
 * 获取作者作品数量
 * @param {number} authorId - 参数：作者ID
 * @returns {Promise<{authorId: number, novelCount: number}>} - 返回作者ID和作品数量
 */
export function getAuthorNovelCount(authorId) {
    return request({
        url: `/api/Author/${authorId}/novel-count`,
        method: 'get'
    })
}

/** 
 * 获取作者作品总字数
 * @param {number} authorId - 参数：作者ID
 * @returns {Promise<{authorId: number, totalWordCount: number}>} - 返回作者ID和总字数
 */
export function getAuthorTotalWordCount(authorId) {
    return request({
        url: `/api/Author/${authorId}/total-wordcount`,
        method: 'get'
    })
}

/** 
 * 获取作者注册天数
 * @param {number} authorId - 参数：作者ID
 * @returns {Promise<{authorId: number, registerDays: number}>} - 返回作者ID和注册天数
 */
export function getAuthorRegisterDays(authorId) {
    return request({
        url: `/api/Author/${authorId}/register-days`,
        method: 'get'
    })
}

/** 
 * 获取作者的所有小说
 * @param {number} authorId - 参数：作者ID
 * @returns {Promise<Array<Novel>>} - 获取该作者的筛选为‘连载和完结’的小说
 * @typedef {Object} Novel - 小说对象
 */
export function getAuthorNovels(authorId) {
    return request({
        url: `/api/Author/${authorId}/novel`,
        method: 'get'
    })
}