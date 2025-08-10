<template>
  <div class="dashboard">
    <h3>数据概览</h3>
    <div class="stats">
      <div class="stat-card total-novels">
        <h4>总小说数</h4>
        <p>256</p>
      </div>
      <div class="stat-card total-users">
        <h4>总用户数</h4>
        <p>1,024</p>
      </div>
      <div class="stat-card active-authors">
        <h4>活跃作者数</h4>
        <p>120</p>
      </div>
      <div class="stat-card new-registrations">
        <h4>今日新增用户</h4>
        <p>35</p>
      </div>
      <div class="stat-card chapters-published">
        <h4>本周章节发布量</h4>
        <p>89</p>
      </div>
      <div class="stat-card revenue">
        <h4>本月收益(元)</h4>
        <p>12,345</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.stats {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
  margin-top: 20px;
}

.stat-card {
  border: 1px solid #ddd;
  padding: 20px;
  border-radius: 8px;
  flex: 1 1 200px; /* 自适应宽度，最小200px */
  text-align: center;
  background-color: #f9f9f9;
  box-shadow: 0 0 6px rgba(0,0,0,0.05);
  transition: transform 0.3s ease;
}

.stat-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
}

.stat-card h4 {
  margin: 0 0 12px;
  color: #444;
  font-weight: 600;
}

.stat-card p {
  margin: 0;
  font-size: 28px;
  font-weight: bold;
  color: #222;
}

/* 颜色区分 */
.total-novels {
  border-color: #3498db;
  color: #2980b9;
}

.total-users {
  border-color: #27ae60;
  color: #1e8449;
}

.active-authors {
  border-color: #e67e22;
  color: #b35910;
}

.new-registrations {
  border-color: #9b59b6;
  color: #71368a;
}

.chapters-published {
  border-color: #e74c3c;
  color: #b03a2e;
}

.revenue {
  border-color: #16a085;
  color: #117a65;
}
</style>
