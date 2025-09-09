import request from '@/API/index'

/**
 * 获取指定章节的管理日志列表
 * @param {number} chapterId - 章节ID
 * @param {number} novelId - 小说ID
 * @returns {Promise<Array<ChapterManagementLog>>} - 返回章节管理日志数组
 * @typedef {Object} ChapterManagementLog
 * @property {number} ManagementId - 管理记录ID
 * @property {number} managerNameanagerId - 管理员ID
 * @property {string} managerName - 管理员名称
 * @property {string} result - 处理结果
 * @property {string} time - 处理时间(yyyy-MM-dd HH:mm:ss格式)
 */
export function getChapterManagementLogs(chapterId, novelId) {
    return request({
        url: `/api/ChapterManagement/logs/${chapterId}?novelId=${novelId}`,
        method: 'get'
    })
}

/**
 * 获取所有章节的管理日志列表
 * @returns {Promise<Array<AllChapterManagementLog>>} - 返回所有章节管理日志数组
 * @typedef {Object} AllChapterManagementLog
 * @property {number} ManagementId - 管理记录ID
 * @property {number} ManagerId - 管理员ID
 * @property {string} ManagerName - 管理员名称
 * @property {number} NovelId - 小说ID
 * @property {number} ChapterId - 章节ID
 * @property {string} Result - 处理结果
 * @property {string} Time - 处理时间(yyyy-MM-dd HH:mm:ss格式)
 */
export function getAllChapterManagementLogs() {
    return request({
        url: '/api/ChapterManagement/logs/all',
        method: 'get'
    })
}
