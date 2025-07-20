import request from '@/API/index'

/**
 * 获取所有评分记录
 * @returns {Promise<Array<Rate>>} 返回评分数组
 * @typedef {Object} Rate - 评分记录
 * @property {number} novelId - 小说ID
 * @property {number} readerId - 读者ID
 * @property {number} score - 评分值 (0-10)
 * @property {string} ratingTime - 评分时间 (ISO格式)
 */
export function getAllRates() {
    return request({
        url: '/api/Rate',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取某本小说的全部评分记录
 * @param {number} novelId - 小说ID
 * @returns {Promise<Array<Rate>>} 返回该小说的评分列表
 */
export function getRatesByNovel(novelId) {
    return request({
        url: `/api/Rate/novel/${novelId}`,
        method: 'get'
    })
}

/**
 * 获取某位读者的全部评分记录
 * @param {number} readerId - 读者ID
 * @returns {Promise<Array<Rate>>} 返回该读者的评分列表
 */
export function getRatesByReader(readerId) {
    return request({
        url: `/api/Rate/reader/${readerId}`,
        method: 'get'
    })
}

/**
 * 添加评分
 * @param {number} novelId - 小说ID
 * @param {number} readerId - 读者ID
 * @param {number} score - 评分值 (0-10)
 * @returns {Promise<string>} 返回操作结果
 */
export function addRate(novelId, readerId, score) {
    return request({
        url: '/api/Rate',
        method: 'post',
        params: {  // 使用 params 传递查询参数
            novelId,
            readerId,
            score
        }
    })
}

/**
 * 删除评分记录
 * @param {number} novelId - 小说ID
 * @param {number} readerId - 读者ID
 * @returns {Promise<string>} 返回操作结果
 */
export function deleteRate(novelId, readerId) {
    return request({
        url: '/api/Rate',
        method: 'delete',
        params: {
            novelId,
            readerId
        }
    })
}