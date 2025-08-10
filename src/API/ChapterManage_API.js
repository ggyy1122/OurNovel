
import request from '@/API/index'

/**
 * @typedef {Object} Chapter
 * @property {number} novelId
 * @property {number} chapterId
 * @property {string} title
 * @property {string} content
 * @property {number} wordCount
 * @property {number} pricePerKilo
 * @property {string} isCharged
 * @property {string} publishTime
 * @property {string} status
 */
//
// 获取所有章节（包括首次审核、审核中、已发布等）
export function getAllChapters() {
  return request({
    url: '/api/Chapter',
    method: 'get',
    headers: {
      Authorization: `Bearer ${localStorage.getItem('token') || sessionStorage.getItem('token')}`
    }
  })
}

/**
 * 审核某章节，通过或封禁
 * @param {number} novelId
 * @param {number} chapterId
 * @param {'已发布' | '封禁'} newStatus 审核状态
 * @returns {Promise<{message: string}>}
 */
/**
* 获取指定章节详情
 * @param {number} novelId
 * @param {number} chapterId
 * @returns {Promise<Chapter>}
 */
export function reviewChapter(novelId, chapterId, newStatus, managerId, result) {
  return request({
    url: `/api/Chapter/novels/${novelId}/chapters/${chapterId}/review`,
    method: 'put',
    headers: {
      Authorization: `Bearer ${localStorage.getItem('token') || sessionStorage.getItem('token')}`
    },
    params: {     // 用 params 传查询参数
      newStatus,
      managerId,
      result
    }
  })
}



export function getChapterDetail(novelId, chapterId) {
  return request({
    url: `/api/Chapter/novels/${novelId}/chapters/${chapterId}`,
    method: 'get',
    headers: {
      Authorization: `Bearer ${localStorage.getItem('token') || sessionStorage.getItem('token')}`
    }
  })
}