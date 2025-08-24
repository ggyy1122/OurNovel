<template>
  <div class="chapter-content">
    <h2>章节内容</h2>
    <div v-if="chapter">
      <p><strong>小说ID：</strong>{{ chapter.novelId }}</p>
      <p><strong>章节ID：</strong>{{ chapter.chapterId }}</p>
      <p><strong>章节标题：</strong>{{ chapter.title }}</p>
      <div class="content-box">
        <div class="content-box" v-if="chapter.content">
            {{ chapter.content }}
        </div>
        <div class="content-box" v-else style="color: gray;">
          章节内容为空
        </div>
      </div>
    </div>
    <div v-else>
      <p style="color: gray;">章节信息为空</p>
    </div>

    <div class="buttons">
      <button class="btn-approve" @click="approveChapter">审核通过</button>
      <button class="btn-reject" @click="rejectChapter">审核不通过</button>
      <button class="btn-back" @click="goBack">返回</button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getChapterDetail, reviewChapter } from '@/API/ChapterManage_API'
import { current_state } from '@/stores/index'
import { storeToRefs } from 'pinia'

const route = useRoute()
const router = useRouter()
const currentState = current_state()
const { id: managerID } = storeToRefs(currentState)

const chapter = ref(null)

onMounted(async () => {
  const novelId = Number(route.params.novel_id)
  const chapterId = Number(route.params.chapter_id)

  // 如果 query 中已有内容（来自上一页跳转），优先显示它
  const queryContent = route.query.content
  const queryTitle = route.query.title

  // 先填充 query 数据
  if (queryTitle && queryContent) {
    if (queryTitle && queryContent !== undefined) {
        chapter.value = {
      novelId,
      chapterId,
      title: queryTitle,
      content: queryContent
    }
    }  
  }

  // 再尝试加载完整数据
  try {
    const res = await getChapterDetail(novelId, chapterId)
    chapter.value = res
  } catch (err) {
    console.error('加载章节失败：', err)
    // 如果 query 没有数据，也没有加载成功，显示为空
    if (!chapter.value) {
      chapter.value = null
    }
  }
})

const goBack = () => {
  router.go(-1) // 确保 router/index.js 里 name 是 ChapterBoard
}

// 审核通过
const approveChapter = async () => {
  if (!chapter.value) {
    alert('未获取到章节信息')
    return
  }
  if (!managerID.value) {
    alert('未检测到管理员身份，请重新登录')
    return
  }

  const result = prompt('请输入审核备注（通过原因等）:')
  if (result === null) return

  try {
    await reviewChapter(chapter.value.novelId, chapter.value.chapterId, '已发布', managerID.value, result)
    alert(`章节「${chapter.value.title || '未知标题'}」已审核通过`)
    goBack()
  } catch (err) {
    console.error('审核通过失败:', err)
    alert('操作失败，请重试')
  }
}

// 审核不通过
const rejectChapter = async () => {
  if (!chapter.value) {
    alert('未获取到章节信息')
    return
  }
  if (!managerID.value) {
    alert('未检测到管理员身份，请重新登录')
    return
  }

  const result = prompt('请输入审核不通过原因（封禁原因等）:')
  if (result === null) return

  const confirmed = confirm(`确定将「${chapter.value.title || '未知标题'}」标记为审核不通过（封禁）吗？`)
  if (!confirmed) return

  try {
    await reviewChapter(chapter.value.novelId, chapter.value.chapterId, '封禁', managerID.value, result)
    alert(`章节「${chapter.value.title || '未知标题'}」已封禁`)
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

.btn-back, .btn-approve, .btn-reject{
  padding: 8px 16px;
  color: #fff;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}

.btn-back { background-color: #486482; }
.btn-back:hover { background-color: #35495e; }

.btn-approve { background-color: #4CAF50; }
.btn-approve:hover { background-color: #388E3C; }

.btn-reject { background-color: #ad7079; }
.btn-reject:hover { background-color: #90555f; }
</style>
