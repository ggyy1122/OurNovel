<template>
  <div class="category-filter">
    <div class="filter-row" v-for="(group, groupKey) in filterGroups" :key="groupKey">
      <span class="filter-label">{{ group.label }}:</span>
      <button v-for="item in group.options" :key="item.value"
        :class="['filter-btn', selected[groupKey] === item.value && 'active']"
        @click="selectFilter(groupKey, item.value)">
        {{ item.text }}
      </button>
    </div>
  </div>
  <!-- 下面是作品列表等内容，可根据需要扩展 -->
</template>

<script setup>
import { reactive } from 'vue'

// 筛选项数据
const filterGroups = {
  channel: {
    label: '频道',
    options: [
      { text: '全部', value: '' },
      { text: '女生原创', value: 'female' },
      { text: '男生原创', value: 'male' },
      { text: '出版图书', value: 'publish' }
    ]
  },
  category: {
    label: '作品分类',
    options: [
      { text: '全部', value: '' },
      { text: '现代言情', value: 'modern' },
      { text: '古代言情', value: 'ancient' },
      { text: '幻想言情', value: 'fantasy' },
      { text: '历史', value: 'history' },
      { text: '军事', value: 'military' },
      { text: '科幻', value: 'science' },
      { text: '游戏', value: 'game' },
      { text: '游戏竞技', value: 'competition' },
      { text: '玄幻奇幻', value: 'magic' },
      { text: '都市', value: 'city' },
      { text: '奇闻异事', value: 'strange' },
      { text: '武侠仙侠', value: 'wuxia' },
      { text: '体育', value: 'sport' },
      { text: 'N次元', value: 'n' },
      { text: '文学艺术', value: 'art' },
      { text: '人文社科', value: 'human' },
      { text: '经管励志', value: 'manage' },
      { text: '经典文学', value: 'classic' },
      { text: '出版小说', value: 'pubnovel' },
      { text: '少儿教育', value: 'child' },
      { text: '衍生言情', value: 'derivative' },
      { text: '现实题材', value: 'real' },
      { text: '现实主义', value: 'realism' }
    ]
  },
  wordCount: {
    label: '作品字数',
    options: [
      { text: '全部', value: '' },
      { text: '30万以下', value: 'lt300k' },
      { text: '30万-50万', value: '300k-500k' },
      { text: '50万-100万', value: '500k-1m' },
      { text: '100万-200万', value: '1m-2m' },
      { text: '200万以上', value: 'gt2m' }
    ]
  },
  updateTime: {
    label: '更新时间',
    options: [
      { text: '全部', value: '' },
      { text: '3天内', value: '3d' },
      { text: '7天内', value: '7d' },
      { text: '30天内', value: '30d' }
    ]
  },
  isFinished: {
    label: '是否完结',
    options: [
      { text: '全部', value: '' },
      { text: '已完结', value: 'finished' },
      { text: '连载中', value: 'serial' }
    ]
  }
}

// 保存当前选择的标签
const selected = reactive({
  channel: '',
  category: '',
  wordCount: '',
  updateTime: '',
  isFinished: ''
})

// 选择标签
function selectFilter(groupKey, value) {
  selected[groupKey] = value
  // 这里可以加与后端交互的逻辑
}
</script>

<style scoped>
.category-filter {
  background: #fafafa;
  border-radius: 16px;
  padding: 32px 32px 16px 32px;
  margin: 24px auto;
  max-width: 1200px;
}

.filter-row {
  display: flex;
  align-items: center;
  margin-bottom: 18px;
  flex-wrap: wrap;
}

.filter-label {
  font-weight: bold;
  font-size: 18px;
  margin-right: 18px;
  min-width: 90px;
  color: #666;
}

.filter-btn {
  background: none;
  border: none;
  color: #222;
  font-size: 16px;
  margin-right: 18px;
  margin-bottom: 6px;
  padding: 0 16px;
  height: 32px;
  border-radius: 16px;
  cursor: pointer;
  transition: background 0.2s, color 0.2s;
  font-weight: 500;
  outline: none;
}

.filter-btn.active {
  background: #ffd100;
  color: #fff;
  font-weight: bold;
}

.filter-btn:hover {
  color: #ff4d4f;
  border: 1.5px solid #ffd100;
  background: #fffbe6;
}
</style>