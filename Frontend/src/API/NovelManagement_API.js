import request from '@/API/index'

/**
 * 获取指定小说的管理日志列表
 * @param {number} novelId - 小说ID
 * @returns {Promise<Array<NovelManagementLog>>} - 返回小说管理日志数组
 * @typedef {Object} NovelManagementLog
 * @property {number} ManagementId - 管理记录ID
 * @property {number} ManagerId - 管理员ID
 * @property {string} ManagerName - 管理员名称
 * @property {string} Result - 处理结果
 * @property {string} Time - 处理时间(yyyy-MM-dd HH:mm:ss格式)
 */
export function getNovelManagementLogs(novelId) {
    return request({
        url: `/api/NovelManagement/logs/${novelId}`,
        method: 'get'
    })
}

/**
 * 获取所有小说的管理日志列表
 * @returns {Promise<Array<AllNovelManagementLog>>} - 返回所有小说管理日志数组
 * @typedef {Object} AllNovelManagementLog
 * @property {number} ManagementId - 管理记录ID
 * @property {number} ManagerId - 管理员ID
 * @property {string} ManagerName - 管理员名称
 * @property {number} NovelId - 小说ID
 * @property {string} Result - 处理结果
 * @property {string} Time - 处理时间(yyyy-MM-dd HH:mm:ss格式)
 */
export function getAllNovelManagementLogs() {
    return request({
        url: '/api/NovelManagement/logs/all',
        method: 'get'
    })
}