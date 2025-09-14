<template>
    <div class="login-left">
        <h2 class="welcome-title">{{ state.Role(0) }}重置密码</h2>
        <div class="subtitle">欢迎加入TJ</div>
        <form @submit.prevent="handleResetPassword">
            <div class="input-group">
                <span class="input-icon">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20" height="20">
                        <path fill="currentColor"
                            d="M288 320a224 224 0 1 0 448 0 224 224 0 1 0-448 0m544 608H160a32 32 0 0 1-32-32v-96a160 160 0 0 1 160-160h448a160 160 0 0 1 160 160v96a32 32 0 0 1-32 32z">
                        </path>
                    </svg>
                </span>
                <input type="text" v-model="username" placeholder="请输入用户名" class="login-input" />
            </div>
            <div v-if="usernameError" class="error-tip">请输入用户名</div>

            <!-- 手机号输入框部分 -->
            <div class="input-group" :class="{ 'error-border': phoneError }">
                <span class="input-icon">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20" height="20">
                        <path fill="currentColor"
                            d="M224 768v96.064a64 64 0 0 0 64 64h448a64 64 0 0 0 64-64V768zm0-64h576V160a64 64 0 0 0-64-64H288a64 64 0 0 0-64 64zm32 288a96 96 0 0 1-96-96V128a96 96 0 0 1 96-96h512a96 96 0 0 1 96 96v768a96 96 0 0 1-96 96zm304-144a48 48 0 1 1-96 0 48 48 0 0 1 96 0">
                        </path>
                    </svg>
                </span>
                <input type="text" v-model="phone" maxlength="11" placeholder="请输入该用户的手机号码" class="login-input" />
                <span class="phone-len">{{ phone.length }} / 11</span>
            </div>
            <div v-if="phoneError" class="error-tip">请输入11位手机号</div>

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
                <input :type="showPassword ? 'text' : 'password'" v-model="password" placeholder="请输入新密码"
                    class="login-input" />
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
            <div v-if="passwordError" class="error-tip">请输入新密码</div>

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
                <input :type="showPassword1 ? 'text' : 'password'" v-model="confirmPassword" placeholder="请再次输入新密码"
                    class="login-input" />
                <span class="input-eye" @click="showPassword1 = !showPassword1">
                    <svg v-if="!showPassword1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20"
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
            <div v-if="confirmPasswordError" class="error-tip">两次输入的密码不一致</div>

            <button type="submit" class="login-btn">确认更改密码</button>
        </form>
        <div class="return-login-tip">
            <a href="#" @click.prevent="returnToLogin">返回登录</a>
        </div>
    </div>
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import { current_state } from '@/stores/index';
import { resetAuthorPassword, resetReaderPassword } from '@/API/Log_API';
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";

const state = current_state();
const phone = ref("");
const phoneError = ref(false);

const username = ref("");
const password = ref("");
const confirmPassword = ref("");
const showPassword = ref(false);
const showPassword1 = ref(false);
const usernameError = ref(false);
const passwordError = ref(false);
const confirmPasswordError = ref(false);
const router = useRouter();

const handleResetPassword = async () => {
    // Reset error states
    usernameError.value = !username.value;
    phoneError.value = !phone.value || phone.value.length !== 11;
    passwordError.value = !password.value;
    confirmPasswordError.value = password.value !== confirmPassword.value;

    if (username.value && phone.value && password.value && !confirmPasswordError.value) {
        try {
            let response;
            const dto = {
                [state.value === 0 ? 'readerName' :
                    state.value === 1 ? 'authorName' : 'managerName']: username.value,
                phone: phone.value,
                password: password.value
            };
            if (state.value === 0) { // 读者
                response = await resetReaderPassword(dto);
            } else if (state.value === 1) { // 作者
                response = await resetAuthorPassword(dto);
            }
            console.log('密码重置成功', response.data);
            toast("密码重置成功，请使用新密码登录", {
                "type": "success",
                "dangerouslyHTMLString": true
            });
            //停顿3s后跳转
            setTimeout(() => {
                router.push('/L_R/login');
            }, 3000);
        } catch (error) {
            console.error('密码重置失败:', error);
            // 根据后端返回的错误信息显示不同的提示
            if (error.response?.data?.includes('手机号码不匹配')) {
                toast("手机号码与注册信息不匹配，请重新输入", {
                    "type": "error",
                    "dangerouslyHTMLString": true
                });
            } else {
                toast("密码重置失败，请检查用户名或联系管理员", {
                    "type": "error",
                    "dangerouslyHTMLString": true
                });
            }
        }
    }
};
const returnToLogin = () => {
    router.push('/L_R/login');
};
</script>

<style scoped>
/* 使用与LoginForm相同的样式 */
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

.error-border .login-input {
    border-color: #eb0b0b;
}

.phone-len {
    position: absolute;
    right: 10px;
    top: 50%;
    transform: translateY(-50%);
    color: #6f6e6e;
    font-size: 13px;
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

.return-login-tip {
    text-align: center;
    margin-top: 22px;
    font-size: 16px;
    color: #888;
}

.return-login-tip a {
    color: #3c75dd;
    text-decoration: none;
    cursor: pointer;
}

.return-login-tip a:hover {
    text-decoration: underline;
}

@media (max-width: 900px) {
    .login-left {
        border-radius: 0;
    }
}
</style>