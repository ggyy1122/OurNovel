<template>
    <div class="rankings">
        <img src="@/assets/rank.png" alt="排行榜" class="banner" />
        <div class="tabs">
            <button v-for="tab in tabs" :key="tab" :class="{ active: tab === selectedTab }" @click="selectedTab = tab">
                {{ tab }}
            </button>
        </div>
        <div class="main">
            <div class="sidebar">
                <div class="sidebar-title">热门作品排行</div>
                <button v-for="side in sideTabs" :key="side" :class="{ active: side === selectedSideTab }"
                    @click="selectedSideTab = side">
                    {{ side }}
                </button>
            </div>
            <div class="content">
                <div class="rank-header">
                    <span class="rank-title">{{ selectedSideTab }}</span>
                    <div class="sub-tabs">
                        <button v-for="t in rankType" :key="t" :class="{ active: t === selectedRankType }"
                            @click="selectedRankType = t">
                            {{ t }}
                        </button>
                    </div>
                </div>
                <div class="novel-list">
                    <!-- <template v-for="(novel, idx) in filteredNovels" :key="novel.id">
                        <Novel_Card :novel="novel" :rank="idx + 1" />
                        <hr v-if="idx < filteredNovels.length - 1" class="novel-divider" />
                    </template> -->
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue'
// import { ref, computed } from 'vue'
// import Novel_Card from './Novel_Card.vue'

const tabs = ['男生榜', '女生榜', '出生榜']
const sideTabs = ['大热榜', '新书榜', '完结榜', '收藏榜', '更新榜']
const rankType = ['日榜', '月榜']

const selectedTab = ref('女生榜')
const selectedSideTab = ref('大热榜')
const selectedRankType = ref('日榜')

// 写死的小说数据
// // 扩展的小说数据
// const novels = [
//     {
//         id: 1,
//         tab: '女生榜',
//         side: '大热榜',
//         type: '日榜',
//         title: '封总，太太想跟你离婚很久了',
//         author: '云中网',
//         category: '现代言情',
//         status: '总裁豪门',
//         wordCount: '53.28万字',
//         desc: '结婚七年，封庭深待她冷淡如对，容辞一直微笑面对。因为她深爱着他，也相信终有一天，她能抚他的心结疙。可她等来的却是他对另一个女人的一见钟情，呵护备至。她...',
//         update: '最新更新 第325章再用失邀请函 2025-07-13 01:00:05',
//         hot: '339.9万',
//         badge: '蝉联 榜首',
//         cover: require('@/assets/novel_cover.jpg'),
//         content: '容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。vvv容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。容/n辞/n站/n在/n落/n地/n窗/n前，望着楼下那辆熟悉的黑色宾利，手指无意识地摩挲着无名指上的婚戒。七年前，她不顾家人反对嫁给了封庭深，原以为能打动这个冰山般的男人，却在今天亲眼目睹他搂着另一个女人走进酒店。玻璃上倒映着她苍白的脸，突然一阵眩晕袭来，她踉跄着扶住桌角，掌心传来一阵刺痛——那是孕检报告，上面清晰地写着“怀孕六周”。'
//     }
// ];

// 过滤展示的小说
// const filteredNovels = computed(() =>
//     novels.filter(n =>
//         n.tab === selectedTab.value &&
//         n.side === selectedSideTab.value &&
//         n.type === selectedRankType.value
//     )
// )
</script>

<style scoped>
.rankings {
    background: #fff;
    font-family: "PingFang SC", "Microsoft YaHei", Arial, sans-serif;
}

.banner {
    width: 100%;
    object-fit: cover;
    margin-bottom: 20px;
}

.tabs {
    display: flex;
    justify-content: center;
    margin-bottom: 24px;
}

.tabs button {
    background: #f4f2f2;
    border: none;
    padding: 10px 34px;
    font-size: 18px;
    font-weight: bold;
    color: #333;
    border-radius: 28px;
    margin: 0 10px;
    cursor: pointer;
    transition: background 0.2s;
}

.tabs button:hover {
    color: #e47f0d;
}

.tabs button.active {
    background: #ffe357;
    color: #333;
}

.main {
    display: flex;
    justify-content: center;
    gap: 40px;
}

.sidebar {
    background: #fafafa;
    border-radius: 8px;
    padding: 16px 0;
    min-width: 140px;
    display: flex;
    flex-direction: column;
    gap: 6px;
    margin-left: 30px;
}

.sidebar-title {
    font-size: 20px;
    font-weight: bold;
    color: #222;
    padding: 8px 22px 10px 22px;
}

.sidebar button {
    background: none;
    border: none;
    padding: 10px 22px;
    font-size: 18px;
    color: #333;
    text-align: left;
    cursor: pointer;
    border-radius: 6px;
    transition: background 0.18s;
}

.sidebar button:hover {
    background-color: #f0deb8;
    color: #e47f0d;
}

.sidebar button.active {
    background: #ffe357;
    color: #f59906;
}

.content {
    flex: 1;
    min-width: 600px;
}

.rank-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 22px;
    background: #f4f2f2;
}

.rank-title {
    font-size: 24px;
    font-weight: bold;
    color: #222;
    margin-left: 10px;
}

.sub-tabs {
    display: flex;
    align-items: center;
    margin-right: 150px;
}

.sub-tabs button {
    background: none;
    border: none;
    font-size: 18px;
    color: #333;
    margin-left: 10px;
    padding: 6px 18px;
    border-radius: 14px;
    cursor: pointer;
    transition: background 0.18s;
}

.sub-tabs button:hover {
    color: #e47f0d;
}

.sub-tabs button.active {
    background: #ffe357;
    color: #f29705;
}

.novel-list {
    display: flex;
    flex-direction: column;
}

.novel-divider {
    border: none;
    border-top: 1px solid #b2b6bb;
    margin: 1.5rem 0;
}
</style>