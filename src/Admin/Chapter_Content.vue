<template>
  <div class="chapter-content">
    <h2>章节内容</h2>
    <div v-if="chapter">
      <p><strong>小说ID：</strong>{{ chapter.novelId }}</p>
      <p><strong>章节标题：</strong>{{ chapter.title }}</p>
      <div class="content-box">
        {{ chapter.content }}
      </div>
    </div>
    <div v-else>
      <p>章节内容为空</p>
    </div>

    <div class="buttons">
      <button class="btn-back" @click="goBack">返回</button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getChapterDetail } from '@/API/ChapterManage_API'

const route = useRoute()
const router = useRouter()

const chapter = ref(null)

onMounted(async () => {
  const novelId = Number(route.params.novel_id)
  const chapterId = Number(route.params.chapter_id)

  // 如果 query 中已有内容（来自上一页跳转），优先显示它
  const queryContent = route.query.content
  const queryTitle = route.query.title

  // 先填充 query 数据
  if (queryTitle && queryContent) {
    chapter.value = {
      novelId,
      chapterId,
      title: queryTitle,
      content: queryContent
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
  text-align: left;
}

.btn-back {
  padding: 8px 16px;
  background-color: #486482;
  color: #fff;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}

.btn-back:hover {
  background-color: #35495e;
}
</style>
