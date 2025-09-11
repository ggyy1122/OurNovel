import request from '@/API/index'

/**
 * 添加或更新收藏记录
 * @param {number} novelId - 小说ID
 * @param {number} readerId - 读者ID
 * @param {string} [isPublic] - 是否公开 ("yes" 或 "no")
 * @returns {Promise<string>} 返回操作成功提示
 */
export function addOrUpdateCollect(novelId, readerId, isPublic) {
    return request({
        url: '/api/Collect',
        method: 'post',
        params: {
            novelId,
            readerId,
            isPublic
        }
    })
}

/**
 * 取消收藏（删除一条收藏记录）
 * @param {number} novelId - 小说ID
 * @param {number} readerId - 读者ID
 * @returns {Promise<string>} 返回操作成功提示
 */
export function deleteCollect(novelId, readerId) {
    return request({
        url: '/api/Collect',
        method: 'delete',
        params: {
            novelId,
            readerId
        }
    })
}

/**
 * 获取某个读者收藏的所有小说记录
 * @param {number} readerId - 读者ID
 * @returns {Promise<Array<Collect>>} 返回该读者收藏的Collect列表
 * @typedef {Object} Collect - 收藏记录
 * @property {number} novelId - 小说ID
 * @property {number} readerId - 读者ID
 * @property {string|null} isPublic - 是否公开 ("yes" 或 "no")
 */
export function getCollectsByReader(readerId) {
    return request({
        url: `/api/Collect/reader/${readerId}`,
        method: 'get'
    })
}

/**
 * 获取某部小说被哪些读者收藏
 * @param {number} novelId - 小说ID
 * @returns {Promise<Array<Collect>>} 返回收藏该小说的Collect列表
 */
export function getCollectsByNovel(novelId) {
    return request({
        url: `/api/Collect/novel/${novelId}`,
        method: 'get'
    })
}

/**
 * 获取所有收藏记录
 * @returns {Promise<Array<Collect>>} 返回所有Collect实体列表
 */
export function getAllCollects() {
    return request({
        url: '/api/Collect',
        method: 'get'
    })
}