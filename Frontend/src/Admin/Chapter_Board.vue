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
          <td>{{ chapter.novelName }}</td>
          <td>{{ chapter.title }}</td>
          <td>
            <button @click="goToContent(chapter)">查看内容</button>
            <button @click="approveChapter(chapter)">审核通过</button>
            <button @click="rejectChapter(chapter)" class="btn-reject">审核不通过</button>
          </td>
        </tr>
        <tr v-if="chapters.length === 0 && !loading">
          <td colspan="3" style="text-align: center; color: #888;">暂无待审核章节</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { getAllChapters, reviewChapter } from '@/API/Chapter_API.js'
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
    chapters.value = res.filter(c => c.status === '首次审核' || c.status === '审核中')
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

const approveChapter = async (chapter) => {
  const result = prompt(`请输入审核备注（通过原因等）:`)
  if (result === null) return
  if (!managerID.value) { alert('未检测到管理员身份，请重新登录'); return }

  try {
    await reviewChapter(chapter.novelId, chapter.chapterId, '已发布', managerID.value, result)
    alert(`已审核通过：「${chapter.title}」`)
    chapters.value = chapters.value.filter(c => c.chapterId !== chapter.chapterId || c.novelId !== chapter.novelId)
  } catch (err) {
    console.error('审核通过失败:', err)
    alert('操作失败，请重试')
  }
}

const rejectChapter = async (chapter) => {
  const result = prompt(`请输入审核不通过原因（封禁原因等）:`)
  if (result === null) return
  if (!managerID.value) { alert('未检测到管理员身份，请重新登录'); return }

  if (!confirm(`确定将「${chapter.title}」标记为审核不通过（封禁）吗？`)) return

  try {
    await reviewChapter(chapter.novelId, chapter.chapterId, '封禁', managerID.value, result)
    alert(`章节「${chapter.title}」已封禁`)
    chapters.value = chapters.value.filter(c => c.chapterId !== chapter.chapterId || c.novelId !== chapter.novelId)
  } catch (err) {
    console.error('封禁失败:', err)
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
</style>
