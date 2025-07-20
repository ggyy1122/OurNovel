// src/stores/CurrentNovel.js
import { ref, onMounted, onUnmounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { novelsStore } from '@/stores/Novels'

export function useNovel() {
  const route = useRoute()
  const router = useRouter()
  
  // 共享的小说数据状态
  const novel = ref({
    novel_id: 0,
    novel_name: '',
    cover_url: 'https://via.placeholder.com/300x400?text=No+Cover',
    status: '',
    category: '',
    total_word_count: 0,
    chapter_count: 0,
    score: 0,
    introduction: '',
    view_count: 0,
    collected_count: 0,
    recommend_count: 0,
  })
  
  // 共享的状态变量
  const editMode = ref(false)
  const showDeleteDialog = ref(false)
  const fileInput = ref(null)
  let isMounted = false

  // 共享的常量
  const statusOptions = [
    { value: 'serializing', label: '连载中' },
    { value: 'completed', label: '已完结' }
  ]

  // 共享的方法 - 数据获取
  const fetchNovel = async () => {
    try {
      const novelId = parseInt(route.params.id)
      if (isNaN(novelId)) {
        throw new Error('无效的小说ID')
      }
      
      const fetchedNovel = novelsStore.getNovelById(novelId)
      
      if (!fetchedNovel) {
        throw new Error('找不到该小说')
      }
      
      if (isMounted) {
        novel.value = fetchedNovel
      }
    } catch (err) {
      console.error(err)
      router.push('/novels')
    }
  }

  // 共享的方法 - 编辑相关
  const enterEditMode = () => {
    editMode.value = true
  }

  const cancelChanges = () => {
    editMode.value = false
    fetchNovel() // 重新获取原始数据
  }

  // 共享的方法 - 图片上传
  const triggerFileInput = () => {
    fileInput.value?.click()
  }

  const handleFileChange = (e) => {
    const file = e.target.files[0]
    if (file) {
      validateImage(file)
    }
  }

  const validateImage = (file) => {
    const validTypes = ['image/jpeg', 'image/png']
    if (!validTypes.includes(file.type)) {
      alert('只支持JPG/PNG格式图片')
      return
    }
    
    const maxSize = 5 * 1024 * 1024
    if (file.size > maxSize) {
      alert('图片大小不能超过5MB')
      return
    }
    
    const reader = new FileReader()
    reader.onload = (e) => {
      novel.value.cover_url = e.target.result
    }
    reader.readAsDataURL(file)
  }

  const handleImageError = (e) => {
    e.target.src = 'https://via.placeholder.com/300x400?text=封面加载失败'
  }

  // 共享的方法 - 删除相关
  const confirmDelete = () => {
    showDeleteDialog.value = true
  }

  const cancelDelete = () => {
    showDeleteDialog.value = false
  }

  const deleteNovel = () => {
    if (!isMounted) return
    
    console.log('删除小说:', novel.value.novel_id)
    showDeleteDialog.value = false
    router.push('/author/novels')
  }

  // 共享的方法 - 保存数据
  const saveNovel = () => {
    console.log('保存小说信息:', novel.value)
    editMode.value = false
  }

  // 生命周期钩子
  onMounted(() => {
    isMounted = true
    fetchNovel()
  })

  onUnmounted(() => {
    isMounted = false
  })

  return {
    // 状态
    novel,
    editMode,
    showDeleteDialog,
    fileInput,
    
    // 常量
    statusOptions,
    
    // 方法
    fetchNovel,
    enterEditMode,
    cancelChanges,
    triggerFileInput,
    handleFileChange,
    handleImageError,
    confirmDelete,
    cancelDelete,
    deleteNovel,
    saveNovel
  }
}