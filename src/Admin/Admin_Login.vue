<template>
    <div class="login-container">
        <h1>管理员登录</h1>
        <form @submit.prevent="handleLogin">
            <div class="form-group">
                <label>用户名：</label>
                <input type="text" v-model="username" placeholder="请输入用户名" required />
            </div>
            <div class="form-group">
                <label>密码：</label>
                <input type="password" v-model="password" placeholder="请输入密码" required />
            </div>
            <button type="submit">登录</button>
        </form>
        <button type="button" @click="Back_Home" style="margin-top:10px;background-color:#ccc;">返回</button>
    </div>
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";

const username = ref("");
const password = ref("");
const router = useRouter();

const handleLogin = () => {
    if (username.value && password.value) {
        localStorage.setItem('isLoggedIn', 'true')
        router.push('/Admin/Admin_Layout/dashboard') // 修改为跳转到仪表盘
    } else {
        alert('请输入用户名和密码')
    }
}
function Back_Home() {
    router.push('/')
}
</script>

<style scoped>
.login-container {
    max-width: 400px;
    margin: 100px auto;
    padding: 20px;
    border: 1px solid #ddd;
    border-radius: 5px;
}

h1 {
    text-align: center;
    margin-bottom: 20px;
}

.form-group {
    margin-bottom: 15px;
}

label {
    display: block;
    margin-bottom: 5px;
}

input {
    width: 100%;
    padding: 8px;
    border: 1px solid #ddd;
    border-radius: 4px;
}

button {
    width: 100%;
    padding: 10px;
    background-color: #42b983;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

button:hover {
    background-color: #3aa876;
}
</style>
