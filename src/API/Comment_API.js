import request from '@/API/index'

/**
 * 获取所有评论
 * @returns {Promise<Array<Comment>>} 返回评论数组
 * @typedef {Object} Comment
 * @property {number} commentId - 评论ID
 * @property {number} readerId - 读者ID
 * @property {number} novelId - 小说ID
 * @property {number} chapterId - 章节ID
 * @property {string} title - 评论标题
 * @property {string} [content] - 评论内容
 * @property {number} likes - 点赞数
 * @property {string} status - 状态（通过/封禁）
 * @property {string} [createTime] - 创建时间
 */
export function getAllComments() {
    return request({
        url: '/api/Comments',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取单个评论详情
 * @param {number} commentId - 评论ID
 * @returns {Promise<Comment>} 返回单个评论对象
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
 * @param {string} [commentData.content] - 评论内容
 * @param {number} [commentData.likes=0] - 点赞数
 * @param {string} [commentData.status="通过"] - 状态
 * @param {string} [commentData.createTime] - 创建时间
 * @returns {Promise<Comment>}
 */
export function createComment(commentData) {
    return request({
        url: '/api/Comments',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            readerId: commentData.readerId,
            novelId: commentData.novelId,
            chapterId: commentData.chapterId,
            title: commentData.title,
            content: commentData.content || '',
            likes: commentData.likes || 0,
            status: commentData.status || "通过",
            createTime: commentData.createTime || new Date().toISOString()
        }
    })
}

/**
 * 更新评论
 * @param {number} commentId - 评论ID
 * @param {Object} updateData - 更新数据
 * @param {number} [updateData.commentId] - 评论ID
 * @param {number} [updateData.readerId] - 读者ID
 * @param {number} [updateData.novelId] - 小说ID
 * @param {number} [updateData.chapterId] - 章节ID
 * @param {string} [updateData.title] - 评论标题
 * @param {string} [updateData.content] - 评论内容
 * @param {number} [updateData.likes] - 点赞数
 * @param {string} [updateData.status] - 状态
 * @returns {Promise<Comment>}
 */
export function updateComment(commentId, updateData) {
    return request({
        url: `/api/Comments/${commentId}`,
        method: 'put',
        headers: {
            'Content-Type': 'application/json'
        },
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
 * 设置评论状态
 * @param {number} commentId - 评论ID
 * @param {string} status - 新状态
 * @param {number} managerId - 管理员ID
 * @returns {Promise<void>}
 */
export function setCommentStatus(commentId, status, managerId) {
    return request({
        url: '/api/Comments/Status',
        method: 'post',
        params: {
            commentId,
            status,
            managerId
        }
    })
}

/**
 * 获取某章节下所有通过审核的评论
 * @param {number} novelId - 小说ID
 * @param {number} chapterId - 章节ID
 * @returns {Promise<Array<Comment>>}
 */
export function getCommentsByChapter(novelId, chapterId) {
    return request({
        url: `/api/Comments/ByChapter/${novelId}/${chapterId}`,
        method: 'get'
    })
}

/**
 * 获取某小说下所有通过审核的评论
 * @param {number} novelId - 小说ID
 * @returns {Promise<Array<Comment>>}
 */
export function getCommentsByNovel(novelId) {
    return request({
        url: `/api/Comments/ByNovel/${novelId}`,
        method: 'get'
    })
}

/**
 * 递归删除评论
 * @param {number} commentId - 评论ID
 * @returns {Promise<void>}
 */
export function deleteCommentRecursive(commentId) {
    return request({
        url: `/api/Comments/DeleteRecursive/${commentId}`,
        method: 'delete'
    })
}

/**
 * 审核评论
 * @param {number} id - 评论ID
 * @param {string} newStatus - 新状态（通过/封禁）
 * @returns {Promise<void>}
 */
export function reviewComment(id, newStatus) {
    return request({
        url: `/api/Comments/${id}/review`,
        method: 'put',
        params: {
            newStatus
        }
    })
}

/**
 * 获取小说顶级评论
 * @param {number} novelId - 小说ID
 * @returns {Promise<Array<Comment>>}
 */
export function getTopLevelComments(novelId) {
    return request({
        url: `/api/Comments/novel/${novelId}/top-level-comments`,
        method: 'get'
    })
}

/**
 * 获取小说前n个点赞最多的评论
 * @param {number} novelId - 小说ID
 * @param {number} topN - 前N名
 * @returns {Promise<Array<Comment>>}
 */
export function getTopLikedComments(novelId, topN) {
    return request({
        url: `/api/Comments/novel/${novelId}/top-liked-comments/${topN}`,
        method: 'get'
    })
}

/**
 * 获取指定章节的顶级评论
 * @param {number} novelId - 小说ID
 * @param {number} chapterId - 章节ID
 * @returns {Promise<Array<Comment>>}
 */
export function getTopLevelCommentsByChapter(novelId, chapterId) {
    return request({
        url: `/api/Comments/novel/${novelId}/chapter/${chapterId}/top-level-comments`,
        method: 'get'
    })
}

/**
 * 获取指定章节前n个点赞最多的评论
 * @param {number} novelId - 小说ID
 * @param {number} chapterId - 章节ID
 * @param {number} topN - 前N名
 * @returns {Promise<Array<Comment>>}
 */
export function getTopLikedCommentsByChapter(novelId, chapterId, topN) {
    return request({
        url: `/api/Comments/novel/${novelId}/chapter/${chapterId}/top-liked-comments/${topN}`,
        method: 'get'
    })
}