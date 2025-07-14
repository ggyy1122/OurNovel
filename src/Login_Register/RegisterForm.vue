<template>
    <div class="register-left">
        <h2 class="reg-title">{{state.Role(0)}}注册</h2>
        <div class="reg-subtitle">欢迎加入TJ</div>
        <form @submit.prevent="handleRegister">
            <div class="input-group" :class="{ 'error-border': usernameError }">
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
            <div class="input-group" :class="{ 'error-border': phoneError }">
                <span class="input-icon">
                    <svg width="20" height="20" fill="none" stroke="#bbb" stroke-width="2" viewBox="0 0 20 20">
                        <rect x="4" y="5" width="12" height="10" rx="2"></rect>
                        <circle cx="10" cy="15" r="1"></circle>
                    </svg>
                </span>
                <input type="text" v-model="phone" maxlength="11" placeholder="请输入手机号" class="login-input"
                    autocomplete="tel" />
                <span class="phone-len">{{ phone.length }} / 11</span>
            </div>
            <div v-if="phoneError" class="error-tip">请输入手机号</div>
            <div class="input-group" :class="{ 'error-border': codeError }">
                <span class="input-icon">
                    <svg width="20" height="20" fill="none" stroke="#bbb" stroke-width="2" viewBox="0 0 20 20">
                        <rect x="4" y="8" width="12" height="8" rx="2"></rect>
                        <path d="M10 8V6a2 2 0 1 1 4 0v2"></path>
                    </svg>
                </span>
                <input type="text" v-model="code" placeholder="手机验证码" class="login-input"
                    autocomplete="one-time-code" />
                <button type="button" class="code-btn" @click="getCode" :disabled="codeCountdown > 0">获取验证码</button>
            </div>
            <div v-if="codeError" class="error-tip">请输入手机验证码</div>
            <div class="input-group" :class="{ 'error-border': passwordError }">
                <span class="input-icon">
                    <svg width="20" height="20" fill="none" stroke="#bbb" stroke-width="2" viewBox="0 0 20 20">
                        <rect x="4" y="8" width="12" height="8" rx="2"></rect>
                        <path d="M10 8V6a2 2 0 1 1 4 0v2"></path>
                    </svg>
                </span>
                <input :type="showPassword ? 'text' : 'password'" v-model="password" placeholder="设置密码"
                    class="login-input" autocomplete="new-password" />
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
            <div class="reg-options">
                <label>
                    <input type="checkbox" v-model="agree" />
                    我已阅读并同意 <a href="#">服务条款和隐私政策</a>
                </label>
            </div>
            <button type="submit" class="reg-btn" :disabled="!agree">立即注册</button>
        </form>
        <div class="login-tip">
            已有账号？<a href="#" @click.prevent="goLogin">返回登录</a>
        </div>
    </div>
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import { current_state} from '@/store/index';
const state =current_state();

const username = ref("");
const phone = ref("");
const code = ref("");
const password = ref("");
const showPassword = ref(false);
const agree = ref(false);

const usernameError = ref(false);
const phoneError = ref(false);
const codeError = ref(false);
const passwordError = ref(false);
const router = useRouter();

const codeCountdown = ref(0);
let timer = null;
function getCode() {
    if (!phone.value || phone.value.length !== 11) {
        phoneError.value = true;
        return;
    }
    codeCountdown.value = 60;
    timer = setInterval(() => {
        codeCountdown.value--;
        if (codeCountdown.value <= 0) clearInterval(timer);
    }, 1000);
    alert("验证码已发送到手机: " + phone.value);
}

const handleRegister = () => {
    usernameError.value = !username.value;
    phoneError.value = !phone.value || phone.value.length !== 11;
    codeError.value = !code.value;
    passwordError.value = !password.value;
    if (!(usernameError.value || phoneError.value || codeError.value || passwordError.value) && agree.value) {
        alert("注册成功！");
        router.push('/L_R/login');
    }
}

function goLogin() {
    router.push('/L_R/login');
}
</script>

<style scoped>
.register-left {
    width: 70%;
    padding: 60px 50px 0 50px;
    display: flex;
    flex-direction: column;
    justify-content: center;
}

.reg-title {
    font-size: 32px;
    font-weight: bold;
    text-align: center;
    margin-bottom: 14px;
    margin-top: 0;
}

.reg-subtitle {
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

.error-border .login-input {
    border-color: #eb0b0b;
}

.input-eye {
    position: absolute;
    right: 13px;
    top: 50%;
    transform: translateY(-50%);
    cursor: pointer;
    z-index: 3;
}

.phone-len {
    position: absolute;
    right: 10px;
    top: 50%;
    transform: translateY(-50%);
    color: #bbb;
    font-size: 13px;
}

.code-btn {
    position: absolute;
    right: 0px;
    top: 7px;
    height: 34px;
    background: #ffd100;
    color: #222;
    border: none;
    border-radius: 8px;
    padding: 0 16px;
    font-size: 15px;
    cursor: pointer;
}

.code-btn:disabled {
    background: #aaa;
    cursor: not-allowed;
}

.reg-options {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 25px;
    margin-top: 6px;
    font-size: 15px;
    color: #333;
    user-select: none;
}

.reg-options a {
    color: #3c75dd;
    text-decoration: none;
    margin-left: 6px;
}
.reg-options a:hover{
    text-decoration: underline;
}

.reg-btn {
    width: 100%;
    height: 50px;
    background: linear-gradient(90deg, #ffd100 0%, #bacd12 100%);
    color: #222;
    border: none;
    border-radius: 25px;
    font-size: 20px;
    font-weight: 500;
    cursor: pointer;
    margin: 6px 0 0;
    box-shadow: 0 5px 20px rgba(60, 117, 221, 0.05);
    transition: box-shadow 0.2s;
}

.reg-btn:disabled {
    background: linear-gradient(90deg, #f1e199 0%, #cad476 100%);
    color: #fff;
    cursor: not-allowed;
}

.reg-btn:hover {
    box-shadow: 0 6px 20px rgba(60, 117, 221, 0.12);
}

.login-tip {
    text-align: center;
    margin-top: 8px;
    font-size: 16px;
    color: #888;
}

.login-tip a {
    color: #3c75dd;
    text-decoration: none;
    cursor: pointer;
    margin-left: 6px;
}

.login-tip a:hover {
    text-decoration: underline;
}

@media (max-width: 900px) {
    .register-left {
        border-radius: 0;
    }
}
</style>