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


/**
 * 整本小说买断接口
 * @param {Object} dto - 买断请求数据
 * @param {number} dto.readerId - 读者ID
 * @param {number} dto.novelId - 小说ID
 * @returns {Promise<Object>} 返回 { success: number, message: string }
 */
export function purchaseWholeNovel(dto) {
    return request({
        url: '/api/WholePurchase',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            readerId: dto.readerId,
            novelId: dto.novelId
        }
    });
}


/**
 * 查询是否已整本买断该小说
 * @param {number} readerId - 读者ID
 * @param {number} novelId - 小说ID
 * @returns {Promise<Object>} 返回 { hasPurchased: boolean }
 */
export function getWholePurchaseStatus(readerId, novelId) {
    return request({
        url: '/api/WholePurchase/status',
        method: 'get',
        params: {
            readerId,
            novelId
        }
    });
}


/** 
 * 获取读者打赏记录接口
 * @param {number} readerId - 读者ID
 * @returns {Promise<Array<Object>>} 返回交易记录数组，包含以下字段：
 *   - readerId: number - 读者ID
 *   - transactionId: number - 交易ID
 *   - amount: number - 交易金额
 *   - rewardTime: string - 交易时间(ISO格式)
 *   - novelId: number - 小说ID
 *   - novelTitle: string - 小说标题
 *   - authorId: number - 作者ID
 *   - authorName: string - 作者名称
 */
export function getReaderReward(readerId) {
    return request({
        url: `/api/Transaction/reward/${readerId}`,
        method: 'get',
        headers: {
            'Content-Type': 'application/json'
        }
    });
}


/** 
 * 获取读者交易记录
 * @param {number} readerId - 读者ID
 * @param {string} [transType] - 可选，交易类型（"充值"/"打赏"/"订阅"）
 * @param {string} [timeRange] - 可选，时间范围（"all"/"year"/"month"/"week"）
 * @returns {Promise<Array>} 返回交易记录数组，包含字段：
 *   - readerId: number
 *   - transactionId: number
 *   - transType: string
 *   - amount: number
 *   - time: string(ISO格式)
 */
export function getReaderTransactions(readerId) {
    return request({
        url: `/api/Transaction/transaction/${readerId}`,
        method: 'get',
        headers: {
            'Content-Type': 'application/json'
        }
    });
}

/** 
 * 获取读者充值记录
 * @param {number} readerId - 读者ID
 * @returns {Promise<Array>} 返回充值记录数组，
 */
export function getReaderRecharge(readerId) {
    return request({
        url: `/api/Transaction/recharge/${readerId}`,
        method: 'get',
        headers: {
            'Content-Type': 'application/json'
        }
    });
}

/** 
 * 获取读者订阅记录
 * @param {number} readerId - 读者ID
 * @returns {Promise<Array>} 返回订阅记录数组，
 */
export function getReaderSubscribtion(readerId) {
    return request({
        url: `/api/Transaction/subscription/${readerId}`,
        method: 'get',
        headers: {
            'Content-Type': 'application/json'
        }
    });
}





