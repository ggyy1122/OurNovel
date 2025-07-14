<template>
    <div class="login-left">
        <h2 class="welcome-title">{{state.Role(0)}}登录</h2>
        <div class="subtitle">欢迎加入TJ</div>
        <form @submit.prevent="handleLogin">
            <div class="input-group">
                <span class="input-icon">
                    <svg width="20" height="20" fill="none" stroke="#bbb" stroke-width="2" viewBox="0 0 20 20">
                        <circle cx="10" cy="7" r="4"></circle>
                        <path d="M2,18 a8,6 0 0,1 16,0"></path>
                    </svg>
                </span>
                <input type="text" v-model="username" placeholder="请输入用户名" class="login-input"
                    autocomplete="username" />
            </div>
            <div v-if="usernameError" class="error-tip">请输入用户名</div>
            <div class="input-group">
                <span class="input-icon">
                    <svg width="20" height="20" fill="none" stroke="#bbb" stroke-width="2" viewBox="0 0 20 20">
                        <rect x="4" y="8" width="12" height="8" rx="2"></rect>
                        <path d="M10 8V6a2 2 0 1 1 4 0v2"></path>
                    </svg>
                </span>
                <input :type="showPassword ? 'text' : 'password'" v-model="password" placeholder="请输入密码"
                    class="login-input" autocomplete="current-password" />
                <span class="input-eye" @click="showPassword = !showPassword">
                    <svg v-if="!showPassword" width="20" height="20" viewBox="0 0 20 20">
                        <path d="M1 10c2.5-5 7.5-7.5 18 0-10.5 7.5-15.5 5-18 0z" fill="none" stroke="#bbb"
                            stroke-width="2" />
                        <circle cx="10" cy="10" r="3" fill="none" stroke="#bbb" stroke-width="2" />
                    </svg>
                    <svg v-else width="20" height="20" viewBox="0 0 20 20">
                        <path d="M1 10c2.5-5 7.5-7.5 18 0-10.5 7.5-15.5 5-18 0z" fill="none" stroke="#bbb"
                            stroke-width="2" />
                        <circle cx="10" cy="10" r="3" fill="none" stroke="#bbb" stroke-width="2" />
                        <line x1="5" y1="15" x2="15" y2="5" stroke="#bbb" stroke-width="2" />
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
import { current_state} from '@/store/index';
const state =current_state();

const username = ref("");
const password = ref("");
const showPassword = ref(false);
const rememberMe = ref(false);
const usernameError = ref(false);
const passwordError = ref(false);
const router = useRouter();
const handleLogin = () => {
    usernameError.value = !username.value;
    passwordError.value = !password.value;
    if (username.value && password.value) {
        localStorage.setItem('isLoggedIn', 'true');
        if(state.value==0){//读者
            router.push('/Novels/Novel_Layout/home');
        }
        else if(state.value==1){//作者，还未实现
            router.push('/Novels/Novel_Layout/home');
        }
        else if(state.value==2){//管理员
            router.push('/Admin/Admin_Layout/dashboard');
        }
    }
};

function forgotPassword() {
    alert("请联系管理员重置密码！");
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