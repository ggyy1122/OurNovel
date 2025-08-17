import request from '@/API/index'

/**
 * 小说对象
 * @typedef {Object} Novel
 * @property {number} novelId - 小说ID
 * @property {number} authorId - 作者ID
 * @property {string} novelName - 小说名称
 * @property {string} introduction - 小说简介
 * @property {string} createTime - 创建时间
 * @property {string|null} coverUrl - 封面URL
 * @property {number} score - 评分
 * @property {number} totalWordCount - 总字数
 * @property {number} recommendCount - 推荐数
 * @property {number} collectedCount - 收藏数
 * @property {string} status - 状态（默认："待审核"）
 * @property {number} originalNovelId - 原始小说ID
 * @property {number} totalPrice - 总价格
 */

/**
 * 获取所有小说
 * @returns {Promise<Array<Novel>>} 返回小说数组
 */
export function getAllNovels() {
    return request({
        url: '/api/Novel',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取单个小说详情
 * @param {number} novelId - 小说ID
 * @returns {Promise<Novel>} 返回单个小说对象
 */
export function getNovel(novelId) {
    return request({
        url: `/api/Novel/${novelId}`,
        method: 'get'
    })
}


/**
 * 获取某作者小说详情
 * @param {number} authorId - 小说ID
 * @returns {Promise<Array<Novel>>} 返回小说数组
 */
export function getAuthorNovel(authorId) {
    return request({
        url: `/api/Author/${authorId}/novels`,
        method: 'get'
    })
}


/**
 * 创建小说
 * @param {Object} novelData - 小说数据
 * @param {string} novelData.novelName - 小说名称
 * @param {number} novelData.authorId - 作者ID
 * @param {string} [novelData.introduction] - 小说简介
 * @param {string} [novelData.coverUrl] - 封面URL
 * @param {number} [novelData.score=0] - 评分
 * @param {number} [novelData.totalWordCount=0] - 总字数
 * @param {number} [novelData.recommendCount=0] - 推荐数
 * @param {number} [novelData.collectedCount=0] - 收藏数
 * @param {string} [novelData.status="待审核"] - 状态
 * @param {number} [novelData.originalNovelId=-1] - 原始小说ID
 * @param {number} [novelData.totalPrice=0] - 总价格
 * @returns {Promise<Novel>}
 */
export function createNovel(novelData) {
    return request({
        url: '/api/Novel',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            novelName: novelData.novelName,
            authorId: novelData.authorId,
            introduction: novelData.introduction || null,
            coverUrl: novelData.coverUrl || null,
            score: novelData.score || 0,
            totalWordCount: novelData.totalWordCount || 0,
            recommendCount: novelData.recommendCount || 0,
            collectedCount: novelData.collectedCount || 0,
            status: novelData.status || "待审核",
            originalNovelId: novelData.originalNovelId || -1,
            totalPrice: novelData.totalPrice || 0
        }
    })
}


/**
 * 创建小说
 * @param {Object} novelData - 小说数据
 * @param {string} novelData.novelName - 小说名称
 * @param {number} novelData.authorId - 作者ID
 * @param {string} [novelData.introduction] - 小说简介
 * @returns {Promise<Novel>}
 */

export function AuthorcreateNovel(novelData) {
    // 转换为查询参数
    const params = new URLSearchParams();
    params.append('authorId', novelData.authorId);
    params.append('novelName', novelData.novelName);
    params.append('introduction', novelData.introduction);

    return request({
        url: `/api/novel/create?${params.toString()}`, // 参数拼接到URL
        method: 'post'
        // 移除headers和data
    })
}


/**
 * 更新小说
 * @param {number} novelId - 小说ID
 * @param {Object} updateData - 更新数据
 * @param {string} [updateData.novelName] - 小说名称
 * @param {number} [updateData.authorId] - 作者ID
 * @param {string} [updateData.introduction] - 小说简介
 * @param {string} [updateData.coverUrl] - 封面URL
 * @param {number} [updateData.score] - 评分
 * @param {number} [updateData.totalWordCount] - 总字数
 * @param {number} [updateData.recommendCount] - 推荐数
 * @param {number} [updateData.collectedCount] - 收藏数
 * @param {string} [updateData.status] - 状态
 * @param {number} [updateData.originalNovelId] - 原始小说ID
 * @param {number} [updateData.totalPrice] - 总价格
 * @returns {Promise<Novel>}
 */
export function updateNovel(novelId, updateData) {
    return request({
        url: `/api/Novel/${novelId}`,
        method: 'put',
        headers: {
            'Content-Type': 'application/json'
        },
        data: updateData
    })
}

/**
 * 删除小说
 * @param {number} novelId - 小说ID
 * @returns {Promise<void>}
 */
export function deleteNovel(novelId) {
    return request({
        url: `/api/Novel/${novelId}`,
        method: 'delete'
    })
}

/**
 * 审核小说
 * @param {number} novelId - 小说ID
 * @param {string} newStatus - 新状态（必须为"连载"或"完结"）
 * @param {number} managerId - 管理员ID
 * @param {string} result - 审核结果
 * @returns {Promise<{success: boolean, message: string}>}
 */
export function reviewNovel(novelId, newStatus, managerId, result) {
    return request({
        url: `/api/Novel/${novelId}/review`,
        method: 'put',
        params: {
            newStatus: newStatus,
            managerId: managerId,
            result: result
        }
    })
}

/**
 * 获取单个小说总字数
 * @param {number} novelId - 小说ID
 * @returns {Promise<{novelId: number, wordCount: number}>} 返回包含小说ID和字数的对象
 */
export function getNovelWordCount(novelId) {
    return request({
        url: `/api/Novel/wordcount/${novelId}`,
        method: 'get'
    })
}

/**
 * 获取小说推荐数
 * @param {number} novelId - 小说ID
 * @returns {Promise<{novelId: number, recommendCount: number}>} 返回包含小说ID和推荐数的对象
 */
export function getNovelRecommendCount(novelId) {
    return request({
        url: `/api/Novel/recommendcount/${novelId}`,
        method: 'get'
    })
}

/**
 * 获取小说收藏数
 * @param {number} novelId - 小说ID
 * @returns {Promise<{novelId: number, collectCount: number}>} 返回包含小说ID和收藏数的对象
 */
export function getNovelCollectCount(novelId) {
    return request({
        url: `/api/Novel/collectcount/${novelId}`,
        method: 'get'
    })
}

/**
 * 上传小说封面
 * @param {number} novelId - 小说ID
 * @param {File} file - 封面文件
 * @returns {Promise<{success: boolean, coverUrl: string}>}
 */
export function uploadNovelCover(novelId, file) {
    const formData = new FormData();
    formData.append('novelId', novelId);
    formData.append('coverFile', file);

    return request({
        url: '/api/Novel/UploadAvatar',
        method: 'post',
        headers: {
            'Content-Type': 'multipart/form-data'
        },
        data: formData
    })
}

/**
 * 提交小说修改
 * @param {number} originalNovelId - 原始小说ID
 * @param {Novel} editedNovel - 修改后的小说数据
 * @returns {Promise<{message: string, newNovelId: number}>}
 */
export function submitNovelEdit(novelId, updateData) {
    return request({
        url: `/api/Novel/submit-edit?originalNovelId=${novelId}`,
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            originalNovelId: novelId,
            edited: updateData
        }
    })
}
/**
 * 获取小说最新已发布章节信息
 * @param {number} novelId - 小说ID
 * @returns {Promise<{chapterId: number, publishTime: string}>}
 */
export function getLatestPublishedChapter(novelId) {
    return request({
        url: `/api/Novel/${novelId}/latest-published-chapter`,
        method: 'get'
    })
}

/**
 * 获取所有已发布的小说（状态为"连载"或"完结"）
 * @returns {Promise<Array<Novel>>} 返回已发布小说数组
 */
export function getPublishedNovels() {
    return request({
        url: '/api/Novel/published',
        method: 'get'
    })
}

/**
 * 获取分页的已发布小说
 * @param {number} page - 页码
 * @param {number} pageSize - 每页数量
 * @returns {Promise<Array<Novel>>} 返回分页小说数组
 */
export function getPaginatedNovels(page = 1, pageSize = 15) {
    return request({
        url: '/published/by-id',
        method: 'get',
        params: {
            page,
            pageSize
        }
    })
}

/**
 * 获取已发布小说（分页 + 条件筛选，按 NovelId 顺序）
 * @param {number} page - 页码
 * @param {number} pageSize - 每页数量
 * @param {string} [category] - 分类
 * @param {number} [minWordCount] - 最小字数
 * @param {number} [maxWordCount] - 最大字数
 * @param {boolean} [isFinished] - 是否完结
 * @returns {Promise<PagedResult<Novel>>} 返回分页结果
 */
export function getFilteredNovels(
    page = 1,
    pageSize = 10,
    category = null,
    minWordCount = null,
    maxWordCount = null,
    isFinished = null
) {
    return request({
        url: '/published/filter-by-id',
        method: 'get',
        params: {
            page,
            pageSize,
            category,
            minWordCount,
            maxWordCount,
            isFinished
        }
    })
}
