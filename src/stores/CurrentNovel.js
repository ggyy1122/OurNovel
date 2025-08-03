// src/stores/CurrentNovel.js
import { ref, onMounted, onUnmounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { authorStore } from './CurrentAuthor'
import { novelsStore } from '@/stores/Novels'
import { deleteNovel, submitNovelEdit } from '@/API/Novel_API'
import { getCategoriesByNovel } from '@/API/NovelCategory_API'
import { uploadNovelCover } from '@/API/Novel_Cover_API'
import { getCommentCountByNovel } from '@/API/Comment_API'

export function useNovel() {
  const route = useRoute()
  const router = useRouter()
  
  // 共享的小说数据状态
  const novel = ref({
    novel_id: 0,
    novel_name: '',
    cover_url: '',
    status: '',
    categories: [], 
    total_word_count: 0,
    chapter_count: 0,
    score: 0,
    introduction: '',
    view_count: 0,
    collected_count: 0,
    recommend_count: 0,
    comment_count: 0 
  })
  
  // 共享的状态变量
  const editMode = ref(false)
  const showDeleteDialog = ref(false)
  const fileInput = ref(null)
  const isDeleting = ref(false)
  const deleteError = ref(null)
  const isLoadingCategories = ref(false)
  const categoriesError = ref(null)
  const isSavingCategories = ref(false)
  const isLoadingComments = ref(false)
  const commentsError = ref(null)
  let isMounted = false

  // 共享的常量
  const statusOptions = [
    { value: '连载', label: '连载' },
    { value: '完结', label: '完结' },
    { value: '待审核', label: '待审核' },
    { value: '封禁', label: '封禁' }
  ]

  // 获取小说评论数
  const fetchCommentCount = async (novelId) => {
    try {
      isLoadingComments.value = true
      commentsError.value = null
      const response = await getCommentCountByNovel(novelId)
      
      if (isMounted) {
        novel.value.comment_count = response.commentCount || 0
      }
    } catch (error) {
      console.error('获取评论数失败:', error)
      commentsError.value = error.message || '获取评论数失败'
    } finally {
      isLoadingComments.value = false
    }
  }

  // 获取小说分类
  const fetchCategories = async (novelId) => {
    try {
      isLoadingCategories.value = true
      categoriesError.value = null
      const categoriesData = await getCategoriesByNovel(novelId)
      
      if (isMounted) {
        novel.value.categories = categoriesData.map(item => item.categoryName)
      }
    } catch (error) {
      console.error('获取分类失败:', error)
      categoriesError.value = error.message || '获取分类失败'
    } finally {
      isLoadingCategories.value = false
    }
  }

  // 数据获取
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
        novel.value = {
          ...fetchedNovel,
          cover_url: fetchedNovel.cover_url || 'https://ftp.bmp.ovh/imgs/2019/11/06800705be93b1bb.png',
          categories: [],
          comment_count: 0
        }
        // 并行获取分类和评论数
        await Promise.all([
          fetchCategories(novelId),
          fetchCommentCount(novelId)
        ])
      }
    } catch (err) {
      console.error(err)
      router.push('/author/novels')
    }
  }

  // 编辑相关方法
  const enterEditMode = () => {
    editMode.value = true
  }

  const cancelChanges = () => {
    editMode.value = false
    fetchNovel()
  }

  // 图片上传相关
  const triggerFileInput = () => {
    fileInput.value?.click()
  }

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (!file) return;
    
    if (!validateImage(file)) return;
    
    const reader = new FileReader();
    reader.onload = (e) => {
      novel.value.cover_url = e.target.result;
    };
    reader.readAsDataURL(file);
  }

  const validateImage = (file) => {
    const validTypes = ['image/jpeg', 'image/png'];
    if (!validTypes.includes(file.type)) {
      alert('只支持JPG/PNG格式图片');
      return false;
    }
    
    const maxSize = 5 * 1024 * 1024;
    if (file.size > maxSize) {
      alert('图片大小不能超过5MB');
      return false;
    }
    
    return true;
  }

  const handleImageError = (e) => {
    e.target.src = 'https://ftp.bmp.ovh/imgs/2019/11/06800705be93b1bb.png'
  }

  // 删除相关方法
  const confirmDelete = () => {
    showDeleteDialog.value = true
    deleteError.value = null
  }

  const cancelDelete = () => {
    showDeleteDialog.value = false
  }

  const performDelete = async () => {
    if (!isMounted || !novel.value.novel_id) return
    
    try {
      isDeleting.value = true
      deleteError.value = null
      await deleteNovel(novel.value.novel_id)
      await novelsStore.removeNovel(novel.value.novel_id)
      router.push('/author/novels')
    } catch (error) {
      console.error('删除小说失败:', error)
      deleteError.value = error.message || '删除失败，请重试'
    } finally {
      isDeleting.value = false
    }
  }

  // 保存小说信息
  const saveNovel = async () => {
    try {
      editMode.value = false;

      await submitNovelEdit(novel.value.novel_id, {
        novelId: novel.value.novel_id,
        authorId: authorStore.currentAuthor.author_id,
        novelName: novel.value.novel_name,
        introduction: novel.value.introduction,
        status: novel.value.status,
        totalPrice: 0
      });

      if (fileInput.value?.files?.length > 0) {
        const coverFile = fileInput.value.files[0];
        
        if (validateImage(coverFile)) {
          const avatarUrl = await uploadNovelCover(novel.value.novel_id, coverFile);
          if (avatarUrl) {
            novel.value.cover_url = novelsStore.getFullCoverUrl(avatarUrl);
          }
        }
      }
      
      // 更新本地存储并重新获取评论数
      await novelsStore.fetchNovels();
      await fetchCommentCount(novel.value.novel_id);
      
    } catch (error) {
      console.error('保存失败:', error);
      throw error;
    }
  }

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
    isDeleting,
    deleteError,
    isLoadingCategories,
    categoriesError,
    isSavingCategories,
    isLoadingComments,
    commentsError,
    
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
    deleteNovel: performDelete,
    saveNovel,
    fetchCommentCount // 暴露获取评论数方法
  }
}