import request from '@/API/index'

/**
 * 通过小说名模糊搜索小说
 * @param {string} keyword - 搜索关键词
 * @returns {Promise<Array<Novel>>} 返回小说数组
 */
export function searchNovels(keyword) {
    return request({
        url: '/api/Search/novel',
        method: 'get',
        params: {
            keyword: keyword
        },
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 通过作者名模糊搜索作者
 * @param {string} keyword - 搜索关键词
 * @returns {Promise<Array<Author>>} 返回作者数组
 * @typedef {Object} Author  - 含6个元素
 * @property {number} authorId - 作者ID
 * @property {string} authorName - 作者名称
 * @property {string} password - 密码
 * @property {number} earning - 收入
 * @property {string|null} phone - 电话
 * @property {string|null} avatarUrl - 头像URL
 */
export function searchAuthors(keyword) {
    return request({
        url: '/api/Search/author',
        method: 'get',
        params: {
            keyword: keyword
        }
    })
}

/**
 * 通过读者名模糊搜索读者
 * @param {string} keyword - 搜索关键词
 * @returns {Promise<Array<Reader>>} 返回读者数组
 */
export function searchReaders(keyword) {
    return request({
        url: '/api/Search/reader',
        method: 'get',
        params: {
            keyword: keyword
        }
    })
}