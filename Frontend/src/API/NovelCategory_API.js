import request from '@/API/index'

/**
 * 获取所有小说与分类的关系
 * @returns {Promise<Array<NovelCategory>>} 返回关系数组
 * @typedef {Object} NovelCategory
 * @property {number} novelId - 小说ID
 * @property {string} categoryName - 分类名称
 */
export function getAllNovelCategories() {
    return request({
        url: '/api/NovelCategory',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 添加小说与分类关系
 * @param {number} novelId - 小说ID
 * @param {string} categoryName - 分类名称
 * @returns {Promise<void>}
 */
export function addNovelCategory(novelId, categoryName) {
    return request({
        url: '/api/NovelCategory',
        method: 'post',
        params: {
            novelId,
            categoryName
        }
    })
}

/**
 * 删除小说与分类关系
 * @param {number} novelId - 小说ID
 * @param {string} categoryName - 分类名称
 * @returns {Promise<void>}
 */
export function deleteNovelCategory(novelId, categoryName) {
    return request({
        url: '/api/NovelCategory',
        method: 'delete',
        params: {
            novelId,
            categoryName
        }
    })
}

/**
 * 获取某本小说的全部分类
 * @param {number} novelId - 小说ID
 * @returns {Promise<Array<string>>} 返回分类名称数组
 */
export function getCategoriesByNovel(novelId) {
    return request({
        url: `/api/NovelCategory/novel/${novelId}`,
        method: 'get'
    })
}

/**
 * 获取某个分类下的所有小说
 * @param {string} categoryName - 分类名称
 * @returns {Promise<Array<number>>} 返回小说ID数组
 */
export function getNovelsByCategory(categoryName) {
    return request({
        url: `/api/NovelCategory/category/${categoryName}`,
        method: 'get'
    })
}