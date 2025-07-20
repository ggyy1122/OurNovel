<template>
    <button @click="goback">返回</button>
    <div class="novel">
        <h2>小说作品</h2>

        <div class="property-item">
            <span class="property-key">ID</span>:
            <span class="property-value">{{ selectNovelState.novelId }}</span>
        </div>

        <div class="property-item">
            <span class="property-key">Name</span>:
            <span class="property-value">{{ selectNovelState.novelName }}</span>
        </div>
    </div>
    <div class="novel-container">
        <nav class="nav-menu" ref="mainNav">
            <ul>
                <li>
                    <router-link to="/Novels/Novel_Info/home" active-class="active-link">
                        作品介绍
                    </router-link>
                </li>
                <li>
                    <router-link to="/Novels/Novel_Info/chapter" active-class="active-link">
                        作品目录
                    </router-link>
                </li>
                <li>
                    <router-link to="/Novels/Novel_Info/comment" active-class="active-link">
                        评论
                    </router-link>
                </li>
            </ul>
        </nav>

        <main class="main-content">
            <router-view></router-view>
        </main>
    </div>

</template>

<script setup>
import { useRouter} from 'vue-router';
import { SelectNovel_State } from '@/stores/index';
const selectNovelState = SelectNovel_State();      //小说对象
const router = useRouter();

function goback() {
    router.push('/Novels/Novel_Layout/home')
}


</script>

<style scoped>
.novel-container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 20px;
}

.nav-menu ul {
    display: flex;
    list-style: none;
    padding: 0;
    margin: 0 0 20px 0;
    border-bottom: 1px solid #e0e0e0;
}

.nav-menu li {
    margin-right: 30px;
}

.nav-menu a {
    text-decoration: none;
    color: #333;
    font-size: 18px;
    padding: 10px 0;
    display: inline-block;
    position: relative;
}

.nav-menu a.active-link {
    color: #1890ff;
    font-weight: bold;
}

.nav-menu a.active-link:after {
    content: '';
    position: absolute;
    bottom: -1px;
    left: 0;
    right: 0;
    height: 2px;
    background-color: #1890ff;
}

.chapter-count {
    font-size: 14px;
    color: #888;
    margin-left: 5px;
    font-weight: normal;
}

.main-content {
    padding: 20px 0;
}
</style>