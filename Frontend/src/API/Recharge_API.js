import request from '@/API/index'

/**
 * 发起充值
 * @param {Object} rechargeData - 充值数据
 * @param {number} rechargeData.ReaderId - 用户ID
 * @param {number} rechargeData.Amount - 充值金额
 * @returns {Promise<Object>} 返回支付URL
 */
export function startRecharge(rechargeData) {
    return request({
        url: '/api/recharge/start',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            ReaderId: rechargeData.ReaderId,
            Amount: rechargeData.Amount
        }
    })
}