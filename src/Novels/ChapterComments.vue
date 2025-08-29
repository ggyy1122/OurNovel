<template>
    <div class="chapter-comments-container">
        <div class="novel-info-sidebar">
            <div class="novel-info-card">
                <img :src="selectNovelState.formattedcoverUrl" :alt="selectNovelState.novelName" class="cover-image"
                    @click="handle_NovelInfro" />
                <h3 @click="handle_NovelInfro">{{ selectNovelState.novelName }}</h3>
                <span class="author-label">作者：</span><span class="author" @click="goAuthorHome">{{
                    selectNovelState.authorName }}</span>
                <div class="stats">
                    <span>
                        收藏数
                        <span class="num">{{ selectNovelState.collectedCount }}</span>
                    </span>
                    <span>
                        推荐数
                        <span class="num">{{ selectNovelState.recommendCount }}</span>
                    </span>
                </div>
            </div>
        </div>
        <div class="comments-container">
            <div class="header">
                <button class="back-button" @click="goBack">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="24" height="24">
                        <path fill="currentColor" d="M20 11H7.83l5.59-5.59L12 4l-8 8 8 8 1.41-1.41L7.83 13H20v-2z" />
                    </svg>
                    返回阅读
                </button>
                <h2>第{{ selectNovelState.chapterId }}章 {{ selectNovelState.cha_title }} 章评
                </h2>
            </div>
            <div class="comments-header">
                <h3>全部评论 ({{ totalComments }})</h3>
            </div>
            <div class="comment-input">
                <img :src="reader_state.formattedAvatarUrl" alt="用户头像" class="user-avatar" v-if="state.isloggedin" />
                <input type="text" placeholder="写下你的想法..." v-model="newComment" @keyup.enter="submitComment"
                    :disabled="!state.isloggedin" />
                <button @click="submitComment" :disabled="!state.isloggedin || !newComment.trim()">发送</button>
            </div>
            <div class="comments-list">
                <div v-if="loading" class="loading">加载中...</div>
                <div v-else-if="comments.length === 0" class="no-comments">暂无评论</div>
                <div v-for="comment in comments" :key="comment.commentId" class="comment-item">
                    <div class="comment-header">
                        <img :src="getReaderAvatar(comment.readerId)" alt="用户头像" class="comment-avatar"
                            @error="handleAvatarError" @click="goReaderHome(comment.readerId)" />
                        <div class="comment-info">
                            <span class="comment-author" @click="goReaderHome(comment.readerId)">{{
                                getReaderName(comment.readerId) }}</span>
                            <span class="comment-time">{{ formatTime(comment.createTime) }}</span>
                        </div>
                        <button class="like-btn" @click="toggleLike(comment)"
                            :class="{ liked: isCommentLiked(comment.commentId) }">
                            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="16" height="16">
                                <path fill="currentColor"
                                    d="M1 21h4V9H1v12zm22-11c0-1.1-.9-2-2-2h-6.31l.95-4.57.03-.32c0-.41-.17-.79-.44-1.06L14.17 1 7.59 7.59C7.22 7.95 7 8.45 7 9v10c0 1.1.9 2 2 2h9c.83 0 1.54-.5 1.84-1.22l3.02-7.05c.09-.23.14-.47.14-.73v-1.91l-.01-.01L23 10z" />
                            </svg>
                            {{ comment.likes }}
                        </button>
                    </div>
                    <div class="comment-content">{{ comment.content }}</div>
                    <div class="comment-actions">
                        <button @click="showReplyInput(comment.commentId)" class="comment-actions-re">回复</button>
                        <button v-if="comment.replies && comment.replies.length > 0"
                            @click="toggleReplies(comment.commentId)" class="show-replies-btn">
                            {{ expandedReplies.has(comment.commentId) ? '收起回复︿' : `展开${comment.replies.length}条回复﹀` }}
                        </button>
                        <span @click="showReportDialog(comment.commentId)" class="report-btn">
                            <svg xmlns="http://www.w3.org/2000/svg" xml:space="preserve" viewBox="0 0 1024 1024"
                                width="16" height="16">
                                <path fill="currentColor"
                                    d="M928.99 755.83 574.6 203.25c-12.89-20.16-36.76-32.58-62.6-32.58s-49.71 12.43-62.6 32.58L95.01 755.83c-12.91 20.12-12.9 44.91.01 65.03 12.92 20.12 36.78 32.51 62.59 32.49h708.78c25.82.01 49.68-12.37 62.59-32.49 12.91-20.12 12.92-44.91.01-65.03M554.67 768h-85.33v-85.33h85.33zm0-426.67v298.66h-85.33V341.32z">
                                </path>
                            </svg>
                            举报
                        </span>
                    </div>
                    <div class="reply-input" v-if="activeReplyCommentId === comment.commentId">
                        <input type="text" placeholder="写下你的回复..." v-model="replyContent"
                            @keyup.enter="submitReply(comment.commentId)" />
                        <button @click="submitReply(comment.commentId)">发送</button>
                        <button @click="cancelReply">取消</button>
                    </div>
                    <div class="comment-replies"
                        v-if="comment.replies && comment.replies.length > 0 && expandedReplies.has(comment.commentId)">
                        <div v-for="reply in comment.replies" :key="reply.commentId" class="reply-item">
                            <div class="reply-header">
                                <img :src="getReaderAvatar(reply.readerId)" alt="用户头像" class="reply-avatar"
                                    @error="handleAvatarError" @click="goReaderHome(reply.readerId)" />
                                <div class="reply-info">
                                    <span class="reply-author" @click="goReaderHome(reply.readerId)">{{
                                        getReaderName(reply.readerId) }}</span>
                                    <span class="reply-time">{{ formatTime(reply.createTime) }}</span>
                                </div>
                                <button class="like-btn" @click="toggleLike(reply)"
                                    :class="{ liked: isCommentLiked(reply.commentId) }">
                                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="16" height="16">
                                        <path fill="currentColor"
                                            d="M1 21h4V9H1v12zm22-11c0-1.1-.9-2-2-2h-6.31l.95-4.57.03-.32c0-.41-.17-.79-.44-1.06L14.17 1 7.59 7.59C7.22 7.95 7 8.45 7 9v10c0 1.1.9 2 2 2h9c.83 0 1.54-.5 1.84-1.22l3.02-7.05c.09-.23.14-.47.14-.73v-1.91l-.01-.01L23 10z" />
                                    </svg>
                                    {{ reply.likes }}
                                </button>
                            </div>
                            <div class="reply-content">{{ reply.content }} <span
                                    @click="showReportDialog(reply.commentId)" class="report-btn">
                                    <svg xmlns="http://www.w3.org/2000/svg" xml:space="preserve" viewBox="0 0 1024 1024"
                                        width="16" height="16">
                                        <path fill="currentColor"
                                            d="M928.99 755.83 574.6 203.25c-12.89-20.16-36.76-32.58-62.6-32.58s-49.71 12.43-62.6 32.58L95.01 755.83c-12.91 20.12-12.9 44.91.01 65.03 12.92 20.12 36.78 32.51 62.59 32.49h708.78c25.82.01 49.68-12.37 62.59-32.49 12.91-20.12 12.92-44.91.01-65.03M554.67 768h-85.33v-85.33h85.33zm0-426.67v298.66h-85.33V341.32z">
                                        </path>
                                    </svg>
                                    举报
                                </span></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { SelectNovel_State, readerState, current_state } from '@/stores/index';
import { useRouter, useRoute } from 'vue-router';
import { ref, onMounted } from 'vue';
import { getReader } from '@/API/Reader_API';
import { getTopLevelCommentsByChapter, createComment, getComment } from '@/API/Comment_API';
import { getRepliesByParentId, addCommentReply } from '@/API/CommentReply_API';
import { likeComment, unlikeComment, isLiked as checkIsLiked } from '@/API/Likes_API';
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";

const selectNovelState = SelectNovel_State();
const reader_state = readerState();
const router = useRouter();
const route = useRoute();
const state = current_state();

const novelId = route.params.novelId;
const chapterId = route.params.chapterId;

const comments = ref([]);
const newComment = ref('');
const replyContent = ref('');
const activeReplyCommentId = ref(null);
const readersCache = ref({});
const likedComments = ref(new Set());
const expandedReplies = ref(new Set());
const loading = ref(true);
const totalComments = ref(0);

const fetchComments = async () => {
    try {
        loading.value = true;
        const response = await getTopLevelCommentsByChapter(novelId, chapterId);
        const processedComments = await Promise.all(response.map(async comment => {
            const replies = await getRepliesByParentId(comment.commentId);
            const fullReplies = await Promise.all(replies.map(async reply => {
                const replyContent = await getComment(reply.commentId);
                return {
                    ...replyContent,
                    commentLevel: reply.commentLevel
                };
            }));
            await getReaderInfo(comment.readerId);
            await Promise.all(fullReplies.map(reply => getReaderInfo(reply.readerId)));
            if (state.isloggedin) {
                const isLikedRes = await checkIsLiked(comment.commentId, reader_state.readerId);
                if (isLikedRes.liked) {
                    likedComments.value.add(comment.commentId);
                }
                await Promise.all(fullReplies.map(async reply => {
                    const isReplyLiked = await checkIsLiked(reply.commentId, reader_state.readerId);
                    if (isReplyLiked.liked) {
                        likedComments.value.add(reply.commentId);
                    }
                }));
            }
            return {
                ...comment,
                replies: fullReplies,
                commentLevel: 1
            };
        }));
        comments.value = processedComments;
        totalComments.value = processedComments.length + processedComments.reduce((sum, c) => sum + (c.replies?.length || 0), 0);
    } catch (error) {
        console.error('获取评论失败:', error);
        toast.error("获取评论失败");
    } finally {
        loading.value = false;
    }
};

const getReaderInfo = async (readerId) => {
    if (!readersCache.value[readerId]) {
        try {
            const reader = await getReader(readerId);
            readersCache.value[readerId] = reader;
        } catch (error) {
            console.error('获取读者信息失败:', error);
            readersCache.value[readerId] = {
                readerName: '未知用户',
                avatarUrl: null
            };
        }
    }
    return readersCache.value[readerId];
};

const getReaderAvatar = (readerId) => {
    const defaultAvatar = 'e165315c-da2b-42c9-b3cf-c0457d168634.jpg';
    return `https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/${readersCache.value[readerId]?.avatarUrl || defaultAvatar}`;
};

const getReaderName = (readerId) => {
    return readersCache.value[readerId]?.readerName || '未知用户';
};

const formatTime = (timeStr) => {
    if (!timeStr) return '';
    const date = new Date(timeStr);
    return date.toLocaleString();
};

const handleAvatarError = (e) => {
    e.target.src = require('@/assets/logo.png');
};

const isCommentLiked = (commentId) => {
    return likedComments.value.has(commentId);
};

const toggleLike = async (comment) => {
    if (!state.isloggedin) {
        toast.warning("请先登录");
        return;
    }
    try {
        if (isCommentLiked(comment.commentId)) {
            await unlikeComment(comment.commentId, reader_state.readerId);
            likedComments.value.delete(comment.commentId);
            comment.likes--;
            toast.success("成功取消点赞！");
        } else {
            await likeComment(comment.commentId, reader_state.readerId);
            likedComments.value.add(comment.commentId);
            comment.likes++;
            toast.success("点赞成功");
        }
    } catch (error) {
        console.error('操作失败:', error);
        toast.error("操作失败");
    }
};

const submitComment = async () => {
    if (!newComment.value.trim()) return;
    try {
        const response = await createComment({
            readerId: reader_state.readerId,
            novelId: novelId,
            chapterId: chapterId,
            title: '评论',
            content: newComment.value,
            likes: 0,
            status: "通过",
            createTime: new Date().toISOString()
        });
        await getReaderInfo(response.readerId);
        comments.value.unshift({
            ...response,
            replies: [],
            commentLevel: 1
        });
        newComment.value = '';
        totalComments.value++;
        toast.success("评论成功");
    } catch (error) {
        console.error('评论失败:', error);
        toast.error("评论失败");
    }
};

const showReplyInput = (commentId) => {
    if (!state.isloggedin) {
        toast.warning("请先登录");
        return;
    }
    const isActive = activeReplyCommentId.value === commentId;
    activeReplyCommentId.value = isActive ? null : commentId;
    replyContent.value = '';
};

const cancelReply = () => {
    activeReplyCommentId.value = null;
    replyContent.value = '';
};

const submitReply = async (parentCommentId) => {
    if (!replyContent.value.trim()) return;
    try {
        const commentResponse = await createComment({
            readerId: reader_state.readerId,
            novelId: novelId,
            chapterId: chapterId,
            title: '回复',
            content: replyContent.value,
            likes: 0,
            status: "通过",
            createTime: new Date().toISOString()
        });
        await getReaderInfo(commentResponse.readerId);
        await addCommentReply({
            commentId: commentResponse.commentId,
            preComId: parentCommentId,
            commentLevel: 2
        });
        const fullReply = await getComment(commentResponse.commentId);
        const parentComment = comments.value.find(c => c.commentId === parentCommentId);
        if (parentComment) {
            if (!parentComment.replies) {
                parentComment.replies = [];
            }
            parentComment.replies.push({
                ...fullReply,
                commentLevel: 2
            });
            if (!expandedReplies.value.has(parentCommentId)) {
                expandedReplies.value.add(parentCommentId);
            }
        }
        replyContent.value = '';
        activeReplyCommentId.value = null;
        totalComments.value++;
        toast.success("回复成功");
    } catch (error) {
        console.error('回复失败:', error);
        toast.error("回复失败");
    }
};

const toggleReplies = (commentId) => {
    if (expandedReplies.value.has(commentId)) {
        expandedReplies.value.delete(commentId);
    } else {
        expandedReplies.value.add(commentId);
    }
};

const goReaderHome = (readerId) => {
    router.push(`/reader/${readerId}`);
};

const goBack = () => {
    router.go(-1);
};
async function handle_NovelInfro() {
    router.push('/Novels/Novel_Info/home');
}
function goAuthorHome() {
    router.push(`/author/${selectNovelState.authorId}`);
}
function showReportDialog(commentId) {
    router.push(`/comment-report/${reader_state.readerId}/${commentId}`);
}
onMounted(async () => {
    await fetchComments();
});
</script>

<style scoped>
.chapter-comments-container {
    max-width: 800px;
    margin: 0 auto;
    padding: 20px;
    background-color: #fff;
    min-height: 100vh;
    position: relative;
}

.header {
    display: flex;
    align-items: center;
    margin-bottom: 40px;
    border-bottom: 1px solid #eee;
    background: #f2f0f0;
}

.back-button {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 10px 18px;
    background-color: #ffd100;
    color: #222;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    margin-right: 40px;
    font-weight: 500;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    transition: all 0.2s ease;
}

.back-button:hover {
    background-color: #ffdd33;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
    transform: translateY(-1px);
}

.back-button:active {
    background-color: #ffc800;
    box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
    transform: translateY(0);
}

.back-button svg {
    width: 18px;
    height: 18px;
}

.cover-image {
    width: 100%;
    height: 100%;
    object-fit: cover;
    cursor: pointer;
}

.header h2 {
    margin: 0;
    font-size: 18px;
    color: #333;
    gap: 10px;
}

.comments-container {
    width: 100%;
    margin-top: 20px;
    margin-left: 30px;
}

.comments-header {
    margin-bottom: 20px;
}

.comments-header h3 {
    margin: 0;
    font-size: 18px;
    color: #333;
}

.comment-input {
    display: flex;
    align-items: center;
    gap: 10px;
    margin-bottom: 20px;
}

.comment-input input {
    flex: 1;
    padding: 10px 15px;
    border: 1px solid #ddd;
    border-radius: 20px;
    outline: none;
}

.comment-input button {
    padding: 10px 20px;
    background-color: #ffd100;
    color: #222;
    border: none;
    border-radius: 20px;
    cursor: pointer;
    font-weight: bold;
}

.comment-input button:disabled {
    background-color: #eee;
    color: #999;
    cursor: not-allowed;
}

.user-avatar {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    object-fit: cover;
}



.comments-list {
    display: flex;
    flex-direction: column;
    gap: 20px;
}

.loading,
.no-comments {
    text-align: center;
    padding: 20px;
    color: #666;
}

.comment-item {
    padding-bottom: 15px;
    border-bottom: 1px solid #f0f0f0;
}

.comment-header {
    display: flex;
    align-items: center;
    margin-bottom: 10px;
    position: relative;
}

.comment-avatar {
    width: 36px;
    height: 36px;
    border-radius: 50%;
    object-fit: cover;
    margin-right: 10px;
    cursor: pointer;
    transition: transform 0.2s;
}

.comment-avatar:hover {
    transform: scale(1.05);
}

.comment-info {
    flex: 1;
}

.comment-author {
    font-weight: bold;
    font-size: 14px;
    color: #333;
    cursor: pointer;
}

.comment-author:hover {
    color: #f0940a;
    text-decoration: underline;
}

.comment-time {
    font-size: 12px;
    color: #999;
    margin-left: 10px;
}

.like-btn {
    background: none;
    border: none;
    display: flex;
    align-items: center;
    gap: 5px;
    color: #666;
    cursor: pointer;
    font-size: 12px;
    padding: 4px 8px;
    border-radius: 4px;
}

.like-btn:hover {
    background-color: #f5f5f5;
}

.like-btn.liked {
    color: #f56c6c;
}

.comment-content {
    font-size: 14px;
    line-height: 1.5;
    color: #333;
    margin-left: 46px;
    white-space: pre-line;
}

.comment-actions {
    margin-left: 46px;
    margin-top: 10px;
    display: flex;
    gap: 15px;
    width: 100%;
}

.comment-actions button {
    background: none;
    border: none;
    color: #1890ff;
    font-size: 12px;
    cursor: pointer;
    padding: 0;
}

.comment-actions button:hover {
    text-decoration: underline;
}

.reply-input {
    margin-top: 10px;
    margin-left: 46px;
    display: flex;
    gap: 10px;
    align-items: center;
}

.reply-input input {
    flex: 1;
    padding: 8px 12px;
    border: 1px solid #ddd;
    border-radius: 20px;
    outline: none;
    font-size: 13px;
}

.reply-input button {
    padding: 6px 12px;
    font-size: 12px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

.reply-input button:first-of-type {
    background-color: #ffd100;
    color: #222;
}

.reply-input button:last-of-type {
    background-color: #f5f5f5;
    color: #666;
}

.comment-replies {
    margin-top: 15px;
    margin-left: 30px;
    border-left: 2px solid #eee;
    padding-left: 15px;
}

.reply-item {
    margin-top: 15px;
}

.reply-header {
    display: flex;
    align-items: center;
    margin-bottom: 5px;
}

.reply-avatar {
    width: 28px;
    height: 28px;
    border-radius: 50%;
    object-fit: cover;
    margin-right: 8px;
    cursor: pointer;
    transition: transform 0.2s;
}

.reply-avatar:hover {
    transform: scale(1.05);
}

.reply-info {
    flex: 1;
}

.reply-author {
    font-weight: bold;
    font-size: 13px;
    color: #333;
    cursor: pointer;
}

.reply-author:hover {
    color: #f0940a;
    text-decoration: underline;
}

.reply-time {
    font-size: 11px;
    color: #999;
    margin-left: 8px;
}

.reply-content {
    font-size: 13px;
    line-height: 1.4;
    color: #333;
    margin-left: 36px;
    white-space: pre-line;
}

.chapter-comments-container {
    display: flex;
    max-width: 1200px;
    margin: 0 auto;
    padding: 20px;
    background-color: #fff;
    min-height: 100vh;
    gap: 20px;
}

.novel-info-sidebar {
    position: sticky;
    margin-top: 30px;
    width: 250px;
    flex-shrink: 0;
    top: 30px;
    align-self: flex-start;
    height: fit-content;
}

.novel-info-card {
    background-color: #f9f9f9;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.novel-info-card h3 {
    margin: 0 0 10px 0;
    font-size: 18px;
    color: #333;
}

.novel-info-card h3:hover {
    color: #f0940a;
}

.author-label {
    color: #666;
    font-size: 14px;
    margin-right: 5px;
    margin-bottom: 40px;
}

.novel-info-card .author {
    color: #333;
    font-size: 16px;
    margin-bottom: 15px;
    font-weight: bold;
}

.novel-info-card .author:hover {
    color: #f0940a;
    transform: scale(1.10);
    cursor: pointer;
}

.novel-info-card .stats {
    display: flex;
    flex-direction: column;
    gap: 8px;
    color: #666;
    font-size: 13px;
    margin-bottom: 20px;
    margin-top: 10px;
}

.num {
    font-weight: 700;
    font-size: 16px;
    color: #e08414;
    margin: 0 3px;
}

.novel-info-card button {
    width: 100%;
    padding: 8px 0;
    margin-bottom: 10px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

.main-content {
    flex: 1;
    max-width: 800px;
}

.report-btn {
    color: #eb1212;
    font-size: 12px;
    cursor: pointer;
    padding: 0;
    display: flex;
    align-items: center;
    position: absolute;
    right: 0;
    left: auto;
    transform: translateX(-28px);
}

.report-btn:hover {
    text-decoration: underline;
}

@media (max-width: 768px) {
    .chapter-comments-container {
        flex-direction: column;
        padding: 15px;
    }

    .novel-info-sidebar {
        position: static;
        width: 100%;
        margin-bottom: 20px;
    }

    .chapter-comments-container {
        padding: 15px;
    }

    .header {
        flex-direction: column;
        align-items: flex-start;
        gap: 10px;
    }

    .back-button {
        margin-right: 0;
    }

    .comment-input {
        flex-direction: column;
        align-items: stretch;
    }

    .comment-input button {
        width: 100%;
        margin-top: 10px;
    }

    .comment-content,
    .reply-content {
        margin-left: 0;
    }

    .comment-actions,
    .reply-input {
        margin-left: 0;
    }
}
</style>