import request from '@/API/index'

/**
 * 评论对象类型定义
 * @typedef {Object} Comment
 * @property {number} commentId - 评论ID
 * @property {number} readerId - 读者ID
 * @property {number} novelId - 小说ID
 * @property {number} chapterId - 章节ID
 * @property {string} title - 评论标题
 * @property {string} [content] - 评论内容（可选）
 * @property {number} likes - 点赞数
 * @property {string} status - 评论状态（如"通过"、"封禁"）
 * @property {string} [createTime] - 创建时间（ISO 8601格式）
 */

/**
 * 获取所有评论
 * @returns {Promise<Array<Comment>>} 评论数组
 */
export function getAllComments() {
    return request({
        url: '/api/Comments',
        method: 'get',
        headers: { 'Accept': 'application/json' }
    })
}

/**
 * 获取单个评论详情
 * @param {number} commentId - 评论ID
 * @returns {Promise<Comment>} 单个评论对象
 */
export function getComment(commentId) {
    return request({
        url: `/api/Comments/${commentId}`,
        method: 'get'
    })
}

/**
 * 创建评论
 * @param {Object} commentData - 评论数据
 * @param {number} commentData.readerId - 读者ID
 * @param {number} commentData.novelId - 小说ID
 * @param {number} commentData.chapterId - 章节ID
 * @param {string} commentData.title - 评论标题
 * @param {string} [commentData.content] - 评论内容（可选）
 * @param {number} [commentData.likes=0] - 初始点赞数（默认0）
 * @param {string} [commentData.status="通过"] - 初始状态（默认"通过"）
 * @param {string} [commentData.createTime] - 创建时间（可选）
 * @returns {Promise<Comment>} 新创建的评论
 */
export function createComment(commentData) {
    return request({
        url: '/api/Comments',
        method: 'post',
        headers: { 'Content-Type': 'application/json' },
        data: {
            readerId: commentData.readerId,
            novelId: commentData.novelId,
            chapterId: commentData.chapterId,
            title: commentData.title,
            content: commentData.content || null,
            likes: commentData.likes || 0,
            status: commentData.status || "通过",
            createTime: commentData.createTime || null
        }
    })
}

/**
 * 更新评论
 * @param {number} commentId - 评论ID
 * @param {Object} updateData - 更新数据
 * @param {number} [updateData.commentId] - 评论ID（必须）
 * @param {number} [updateData.readerId] - 读者ID
 * @param {number} [updateData.novelId] - 小说ID
 * @param {number} [updateData.chapterId] - 章节ID
 * @param {string} [updateData.title] - 新标题
 * @param {string} [updateData.content] - 新内容
 * @param {number} [updateData.likes] - 新点赞数
 * @param {string} [updateData.status] - 新状态
 * @param {string} [updateData.createTime] - 新创建时间
 * @returns {Promise<Comment>} 更新后的评论
 */
export function updateComment(commentId, updateData) {
    return request({
        url: `/api/Comments/${commentId}`,
        method: 'put',
        headers: { 'Content-Type': 'application/json' },
        data: updateData
    })
}

/**
 * 删除评论
 * @param {number} commentId - 评论ID
 * @returns {Promise<void>}
 */
export function deleteComment(commentId) {
    return request({
        url: `/api/Comments/${commentId}`,
        method: 'delete'
    })
}

/**
 * 给评论点赞（点赞数+1）
 * @param {number} commentId - 评论ID
 * @returns {Promise<{success: boolean, message: string}>} 操作结果
 */
export function likeComment(commentId) {
    return request({
        url: `/api/Comments/Like/${commentId}`,
        method: 'post'
    })
}

/**
 * 设置评论状态
 * @param {Object} params - 参数对象
 * @param {number} params.commentId - 评论ID
 * @param {string} params.status - 目标状态（如"通过"、"封禁"）
 * @returns {Promise<{success: boolean, message: string}>} 操作结果
 */
export function setCommentStatus(params) {
    return request({
        url: '/api/Comments/Status',
        method: 'post',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        data: `commentId=${params.commentId}&status=${encodeURIComponent(params.status)}`
    })
}

/**
 * 获取章节下通过审核的评论
 * @param {number} novelId - 小说ID
 * @param {number} chapterId - 章节ID
 * @returns {Promise<Array<Comment>>} 符合条件的评论数组
 */
export function getCommentsByChapter(novelId, chapterId) {
    return request({
        url: `/api/Comments/ByChapter/${novelId}/${chapterId}`,
        method: 'get'
    })
}

/**
 * 获取小说下所有通过审核的评论
 * @param {number} novelId - 小说ID
 * @returns {Promise<Array<Comment>>} 符合条件的评论数组
 */
export function getCommentsByNovel(novelId) {
    return request({
        url: `/api/Comments/ByNovel/${novelId}`,
        method: 'get'
    })
}