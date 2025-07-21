import request from '@/API/index'

/**
 * 获取收藏榜单
 * @param {number} [topN=10] - 前几名，默认为10
 * @returns {Promise<Array<CollectRankingDto>>} 返回收藏榜单
 */
export function getCollectRanking(topN = 10) {
    return request({
        url: '/api/Ranking/collect',
        method: 'get',
        params: {
            topN
        },
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取推荐榜单
 * @param {number} [top=10] - 前几名，默认为10
 * @returns {Promise<Array<RecommendRankingDto>>} 返回推荐榜单
 */
export function getRecommendRanking(top = 10) {
    return request({
        url: '/api/Ranking/recommend',
        method: 'get',
        params: {
            top
        },
        headers: {
            'Accept': 'application/json'
        }
    })
}

/**
 * 获取评分榜单
 * @param {number} [top=10] - 前几名，默认为10
 * @returns {Promise<Array<ScoreRankingDto>>} 返回评分榜单
 */
export function getScoreRanking(top = 10) {
    return request({
        url: '/api/Ranking/score',
        method: 'get',
        params: {
            top
        },
        headers: {
            'Accept': 'application/json'
        }
    })
}