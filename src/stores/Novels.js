// src/stores/Novels.js
import { reactive } from 'vue'

export const novelsStore = reactive({
  novels: [
    {
      novel_id: 1,
      novel_name: '剑神传说',
      cover_url: 'https://picsum.photos/300/400?random=1',
      status: '连载中',
      category: '武侠',
      total_word_count: 125000,
      chapter_count: 15,
      introduction: '这是一个关于剑与江湖的传奇故事',
      view_count: 5842,
      collected_count: 1287,
      recommend_count: 584,
      score: 8.5
    },
     {
      novel_id: 2,
      novel_name: '666',
      cover_url: 'https://picsum.photos/300/400?random=1',
      status: '连载中',
      category: '武侠',
      total_word_count: 125000,
      chapter_count: 15,
      introduction: '这是一个关于剑与江湖的传奇故事',
      view_count: 5842,
      collected_count: 1287,
      recommend_count: 584,
      score: 8.5
    },
    // 其他小说数据...
  ],
  
  getNovelById(id) {
    return this.novels.find(novel => novel.novel_id === id) || null
  },
  
  removeNovel(id) {
    this.novels = this.novels.filter(novel => novel.novel_id !== id)
  }
})