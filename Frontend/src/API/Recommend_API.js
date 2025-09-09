import request from '@/API/index'

/**
 * 获取所有推荐记录
 * @returns {Promise<Array<Recommend>>} 返回推荐记录数组
 * @typedef {Object} Recommend - 推荐记录对象
 * @property {number} novelId - 小说ID
 * @property {number} readerId - 读者ID
 * @property {string|null} reason - 推荐理由
 */
export function getAllRecommends() {
    return request({
        url: '/api/Recommend',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取某个读者推荐的所有小说
 * @param {number} readerId - 读者ID
 * @returns {Promise<Array<Recommend>>} 返回该读者的推荐记录数组
 */
export function getRecommendsByReader(readerId) {
    return request({
        url: `/api/Recommend/reader/${readerId}`,
        method: 'get'
    })
}

/**
 * 获取某部小说被哪些读者推荐
 * @param {number} novelId - 小说ID
 * @returns {Promise<Array<Recommend>>} 返回推荐该小说的记录数组
 */
export function getRecommendsByNovel(novelId) {
    return request({
        url: `/api/Recommend/novel/${novelId}`,
        method: 'get'
    })
}

/**
 * 添加推荐记录
 * @param {number} novelId - 小说ID
 * @param {number} readerId - 读者ID
 * @param {string} [reason] - 推荐理由（可选）
 * @returns {Promise<Recommend>} 返回新增的推荐记录
 */
export function addRecommend(novelId, readerId, reason) {
    return request({
        url: '/api/Recommend',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        params: {
            novelId: novelId,
            readerId: readerId,
            reason: reason || null
        }
    })
}

/**
 * 取消推荐（删除推荐记录）
 * @param {number} novelId - 小说ID
 * @param {number} readerId - 读者ID
 * @returns {Promise<void>}
 */
export function deleteRecommend(novelId, readerId) {
    return request({
        url: '/api/Recommend',
        method: 'delete',
        params: {
            novelId: novelId,
            readerId: readerId
        }
    })
}