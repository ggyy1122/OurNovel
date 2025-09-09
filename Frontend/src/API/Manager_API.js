import request from '@/API/index'

/**
 * 获取所有管理员
 * @returns {Promise<Array<Manager>>} 返回管理员数组
 * @typedef {Object} Manager
 * @property {number} managerId - 管理员ID
 * @property {string} managerName - 管理员名称
 * @property {string} password - 密码
 */
export function getAllManagers() {
    return request({
        url: '/api/Manager',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取单个管理员详情
 * @param {number} managerId - 管理员ID
 * @returns {Promise<Manager>} 返回单个管理员对象
 */
export function getManager(managerId) {
    return request({
        url: `/api/Manager/${managerId}`,
        method: 'get'
    })
}

/**
 * 创建管理员
 * @param {Object} managerData - 管理员数据
 * @param {string} managerData.managerName - 管理员名称
 * @param {string} managerData.password - 密码
 * @returns {Promise<Manager>}
 */
export function createManager(managerData) {
    return request({
        url: '/api/Manager',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            managerName: managerData.managerName,
            password: managerData.password
        }
    })
}

/**
 * 更新管理员
 * @param {number} managerId - 管理员ID
 * @param {Object} updateData - 更新数据
 * @param {number} [updateData.managerId] - 管理员ID
 * @param {string} [updateData.managerName] - 管理员名称
 * @param {string} [updateData.password] - 密码
 * @returns {Promise<Manager>}
 */
export function updateManager(managerId, updateData) {
    return request({
        url: `/api/Manager/${managerId}`,
        method: 'put',
        headers: {
            'Content-Type': 'application/json'
        },
        data: updateData
    })
}

/**
 * 删除管理员
 * @param {number} managerId - 管理员ID
 * @returns {Promise<void>}
 */
export function deleteManager(managerId) {
    return request({
        url: `/api/Manager/${managerId}`,
        method: 'delete'
    })
}