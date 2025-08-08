import request from '@/API/index'

/**
 * 获取所有章节
 * @returns {Promise<Array<Chapter>>} 返回章节数组
 * @typedef {Object} Chapter
 * @property {number} novelId - 所属小说ID
 * @property {number} chapterId - 章节编号
 * @property {string} title - 章节标题
 * @property {string} [content] - 章节内容
 * @property {number} wordCount - 字数
 * @property {number} pricePerKilo - 千字单价
 * @property {string} [isCharged] - 是否收费
 * @property {string} [publishTime] - 发布时间
 * @property {string} status - 章节状态
 */
export function getAllChapters() {
    return request({
        url: '/api/Chapter',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取指定小说下的所有章节
 * @param {number} novelId - 小说ID
 * @returns {Promise<Array<Chapter>>} 返回章节数组
 */
export function getChaptersByNovel(novelId) {
    return request({
        url: `/api/Chapter/novel/${novelId}`,
        method: 'get'
    })
}

/**
 * 获取指定章节
 * @param {number} novelId - 小说ID
 * @param {number} chapterId - 章节ID
 * @returns {Promise<Chapter>} 返回章节对象
 */
export function getChapter(novelId, chapterId) {
    return request({
        url: `/api/Chapter/${novelId}/${chapterId}`,
        method: 'get'
    })
}

/**
 * 创建章节
 * @param {Object} chapterData - 章节数据
 * @param {number} chapterData.novelId - 所属小说ID
 * @param {number} chapterData.chapterId - 章节编号
 * @param {string} chapterData.title - 章节标题
 * @param {string} [chapterData.content] - 章节内容
 * @param {number} [chapterData.wordCount=0] - 字数
 * @param {number} [chapterData.pricePerKilo=0.50] - 千字单价
 * @param {string} [chapterData.isCharged] - 是否收费
 * @param {string} [chapterData.publishTime] - 发布时间
 * @param {string} [chapterData.status="通过"] - 章节状态
 * @returns {Promise<Chapter>}
 */
export function createChapter(chapterData) {
    return request({
        url: '/api/Chapter',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            novelId: chapterData.novelId,
            chapterId: chapterData.chapterId,
            title: chapterData.title,
            content: chapterData.content || null,
            wordCount: chapterData.wordCount || 0,
            pricePerKilo: chapterData.pricePerKilo || 0.50,
            isCharged: chapterData.isCharged || null,
            publishTime: chapterData.publishTime || null,
            status: chapterData.status || "通过"
        }
    })
}

/**
 * 更新章节
 * @param {number} novelId - 小说ID
 * @param {number} chapterId - 章节ID
 * @param {Object} updateData - 更新数据
 * @param {string} [updateData.title] - 章节标题
 * @param {string} [updateData.content] - 章节内容
 * @param {number} [updateData.wordCount] - 字数
 * @param {number} [updateData.pricePerKilo] - 千字单价
 * @param {string} [updateData.isCharged] - 是否收费
 * @param {string} [updateData.publishTime] - 发布时间
 * @param {string} [updateData.status] - 章节状态
 * @returns {Promise<Chapter>}
 */
export function updateChapter(novelId, chapterId, updateData) {
    return request({
        url: `/api/Chapter/${novelId}/${chapterId}`,
        method: 'put',
        headers: {
            'Content-Type': 'application/json'
        },
        data: updateData
    })
}

/**
 * 删除章节
 * @param {number} novelId - 小说ID
 * @param {number} chapterId - 章节ID
 * @returns {Promise<void>}
 */
export function deleteChapter(novelId, chapterId) {
    return request({
        url: `/api/Chapter/${novelId}/${chapterId}`,
        method: 'delete'
    })
}

/**
 * 审核章节
 * @param {number} novelId - 小说ID
 * @param {number} chapterId - 章节ID
 * @param {string} newStatus - 新状态（"通过" 或 "封禁"）
 * @returns {Promise<{success: boolean, message: string}>}
 */
export function reviewChapter(novelId, chapterId,newStatus, managerId,result) {
    return request({
        url: `/api/Chapter/novels/${novelId}/chapters/${chapterId}/review`,
        method: 'put',
        params: {
            managerId: managerId,
            result: result,
            newStatus: newStatus
        }
    })
}

/**
 * 新：获取小说的所有章节（不含内容）
 * @param {number} novelId - 小说ID
 * @returns {Promise<Array>} 返回包含计算价格的章节数组
 */
export function getNovelChaptersWithoutContent(novelId) {
    return request({
        url: `/api/Chapter/novels/${novelId}/chapters`,
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}