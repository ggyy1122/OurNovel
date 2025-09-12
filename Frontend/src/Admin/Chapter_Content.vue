<template>
  <div class="chapter-content">
    <h2>章节内容</h2>

    <div v-if="loading">加载中...</div>

    <div v-else-if="chapter">
      <p><strong>小说名称：</strong>{{ chapter.novelName }}</p>
      <p><strong>章节标题：</strong>{{ chapter.title }}</p>

      <div class="content-box">
        <div v-if="chapter.content && chapter.content.trim()">
          {{ chapter.content }}
        </div>
        <div v-else style="color: gray;">
          章节内容为空
        </div>
      </div>
    </div>

    <div v-else>
      <p style="color: gray;">章节不存在或已删除</p>
    </div>

    <div class="buttons" v-if="chapter">
      <button class="btn-approve" @click="approveChapter">审核通过</button>
      <button class="btn-reject" @click="rejectChapter">审核不通过</button>
      <button class="btn-back" @click="goBack">返回</button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getChapter, reviewChapter } from '@/API/Chapter_API.js'
import { getNovel } from '@/API/Novel_API.js'
import { current_state } from '@/stores/index'
import { storeToRefs } from 'pinia'

const route = useRoute()
const router = useRouter()
const chapter = ref(null)
const loading = ref(true)

const currentState = current_state()
const { id: managerID } = storeToRefs(currentState)

onMounted(async () => {
  const novelId = Number(route.params.novel_id)
  const chapterId = Number(route.params.chapter_id)

  try {
    const data = await getChapter(novelId, chapterId)

    // 获取小说名称
    try {
      const novel = await getNovel(novelId)
      data.novelName = novel.novelName
    } catch (e) {
      console.error(`获取小说名称失败 novelId=${novelId}`, e)
      data.novelName = '未知小说'
    }

    chapter.value = data
  } catch (err) {
    if (err.response && err.response.status === 404) {
      chapter.value = null
    } else {
      console.error('加载章节失败:', err)
      chapter.value = null
      alert('加载章节失败，请检查网络或权限。')
    }
  } finally {
    loading.value = false
  }
})

const goBack = () => router.back()

const approveChapter = async () => {
  if (!chapter.value) return
  if (!managerID.value) { alert('未检测到管理员身份，请重新登录'); return }

  const result = prompt('请输入审核备注（通过原因等）:')
  if (result === null) return

  try {
    await reviewChapter(chapter.value.novelId, chapter.value.chapterId, '已发布', managerID.value, result)
    alert(`章节「${chapter.value.novelName} - ${chapter.value.title || '未知标题'}」已审核通过`)
    goBack()
  } catch (err) {
    console.error('审核通过失败:', err)
    alert('操作失败，请重试')
  }
}

const rejectChapter = async () => {
  if (!chapter.value) return
  if (!managerID.value) { alert('未检测到管理员身份，请重新登录'); return }

  const result = prompt('请输入审核不通过原因（封禁原因等）:')
  if (result === null) return

  if (!confirm(`确定将「${chapter.value.novelName} - ${chapter.value.title || '未知标题'}」标记为审核不通过（封禁）吗？`)) return

  try {
    await reviewChapter(chapter.value.novelId, chapter.value.chapterId, '封禁', managerID.value, result)
    alert(`章节「${chapter.value.novelName} - ${chapter.value.title || '未知标题'}」已封禁`)
    goBack()
  } catch (err) {
    console.error('封禁失败:', err)
    alert('操作失败，请重试')
  }
}
</script>

<style scoped>
.chapter-content {
  max-width: 800px;
  margin: 40px auto;
  background: #fff;
  padding: 24px;
  border-radius: 10px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.content-box {
  margin-top: 20px;
  padding: 16px;
  background-color: #f9f9fc;
  border-radius: 6px;
  white-space: pre-wrap;
  color: #333;
}

.buttons {
  margin-top: 24px;
  display: flex;
  gap: 10px;
}

.btn-back, .btn-approve, .btn-reject {
  padding: 8px 16px;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  color: #fff;
}

.btn-back { background-color: #486482; }
.btn-back:hover { background-color: #35495e; }

.btn-approve { background-color: #4CAF50; }
.btn-approve:hover { background-color: #388E3C; }

.btn-reject { background-color: #ad7079; }
.btn-reject:hover { background-color: #90555f; }
</style>
