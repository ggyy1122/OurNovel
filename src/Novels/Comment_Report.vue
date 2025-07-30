<template>
    <div class="report-view">
        <div class="report-container">
            <div class="logo-header">
                <img src="@/assets/logo.png" alt="logo" class="logo-img" />
                <span class="logo-title">TJ集团</span>
                <span class="logo-subtitle">TJ举报</span>
            </div>
            <h2>
                <span class="icon-warning">⚠️</span>
                举报评论
            </h2>
            <div class="comment-display">
                <h3>被举报评论内容：</h3>
                <div class="comment-content">{{ comment ? comment.content : '加载中...' }}</div>
            </div>
            <div class="report-content">
                <div class="reasons-list">
                    <h3>请选择举报原因</h3>
                    <div class="reasons-grid">
                        <div v-for="reason in predefinedReasons" :key="reason" class="reason-item"
                            :class="{ active: selectedReason === reason }">
                            <label :for="reason" class="reason-label">
                                <input type="radio" :id="reason" :value="reason" v-model="selectedReason"
                                    class="reason-radio" />
                                <span class="custom-radio"></span>
                                {{ reason }}
                            </label>
                        </div>
                        <div class="reason-item other-reason" :class="{ active: selectedReason === 'other' }">
                            <label for="other" class="reason-label">
                                <input type="radio" id="other" value="other" v-model="selectedReason"
                                    class="reason-radio" />
                                <span class="custom-radio"></span>
                                其他违规
                            </label>
                            <transition name="fade">
                                <textarea v-if="selectedReason === 'other'" v-model="customReason" placeholder="请输入具体原因"
                                    class="custom-reason-input animated-input"></textarea>
                            </transition>
                        </div>
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
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { reportComment } from '@/API/Report_API';
import { getComment } from '@/API/Comment_API';
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";

const route = useRoute();
const router = useRouter();

const readerId = route.params.readerId;
const commentId = route.params.commentId;
const comment = ref(null);

const selectedReason = ref('');
const customReason = ref('');
const predefinedReasons = ref([
    '政治敏感',
    '淫秽色情',
    '低俗恶趣',
    '抄袭信息',
    '恶意营销',
    '人身攻击',
    '欺诈广告',
    '暴力血腥',
    '涉未成年人不良信息'
]);

const fetchComment = async () => {
    try {
        comment.value = await getComment(commentId);
        if (!comment.value) {
            toast.error("评论不存在或已被删除");
            router.back();
        }
    } catch (error) {
        console.error('获取评论失败:', error);
        toast.error("获取评论失败，请稍后再试");
        router.back();
    }
};

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
        await new Promise(resolve => setTimeout(resolve, 4000));
        router.back();
    } catch (error) {
        console.error('举报失败:', error);
        toast.error("举报失败，请稍后再试");
    }
};

const cancelReport = () => {
    router.back();
};
onMounted(async () => {
    await fetchComment();
});

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
    margin-left: 20px;
}

.logo-subtitle {
    font-size: 1.2rem;
    color: #2e4c8c;
    margin-left: 30px;
}

.report-container {
    background: rgba(255, 255, 255, 0.97);
    border-radius: 18px;
    box-shadow: 0 8px 32px 0 rgba(76, 78, 100, 0.18);
    width: 100%;
    max-width: 900px;
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

.comment-display {
    background: #f9f9f9;
    border-radius: 10px;
    padding: 20px;
    margin-bottom: 30px;
    border-left: 4px solid #f56c6c;
}

.comment-display h3 {
    color: #666;
    margin-top: 0;
    margin-bottom: 10px;
    font-size: 1.1rem;
}

.comment-content {
    font-size: 1.1rem;
    line-height: 1.6;
    color: #333;
    padding: 10px;
    background: white;
    border-radius: 5px;
    border: 1px solid #eee;
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

.reasons-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 12px;
    margin-bottom: 20px;
}

.reason-item {
    display: flex;
    align-items: center;
    gap: 10px;
    padding: 13px 16px;
    border-radius: 7px;
    background: transparent;
    transition: background 0.25s, box-shadow 0.25s;
    cursor: pointer;
    border: 1.5px solid transparent;
    position: relative;
    min-height: 60px;
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

.other-reason {
    grid-column: span 3;
}

.custom-reason-input {
    width: 100%;
    margin-top: 10px;
    padding: 12px 15px;
    border: 1.5px solid #f56c6c;
    border-radius: 6px;
    font-size: 15px;
    background: #fff;
    color: #a03232;
    outline: none;
    box-shadow: 0 2px 8px 0 rgba(245, 108, 108, 0.07);
    transition: border-color 0.2s, box-shadow 0.2s;
    min-height: 100px;
    resize: vertical;
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

@media (max-width: 768px) {
    .reasons-grid {
        grid-template-columns: repeat(2, 1fr);
    }

    .other-reason {
        grid-column: span 2;
    }
}

@media (max-width: 600px) {
    .report-container {
        padding: 18px 5vw;
    }

    .reasons-grid {
        grid-template-columns: 1fr;
    }

    .other-reason {
        grid-column: span 1;
    }

    .reason-item {
        flex-direction: row;
        align-items: center;
        gap: 10px;
    }

    .custom-reason-input {
        margin-left: 0;
        margin-top: 10px;
        width: 100%;
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