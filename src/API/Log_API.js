// Log_API.js
import request from '@/API/index'

/**
 * 作者注册 DTO
 * @typedef {Object} AuthorRegisterDto
 * @property {string} authorName - 作者名称
 * @property {string} password - 密码
 */

/**
 * 作者登录响应
 * @typedef {Object} LoginResponse
 * @property {string} token - JWT Token
 * @property {string} authorName - 作者名称
 * @property {number} authorId - 作者ID
 */

/**
 * 作者注册接口
 * @param {AuthorRegisterDto} dto - 注册数据
 * @returns {Promise<Object>} 注册结果
 */
export function registerAuthor(dto) {
    return request({
        url: '/api/LogAuthor/register-author',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            authorName: dto.authorName,
            password: dto.password
        }
    })
}

/**
 * 作者登录接口
 * @param {AuthorRegisterDto} dto - 登录数据
 * @returns {Promise<LoginResponse>} 登录响应（包含token和用户信息）
 */
export function loginAuthor(dto) {
    return request({
        url: '/api/LogAuthor/login-author',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            authorName: dto.authorName,
            password: dto.password
        }
    })
}

/**
 * 重置作者密码接口
 * @param {AuthorRegisterDto} dto - 包含作者名称和新密码的对象
 * @returns {Promise<{message: string}>} 操作结果
 */
export function resetAuthorPassword(dto) {
    return request({
        url: '/api/LogAuthor/reset-author-password',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            authorName: dto.authorName,
            password: dto.password
        }
    })
}
/**
 * 登出接口（前端清除token即可）
 * @returns {Promise<{message: string}>} 操作结果
 */
export function logout() {
    return request({
        url: '/api/LogAuthor/logout',
        method: 'post'
    })
}




/**
 * 管理员注册 DTO
 * @typedef {Object} ManagerRegisterDto
 * @property {string} managerName - 管理员名称
 * @property {string} password - 密码
 */

/**
 * 管理员登录响应
 * @typedef {Object} ManagerLoginResponse
 * @property {string} token - JWT Token
 * @property {string} managerName - 管理员名称
 * @property {number} managerId - 管理员ID
 */

/**
 * 管理员注册接口
 * @param {ManagerRegisterDto} dto - 注册数据
 * @returns {Promise<Object>} 注册结果
 */
export function registerManager(dto) {
    return request({
        url: '/api/LogManager/register-manager',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            managerName: dto.managerName,
            password: dto.password
        }
    })
}

/**
 * 管理员登录接口
 * @param {ManagerRegisterDto} dto - 登录数据
 * @returns {Promise<ManagerLoginResponse>} 登录响应（包含token和管理员信息）
 */
export function loginManager(dto) {
    return request({
        url: '/api/LogManager/login-manager',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            managerName: dto.managerName,
            password: dto.password
        }
    })
}


/**
 * 重置管理员密码接口
 * @param {ManagerRegisterDto} dto - 包含管理员名称和新密码的对象
 * @returns {Promise<{message: string}>} 操作结果
 */
export function resetManagerPassword(dto) {
    return request({
        url: '/api/LogManager/reset-manager-password',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            managerName: dto.managerName,
            password: dto.password
        }
    })
}
/**
 * 管理员登出接口
 * @returns {Promise<{message: string}>} 操作结果
 */
export function logoutManager() {
    return request({
        url: '/api/LogManager/logout',
        method: 'post'
    })
}


/**
 * 读者注册 DTO
 * @typedef {Object} ReaderRegisterDto
 * @property {string} readerName - 读者名称
 * @property {string} password - 密码
 */

/**
 * 读者登录响应
 * @typedef {Object} ReaderLoginResponse
 * @property {string} token - JWT Token
 * @property {string} readerName - 读者名称
 * @property {number} readerId - 读者ID
 */

/**
 * 读者注册接口
 * @param {ReaderRegisterDto} dto - 注册数据
 * @returns {Promise<Object>} 注册结果
 */
export function registerReader(dto) {
    return request({
        url: '/api/LogReader/register-reader',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            readerName: dto.readerName,
            password: dto.password
        }
    })
}

/**
 * 读者登录接口
 * @param {ReaderRegisterDto} dto - 登录数据
 * @returns {Promise<ReaderLoginResponse>} 登录响应（包含token和读者信息）
 */
export function loginReader(dto) {
    return request({
        url: '/api/LogReader/login-reader',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            readerName: dto.readerName,
            password: dto.password
        }
    })
}
/**
 * 重置读者密码接口
 * @param {ReaderRegisterDto} dto - 包含读者名称和新密码的对象
 * @returns {Promise<{message: string}>} 操作结果
 */
export function resetReaderPassword(dto) {
    return request({
        url: '/api/LogReader/reset-reader-password',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            readerName: dto.readerName,
            password: dto.password
        }
    })
}
/**
 * 读者登出接口
 * @returns {Promise<{message: string}>} 操作结果
 */
export function logoutReader() {
    return request({
        url: '/api/LogReader/logout',
        method: 'post'
    })
}