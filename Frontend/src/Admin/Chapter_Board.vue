<template>
  <div class="chapter-board">
    <h2>待审核章节</h2>

    <!-- 加载中提示 -->
    <div v-if="loading" class="loading-indicator">
      <div class="spinner"></div>
      <span>加载中...</span>
    </div>

    <table v-else class="chapter-table">
      <thead>
        <tr>
          <th>小说名称</th>
          <th>章节标题</th>
          <th>操作</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="chapter in chapters" :key="`${chapter.novelId}-${chapter.chapterId}`">
          <td>{{ chapter.novelName || '加载中...' }}</td>
          <td>{{ chapter.title }}</td>
          <td>
            <button @click="goToContent(chapter)">查看内容</button>
            <button @click="showModal=true;index=chapter.chapterId;noindex=chapter.novelId;ar=1">审核通过</button>
            <button @click="showModal=true;index=chapter.chapterId;noindex=chapter.novelId;ar=2" class="btn-reject">审核不通过</button>
          </td>
        </tr>
        <tr v-if="chapters.length === 0 && !loading">
          <td colspan="3" style="text-align: center; color: #888;">暂无待审核章节</td>
        </tr>
      </tbody>
    </table>
     <!-- result的输入框 -->
    <div v-if="showModal" class="modal-overlay">
      <div class="modal-content">
        <h3>请输入审核备注</h3>
        <input 
          v-model="inputValue" 
          @keyup.enter="confirmInput"
          placeholder="请输入内容"
          class="modal-input"
        >
        <div class="modal-buttons">
          <button @click="approveChapter()" class="confirm-btn">确定</button>
          <button @click=" showModal=false;inputValue=''" class="cancel-btn">取消</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { getAllChapters, reviewChapter } from '@/API/Chapter_API.js'
import { getNovel } from '@/API/Novel_API.js'
import { managerState } from '@/stores/index'
import { storeToRefs } from 'pinia'

const router = useRouter()
const chapters = ref([])
const loading = ref(false)

const currentState = managerState()
const { id: managerID } = storeToRefs(currentState)

const fetchChapters = async () => {
  loading.value = true
  try {
    const res = await getAllChapters()
    // 只取待审核的章节
    const pendingChapters = res.filter(c => c.status === '首次审核' || c.status === '审核中')

    // 并行获取小说名称
    const chaptersWithNames = await Promise.all(
      pendingChapters.map(async (c) => {
        try {
          const novel = await getNovel(c.novelId)
          return { ...c, novelName: novel.novelName }
        } catch (e) {
          console.error(`获取小说名称失败 novelId=${c.novelId}`, e)
          return { ...c, novelName: '未知小说' }
        }
      })
    )

    chapters.value = chaptersWithNames
  } catch (err) {
    console.error('获取未审核章节失败:', err)
    alert('加载章节数据失败，请检查网络或权限。')
  } finally {
    loading.value = false
  }
}

onMounted(fetchChapters)

const goToContent = (chapter) => {
  router.push({
    name: 'ChapterContent',
    params: {
      novel_id: chapter.novelId,
      chapter_id: chapter.chapterId
    }
  })
}

const showModal=ref(false)
const index=ref(0)
const noindex=ref(0)
const inputValue = ref('');
const result = ref('');
const ar=ref(0)
const approveChapter = async () => {
 if (inputValue.value.trim()) {
    result.value = inputValue.value;
    showModal.value = false;
    inputValue.value = '';
  }
  else {
    alert('请输入有效的内容');
    return;
  }
  try {
    if(ar.value===1){
          await reviewChapter(noindex.value,index.value, '已发布', managerID.value, result.value)
          chapters.value = chapters.value.filter(c => c.chapterId !== index.value || c.novelId !== noindex.value)
    }
    else{
            await reviewChapter(noindex.value,index.value, '封禁', managerID.value, result.value)
            chapters.value = chapters.value.filter(c => c.chapterId !== index.value || c.novelId !== noindex.value)
    }
  } catch (err) {
    console.error('审核失败:', err)
    alert('操作失败，请重试')
  }
}


</script>

<style scoped>
.chapter-board {
  max-width: 960px;
  margin: 30px auto;
  background: #fff;
  padding: 20px;
  border-radius: 10px;
}

.chapter-table {
  width: 100%;
  border-collapse: collapse;
}

.chapter-table th,
.chapter-table td {
  padding: 12px 16px;
  border-bottom: 1px solid #eee;
  text-align: left;
  color: #333;
}

.chapter-table th {
  background: #e3eaf1;
}

button {
  margin-right: 10px;
  padding: 6px 12px;
  border: none;
  background-color: #486482;
  color: #fff;
  border-radius: 4px;
  cursor: pointer;
}

button:hover {
  background-color: #35495e;
}

.btn-reject {
  background-color: #ad7079;
}

.btn-reject:hover {
  background-color: #90555f;
}

.loading-indicator {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  margin: 20px 0;
  color: #666;
  font-size: 16px;
}

.spinner {
  width: 24px;
  height: 24px;
  border: 3px solid #ccc;
  border-top-color: #486482;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  padding: 20px;
  border-radius: 8px;
  width: 300px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.modal-input {
  width: 100%;
  padding: 8px;
  margin: 15px 0;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.modal-buttons {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

.confirm-btn {
  background-color: #42b983;
  color: white;
}

.cancel-btn {
  background-color: #f0f0f0;
}
</style>
