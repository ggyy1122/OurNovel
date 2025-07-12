<template>
    <div class="dashboard">
        <h3>数据概览</h3>
        <div class="stats">
            <div class="stat-card">
                <h4>总小说数</h4>
                <p>256</p>
            </div>
            <div class="stat-card">
                <h4>总用户数</h4>
                <p>1,024</p>
            </div>
        </div>
    </div>
</template>

<style scoped>
.stats {
    display: flex;
    gap: 20px;
    margin-top: 20px;
}

.stat-card {
    border: 1px solid #ddd;
    padding: 15px;
    border-radius: 5px;
    flex: 1;
    text-align: center;
}

.stat-card h4 {
    margin: 0 0 10px 0;
    color: #666;
}

.stat-card p {
    margin: 0;
    font-size: 24px;
    font-weight: bold;
}
</style>