// src/stores/Chapters.js
import { ref, computed, onMounted } from 'vue'
//import { useRoute } from 'vue-router'
import { chaptersStore } from '@/stores/Chapters'

export function useChapters(novelId) {
  //const route = useRoute()
  
  // 章节数据状态
  const chapters = ref([])
  const activeChapterId = ref(null)
  const searchQuery = ref('')
  const showDeleteConfirm = ref(false)
  const chapterToDelete = ref(null)
  const isLoading = ref(false)
  const error = ref(null)

  // 计算属性
  const activeChapter = computed(() => 
    chapters.value.find(ch => ch.chapter_id === activeChapterId.value)
  )
  
  const filteredChapters = computed(() => 
    chapters.value.filter(ch => 
      ch.title.toLowerCase().includes(searchQuery.value.toLowerCase())
    )
  )
  
  const canChangeStatus = computed(() => {
    if (!activeChapter.value) return false
    const status = activeChapter.value.status
    return status === '草稿' || status === '封禁' || status === '已发布'
  })
 
  // 获取章节列表
  const fetchChapters = async () => {
    isLoading.value = true
    error.value = null
    try {
      const fetchedChapters = await chaptersStore.getChaptersByNovelId(novelId)
      chapters.value = fetchedChapters
      
      // 默认选择第一个章节
      if (chapters.value.length > 0) {
        activeChapterId.value = chapters.value[0].chapter_id
      }
    } catch (err) {
      error.value = '获取章节列表失败: ' + err.message
      console.error(err)
    } finally {
      isLoading.value = false
    }
  }

  // 保存章节
  const saveChapter = async () => {
    if (!activeChapter.value) return
    
    try {
      await chaptersStore.updateChapter(activeChapter.value)
      
      // 更新本地数据
      const index = chapters.value.findIndex(ch => ch.chapter_id === activeChapterId.value)
      if (index !== -1) {
        chapters.value.splice(index, 1, {...activeChapter.value})
      }
    } catch (err) {
      console.error('保存章节失败:', err)
      throw err
    }
  }

  // 创建新章节
  const createNewChapter = () => {
    const maxId = chapters.value.length > 0 
      ? Math.max(...chapters.value.map(c => c.chapter_id)) 
      : 0
    
    const newChapter = {
      novel_id: novelId,
      chapter_id: maxId + 1,
      title: '新章节',
      content: '',
      word_count: 0,
      price_per_kilo: 0.50,
      calculated_price: 0,
      is_charged: '是',
      status: '草稿',
      publish_time: ''
    }
    
    chapters.value.unshift(newChapter)
    activeChapterId.value = newChapter.chapter_id
  }

  // 切换章节状态
  const toggleChapterStatus = async () => {
    if (!activeChapter.value || !canChangeStatus.value) return
    
    const currentStatus = activeChapter.value.status
    let newStatus = currentStatus
    
    if (currentStatus === '草稿') newStatus = '审核中'
    else if (currentStatus === '封禁') newStatus = '审核中'
    else if (currentStatus === '已发布') newStatus = '草稿'
    
    try {
      await chaptersStore.updateChapterStatus(
        activeChapter.value.chapter_id, 
        newStatus
      )
      
      activeChapter.value.status = newStatus
      
      if (currentStatus === '草稿' && newStatus === '审核中') {
        activeChapter.value.publish_time = new Date().toISOString().split('T')[0]
      }
    } catch (err) {
      console.error('更新章节状态失败:', err)
      throw err
    }
  }

  // 删除章节
  const deleteChapter = async () => {
    if (!chapterToDelete.value) return
    
    try {
      await chaptersStore.deleteChapter(chapterToDelete.value.chapter_id)
      
      chapters.value = chapters.value.filter(
        ch => ch.chapter_id !== chapterToDelete.value.chapter_id
      )
      
      if (activeChapterId.value === chapterToDelete.value.chapter_id) {
        activeChapterId.value = null
      }
      
      showDeleteConfirm.value = false
      chapterToDelete.value = null
    } catch (err) {
      console.error('删除章节失败:', err)
      throw err
    }
  }

  // 字数统计
  const updateWordCount = () => {
    if (activeChapter.value) {
      const chineseChars = activeChapter.value.content.match(/[\u4e00-\u9fa5]/g) || []
      const otherWords = activeChapter.value.content.replace(/[\u4e00-\u9fa5]/g, '')
        .split(/\s+/)
        .filter(word => word.length > 0)
      
      activeChapter.value.word_count = chineseChars.length + otherWords.length
      updateCalculatedPrice()
    }
  }

  // 价格计算
  const updateCalculatedPrice = () => {
    if (activeChapter.value) {
      activeChapter.value.calculated_price = 
        Math.round((activeChapter.value.word_count / 1000) * 
        activeChapter.value.price_per_kilo * 100) / 100
    }
  }

  // 初始化
  onMounted(() => {
    fetchChapters()
  })

  return {
    // 状态
    chapters,
    activeChapterId,
    searchQuery,
    showDeleteConfirm,
    chapterToDelete,
    isLoading,
    error,
    
    // 计算属性
    activeChapter,
    filteredChapters,
    canChangeStatus,
    
    // 方法
    fetchChapters,
    saveChapter,
    createNewChapter,
    toggleChapterStatus,
    deleteChapter,
    updateWordCount,
    updateCalculatedPrice,
    
    // 工具方法
    getStatusText: (status) => {
      const statusMap = {
        '草稿': '草稿',
        '审核中': '审核中',
        '封禁': '封禁',
        '已发布': '已发布'
      }
      return statusMap[status] || status
    },
    
    getStatusButtonText: (status) => {
      const textMap = {
        '草稿': '提交审核',
        '审核中': '审核中',
        '封禁': '申请解封',
        '已发布': '撤回为草稿'
      }
      return textMap[status] || status
    },
    
    formatDate: (dateString) => dateString || '未发布'
  }
}