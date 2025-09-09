import request from '@/API/index'

/**
 * 获取所有分类
 * @returns {Promise<Array<Category>>} 返回分类数组
 * @typedef {Object} Category
 * @property {string} categoryName - 分类名称（也是主键）
 */
export function getAllCategories() {
    return request({
        url: '/api/Category',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取单个分类详情
 * @param {string} categoryName - 参数：分类名称
 * @returns {Promise<Category>} - 返回单个分类对象
 */
export function getCategory(categoryName) {
    return request({
        url: `/api/Category/${categoryName}`,
        method: 'get'
    })
}

/**
 * 创建分类
 * @param {Object} categoryData - 分类数据
 * @param {string} categoryData.categoryName - 分类名称
 * @returns {Promise<Category>}
 */
export function createCategory(categoryData) {
    return request({
        url: '/api/Category',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            categoryName: categoryData.categoryName
        }
    })
}

/**
 * 更新分类
 * @param {string} oldName - 原始分类名称
 * @param {Object} updateData - 更新数据
 * @param {string} updateData.categoryName - 新分类名称
 * @returns {Promise<void>}
 */
export function updateCategory(oldName, updateData) {
    return request({
        url: `/api/Category/${oldName}`,
        method: 'put',
        headers: {
            'Content-Type': 'application/json'
        },
        data: updateData
    })
}

/**
 * 重命名分类
 * @param {string} oldName - 旧分类名称
 * @param {string} newName - 新分类名称
 * @returns {Promise<void>}
 */
export function renameCategory(oldName, newName) {
    return request({
        url: '/api/Category/rename',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        params: {
            oldName,
            newName
        }
    })
}

/**
 * 删除分类
 * @param {string} categoryName - 分类名称
 * @returns {Promise<void>}
 */
export function deleteCategory(categoryName) {
    return request({
        url: `/api/Category/${categoryName}`,
        method: 'delete'
    })
}