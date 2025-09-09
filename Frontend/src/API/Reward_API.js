import request from '@/API/index'

/**
 * 用户打赏小说
 * @param {Object} rewardData - 打赏数据
 * @typedef {Object} RewardData
 * @property {number} readerId - 读者ID
 * @property {number} novelId - 小说ID
 * @property {number} amount - 打赏金额
 * @returns {Promise<Object>} 返回打赏结果
 */
export function rewardNovel(rewardData) {
    return request({
        url: '/api/Reward',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            readerId: rewardData.readerId,
            novelId: rewardData.novelId,
            amount: rewardData.amount
        }
    })
}