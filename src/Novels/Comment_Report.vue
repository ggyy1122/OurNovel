<template>
    <div class="report-view">
        <div class="report-container">
            <h2>
                <span class="icon-warning">⚠️</span>
                举报评论
            </h2>
            <div class="report-content">
                <div class="reasons-list">
                    <h3>请选择举报原因</h3>
                    <div v-for="reason in predefinedReasons" :key="reason" class="reason-item"
                        :class="{ active: selectedReason === reason }">
                        <label :for="reason" class="reason-label">
                            <input type="radio" :id="reason" :value="reason" v-model="selectedReason"
                                class="reason-radio" />
                            <span class="custom-radio"></span>
                            {{ reason }}
                        </label>
                    </div>
                    <div class="reason-item" :class="{ active: selectedReason === 'other' }">
                        <label for="other" class="reason-label">
                            <input type="radio" id="other" value="other" v-model="selectedReason"
                                class="reason-radio" />
                            <span class="custom-radio"></span>
                            其他原因
                        </label>
                        <transition name="fade">
                            <input v-if="selectedReason === 'other'" type="text" v-model="customReason"
                                placeholder="请输入具体原因" class="custom-reason-input animated-input">
                        </transition>
                    </div>
                </div>
                <div class="actions">
                    <button @click="submitReport" class="submit-btn gradient-btn">提交举报</button>
                    <button @click="cancelReport" class="cancel-btn">返回</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { reportComment } from '@/API/Report_API';
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";

const route = useRoute();
const router = useRouter();

const readerId = route.params.readerId;
const commentId = route.params.commentId;

const selectedReason = ref('');
const customReason = ref('');
const predefinedReasons = ref([
    '包含不当语言',
    '包含广告或垃圾信息',
    '包含人身攻击',
    '包含不实信息',
    '包含敏感内容'
]);

const submitReport = async () => {
    if (!selectedReason.value) {
        toast.warning("请选择举报原因");
        return;
    }

    const reason = selectedReason.value === 'other'
        ? customReason.value
        : selectedReason.value;

    if (!reason.trim()) {
        toast.warning("请输入举报原因");
        return;
    }

    try {
        await reportComment(commentId, readerId, reason);
        toast.success("举报已提交，我们会尽快处理");
        router.back();
    } catch (error) {
        console.error('举报失败:', error);
        toast.error("举报失败，请稍后再试");
    }
};

const cancelReport = () => {
    router.back();
};
</script>

<style scoped>
.report-view {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    background: linear-gradient(120deg, #f8fafc 0%, #f3e6d6 100%);
    padding: 20px;
}

.report-container {
    background: rgba(255, 255, 255, 0.97);
    border-radius: 18px;
    box-shadow: 0 8px 32px 0 rgba(76, 78, 100, 0.18);
    width: 100%;
    max-width: 480px;
    padding: 38px 32px 30px 32px;
    position: relative;
    transition: box-shadow 0.3s;
}

.report-container:hover {
    box-shadow: 0 12px 40px 0 rgba(76, 78, 100, 0.28);
}

.report-container h2 {
    display: flex;
    align-items: center;
    margin-top: 0;
    color: #e25353;
    text-align: center;
    font-size: 2.1rem;
    font-weight: 700;
    justify-content: center;
    letter-spacing: 1px;
    padding-bottom: 22px;
    border-bottom: 1px solid #f0e7ea;
    margin-bottom: 28px;
}

.icon-warning {
    font-size: 2.2rem;
    margin-right: 10px;
}

.reasons-list {
    margin-bottom: 32px;
}

.reasons-list h3 {
    margin-bottom: 18px;
    font-size: 1.06rem;
    color: #8f8f8f;
    font-weight: 600;
    letter-spacing: 0.5px;
}

.reason-item {
    display: flex;
    align-items: center;
    gap: 10px;
    margin-bottom: 14px;
    padding: 13px 16px;
    border-radius: 7px;
    background: transparent;
    transition: background 0.25s, box-shadow 0.25s;
    cursor: pointer;
    border: 1.5px solid transparent;
    position: relative;
}

.reason-item.active,
.reason-item:hover {
    background: #fef2f2;
    border-color: #f56c6c;
    box-shadow: 0 1px 4px 0 rgba(245, 108, 108, 0.04);
}

.reason-label {
    display: flex;
    align-items: center;
    flex: 1;
    cursor: pointer;
    font-size: 1.05rem;
    color: #444;
    font-weight: 500;
    gap: 7px;
    user-select: none;
}

/* Custom radio style */
.reason-radio {
    opacity: 0;
    position: absolute;
    left: 0;
    width: 24px;
    height: 24px;
    cursor: pointer;
}

.custom-radio {
    display: inline-block;
    width: 20px;
    height: 20px;
    border: 2px solid #e5e5e5;
    border-radius: 50%;
    margin-right: 9px;
    background: #fff;
    position: relative;
    transition: border-color 0.2s;
    flex-shrink: 0;
}

.reason-radio:checked+.custom-radio {
    border-color: #f56c6c;
    background: #fff5f5;
}

.reason-radio:checked+.custom-radio::after {
    content: '';
    display: block;
    width: 10px;
    height: 10px;
    background: #f56c6c;
    border-radius: 50%;
    position: absolute;
    left: 3px;
    top: 3px;
}

.custom-reason-input {
    flex: 2;
    margin-left: 15px;
    padding: 8px 13px;
    border: 1.5px solid #f56c6c;
    border-radius: 4px;
    font-size: 15px;
    background: #fff;
    color: #a03232;
    outline: none;
    box-shadow: 0 2px 8px 0 rgba(245, 108, 108, 0.07);
    transition: border-color 0.2s, box-shadow 0.2s;
    min-width: 160px;
    margin-top: 0;
}

.custom-reason-input:focus {
    border-color: #e25353;
    box-shadow: 0 4px 16px 0 rgba(245, 108, 108, 0.11);
}

.animated-input {
    animation: fadeInInput 0.33s;
}

@keyframes fadeInInput {
    from {
        opacity: 0;
        transform: translateY(-10px);
    }

    to {
        opacity: 1;
        transform: translateY(0);
    }
}

.fade-enter-active,
.fade-leave-active {
    transition: opacity 0.3s;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}

.fade-enter-to,
.fade-leave-from {
    opacity: 1;
}

.actions {
    display: flex;
    justify-content: flex-end;
    gap: 14px;
    margin-top: 20px;
}

.gradient-btn {
    background: linear-gradient(90deg, #f56c6c 0%, #f78ca5 100%);
    color: white;
    border: none;
    border-radius: 5px;
    padding: 11px 30px;
    font-size: 1.07rem;
    font-weight: 600;
    box-shadow: 0 2px 8px 0 rgba(255, 115, 115, 0.08);
    cursor: pointer;
    transition: background 0.22s, transform 0.1s;
    outline: none;
}

.gradient-btn:hover {
    background: linear-gradient(90deg, #e25353 0%, #f78ca5 100%);
    transform: translateY(-1.5px) scale(1.025);
}

.cancel-btn {
    padding: 11px 30px;
    background-color: #f0f0f0;
    color: #666;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-size: 1.07rem;
    font-weight: 500;
    transition: background-color 0.18s, color 0.18s, transform 0.1s;
    outline: none;
}

.cancel-btn:hover {
    background-color: #e0e0e0;
    color: #d35454;
    transform: translateY(-1.5px) scale(1.03);
}

@media (max-width: 600px) {
    .report-container {
        padding: 18px 5vw;
    }

    .reason-item {
        flex-direction: column;
        align-items: flex-start;
        gap: 5px;
    }

    .custom-reason-input {
        margin-left: 0;
        margin-top: 8px;
        width: 100%;
        min-width: 0;
    }

    .actions {
        flex-direction: column;
        gap: 10px;
    }

    .gradient-btn,
    .cancel-btn {
        width: 100%;
        padding-left: 0;
        padding-right: 0;
    }
}
</style>