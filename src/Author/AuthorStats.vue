<template>
  <div class="page-container">
    <h2>数据统计</h2>
    
    <div class="stats-grid">
      <div class="stat-card">
        <h3>总字数</h3>
        <p class="stat-value">{{ stats.total_word_count }}</p>
      </div>
      
      <div class="stat-card">
        <h3>总章节数</h3>
        <p class="stat-value">{{ stats.total_chapter_count }}</p>
      </div>
      
      <div class="stat-card">
        <h3>总阅读量</h3>
        <p class="stat-value">{{ stats.total_view_count }}</p>
      </div>
      
      <div class="stat-card">
        <h3>总收藏数</h3>
        <p class="stat-value">{{ stats.total_collected_count }}</p>
      </div>
      
      <div class="stat-card">
        <h3>总推荐数</h3>
        <p class="stat-value">{{ stats.total_recommend_count }}</p>
      </div>
      
      <div class="stat-card">
        <h3>总收益</h3>
        <p class="stat-value">¥{{ stats.total_earning }}</p>
      </div>
    </div>
    
    <div class="chart-container">
      <h3>最近30天阅读量趋势</h3>
      <div class="chart-placeholder"></div>
    </div>
    
    <div class="chart-container">
      <h3>各作品数据对比</h3>
      <div class="chart-placeholder"></div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      stats: {
        total_word_count: 350000,
        total_chapter_count: 70,
        total_view_count: 18718,
        total_collected_count: 3851,
        total_recommend_count: 1460,
        total_earning: 1250.50
      }
    }
  }
}
</script>

<style scoped>
.page-container {
  padding: 30px;
}

h2 {
  margin-bottom: 25px;
  color: #2c3e50;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 20px;
  margin-bottom: 40px;
}

.stat-card {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0,0,0,0.05);
  text-align: center;
}

.stat-card h3 {
  color: #7f8c8d;
  font-size: 16px;
  margin-bottom: 10px;
}

.stat-value {
  font-size: 24px;
  font-weight: bold;
  color: #2c3e50;
}

.chart-container {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0,0,0,0.05);
  margin-bottom: 30px;
}

.chart-container h3 {
  margin-bottom: 15px;
  color: #2c3e50;
}

.chart-placeholder {
  height: 300px;
  background-color: #f8f9fa;
  border-radius: 4px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #95a5a6;
}
</style>