import request from '@/API/index'

/**
 * 购买章节
 * @param {Object} purchaseData - 购买数据
 * @typedef {Object} PurchaseData
 * @property {number} readerId - 读者ID
 * @property {number} novelId - 小说ID
 * @property {number} chapterId - 章节ID
 * @returns {Promise<Object>} 返回购买结果
 */
export function purchaseChapter(purchaseData) {
    return request({
        url: '/api/Purchase',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            readerId: purchaseData.readerId,
            novelId: purchaseData.novelId,
            chapterId: purchaseData.chapterId
        }
    })
}