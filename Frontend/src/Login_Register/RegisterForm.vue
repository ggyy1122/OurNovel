<template>
    <div class="register-left">
        <h2 class="reg-title">{{ state.Role(0) }}注册</h2>
        <div class="reg-subtitle">欢迎加入TJ</div>
        <form @submit.prevent="handleRegister">
            <div class="input-group" :class="{ 'error-border': usernameError }">
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
            <div class="input-group" :class="{ 'error-border': phoneError }">
                <span class="input-icon">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20" height="20">
                        <path fill="currentColor"
                            d="M224 768v96.064a64 64 0 0 0 64 64h448a64 64 0 0 0 64-64V768zm0-64h576V160a64 64 0 0 0-64-64H288a64 64 0 0 0-64 64zm32 288a96 96 0 0 1-96-96V128a96 96 0 0 1 96-96h512a96 96 0 0 1 96 96v768a96 96 0 0 1-96 96zm304-144a48 48 0 1 1-96 0 48 48 0 0 1 96 0">
                        </path>
                    </svg>
                </span>
                <input type="text" v-model="phone" maxlength="11" placeholder="请输入手机号" class="login-input"
                    autocomplete="tel" />
                <span class="phone-len">{{ phone.length }} / 11</span>
            </div>
            <div v-if="phoneError" class="error-tip">请输入11位手机号</div>
            <!-- 图形验证码部分 -->
            <div class="input-group" :class="{ 'error-border': captchaError }">
                <span class="input-icon">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20" height="20">
                        <path fill="currentColor"
                            d="M160 160v704h704V160zm-32-64h768a32 32 0 0 1 32 32v768a32 32 0 0 1-32 32H128a32 32 0 0 1-32-32V128a32 32 0 0 1 32-32">
                        </path>
                        <path fill="currentColor"
                            d="M384 288q64 0 64 64t-64 64q-64 0-64-64t64-64M185.408 876.992l-50.816-38.912L350.72 556.032a96 96 0 0 1 134.592-17.856l1.856 1.472 122.88 99.136a32 32 0 0 0 44.992-4.864l216-269.888 49.92 39.936-215.808 269.824-.256.32a96 96 0 0 1-135.04 14.464l-122.88-99.072-.64-.512a32 32 0 0 0-44.8 5.952z">
                        </path>
                    </svg>
                </span>
                <input type="text" v-model="captcha" placeholder="请输入图形验证码" class="login-input" />
                <div class="captcha-container">
                    <canvas ref="captchaCanvas" width="120" height="40" @click="generateCaptcha"></canvas>
                </div>
            </div>
            <div v-if="captchaError" class="error-tip">{{ captchaErrorMsg }}</div>
            <!-- 短信验证码输入框 -->
            <div class="input-group" :class="{ 'error-border': codeError }">
                <span class="input-icon">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20" height="20">
                        <path fill="currentColor"
                            d="M160 826.88 273.536 736H800a64 64 0 0 0 64-64V256a64 64 0 0 0-64-64H224a64 64 0 0 0-64 64zM296 800 147.968 918.4A32 32 0 0 1 96 893.44V256a128 128 0 0 1 128-128h576a128 128 0 0 1 128 128v416a128 128 0 0 1-128 128z">
                        </path>
                        <path fill="currentColor"
                            d="M352 512h320q32 0 32 32t-32 32H352q-32 0-32-32t32-32m0-192h320q32 0 32 32t-32 32H352q-32 0-32-32t32-32">
                        </path>
                    </svg>
                </span>
                <input type="text" v-model="code" placeholder="请输入验证码" class="login-input"
                    autocomplete="one-time-code" />
                <button type="button" class="code-btn" @click="getCode" :disabled="codeCountdown > 0">
                    {{ codeCountdown > 0 ? `${codeCountdown}秒后重试` : '获取验证码' }}
                </button>
            </div>
            <div v-if="codeError" class="error-tip">{{ codeErrorMsg }}</div>
            <div class="input-group" :class="{ 'error-border': passwordError }">
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
                <input :type="showPassword ? 'text' : 'password'" v-model="password" placeholder="设置密码"
                    class="login-input" autocomplete="new-password" />
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
            <div class="reg-options">
                <label>
                    <input type="checkbox" v-model="agree" />
                    我已阅读并同意 <a href="#" @click.prevent="showModal = true">服务条款和隐私政策</a>
                </label>
                <div class="modal-overlay" v-if="showModal" @click.self="showModal = false">
                    <div class="modal-content">
                        <h2>服务条款和隐私政策</h2>
                        <div class="modal-body">
                            <p>这里是个人信息保护及隐私政策详细内容...</p>
                            <p>请您务必审慎阅读、充分理解本协议各条款内容，特别是以加粗形式标注提示的条款，以及开通或使用某项服务的单独政策，并选择接受或不接受。我们将在明确获得您同意和接受后，为您提供相应的服务。
                            </p>
                            <p>1. 如果您未满18周岁，请在监护人的陪同下阅读本协议，并特别注意未成年人保护条款。</p>
                        </div>
                        <button @click="showModal = false">关闭</button>
                    </div>
                </div>
            </div>
            <button type="submit" class="reg-btn" :disabled="!agree">立即注册</button>
        </form>
        <div class="login-tip">
            已有账号？<a href="#" @click.prevent="goLogin">返回登录</a>
        </div>

        <!-- 开发模式验证码提示 -->
        <div v-if="showDevCode" class="dev-code-hint">
            <p>验证码: <strong>{{ devCode }}</strong> (仅用于测试)</p>
        </div>
    </div>
</template>

<script setup>
import { ref, onBeforeUnmount, onMounted } from "vue";
import { useRouter } from "vue-router";
import { current_state } from '@/stores/index';
import { registerAuthor, registerManager, registerReader } from '@/API/Log_API';
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";

const showModal = ref(false);
const state = current_state();

// 表单数据
const username = ref("");
const phone = ref("");
const code = ref("");
const password = ref("");
const showPassword = ref(false);
const agree = ref(false);

// 错误状态
const usernameError = ref(false);
const phoneError = ref(false);
const codeError = ref(false);
const codeErrorMsg = ref("");
const passwordError = ref(false);

// 验证码相关
const codeCountdown = ref(0);
const timer = ref(null);
const devCode = ref("");
const showDevCode = ref(false);

const router = useRouter();

// 新增图形验证码相关变量
const captchaCanvas = ref(null);
const captchaText = ref("");
const captcha = ref("");
const captchaError = ref(false);
const captchaErrorMsg = ref("");
// 组件挂载时生成图形验证码
onMounted(() => {
    generateCaptcha();
});

// 生成图形验证码
function generateCaptcha() {
    const ctx = captchaCanvas.value.getContext('2d');
    ctx.clearRect(0, 0, 120, 40);
    // 生成随机字符串（排除容易混淆的字符）
    const chars = 'ABCDEFGHJKLMNPQRSTUVWXYZ23456789';
    let text = '';
    for (let i = 0; i < 4; i++) {
        text += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    captchaText.value = text;
    // 绘制背景
    ctx.fillStyle = '#f8f9fa';
    ctx.fillRect(0, 0, 120, 40);
    // 绘制干扰线
    for (let i = 0; i < 5; i++) {
        ctx.strokeStyle = getRandomColor(100, 200);
        ctx.beginPath();
        ctx.moveTo(Math.random() * 120, Math.random() * 40);
        ctx.lineTo(Math.random() * 120, Math.random() * 40);
        ctx.stroke();
    }
    // 绘制文字
    for (let i = 0; i < text.length; i++) {
        ctx.fillStyle = getRandomColor(0, 100);
        ctx.font = `${20 + Math.random() * 5}px Arial`;
        ctx.fillText(
            text[i],
            20 + i * 25,
            25 + Math.random() * 10
        );
    }
}

// 生成随机颜色
function getRandomColor(min = 0, max = 255) {
    const r = min + Math.floor(Math.random() * (max - min));
    const g = min + Math.floor(Math.random() * (max - min));
    const b = min + Math.floor(Math.random() * (max - min));
    return `rgb(${r},${g},${b})`;
}

// 验证图形验证码
function verifyCaptcha() {
    if (!captcha.value) {
        captchaError.value = true;
        captchaErrorMsg.value = "请输入图形验证码";
        toast.error("请输入图形验证码");
        return false;
    }

    if (captcha.value.toUpperCase() !== captchaText.value) {
        captchaError.value = true;
        captchaErrorMsg.value = "图形验证码错误";
        toast.error("图形验证码错误");
        generateCaptcha(); // 验证失败刷新验证码
        return false;
    }
    captchaError.value = false;
    return true;
}
// 组件卸载前清除定时器
onBeforeUnmount(() => {
    if (timer.value) clearInterval(timer.value);
});

// 生成6位随机验证码
function generateRandomCode() {
    return Math.floor(100000 + Math.random() * 900000).toString();
}

// 获取验证码
function getCode() {
    // 验证手机号格式
    if (!phone.value || phone.value.length !== 11) {
        phoneError.value = true;
        toast.error("请输入正确的11位手机号");
        return;
    }
    // 生成验证码
    const verificationCode = generateRandomCode();
    devCode.value = verificationCode;
    showDevCode.value = true;
    // 存储验证码到localStorage，设置5分钟过期
    const codeData = {
        code: verificationCode,
        phone: phone.value,
        expires: Date.now() + 5 * 60 * 1000 // 5分钟后过期
    };
    localStorage.setItem("verificationCode", JSON.stringify(codeData));
    // 启动倒计时
    codeCountdown.value = 60;
    timer.value = setInterval(() => {
        codeCountdown.value--;
        if (codeCountdown.value <= 0) {
            clearInterval(timer.value);
            timer.value = null;
        }
    }, 1000);
    // 提示用户(开发模式下显示验证码)
    toast.success(`验证码已生成: ${verificationCode}`, {
        description: "开发模式下直接显示验证码",
        autoClose: 10000
    });
}

// 验证验证码
function verifyCode() {
    const storedData = JSON.parse(localStorage.getItem("verificationCode"));
    // 检查是否已发送验证码
    if (!storedData) {
        codeErrorMsg.value = "请先获取验证码";
        toast.error("请先获取验证码");
        return false;
    }
    // 检查验证码是否过期
    if (storedData.expires < Date.now()) {
        codeErrorMsg.value = "验证码已过期，请重新获取";
        toast.error("验证码已过期，请重新获取");
        localStorage.removeItem("verificationCode");
        return false;
    }
    // 检查手机号是否匹配
    if (storedData.phone !== phone.value) {
        codeErrorMsg.value = "验证码与手机号不匹配";
        toast.error("验证码与手机号不匹配");
        return false;
    }
    // 检查验证码是否正确
    if (storedData.code !== code.value) {
        codeErrorMsg.value = "验证码错误";
        toast.error("验证码错误");
        return false;
    }
    return true;
}

const handleRegister = async () => {
    // 重置错误状态
    usernameError.value = !username.value;
    phoneError.value = !phone.value || phone.value.length !== 11;
    codeError.value = !code.value;
    passwordError.value = !password.value;
    // 验证图形验证码
    const isCaptchaValid = verifyCaptcha();
    // 验证短信验证码
    const isCodeValid = code.value ? verifyCode() : false;
    if (usernameError.value || phoneError.value || !isCaptchaValid ||
        !isCodeValid || passwordError.value || !agree.value) {
        if (!code.value) {
            codeError.value = true;
            codeErrorMsg.value = "请输入短信验证码";
            toast.error("请输入短信验证码");
        }
        return;
    }
    try {
        let response;
        if (state.value === 0) { // 读者注册
            response = await registerReader({
                readerName: username.value,
                password: password.value,
                phone: phone.value
            });
        } else if (state.value === 1) { // 作者注册
            response = await registerAuthor({
                authorName: username.value,
                password: password.value
            });
        } else if (state.value === 2) { // 管理员注册
            response = await registerManager({
                managerName: username.value,
                password: password.value
            });
        }
        if (response) {
            toast.success("注册成功！");
            //停顿3s后跳转登录
            setTimeout(() => {
                router.push('/L_R/login');
            }, 3000);
        }
    } catch (error) {
        console.error('注册失败:', error);
        toast.error("注册失败:用户名已存在！");
    }
};

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


.captcha-container {
    position: absolute;
    right: 0;
    top: 50%;
    transform: translateY(-50%);
    margin-right: 0px;
    margin-top: 3px;
}

.captcha-container canvas {
    border: 1px solid #e0e3e7;
    border-radius: 4px;
    cursor: pointer;
    background: #f8f9fa;
}

/* 调整短信验证码按钮位置 */
.code-btn {
    right: 0;
    margin-right: 0;
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

.reg-options a:hover {
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

.modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
}

.modal-content {
    background: white;
    padding: 2rem;
    border-radius: 8px;
    max-width: 600px;
    max-height: 80vh;
    overflow-y: auto;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.modal-body {
    margin: 1.5rem 0;
}

button {
    padding: 0.5rem 1rem;
    background-color: #ffd100;
    color: #f5eeee;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

button:hover {
    background-color: #e89a09;
    color: #222
}

/* 开发模式验证码提示样式 */
.dev-code-hint {
    margin-top: 20px;
    padding: 12px;
    background-color: #f8f9fa;
    border-radius: 8px;
    text-align: center;
    font-size: 14px;
    color: #666;
    border: 1px dashed #ffd100;
    margin-bottom: 20px;
}

.dev-code-hint strong {
    color: #ff4757;
    font-size: 16px;
}

@media (max-width: 900px) {
    .register-left {
        border-radius: 0;
    }
}
</style>