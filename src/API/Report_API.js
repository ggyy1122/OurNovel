import request from '@/API/index'

/**
 * 获取所有举报记录
 * @returns {Promise<Array<Report>>} 返回举报记录数组
 * @typedef {Object} Report
 * @property {number} reportId - 举报ID
 * @property {string} reason - 举报原因
 * @property {string} reportTime - 举报时间
 * @property {string} progress - 处理进度
 * @property {number} commentId - 评论ID
 * @property {number} readerId - 读者ID
 */
export function getAllReports() {
    return request({
        url: '/api/Reports',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取单个举报详情
 * @param {number} reportId - 举报ID
 * @returns {Promise<Report>} 返回单个举报对象
 */
export function getReport(reportId) {
    return request({
        url: `/api/Reports/${reportId}`,
        method: 'get'
    })
}

/**
 * 举报评论
 * @param {number} commentId - 评论ID
 * @param {number} readerId - 读者ID
 * @param {string} reason - 举报原因
 * @returns {Promise<Object>} 返回操作结果
 */
export function reportComment(commentId, readerId, reason) {
    return request({
        url: `/api/comment/${commentId}/report`,
        method: 'post',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        data: {
            readerId,
            reason
        }
    })
}

/**
 * 处理举报
 * @param {number} reportId - 举报ID
 * @param {string} progress - 处理进度
 * @param {number} managerId - 管理员ID
 * @returns {Promise<Object>} 返回操作结果
 */
export function processReport(reportId, progress, managerId) {
    return request({
        url: `/api/report/${reportId}/process`,
        method: 'post',
        params: {
            progress,
            managerId
        }
    })
}

//根据id获取单个举报
export function getReportById(reportId) {
    return request({
        url: `/api/Reports/${reportId}`,
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}