<template>
    <div class="recharge-main">
        <div class="logo-header">
            <img src="@/assets/logo.png" alt="logo" class="logo-img" />
            <span class="logo-title">TJ集团</span>
            <span class="logo-subtitle">TJ充值</span>
            <span class="return" @click="handle_return"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024"
                    height="20" width="20" style="vertical-align: -4px;">
                    <path fill="currentColor" d="M224 480h640a32 32 0 1 1 0 64H224a32 32 0 0 1 0-64"></path>
                    <path fill="currentColor"
                        d="m237.248 512 265.408 265.344a32 32 0 0 1-45.312 45.312l-288-288a32 32 0 0 1 0-45.312l288-288a32 32 0 1 1 45.312 45.312z">
                    </path>
                </svg> 返回</span>
        </div>
        <div class="recharge-panel">
            <!-- 账号 -->
            <div class="account-row">
                <div class="account-info">
                    <img :src="reader_state.formattedAvatarUrl" alt="用户头像" class="user-avatar" />
                    <div class="account-details">
                        <div class="detail-row">
                            <span class="detail-label">账号名称：</span>
                            <span class="detail-value">{{ reader_state.readerName }}</span>
                        </div>
                        <div class="detail-row">
                            <span class="detail-label">账号ID：</span>
                            <span class="detail-value">{{ reader_state.readerId }}</span>
                        </div>
                        <div class="detail-row">
                            <span class="detail-label">账号余额：</span>
                            <span class="detail-value">{{ accountBalance }}</span>
                        </div>
                    </div>
                </div>
            </div>
            <!-- 充值方式 (仅支付宝) -->
            <div class="section">
                <div class="section-label">充值方式(仅支付宝)</div>
                <div class="method-row">
                    <button class="pay-method active">
                        <img src="@/assets/bao.png" alt="支付宝" class="pay-icon" />
                        支付宝
                    </button>
                </div>
            </div>
            <!-- 金额选择 -->
            <div class="section">
                <div class="section-label">充值金额</div>
                <div class="amount-grid">
                    <button v-for="amount in fixedAmounts" :key="amount.value"
                        :class="['amount-btn', { active: selectedAmount === amount.value }]"
                        @click="selectAmount(amount.value)">
                        <span class="amount-val">¥{{ amount.value }}</span>
                        <span class="amount-desc">({{ amount.points }}起点币)</span>
                    </button>
                    <div class="amount-btn custom" :class="{ active: selectedAmount === 'custom' }">
                        <span>其它金额 ¥</span>
                        <input type="number" v-model="customAmount" placeholder="输入金额" @focus="selectCustomAmount"
                            class="custom-input" />
                        <span class="amount-desc">（{{ customAmount || 0 }}起点币）</span>
                    </div>
                </div>
            </div>
            <!-- 支付按钮 -->
            <button class="pay-mainbtn" :disabled="!agreed || !(selectedAmount || customAmount)" @click="handlePayment">
                立即支付 ¥{{ paymentAmount }}
            </button>
            <!-- 协议 -->
            <div class="agreement-row">
                <input type="checkbox" id="agree" v-model="agreed" />
                <label for="agree" @click.prevent="showAgreement = true" class="agreement-link">
                    我同意《用户服务协议》
                </label>
            </div>
            <!-- 协议弹窗 -->
            <div v-if="showAgreement" class="agreement-popup">
                <div class="popup-content">
                    <h3>用户服务协议</h3>
                    <div class="popup-text">
                        为使用阅文集团的服务，您应当阅读并遵守《用户服务协议》（以下简称"本协议"）。请您务必审慎阅读、充分理解各条款内容，特别是免除或者限制责任的条款，所有加粗字体为我方重点提示的内容。如果您未满18周岁，或者不具备完全民事行为能力，请在法定监护人的陪同下阅读本协议，并特别注意未成年人使用条款。否则阅文集团对于该等后续注册、使用网站服务等行为而对您所发生的不利后果不承担责任，并有权在知晓该等情况后解除双方间的服务协议。
                    </div>
                    <button class="close-popup" @click="showAgreement = false">关闭</button>
                </div>
                <div class="popup-mask" @click="showAgreement = false"></div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, computed,onMounted } from 'vue'
import { startRecharge } from '@/API/Recharge_API'
import { readerState } from '@/stores/index'
import { useRouter } from 'vue-router'
import { toast } from "vue3-toastify"
import "vue3-toastify/dist/index.css"
import {getReaderBalance} from '@/API/Reader_API';

const reader_state = readerState()
const router = useRouter()
const fixedAmounts = [
    { value: 10, points: 1000 },
    { value: 20, points: 2000 },
    { value: 30, points: 3000 },
    { value: 50, points: 5000 },
    { value: 100, points: 10000 }
]
const selectedAmount = ref(fixedAmounts[0].value)
const customAmount = ref('')
const agreed = ref(false)
const showAgreement = ref(false)
const accountBalance = ref(0);                        // 账号余额

const paymentAmount = computed(() => {
    if (selectedAmount.value === 'custom') {
        return customAmount.value || 0
    }
    return selectedAmount.value || customAmount.value || 0
})

function selectAmount(amount) {
    selectedAmount.value = amount
    customAmount.value = ''
}
function selectCustomAmount() {
    selectedAmount.value = 'custom'
}
function handle_return() {
    router.back()
}
const fetchReaderBalance=async()=>{
  try {
    const response = await getReaderBalance(reader_state.readerId)
    accountBalance.value = response.data?.balance || response?.balance|| 0
   console.log('获取用户余额:', accountBalance.value)
  } catch (error) {
    console.error('获取用户余额失败:', error)
     accountBalance.value = 0
  }
}
async function handlePayment() {
    if (!agreed.value) {
        toast("请同意用户服务协议！", { type: "warning", dangerouslyHTMLString: true })
        return
    }
    if (!paymentAmount.value || paymentAmount.value <= 0) {
        toast("请输入有效的充值金额！", { type: "warning", dangerouslyHTMLString: true })
        return
    }
    try {
        const rechargeData = {
            ReaderId: parseInt(reader_state.readerId),
            Amount: parseFloat(paymentAmount.value)
        }
        const response = await startRecharge(rechargeData)
        toast(`即将跳转到支付宝支付页面`, { type: "success", dangerouslyHTMLString: true })
        await new Promise(resolve => setTimeout(resolve, 1500))
        window.open(response.paymentUrl, '_blank')
    } catch (error) {
        toast("充值失败，请稍后重试！", { type: "error", dangerouslyHTMLString: true })
    }
}
 onMounted(() => {
      fetchReaderBalance(); // 页面挂载后立即执行
    });


</script>

<style scoped>
.recharge-main {
    background: #f7f7f7;
    min-height: 100vh;
}

.logo-header {
    display: flex;
    align-items: center;
    background: #fff;
    max-width: 1000px;
    margin: 10px auto;
    border-radius: 8px;
    box-shadow: 0 2px 12px #00000010;
    padding: 10px 32px;
}

.logo-img {
    height: 48px;
    margin-right: 12px;
}

.logo-title {
    font-size: 2rem;
    font-weight: bold;
    color: #1a2d51;
    margin-right: 18px;
}

.logo-subtitle {
    font-size: 1.2rem;
    color: #2e4c8c;
}

.return {
    font-size: 18px;
    color: #383737;
    cursor: pointer;
    margin-left: 650px;
}

.return:hover {
    color: #e67d06;
}

.recharge-panel {
    background: #fff;
    max-width: 1000px;
    margin: 32px auto;
    border-radius: 8px;
    box-shadow: 0 2px 12px #00000010;
    padding: 42px 38px 32px 38px;
}

.account-row {
    margin-bottom: 32px;
}

.account-label {
    color: #333;
    font-weight: 600;
    font-size: 1.1rem;
    margin-bottom: 12px;
    display: block;
}

.account-info {
    display: flex;
    align-items: flex-start;
    gap: 20px;
    background: #f9f9f9;
    padding: 16px;
    border-radius: 8px;
    border: 1px solid #eaeaea;
}

.user-avatar {
    width: 60px;
    height: 60px;
    border-radius: 50%;
    border: 2px solid #2e4c8c;
    object-fit: cover;
}

.account-details {
    flex: 1;
}

.detail-row {
    display: flex;
    margin-bottom: 8px;
    font-size: 1rem;
}

.detail-row:last-child {
    margin-bottom: 0;
}

.detail-label {
    color: #666;
    min-width: 80px;
}

.detail-value {
    font-size: 18px;
    color: #2b1a0c;
    font-weight: 700;
}

.account-row {
    display: flex;
    align-items: center;
    margin-bottom: 32px;
}

.account-label {
    color: #333;
    font-weight: 500;
    margin-right: 18px;
    font-size: 1.1rem;
}

.user-avatar {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    border: 2px solid #2e4c8c;
    object-fit: cover;
    margin-right: 18px;
}

.account-value {
    font-size: 1.05rem;
    color: #324d6d;
    font-weight: 500;
}

.section {
    margin-bottom: 30px;
}

.section-label {
    font-size: 1rem;
    font-weight: 600;
    color: #333;
    margin-bottom: 14px;
}

.method-row {
    display: flex;
    gap: 12px;
    margin-bottom: 14px;
}

.pay-method {
    background: #fff;
    border: 2px solid #1890ff;
    color: #1890ff;
    font-weight: 600;
    font-size: 1rem;
    padding: 8px 26px 8px 20px;
    border-radius: 6px;
    display: flex;
    align-items: center;
    gap: 10px;
    cursor: default;
    position: relative;
}

.pay-method.active {
    box-shadow: 0 2px 8px #1890ff22;
}

.pay-icon {
    width: 28px;
    height: 28px;
}

.amount-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 20px 24px;
}

.amount-btn {
    background: #fff;
    border: 2px solid #d6d6d6;
    border-radius: 8px;
    padding: 16px 18px 10px 18px;
    font-size: 1.15rem;
    color: #222;
    display: flex;
    align-items: flex-start;
    cursor: pointer;
    position: relative;
    transition: border-color 0.2s;
}

.amount-btn.active {
    border-color: #d33d3d;
    color: #d33d3d;
}

.amount-val {
    font-size: 1.25rem;
    font-weight: 600;
}

.amount-desc {
    font-size: 0.95rem;
    color: #666;
    margin-left: 15px;
}

.amount-btn.custom {
    grid-column: 1 / span 2;
    flex-direction: row;
    padding: 10px 18px;
    align-items: center;
    gap: 10px;
}

.custom-input {
    border: none;
    border-bottom: 2px solid #d6d6d6;
    border-radius: 0;
    padding: 6px 0;
    width: 98px;
    font-size: 1.1rem;
    outline: none;
    margin: 0 6px;
}

.pay-mainbtn {
    width: 100%;
    padding: 13px 0;
    background: #bbb;
    color: #fff;
    border: none;
    border-radius: 6px;
    font-size: 1.18rem;
    font-weight: 600;
    margin-top: 5px;
    margin-bottom: 12px;
    cursor: pointer;
    transition: background 0.16s;
}

.pay-mainbtn[disabled] {
    background: #eee;
    color: #aaa;
    cursor: not-allowed;
}

.pay-mainbtn:not([disabled]) {
    background: #d33d3d;
}

.agreement-row {
    display: flex;
    align-items: center;
    font-size: 1rem;
    gap: 6px;
    margin-top: 8px;
}

.agreement-link {
    color: #1890ff;
    text-decoration: underline;
    cursor: pointer;
    margin-left: 3px;
}

.agreement-link:hover {
    color: #fd190d;
}

.agreement-popup {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    z-index: 9999;
}

.popup-mask {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: #0002;
}

.popup-content {
    position: absolute;
    left: 50%;
    top: 50%;
    transform: translate(-50%, -50%);
    background: #ffffff;
    border-radius: 10px;
    box-shadow: 0 2px 20px #0002;
    padding: 32px 34px 22px 34px;
    width: 400px;
    max-width: 90vw;
    max-height: 70vh;
    display: flex;
    flex-direction: column;
    align-items: center;
}

.popup-content h3 {
    margin-bottom: 16px;
    font-size: 1.15rem;
    color: #222;
}

.popup-text {
    font-size: 0.99rem;
    color: #444;
    margin-bottom: 20px;
    overflow-y: auto;
    max-height: 260px;
    width: 100%;
    line-height: 1.6;
}

.close-popup {
    padding: 7px 20px;
    border-radius: 5px;
    border: none;
    background: #1890ff;
    color: #fff;
    font-size: 1rem;
    cursor: pointer;
}
</style>