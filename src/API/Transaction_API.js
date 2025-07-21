import request from '@/API/index'

/**
 * 获取所有交易记录
 * @returns {Promise<Array<Transaction>>} 返回交易记录数组
 * @typedef {Object} Transaction
 * @property {number} transactionId - 交易ID
 * @property {number} readerId - 读者ID
 * @property {string} transType - 交易类型
 * @property {number} amount - 交易金额
 * @property {string} time - 交易时间
 */
export function getAllTransactions() {
    return request({
        url: '/api/Transaction',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取单个交易详情
 * @param {number} transactionId - 交易ID
 * @returns {Promise<Transaction>} 返回单个交易对象
 */
export function getTransaction(transactionId) {
    return request({
        url: `/api/Transaction/${transactionId}`,
        method: 'get'
    })
}

/**
 * 创建交易记录
 * @param {Object} transactionData - 交易数据
 * @param {number} transactionData.readerId - 读者ID
 * @param {string} transactionData.transType - 交易类型
 * @param {number} transactionData.amount - 交易金额
 * @property {string} [transactionData.time] - 交易时间
 * @returns {Promise<Transaction>}
 */
export function createTransaction(transactionData) {
    return request({
        url: '/api/Transaction',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            readerId: transactionData.readerId,
            transType: transactionData.transType,
            amount: transactionData.amount,
            time: transactionData.time
        }
    })
}

/**
 * 更新交易记录
 * @param {number} transactionId - 交易ID
 * @param {Object} updateData - 更新数据
 * @param {number} [updateData.readerId] - 读者ID
 * @param {number} [updateData.transactionId] - 交易ID
 * @param {string} [updateData.transType] - 交易类型
 * @param {number} [updateData.amount] - 交易金额
 * @property {string} [updateData.time] - 交易时间
 * @returns {Promise<Transaction>}
 */
export function updateTransaction(transactionId, updateData) {
    return request({
        url: `/api/Transaction/${transactionId}`,
        method: 'put',
        headers: {
            'Content-Type': 'application/json'
        },
        data: updateData
    })
}

/**
 * 删除交易记录
 * @param {number} transactionId - 交易ID
 * @returns {Promise<void>}
 */
export function deleteTransaction(transactionId) {
    return request({
        url: `/api/Transaction/${transactionId}`,
        method: 'delete'
    })
}