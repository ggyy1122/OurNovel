import request from '@/API/index'

/**
 * 获取作者收入记录列表
 * @param {number} authorId - 作者ID
 * @returns {Promise<Array<AuthorIncome>>} 返回作者收入记录数组
 * @typedef {Object} AuthorIncome
 * @property {number} id - 记录ID
 * @property {number} authorId - 作者ID
 * @property {string} type - 收入类型（打赏、分成等）
 * @property {number} amount - 收入金额
 * @property {string} createTime - 创建时间
 */
export function getAuthorIncomes(authorId) {
    return request({
        url: `/api/AuthorIncome/list/${authorId}`,
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取作者总收入
 * @param {number} authorId - 作者ID
 * @returns {Promise<Object>} 返回包含作者ID和总收入的对象
 * @property {number} authorId - 作者ID
 * @property {number} totalIncome - 总收入金额
 */
export function getAuthorTotalIncome(authorId) {
    return request({
        url: `/api/AuthorIncome/total/${authorId}`,
        method: 'get'
    })
}