import request from '@/API/index'

/**
 * 获取所有读者
 * @returns {Promise<Array<Reader>>} 返回读者数组
 * @typedef {Object} Reader  
 * @property {number} readerId - 读者ID
 * @property {string} readerName - 读者名称
 * @property {string} password - 密码
 * @property {string|null} phone - 电话
 * @property {string|null} gender - 性别 ("男" 或 "女")
 * @property {number} balance - 余额
 * @property {string|null} avatarUrl - 头像URL
 * @property {string|null} backgroundUrl - 背景URL
 * @property {string|null} isCollectVisible - 收藏是否可见 ("是"/"否")
 * @property {string|null} isRecommendVisible - 推荐是否可见 ("是"/"否")
 */
export function getAllReaders() {
    return request({
        url: '/api/Reader',
        method: 'get',
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取单个读者详情
 * @param {number} readerId - 参数：读者ID
 * @returns {Promise<Reader>} - 返回单个读者对象
 */
export function getReader(readerId) {
    return request({
        url: `/api/Reader/${readerId}`,
        method: 'get'
    })
}

/**
 * 创建读者
 * @param {Object} readerData - 读者数据
 * @param {string} readerData.readerName - 读者名称
 * @param {string} readerData.password - 密码
 * @param {string} [readerData.phone] - 电话
 * @param {string} [readerData.gender] - 性别 ("男" 或 "女")
 * @param {number} [readerData.balance=0] - 余额
 * @param {string} [readerData.avatarUrl] - 头像URL
 * @param {string} [readerData.backgroundUrl] - 背景URL
 * @param {string} [readerData.isCollectVisible] - 收藏是否可见 ("是"/"否")
 * @param {string} [readerData.isRecommendVisible] - 推荐是否可见 ("是"/"否")
 * @returns {Promise<Reader>}
 */
export function createReader(readerData) {
    return request({
        url: '/api/Reader',
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        data: {
            readerName: readerData.readerName,
            password: readerData.password,
            phone: readerData.phone || null,
            gender: readerData.gender || null,
            balance: readerData.balance || 0,
            avatarUrl: readerData.avatarUrl || null,
            backgroundUrl: readerData.backgroundUrl || null,
            isCollectVisible: readerData.isCollectVisible || null,
            isRecommendVisible: readerData.isRecommendVisible || null
        }
    })
}

/**
 * 更新读者
 * @param {number} readerId - 读者ID
 * @param {Object} updateData - 更新数据
 * @param {string} [updateData.readerName] - 读者名称
 * @param {string} [updateData.password] - 密码
 * @param {string} [updateData.phone] - 电话
 * @param {string} [updateData.gender] - 性别 ("男" 或 "女")
 * @param {number} [updateData.balance] - 余额
 * @param {string} [updateData.avatarUrl] - 头像URL
 * @param {string} [updateData.backgroundUrl] - 背景URL
 * @param {string} [updateData.isCollectVisible] - 收藏是否可见 ("是"/"否")
 * @param {string} [updateData.isRecommendVisible] - 推荐是否可见 ("是"/"否")
 * @returns {Promise<Reader>}
 */
export function updateReader(readerId, updateData) {
    return request({
        url: `/api/Reader/${readerId}`,
        method: 'put',
        headers: {
            'Content-Type': 'application/json'
        },
        data: updateData
    })
}

/**
 * 删除读者
 * @param {number} readerId - 读者ID
 * @returns {Promise<void>}
 */
export function deleteReader(readerId) {
    return request({
        url: `/api/Reader/${readerId}`,
        method: 'delete'
    })
}

/**
 * 上传读者头像
 * @param {number} readerId - 读者ID
 * @param {FormData} avatarFile - 头像文件
 * @returns {Promise<{success: boolean, avatarUrl: string}>}
 */
export function uploadReaderAvatar(readerId, avatarFile) {
    const formData = new FormData();
    formData.append('readerId', readerId);
    formData.append('avatarFile', avatarFile);

    return request({
        url: '/api/Reader/UploadAvatar',
        method: 'post',
        headers: {
            'Content-Type': 'multipart/form-data'
        },
        data: formData
    })
}

/** 
 * 获取读者余额
 * @param {number} readerId - 读者ID
 * @returns {Promise<{ Balance: number, Currency: string }>}
 */
export function getReaderBalance(readerId) {
    return request({
        url: `/api/Reader/${readerId}/balance`,
        method: 'get'
    })
}

/**
 * 获取指定读者的举报及管理日志
 * @param {number} readerId 
 * @returns {Promise<any>}
 */
export function getReportsWithLogsByReader(readerId) {
    return request({
        url: `/api/ReportManagement/reader/${readerId}/reports-with-logs`,
        method: 'get'
    })
}

/**
 * 获取指定读者的最近阅读记录列表，按时间降序排列
 * @param {number} readerId
 * @returns {Promise<any>} 
 */
export function getRecentReadingsByReaderId(readerId) {
    return request({
        url: '/api/RecentReadings/list',
        method: 'get',
        params: { readerId }
    })
}

/**
 * 获取读者最近阅读的章节ID
 * @param {number} readerId - 读者ID
 * @param {number} novelId - 小说ID
 * @returns {Promise<any>} 最近阅读的章节ID，如果没有记录则返回1（首章）
 */
export function getLastReadChapterId(readerId, novelId) {
    return request({
        url: '/api/RecentReadings/last-read-chapter',
        method: 'get',
        params: {
            readerId,
            novelId
        }
    });
}

/**
 * 删除指定读者的某本小说的最近阅读记录
 * @param {number} readerId - 读者 ID
 * @param {number} novelId - 小说 ID
 * @returns {Promise<any>}
 */
export function deleteRecentReading(readerId, novelId) {
    return request({
        url: '/api/RecentReadings/delete',
        method: 'delete',
        params: {
            readerId,
            novelId
        }
    });
}

/**
 * 添加或更新指定读者的某本小说的最近阅读记录
 * @param {number} readerId - 读者 ID
 * @param {number} novelId - 小说 ID
 * @param {number} chapterId - 章节 ID（可选，默认为1）
 * @returns {Promise<any>}
 */
export function addOrUpdateRecentReading(readerId, novelId, chapterId = 1) {
    return request({
        url: '/api/RecentReadings/add-or-update',
        method: 'post',
        params: {
            readerId,
            novelId,
            chapterId
        }
    });
}