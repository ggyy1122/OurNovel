// src/stores/Novels.js
import { reactive } from 'vue'
import { getAuthorNovel } from '@/API/Novel_API'
import { authorStore} from './CurrentAuthor'

export const novelsStore = reactive({
  novels: [],
  isLoading: false,
  error: null,
  shouldRefresh: false,
  async fetchNovels() {
    this.isLoading = true
    this.error = null
    
    try {
      const response = await getAuthorNovel(authorStore.currentAuthor.author_id)
      console.log('API响应:', response)
      
      const rawData = Array.isArray(response) ? response : response?.data || []
      
      this.novels = rawData.map(novel => ({
        novel_id: novel.novelId,
        author_id: novel.authorId,
        novel_name: novel.novelName || '未命名小说',
        cover_url: this.getFullCoverUrl(novel.coverUrl), 
        status: this.mapStatus(novel.status),
        introduction: novel.introduction || '暂无简介',
        total_word_count: novel.totalWordCount || 0,
        score: novel.score || 0,
        create_time: novel.createTime,
        recommend_count: novel.recommendCount || 0,
        collected_count: novel.collectedCount || 0,
        total_price:novel.totalPrice
      }))
      
    } catch (error) {
      console.error('获取小说列表失败:', error)
      this.error = error
      this.novels = [{
        novel_id: 0,
        novel_name: '数据加载失败',
        cover_url: 'https://picsum.photos/300/400?error',
        status: '错误'
      }]
    } finally {
      this.isLoading = false
    }
  },

  // 统一的封面URL处理方法
  getFullCoverUrl(partialPath) {
    if (!partialPath) {
      // 随机默认封面
      return 'https://picsum.photos/300/400?random=' + Math.floor(Math.random() * 1000)
    }
    
    // 如果已经是完整URL，直接返回
    if (partialPath.startsWith('http')) return partialPath
    const ossBase = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/'
    return `${ossBase}${partialPath.replace(/^\//, '')}`
  },

  // 状态映射方法
  mapStatus(status) {
    const statusMap = {
      '连载': '连载',
      '完结': '完结',
      '待审核': '待审核',
      '封禁': '封禁'
    }
    return statusMap[status] || status || '未知状态'
  },

  getNovelById(id) {
    return this.novels.find(novel => novel.novel_id === id) || {
      novel_id: -1,
      novel_name: '未知小说',
      cover_url: 'https://picsum.photos/300/400?random=999',
      status: '未知',
      introduction: '暂无简介',
      total_word_count: 0,
      chapter_count: 0,
      score: 0
    }
  },

  async removeNovel(id) {
    try {
      this.novels = this.novels.filter(novel => novel.novel_id !== id)
    } catch (error) {
      console.error('删除小说失败:', error)
      throw error
    }
  }
})