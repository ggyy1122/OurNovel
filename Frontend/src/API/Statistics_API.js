// src/API/Statistics_API.js
import request from '@/API/index'

/**
 * fetch a single stat endpoint and return { value, raw }
 */
async function fetchStatRaw(url) {
  try {
    const res = await request({
      url,
      method: 'get',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token') || sessionStorage.getItem('token')}`,
        Accept: 'application/json'
      }
    })
    // Try to find numeric value if API returns a bare number or common wrappers
    const payload = res && res.data !== undefined ? res.data : res
    // If payload is an object and contains our field names we will use in getAllStatistics
    return { value: payload, raw: res }
  } catch (err) {
    console.error(`fetchStatRaw ${url} error:`, err)
    return { value: null, raw: err && err.response ? err.response : err }
  }
}

/**
 * 单个接口的简便封装（如果你还需要单独调用）
 */
export async function getTotalNovelsRaw() {
  return fetchStatRaw('/api/Statistics/total-novels')
}
export async function getTotalAuthorsRaw() {
  return fetchStatRaw('/api/Statistics/total-authors')
}
export async function getTotalReadersRaw() {
  return fetchStatRaw('/api/Statistics/total-readers')
}
export async function getPendingChaptersRaw() {
  return fetchStatRaw('/api/Statistics/pending-chapters')
}
export async function getPendingNovelsRaw() {
  return fetchStatRaw('/api/Statistics/pending-novels')
}
export async function getPendingReportsRaw() {
  return fetchStatRaw('/api/Statistics/pending-reports')
}

/**
 * 获取所有统计项（并行），并返回标准化对象
 * 返回格式：
 * {
 *   totalNovels: number,
 *   totalAuthors: number,
 *   totalReaders: number,
 *   pendingChapters: number,
 *   pendingNovels: number,
 *   pendingReports: number,
 *   raw: { ...原始响应... }
 * }
 */
export async function getAllStatistics() {
  const [
    totalNovelsRes,
    totalAuthorsRes,
    totalReadersRes,
    pendingChaptersRes,
    pendingNovelsRes,
    pendingReportsRes
  ] = await Promise.all([
    getTotalNovelsRaw(),
    getTotalAuthorsRaw(),
    getTotalReadersRaw(),
    getPendingChaptersRaw(),
    getPendingNovelsRaw(),
    getPendingReportsRaw()
  ])

  // helper: try to extract field from payload (payload may be number or object)
  function extractField(payload, fieldName) {
    if (payload === null || payload === undefined) return 0
    // payload may be the axios response.data already or a primitive
    const p = payload && payload.data !== undefined ? payload.data : payload
    if (p === null || p === undefined) return 0
    // if p is a number/string parse
    if (typeof p === 'number') return p
    if (typeof p === 'string' && !isNaN(Number(p))) return Number(p)
    // if object and contains the exact field
    if (typeof p === 'object') {
      if (fieldName in p && (typeof p[fieldName] === 'number' || (typeof p[fieldName] === 'string' && !isNaN(Number(p[fieldName]))))) {
        return Number(p[fieldName])
      }
      // sometimes nested under data
      if (p.data && typeof p.data === 'object' && fieldName in p.data) {
        return Number(p.data[fieldName])
      }
      // common keys fallback:
      if (typeof p.totalCount === 'number') return p.totalCount
      if (typeof p.value === 'number') return p.value
      if (typeof p.count === 'number') return p.count
    }
    return 0
  }

  const totalNovels = extractField(totalNovelsRes.value, 'totalNovels')
  const totalAuthors = extractField(totalAuthorsRes.value, 'totalAuthors')
  const totalReaders = extractField(totalReadersRes.value, 'totalReaders')
  const pendingChapters = extractField(pendingChaptersRes.value, 'pendingChapters')
  const pendingNovels = extractField(pendingNovelsRes.value, 'pendingNovels')
  const pendingReports = extractField(pendingReportsRes.value, 'pendingReports')

  return {
    totalNovels,
    totalAuthors,
    totalReaders,
    pendingChapters,
    pendingNovels,
    pendingReports,
    raw: {
      totalNovelsRaw: totalNovelsRes.raw,
      totalAuthorsRaw: totalAuthorsRes.raw,
      totalReadersRaw: totalReadersRes.raw,
      pendingChaptersRaw: pendingChaptersRes.raw,
      pendingNovelsRaw: pendingNovelsRes.raw,
      pendingReportsRaw: pendingReportsRes.raw
    }
  }
}