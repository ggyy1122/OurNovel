// User_API.js
import request from '@/API/index'


/**
 * @typedef {Object} Reader
 * @property {number} readerId
 * @property {string} readerName
 * @property {string} phone
 * @property {string} gender
 * @property {string} avatarUrl
 * @property {string} backgroundUrl
 * @property {string} isCollectVisible
 * @property {string} isRecommendVisible
 * @property {number} balance
 */

/**
 * @typedef {Object} Author
 * @property {number} authorId
 * @property {string} authorName
 * @property {string} phone
 * @property {string} avatarUrl
 * @property {number} earning
 */

/**
 * 获取所有读者
 * @returns {Promise<Reader[]>}
 */
export function getAllReaders() {
  return request({
    url: '/api/Reader',
    method: 'get',
    headers: {
      Authorization: `Bearer ${localStorage.getItem('token') || sessionStorage.getItem('token')}`
    }
  })
}
//
/**
 * 获取所有作者
 * @returns {Promise<Author[]>}
 */
export function getAllAuthors() {
  return request({
    url: '/api/Author',
    method: 'get',
    headers: {
      Authorization: `Bearer ${localStorage.getItem('token') || sessionStorage.getItem('token')}`
    }
  })
}

/**
 * 删除读者
 * @param {number} readerId
 * @returns {Promise<{message: string}>}
 */
export function deleteReader(readerId) {
  return request({
    url: `/api/Reader/${readerId}`,
    method: 'delete',
    headers: {
      Authorization: `Bearer ${localStorage.getItem('token') || sessionStorage.getItem('token')}`
    }
  })
}



/**
 * 删除作者及其所有小说
 * @param {number} authorId 
 * @returns {Promise<void>}
 */
export async function deleteAuthorWithNovels(authorId) {
  try {
    // 先获取作者所有小说
    const novelsRes = await request({
      url: `/api/Author/${authorId}/novels`,
      method: 'get',
    })
    const novels = novelsRes.data || []

    // 如果有小说，则循环删除每本小说
    for (const novel of novels) {
      const novelId = novel.id || novel.novelId
      if (novelId) {
        await request({
          url: `/api/Novel/${novelId}`,
          method: 'delete',
        })
      }
    }

    // 删除作者
    await request({
      url: `/api/Author/${authorId}`,
      method: 'delete',
    })

  } catch (error) {
    console.error('删除作者及其小说失败:', error)
    throw error
  }
}
