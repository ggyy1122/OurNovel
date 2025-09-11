import request from '@/API/index'

/**
 * 获取所有点赞记录
 * @returns {Promise<Array<Like>>} 返回点赞记录数组
 * @typedef {Object} Like
 * @property {number} commentId - 评论ID
 * @property {number} readerId - 读者ID
 */
export function getAllLikes() {
    return request({
        url: '/api/Likes',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 给评论点赞
 * @param {number} commentId - 评论ID
 * @param {number} readerId - 读者ID
 * @returns {Promise<Object>} 返回操作结果
 */
export function likeComment(commentId, readerId) {
    return request({
        url: '/api/Likes/Like',
        method: 'post',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        data: {
            commentId,
            readerId
        }
    })
}

/**
 * 取消点赞
 * @param {number} commentId - 评论ID
 * @param {number} readerId - 读者ID
 * @returns {Promise<Object>} 返回操作结果
 */
export function unlikeComment(commentId, readerId) {
    return request({
        url: '/api/Likes/Unlike',
        method: 'post',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        data: {
            commentId,
            readerId
        }
    })
}

/**
 * 检查是否已点赞
 * @param {number} commentId - 评论ID
 * @param {number} readerId - 读者ID
 * @returns {Promise<Object>} 返回是否已点赞
 */
export function isLiked(commentId, readerId) {
    return request({
        url: '/api/Likes/IsLiked',
        method: 'get',
        params: {
            commentId,
            readerId
        }
    })
}

/**
 * 获取评论点赞数
 * @param {number} commentId - 评论ID
 * @returns {Promise<Object>} 返回点赞数
 */
export function getLikeCount(commentId) {
    return request({
        url: `/api/Likes/Count/${commentId}`,
        method: 'get'
    })
}