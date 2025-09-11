import request from '@/API/index'

/**
 * 获取所有评论回复关系
 * @returns {Promise<Array<CommentReply>>} 返回评论回复关系数组
 * @typedef {Object} CommentReply
 * @property {number} commentId - 评论ID
 * @property {number|null} preComId - 父评论ID
 * @property {number} commentLevel - 评论层级(1-3)
 */
export function getAllCommentReplies() {
    return request({
        url: '/api/CommentReply',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 根据评论ID获取回复关系
 * @param {number} commentId - 评论ID
 * @returns {Promise<CommentReply>} 返回单个评论回复关系
 */
export function getReplyByCommentId(commentId) {
    return request({
        url: `/api/CommentReply/${commentId}`,
        method: 'get'
    })
}

/**
 * 获取某条评论下的所有直接回复
 * @param {number} parentId - 父评论ID
 * @returns {Promise<Array<CommentReply>>} 返回回复列表
 */
export function getRepliesByParentId(parentId) {
    return request({
        url: `/api/CommentReply/parent/${parentId}`,
        method: 'get'
    })
}

/**
 * 添加评论回复
 * @param {Object} replyData - 回复数据
 * @param {number} replyData.commentId - 评论ID
 * @param {number|null} replyData.preComId - 父评论ID
 * @param {number} replyData.commentLevel - 评论层级(1-3)
 * @returns {Promise<Object>} 返回操作结果
 */
export function addCommentReply(replyData) {
    return request({
        url: '/api/CommentReply',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            commentId: replyData.commentId,
            preComId: replyData.preComId || null,
            commentLevel: replyData.commentLevel
        }
    })
}