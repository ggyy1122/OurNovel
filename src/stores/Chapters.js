// src/stores/Chapters.js
import { defineStore } from 'pinia'
import { getChaptersByNovel, deleteChapter,createChapter,updateChapter } from '@/API/Chapter_API'

// 状态配置
export const statusConfig = {
  '草稿': {
    text: '草稿',
    actionText: '提交审核',
    nextStatus: '首次审核', 
    color: '#888'
  },
  '首次审核': {
    text: '首次审核',
    actionText: '审核中',
    nextStatus: null, 
    color: '#FFA500'
  },
  '审核中': {
    text: '审核中',
    actionText: '审核中',
    nextStatus: null, 
    color: '#FFA500'
  },
  '封禁': {
    text: '封禁',
    actionText: '申请解封',
    nextStatus: '审核中', 
    color: '#FF0000'
  },
  '已发布': {
    text: '已发布',
    actionText: '提交更改', 
    nextStatus: '审核中',
    color: '#4CAF50'
  }
}

export const ChaptersStore = defineStore('chaptersStore', {
  state: () => ({
    novelId:null,
    chapters: [],
    activeChapterId: null,
    searchQuery: '',
    showDeleteConfirm: false,
    chapterToDelete: null,
    isLoading: false,
    error: null,
    isSaving: false,
    currentNovelId: null
  }),
  
  getters: {
    activeChapter(state) {
    if (!state.activeChapterId || !Array.isArray(state.chapters)) return null
    
    return state.chapters.find(ch => 
      ch.chapter_id === state.activeChapterId
    )
  },
    
    filteredChapters: (state) => {
      const query = state.searchQuery.toLowerCase()
      return state.chapters.filter(ch => 
        ch.title.toLowerCase().includes(query) ||
        (ch.content && ch.content.toLowerCase().includes(query))
      )
    },
    
    canChangeStatus: (state) => {
      if (!state.activeChapter) return false
      return ['草稿', '封禁', '已发布'].includes(state.activeChapter.status)
    }
  },
  
  actions: {
    async initialize(novelId) {
      if (!novelId) throw new Error('必须提供 novelId')
      this.novelId = novelId
      await this.fetchChapters(novelId)
    },

    setNovelId(novelId) {
      this.novelId = novelId;
    },
    
   async fetchChapters(novelId) {
      this.chapters = []
      this.activeChapterId = null
      this.novelId = novelId
      this.isLoading = true
      this.error = null

      try {
        const chaptersData = await getChaptersByNovel(novelId)
        console.log('API返回数据:', chaptersData)
        
        this.chapters = chaptersData.map(chapter => ({
          novel_id: chapter.novelId,      
          chapter_id: chapter.chapterId,  
          title: chapter.title,            
          content: chapter.content,
          word_count: chapter.wordCount,  
          price_per_kilo: chapter.pricePerKilo,
          calculated_price: chapter.calculatedPrice,
          is_charged: chapter.isCharged,    
          publish_time: chapter.publishTime,
          status: chapter.status
        }))

        // 确保有章节时才设置活动章节
        if (this.chapters.length > 0) {
          this.activeChapterId = this.chapters[0].chapter_id
        }
      } catch (err) {
        this.error = `获取章节失败: ${err.message}`
        console.error('错误详情:', {
          novelId: this.novelId,
          error: err.message
        })
      } finally {
        this.isLoading = false
      }
    },
    $resetState() {
      this.chapters = [];
      this.activeChapterId = null;
      this.searchQuery = '';
      this.error = null;
    },

    // 设置当前活动章节
    
    setActiveChapter(chapterId) {
      if (this.chapters.some(ch => ch.chapter_id === chapterId)) {
        this.activeChapterId = chapterId
      } else {
        console.warn(`章节ID ${chapterId} 不存在`)
      }
    },

    
    async saveChapter() {
      if (!this.activeChapter) {
        console.error('没有选中的章节');
        return;
      }
      const novelId = Number(this.novelId || 
                    this.novel?.novel_id || 
                    this.activeChapter?.novel_id || 
                    this.$route.params.novelId);
      this.isSaving = true;
      try {
        this.updateCalculatedPrice();
        
        // 更新的数据
        const updateData = {
          novelId,       
          chapterId:this.activeChapter.chapter_id,
          title: this.activeChapter.title,
          content: this.activeChapter.content,
          wordCount: this.activeChapter.word_count,
          pricePerKilo: this.activeChapter.price_per_kilo,
          isCharged: this.activeChapter.is_charged,
          publishTime:this.activeChapter.publish_time,
          status: this.activeChapter.status
        };
        // 有发布时间且状态是已发布
        if (this.activeChapter.status === '已发布' && this.activeChapter.publish_time) {
          updateData.publishTime = this.activeChapter.publish_time;
        }
        // 调用API更新章节
        const response = await updateChapter(
          novelId,
          this.activeChapter.chapter_id,
          updateData
        );
        
        // 更新本地章节数据
        const index = this.chapters.findIndex(
          ch => ch.chapter_id === this.activeChapter.chapter_id
        );
        if (index !== -1) {
          this.chapters[index] = { ...this.chapters[index], ...response };
        }
        this.$patch({
        activeChapter: { 
          ...this.activeChapter, 
          ...response 
        }
      });
        
        // 显示成功消息
        this.error = null;
        console.log('章节保存成功');
        return true;
      } catch (err) {
        this.error = `保存失败: ${err.message || '服务器错误'}`;
        console.error('保存章节失败:', err);
        throw err;
      } finally {
        this.isSaving = false;
      }
    },

  // 创建新章节
   async createNewChapter(novelId) {
      try {
        console.log('当前 novelId:', novelId);
        let nextChapterId = 1;
        const existingIds = this.chapters.map(ch => ch.chapter_id).sort((a, b) => a - b);
        
        for (const id of existingIds) {
          if (id > nextChapterId) break; // 找到空缺位置
          nextChapterId = id + 1;
        }
        const newChapter = {
          novel_id: novelId,
          title: `第${nextChapterId}章 `,
          content: null,
          word_count: 0,
          price_per_kilo: 50,
          calculated_price: 0,
          is_charged: "是",
          status: "草稿",
          publish_time: null 
        }

        // API
        const response = await createChapter({
          novelId:novelId,
          chapterId: nextChapterId, 
          title: newChapter.title,
          content: newChapter.content,
          wordCount: newChapter.word_count,
          pricePerKilo: newChapter.price_per_kilo,
          calculatedPrice:newChapter.calculated_price,
          isCharged: newChapter.is_charged,
          publishTime: newChapter.publish_time, 
          status: newChapter.status
        });
        await this.fetchChapters(novelId);
        
        // 设置活动章节
        this.activeChapterId = response.chapterId

        return response.chapterId

      } catch (err) {
        this.error = `创建失败: ${err.message || '服务器错误'}`
        console.error('创建章节失败:', {
          error: err.message
        })
        throw err
      }
    },
    
    async toggleChapterStatus() {
      if (!this.activeChapter || !this.canChangeStatus) return;
      
      const currentStatus = this.activeChapter.status;
      const config = statusConfig[currentStatus];
      
      try {
        const newStatus = config.nextStatus;
        
        this.activeChapter.status = newStatus;       
        // 调用saveChapter函数保存更改
        await this.saveChapter();
        
        return newStatus;
      } catch (err) {
        this.error = `状态更新失败: ${err.message || '服务器错误'}`;
        console.error('更新章节状态失败:', err);
        throw err;
      }
    },
    
    
    async deletethisChapter(novelId, chapterId) {
      
      if (!novelId || !chapterId) {
        throw new Error(`参数缺失: novelId=${novelId}, chapterId=${chapterId}`);
      }
      try {
        await deleteChapter(novelId, chapterId);
        this.chapters = this.chapters.filter(ch => ch.chapter_id !== chapterId);
        
        if (this.activeChapterId === chapterId) {
          this.activeChapterId = this.chapters[0]?.chapter_id || null;
        }
        return true;
      } catch (err) {
        console.error('删除失败:', {
          novelId: novelId,
          chapterId: chapterId,
          error: err
        });
        throw err;
      } finally {
        this.showDeleteConfirm = false;
        this.chapterToDelete = null;
      }
    },

    prepareDeleteChapter(chapter) {
      
      // 确保深拷贝且类型正确
      this.chapterToDelete = {
        ...chapter,
        chapter_id: chapter.chapter_id 
      };
      this.showDeleteConfirm = true;
    },

    //总价格计算
    updateCalculatedPrice() {
      if (!this.activeChapter) return
      this.activeChapter.calculated_price = 
        Math.round((this.activeChapter.word_count / 1000) * 
        this.activeChapter.price_per_kilo )
    },
    
    // 更新字数
    updateWordCount() {
      if (!this.activeChapter) return
      
      const content = this.activeChapter.content || ''
      const chineseChars = content.match(/[\u4e00-\u9fa5]/g) || []
      const otherWords = content.replace(/[\u4e00-\u9fa5]/g, '')
        .split(/\s+/)
        .filter(word => word.length > 0)
      
      this.activeChapter.word_count = chineseChars.length + otherWords.length
      this.updateCalculatedPrice()
    },
    
    getStatusText(status) {
      return statusConfig[status]?.text || status
    },
    
    getStatusButtonText(status) {
      return statusConfig[status]?.actionText || status
    },
    
    formatDate(isoString) {
      if (!isoString) return '未发布'
      const date = new Date(isoString)
      return date.toLocaleString('zh-CN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
      }).replace(/\//g, '-')
    }
  }
})