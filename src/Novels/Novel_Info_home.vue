<template>  
    <div class="introduction-section">
    <strong class="intro-label">简介：</strong>
    <p>{{selectNovelState.introduction}} </p>
    </div>
</template>

<script setup>
import { SelectNovel_State } from '@/stores/index';
const selectNovelState = SelectNovel_State();      //小说对象
</script>

<style scoped>
.introduction-section {
  margin: 0px 0;
  line-height: 1.6;
}

.intro-label {
  font-weight: 600; /* 加粗 */
   font-size: 25px;
  color: #1e293b; /* 深灰色 */
}

.intro-content {
  display: inline; /* 让文本与标签同行 */
  margin-left: 4px;
  color: #475569; /* 中灰色 */
}
</style>