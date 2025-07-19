<template>
    <div class="login-left">
        <h2 class="welcome-title">{{ state.Role(0) }}登录</h2>
        <div class="subtitle">欢迎加入TJ</div>
        <form @submit.prevent="handleLogin">
            <div class="input-group">
                <span class="input-icon">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20" height="20">
                        <path fill="currentColor"
                            d="M288 320a224 224 0 1 0 448 0 224 224 0 1 0-448 0m544 608H160a32 32 0 0 1-32-32v-96a160 160 0 0 1 160-160h448a160 160 0 0 1 160 160v96a32 32 0 0 1-32 32z">
                        </path>
                    </svg>
                </span>
                <input type="text" v-model="username" placeholder="请输入用户名" class="login-input"
                    autocomplete="username" />
            </div>
            <div v-if="usernameError" class="error-tip">请输入用户名</div>
            <div class="input-group">
                <span class="input-icon">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20" height="20">
                        <path fill="currentColor"
                            d="M224 448a32 32 0 0 0-32 32v384a32 32 0 0 0 32 32h576a32 32 0 0 0 32-32V480a32 32 0 0 0-32-32zm0-64h576a96 96 0 0 1 96 96v384a96 96 0 0 1-96 96H224a96 96 0 0 1-96-96V480a96 96 0 0 1 96-96">
                        </path>
                        <path fill="currentColor"
                            d="M512 544a32 32 0 0 1 32 32v192a32 32 0 1 1-64 0V576a32 32 0 0 1 32-32m192-160v-64a192 192 0 1 0-384 0v64zM512 64a256 256 0 0 1 256 256v128H256V320A256 256 0 0 1 512 64">
                        </path>
                    </svg>
                </span>
                <input :type="showPassword ? 'text' : 'password'" v-model="password" placeholder="请输入密码"
                    class="login-input" autocomplete="current-password" />
                <span class="input-eye" @click="showPassword = !showPassword">
                    <svg v-if="!showPassword" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20"
                        height="20">
                        <path fill="currentColor"
                            d="M512 160c320 0 512 352 512 352S832 864 512 864 0 512 0 512s192-352 512-352m0 64c-225.28 0-384.128 208.064-436.8 288 52.608 79.872 211.456 288 436.8 288 225.28 0 384.128-208.064 436.8-288-52.608-79.872-211.456-288-436.8-288zm0 64a224 224 0 1 1 0 448 224 224 0 0 1 0-448m0 64a160.192 160.192 0 0 0-160 160c0 88.192 71.744 160 160 160s160-71.808 160-160-71.744-160-160-160">
                        </path>
                    </svg>
                    <svg v-else xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20" height="20">
                        <path fill="currentColor"
                            d="M876.8 156.8c0-9.6-3.2-16-9.6-22.4-6.4-6.4-12.8-9.6-22.4-9.6-9.6 0-16 3.2-22.4 9.6L736 220.8c-64-32-137.6-51.2-224-60.8-160 16-288 73.6-377.6 176C44.8 438.4 0 496 0 512s48 73.6 134.4 176c22.4 25.6 44.8 48 73.6 67.2l-86.4 89.6c-6.4 6.4-9.6 12.8-9.6 22.4 0 9.6 3.2 16 9.6 22.4 6.4 6.4 12.8 9.6 22.4 9.6 9.6 0 16-3.2 22.4-9.6l704-710.4c3.2-6.4 6.4-12.8 6.4-22.4Zm-646.4 528c-76.8-70.4-128-128-153.6-172.8 28.8-48 80-105.6 153.6-172.8C304 272 400 230.4 512 224c64 3.2 124.8 19.2 176 44.8l-54.4 54.4C598.4 300.8 560 288 512 288c-64 0-115.2 22.4-160 64s-64 96-64 160c0 48 12.8 89.6 35.2 124.8L256 707.2c-9.6-6.4-19.2-16-25.6-22.4Zm140.8-96c-12.8-22.4-19.2-48-19.2-76.8 0-44.8 16-83.2 48-112 32-28.8 67.2-48 112-48 28.8 0 54.4 6.4 73.6 19.2zM889.599 336c-12.8-16-28.8-28.8-41.6-41.6l-48 48c73.6 67.2 124.8 124.8 150.4 169.6-28.8 48-80 105.6-153.6 172.8-73.6 67.2-172.8 108.8-284.8 115.2-51.2-3.2-99.2-12.8-140.8-28.8l-48 48c57.6 22.4 118.4 38.4 188.8 44.8 160-16 288-73.6 377.6-176C979.199 585.6 1024 528 1024 512s-48.001-73.6-134.401-176Z">
                        </path>
                        <path fill="currentColor"
                            d="M511.998 672c-12.8 0-25.6-3.2-38.4-6.4l-51.2 51.2c28.8 12.8 57.6 19.2 89.6 19.2 64 0 115.2-22.4 160-64 41.6-41.6 64-96 64-160 0-32-6.4-64-19.2-89.6l-51.2 51.2c3.2 12.8 6.4 25.6 6.4 38.4 0 44.8-16 83.2-48 112-32 28.8-67.2 48-112 48Z">
                        </path>
                    </svg>
                </span>
            </div>
            <div v-if="passwordError" class="error-tip">请输入密码</div>
            <div class="login-options">
                <label>
                    <input type="checkbox" v-model="rememberMe" />
                    记住我
                </label>
                <a class="forgot-link" href="#" @click.prevent="forgotPassword">忘记密码?</a>
            </div>
            <button type="submit" class="login-btn">登录</button>
        </form>
        <div class="signup-tip">
            还没有账号？<a href="#" @click.prevent="register">立即注册</a>
        </div>
    </div>
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import { current_state, readerState } from '@/stores/index';
import { loginAuthor, loginManager, loginReader } from '@/API/Log_API';
import { getReader } from '@/API/Reader_API';


const state = current_state();
const reader_state = readerState();

const username = ref("");
const password = ref("");
const showPassword = ref(false);
const rememberMe = ref(false);
const usernameError = ref(false);
const passwordError = ref(false);
const router = useRouter();
function saveToken(token, name, id) {
    if (rememberMe.value) {
        localStorage.setItem('token', token);
        localStorage.setItem('name', name);
        localStorage.setItem('id', id);
    } else {
        sessionStorage.setItem('token', token);
        sessionStorage.setItem('name', name);
        sessionStorage.setItem('id', id);
    }
}
const handleLogin = async () => {
    usernameError.value = !username.value;
    passwordError.value = !password.value;
    if (username.value && password.value) {
        let response;
        try {
            if (state.value === 0) { // 读者
                response = await loginReader({
                    readerName: username.value,
                    password: password.value
                });
                saveToken(response.token, response.readerName, response.readerId);
                state.setUserInfo(response.readerName, response.readerId); // 存入 Pinia
                const readerDetails = await getReader(response.readerId);
                reader_state.initializeReader(readerDetails.readerId,
                    readerDetails.readerName,
                    readerDetails.avatarUrl
                );
                // reader_state.initializeReader(readerDetails.readerId,
                //     readerDetails.readerName,
                //     readerDetails.password,
                //     readerDetails.phone,
                //     readerDetails.gender,
                //     readerDetails.balance,
                //     readerDetails.avatarUrl,
                //     readerDetails.backgroundUrl,
                //     readerDetails.isCollectVisible,
                //     readerDetails.isRecommendVisible
                // );
                router.push('/Novels/Novel_Layout/home');
            } else if (state.value === 1) { // 作者
                response = await loginAuthor({
                    authorName: username.value,
                    password: password.value
                });
                saveToken(response.token, response.authorName, response.authorId);
                state.setUserInfo(response.authorName, response.authorId); // 存入 Pinia
                router.push('/author/novels');
            } else if (state.value === 2) { // 管理员
                response = await loginManager({
                    managerName: username.value,
                    password: password.value
                });
                saveToken(response.token, response.managerName, response.managerId);
                state.setUserInfo(response.managerName, response.managerId); // 存入 Pinia
                router.push('/Admin/Admin_Layout/dashboard');
            }
            console.log('登录成功', response.data);
        } catch (error) {
            console.error('登录失败:', error);
            alert('登录失败，请检查用户名和密码');
        }
    }
};

function forgotPassword() {
    router.push('/L_R/reset');
}
function register() {
    router.push('/L_R/register');
}
</script>

<style scoped>
.login-left {
    width: 70%;
    padding: 60px 50px 0 50px;
    display: flex;
    flex-direction: column;
    justify-content: center;
}

.welcome-title {
    font-size: 32px;
    font-weight: bold;
    text-align: center;
    margin-bottom: 14px;
    margin-top: 0;
}

.subtitle {
    text-align: center;
    color: #888;
    margin-bottom: 40px;
    font-size: 18px;
}

.input-group {
    position: relative;
    margin-bottom: 10px;
}

.input-icon {
    position: absolute;
    left: 13px;
    top: 50%;
    transform: translateY(-50%);
    z-index: 2;
}

.input-eye {
    position: absolute;
    right: 13px;
    top: 50%;
    transform: translateY(-50%);
    cursor: pointer;
    z-index: 3;
}

.login-input {
    width: 85%;
    height: 48px;
    padding: 0 40px 0 40px;
    border: 1px solid #e0e3e7;
    border-radius: 8px;
    background: #fafbfc;
    font-size: 16px;
    outline: none;
}

.login-input:focus {
    border-color: #ffd100;
    background: #fff;
}

.error-tip {
    color: #eb0b0b;
    font-size: 13px;
    margin-left: 5px;
    margin-bottom: 6px;
}

.login-options {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 25px;
    margin-top: 6px;
}

.login-options label {
    font-size: 15px;
    color: #333;
    user-select: none;
}

.forgot-link {
    color: #3c75dd;
    font-size: 15px;
    text-decoration: none;
    cursor: pointer;
}

.forgot-link:hover {
    text-decoration: underline;
}

.login-btn {
    width: 100%;
    height: 50px;
    background: linear-gradient(90deg, #ffd100 0%, #aebf14 100%);
    color: #222;
    border: none;
    border-radius: 25px;
    font-size: 20px;
    font-weight: 500;
    cursor: pointer;
    margin: 26px 0 0;
    box-shadow: 0 5px 20px rgba(60, 117, 221, 0.05);
    transition: box-shadow 0.2s;
}

.login-btn:hover {
    box-shadow: 0 6px 20px rgba(60, 117, 221, 0.12);
}

.signup-tip {
    text-align: center;
    margin-top: 22px;
    font-size: 16px;
    color: #888;
}

.signup-tip a {
    color: #3c75dd;
    text-decoration: none;
    cursor: pointer;
    margin-left: 6px;
}

.signup-tip a:hover {
    text-decoration: underline;
}

@media (max-width: 900px) {
    .login-left {
        border-radius: 0;
    }
}
</style>