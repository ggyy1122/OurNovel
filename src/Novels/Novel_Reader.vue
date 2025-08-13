<template>
    <div class="reader-bg" :style="readerBgStyle">
        <div class="guide-modal" v-if="showGuide">
            <div class="guide-content">
                <h3>快捷键指南</h3>
                <div class="shortcut-item">
                    <span class="key">F11</span>
                    <span>全屏模式</span>
                </div>
                <div class="shortcut-item">
                    <span class="key">↑ ↓</span>
                    <span>上下移动</span>
                </div>
                <div class="shortcut-item">
                    <span class="key">← →</span>
                    <span>换章</span>
                </div>
                <button class="confirm-btn" @click="showGuide = false">知道了</button>
            </div>
        </div>
        <div class="settings-modal" v-if="showSettings">
            <div class="settings-content">
                <h3>阅读设置</h3>
                <div class="setting-group">
                    <h4>阅读背景：</h4>
                    <div class="bg-options">
                        <button v-for="option in bgOptions" :key="option.value"
                            :style="{ backgroundColor: option.value }" @click="updateBgColor(option.value)"
                            :class="{ active: bgColor === option.value }">
                            {{ option.name }}
                        </button>
                    </div>
                </div>
                <div class="setting-group">
                    <h4>字体大小：</h4>
                    <div class="font-size-control">
                        <button @click="updateFontSize(-1)">A-</button>
                        <span>{{ fontSize }}</span>
                        <button @click="updateFontSize(1)">A+</button>
                    </div>
                </div>
                <div class="setting-group">
                    <h4>正文字体：</h4>
                    <div class="font-family-options">
                        <button v-for="option in fontOptions" :key="option.value"
                            @click="updateFontFamily(option.value)" :class="{ active: fontFamily === option.value }">
                            {{ option.name }}
                        </button>
                    </div>
                </div>
                <button class="close-btn" @click="showSettings = false">关闭</button>
            </div>
        </div>
        <div class="reader-main">
            <div class="left-menu" :style="{
                left: showComments ? '-25px' : '156px'
            }">
                <button class="back-btn" @click="handleBack">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="M896 192H128v128h768zm0 256H384v128h512zm0 256H128v128h768zM320 384 128 512l192 128z">
                        </path>
                    </svg>
                    返回
                </button>
                <button class="menu-item guide" @click="handleGuideClick">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="M512 896a384 384 0 1 0 0-768 384 384 0 0 0 0 768m0 64a448 448 0 1 1 0-896 448 448 0 0 1 0 896">
                        </path>
                        <path fill="currentColor"
                            d="M725.888 315.008C676.48 428.672 624 513.28 568.576 568.64c-55.424 55.424-139.968 107.904-253.568 157.312a12.8 12.8 0 0 1-16.896-16.832c49.536-113.728 102.016-198.272 157.312-253.632 55.36-55.296 139.904-107.776 253.632-157.312a12.8 12.8 0 0 1 16.832 16.832">
                        </path>
                    </svg>
                    指南
                </button>
            </div>
            <div class="reader-card"
                :style="{ backgroundColor: cardBgColor, marginRight: showComments ? '350px' : '110px', marginLeft: showComments ? '-20px' : '100px' }">
                <header class="header" ref="mainHeader" :style="{ backgroundColor: cardBgColor }">
                    <div class="logo">
                        <img src="@/assets/logo.png" alt="TJ小说网" />
                        <h1>TJ小说网</h1>
                    </div>
                    <div class="search-bar">
                        <input type="text" placeholder="输入小说名/作者名/读者名" v-model="searchQuery"
                            @keyup.enter="handleSearch" />
                        <button @click="handleSearch">搜索</button>
                    </div>
                    <div class="user-actions">
                        <!-- 未登录状态 -->
                        <button v-if="!state.isloggedin" class="login-btn" @click="goToLogin">
                            <span class="login-icon">
                                <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
                                    <circle cx="12" cy="8" r="4" stroke="#222" stroke-width="2" />
                                    <ellipse cx="12" cy="17" rx="7" ry="4" stroke="#222" stroke-width="2" />
                                </svg>
                            </span>
                            立即登录
                        </button>
                        <!-- 已登录状态 -->
                        <div v-else class="user-dropdown" @mouseenter="showDropdown = true"
                            @mouseleave="showDropdown = false">
                            <img :src="reader_state.formattedAvatarUrl" alt="用户头像" class="user-avatar" />
                            <div v-if="showDropdown" class="dropdown-menu">
                                <div class="user-info">
                                    <p>用户名：{{ reader_state.readerName }}</p>
                                    <p>Lv1</p>
                                </div>
                                <div class="dropdown-divider"></div>
                                <a href="#" @click.prevent="openMyHomePage" class="dropdown-item">
                                    <i class="fa fa-home mr-2"></i> 我的主页
                                </a>
                                <a href="#" @click.prevent="goToRecharge" class="dropdown-item">
                                    <i class="fa fa-yen mr-2"></i> 去充值
                                </a>
                                <a href="#" @click.prevent="logout" class="dropdown-item">
                                    <i class="fa fa-sign-out mr-2"></i> 退出
                                </a>
                            </div>
                        </div>
                    </div>
                </header>
                <div class="chapter-header" :style="{ color: textColor }">
                    <h2 class="chapter-title">第{{ selectNovelState.chapterId }}章 {{ selectNovelState.cha_title }}</h2>
                    <div class="meta-row">
                        <span>书名：{{ selectNovelState.novelName }}</span>
                        <span>作者：{{ selectNovelState.authorName }}</span>
                        <span>本章字数：{{ selectNovelState.cha_wordCount }}</span>
                        <span>更新时间：{{ selectNovelState.cha_publishTime }}</span>
                    </div>
                    <div class="chapter-divider"></div>
                </div>
                <div class="novel-content"
                    :style="{ fontSize: fontSize + 'px', fontFamily: fontFamily, color: textColor }"
                    v-html="formatContent(selectNovelState.cha_content)">
                </div>
                <div class="chapter-nav-buttons">
                    <button class="nav-button prev" @click="changeChapter(-1)">上一章</button>
                    <button class="nav-button next" @click="changeChapter(1)">下一章</button>
                </div>
            </div>
            <!-- 可滑动评论区域 -->
            <div class="comments-section" v-if="showComments" :style="{
                backgroundColor: cardBgColor, right: '0px'
            }">
                <div class="comments-header">
                    <h3>热门评论 ({{ comments.length }})</h3>
                    <button class="close-comments" @click="toggleComments">
                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="20" height="20">
                            <path fill="currentColor"
                                d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z" />
                        </svg>
                    </button>
                </div>
                <div class="comment-input">
                    <img :src="reader_state.formattedAvatarUrl" alt="用户头像" class="user-avatar"
                        v-if="state.isloggedin" />
                    <input type="text" placeholder="写下你的想法..." v-model="newComment" @keyup.enter="submitComment"
                        :disabled="!state.isloggedin" />
                    <button @click="submitComment" :disabled="!state.isloggedin || !newComment.trim()">发送</button>
                </div>
                <div v-if="comments.length === 0" class="no-comments">暂无评论</div>
                <div class="comments-list" v-if="comments.length > 0">
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
                                :class="{ liked: isLikeds(comment.commentId) }">
                                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="16" height="16">
                                    <path fill="currentColor"
                                        d="M1 21h4V9H1v12zm22-11c0-1.1-.9-2-2-2h-6.31l.95-4.57.03-.32c0-.41-.17-.79-.44-1.06L14.17 1 7.59 7.59C7.22 7.95 7 8.45 7 9v10c0 1.1.9 2 2 2h9c.83 0 1.54-.5 1.84-1.22l3.02-7.05c.09-.23.14-.47.14-.73v-1.91l-.01-.01L23 10z" />
                                </svg>
                                {{ comment.likes }}
                            </button>
                        </div>
                        <div class="comment-content">{{ comment.content }}</div>
                        <!-- 回复操作和回复数量 -->
                        <div class="comment-actions">
                            <button @click="showReplyInput(comment.commentId)" class="comment-actions-re">回复</button>
                            <button v-if="comment.replies && comment.replies.length > 0"
                                @click="toggleReplies(comment.commentId)" class="show-replies-btn">
                                {{ expandedReplies.has(comment.commentId) ? '收起回复︿' : `展开${comment.replies.length}条回复﹀`
                                }}
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
                        <!-- 回复输入框 -->
                        <div class="reply-input" v-if="activeReplyCommentId === comment.commentId">
                            <input type="text" placeholder="写下你的回复..." v-model="replyContent"
                                @keyup.enter="submitReply(comment.commentId)" />
                            <button @click="submitReply(comment.commentId)">发送</button>
                            <button @click="cancelReply">取消</button>
                        </div>
                        <!-- 回复列表（默认隐藏，点击后展开） -->
                        <div class="comment-replies"
                            v-if="comment.replies && comment.replies.length > 0 && expandedReplies.has(comment.commentId)">
                            <div v-for="reply in comment.replies" :key="reply.commentId" class="reply-item">
                                <div class="reply-header">
                                    <img :src="getReaderAvatar(reply.readerId)" alt="用户头像" class="reply-avatar"
                                        @error="handleAvatarError" @click="goReaderHome(comment.readerId)" />
                                    <div class="reply-info">
                                        <span class="reply-author" @click="goReaderHome(comment.readerId)">{{
                                            getReaderName(reply.readerId) }}</span>
                                        <span class="reply-time">{{ formatTime(reply.createTime) }}</span>
                                    </div>
                                    <button class="like-btn" @click="toggleLike(reply)"
                                        :class="{ liked: isLikeds(reply.commentId) }">
                                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="16"
                                            height="16">
                                            <path fill="currentColor"
                                                d="M1 21h4V9H1v12zm22-11c0-1.1-.9-2-2-2h-6.31l.95-4.57.03-.32c0-.41-.17-.79-.44-1.06L14.17 1 7.59 7.59C7.22 7.95 7 8.45 7 9v10c0 1.1.9 2 2 2h9c.83 0 1.54-.5 1.84-1.22l3.02-7.05c.09-.23.14-.47.14-.73v-1.91l-.01-.01L23 10z" />
                                        </svg>
                                        {{ reply.likes }}
                                    </button>
                                </div>
                                <div class="reply-content">{{ reply.content }} <span
                                        @click="showReportDialog(reply.commentId)" class="report-btn">
                                        <svg xmlns="http://www.w3.org/2000/svg" xml:space="preserve"
                                            viewBox="0 0 1024 1024" width="16" height="16">
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
                <div class="view-more-comments" v-if="comments.length > 0">
                    <button @click="viewAllComments">查看更多评论></button>
                </div>
            </div>
            <div class="right-menu" v-if="!showComments" :style="{
                right: '166px'
            }">
                <div class="menu-item" @click="showCatalog = !showCatalog"><svg xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="M640 384v256H384V384zm64 0h192v256H704zm-64 512H384V704h256zm64 0V704h192v192zm-64-768v192H384V128zm64 0h192v192H704zM320 384v256H128V384zm0 512H128V704h192zm0-768v192H128V128z">
                        </path>
                    </svg> 目录</div>
                <div class="menu-item" @click="handletonovel"><svg xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="M832 384H576V128H192v768h640zm-26.496-64L640 154.496V320zM160 64h480l256 256v608a32 32 0 0 1-32 32H160a32 32 0 0 1-32-32V96a32 32 0 0 1 32-32m160 448h384v64H320zm0-192h160v64H320zm0 384h384v64H320z">
                        </path>
                    </svg> 书详情</div>
                <div class="menu-item" @click="toggleComments"><svg xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="m174.72 855.68 135.296-45.12 23.68 11.84C388.096 849.536 448.576 864 512 864c211.84 0 384-166.784 384-352S723.84 160 512 160 128 326.784 128 512c0 69.12 24.96 139.264 70.848 199.232l22.08 28.8-46.272 115.584zm-45.248 82.56A32 32 0 0 1 89.6 896l58.368-145.92C94.72 680.32 64 596.864 64 512 64 299.904 256 96 512 96s448 203.904 448 416-192 416-448 416a461.056 461.056 0 0 1-206.912-48.384l-175.616 58.56z">
                        </path>
                        <path fill="currentColor"
                            d="M352 576h320q32 0 32 32t-32 32H352q-32 0-32-32t32-32m32-192h256q32 0 32 32t-32 32H384q-32 0-32-32t32-32">
                        </path>
                    </svg> 评论</div>
                <div class="menu-item" @click="showRewardDialog = true"><svg xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="m161.92 580.736 29.888 58.88C171.328 659.776 160 681.728 160 704c0 82.304 155.328 160 352 160s352-77.696 352-160c0-22.272-11.392-44.16-31.808-64.32l30.464-58.432C903.936 615.808 928 657.664 928 704c0 129.728-188.544 224-416 224S96 833.728 96 704c0-46.592 24.32-88.576 65.92-123.264z">
                        </path>
                        <path fill="currentColor"
                            d="m161.92 388.736 29.888 58.88C171.328 467.84 160 489.792 160 512c0 82.304 155.328 160 352 160s352-77.696 352-160c0-22.272-11.392-44.16-31.808-64.32l30.464-58.432C903.936 423.808 928 465.664 928 512c0 129.728-188.544 224-416 224S96 641.728 96 512c0-46.592 24.32-88.576 65.92-123.264z">
                        </path>
                        <path fill="currentColor"
                            d="M512 544c-227.456 0-416-94.272-416-224S284.544 96 512 96s416 94.272 416 224-188.544 224-416 224m0-64c196.672 0 352-77.696 352-160S708.672 160 512 160s-352 77.696-352 160 155.328 160 352 160">
                        </path>
                    </svg> 打赏</div>
                <div class="menu-item" @click="toggleTextToSpeech"><svg xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="M896 529.152V512a384 384 0 1 0-768 0v17.152A128 128 0 0 1 320 640v128a128 128 0 1 1-256 0V512a448 448 0 1 1 896 0v256a128 128 0 1 1-256 0V640a128 128 0 0 1 192-110.848M896 640a64 64 0 0 0-128 0v128a64 64 0 0 0 128 0zm-768 0v128a64 64 0 0 0 128 0V640a64 64 0 1 0-128 0">
                        </path>
                    </svg> {{ isSpeaking ? '停止听书' : '听书' }}</div>
                <div class="menu-item" @click="showSettings = !showSettings"><svg xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="M600.704 64a32 32 0 0 1 30.464 22.208l35.2 109.376c14.784 7.232 28.928 15.36 42.432 24.512l112.384-24.192a32 32 0 0 1 34.432 15.36L944.32 364.8a32 32 0 0 1-4.032 37.504l-77.12 85.12a357.12 357.12 0 0 1 0 49.024l77.12 85.248a32 32 0 0 1 4.032 37.504l-88.704 153.6a32 32 0 0 1-34.432 15.296L708.8 803.904c-13.44 9.088-27.648 17.28-42.368 24.512l-35.264 109.376A32 32 0 0 1 600.704 960H423.296a32 32 0 0 1-30.464-22.208L357.696 828.48a351.616 351.616 0 0 1-42.56-24.64l-112.32 24.256a32 32 0 0 1-34.432-15.36L79.68 659.2a32 32 0 0 1 4.032-37.504l77.12-85.248a357.12 357.12 0 0 1 0-48.896l-77.12-85.248A32 32 0 0 1 79.68 364.8l88.704-153.6a32 32 0 0 1 34.432-15.296l112.32 24.256c13.568-9.152 27.776-17.408 42.56-24.64l35.2-109.312A32 32 0 0 1 423.232 64H600.64zm-23.424 64H446.72l-36.352 113.088-24.512 11.968a294.113 294.113 0 0 0-34.816 20.096l-22.656 15.36-116.224-25.088-65.28 113.152 79.68 88.192-1.92 27.136a293.12 293.12 0 0 0 0 40.192l1.92 27.136-79.808 88.192 65.344 113.152 116.224-25.024 22.656 15.296a294.113 294.113 0 0 0 34.816 20.096l24.512 11.968L446.72 896h130.688l36.48-113.152 24.448-11.904a288.282 288.282 0 0 0 34.752-20.096l22.592-15.296 116.288 25.024 65.28-113.152-79.744-88.192 1.92-27.136a293.12 293.12 0 0 0 0-40.256l-1.92-27.136 79.808-88.128-65.344-113.152-116.288 24.96-22.592-15.232a287.616 287.616 0 0 0-34.752-20.096l-24.448-11.904L577.344 128zM512 320a192 192 0 1 1 0 384 192 192 0 0 1 0-384m0 64a128 128 0 1 0 0 256 128 128 0 0 0 0-256">
                        </path>
                    </svg> 阅读设置</div>
                <div class="menu-item" @click="handleAddShelf" :class="{ 'in-shelf': isFavorite }"
                    :disabled="isFavorite"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="24"
                        height="24">
                        <path fill="currentColor"
                            d="M192 128v768h640V128zm-32-64h704a32 32 0 0 1 32 32v832a32 32 0 0 1-32 32H160a32 32 0 0 1-32-32V96a32 32 0 0 1 32-32">
                        </path>
                        <path fill="currentColor"
                            d="M672 128h64v768h-64zM96 192h128q32 0 32 32t-32 32H96q-32 0-32-32t32-32m0 192h128q32 0 32 32t-32 32H96q-32 0-32-32t32-32m0 192h128q32 0 32 32t-32 32H96q-32 0-32-32t32-32m0 192h128q32 0 32 32t-32 32H96q-32 0-32-32t32-32">
                        </path>
                    </svg> {{ isFavorite ? '已在书架' : '加入书架' }}</div>
                <div class="menu-item to-top" @click="scrollToTop"><svg xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 1024 1024" width="24" height="24">
                        <path fill="currentColor"
                            d="M160 832h704a32 32 0 1 1 0 64H160a32 32 0 1 1 0-64m384-578.304V704h-64V247.296L237.248 490.048 192 444.8 508.8 128l316.8 316.8-45.312 45.248z">
                        </path>
                    </svg> 回到顶部</div>
            </div>
        </div>
        <div class="catalog-modal" v-if="showCatalog">
            <div class="catalog-content" :style="{
                backgroundColor: cardBgColor,
                color: textColor,
                border: `1px solid ${textColor}20`
            }">
                <div class="catalog-header">
                    <div class="header-top">
                        <h3>{{ selectNovelState.novelName }}</h3>
                        <button class="ca_close-btn" @click="showCatalog = false">
                            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="22" height="22">
                                <path fill="currentColor"
                                    d="M764.288 214.592 512 466.88 259.712 214.592a31.936 31.936 0 0 0-45.12 45.12L466.752 512 214.528 764.224a31.936 31.936 0 1 0 45.12 45.184L512 557.184l252.288 252.288a31.936 31.936 0 0 0 45.12-45.12L557.12 512.064l252.288-252.352a31.936 31.936 0 1 0-45.12-45.184z">
                                </path>
                            </svg>
                        </button>
                    </div>
                    <div class="header-meta">
                        <span class="status-badge">{{ catalogStatus }}</span>
                        <span class="chapter-count">共{{ chapters.length }}章</span>
                        <button @click="toggleSortOrder" class="sort-btn">
                            {{ sortAscending ? '正序 ↓' : '倒序 ↑' }}
                        </button>
                    </div>
                </div>
                <div class="catalog-grid">
                    <div v-for="chapter in sortedChapters" :key="chapter.chapterId" class="chapter-card" :class="{
                        'current': chapter.chapterId === selectNovelState.chapterId,
                        'locked': chapter.isCharged === '是' && !chapter.hasPurchased,
                        'vip': chapter.isCharged === '是'
                    }" @click="goToChapter(chapter)">
                        <div class="card-content">
                            <span class="chapter-number">第{{ chapter.chapterId }}章</span>
                            <span class="ca_chapter-title">{{ chapter.title }}</span>
                            <span class="chapter-status">{{ chapter.status }}</span>
                            <span v-if="chapter.isCharged === '是'" class="vip-tag">VIP</span>
                            <span v-if="chapter.chapterId === selectNovelState.chapterId"
                                class="current-badge">正在阅读</span>
                            <div v-if="chapter.isCharged === '是' && !chapter.hasPurchased" class="lock-overlay">
                                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20" height="20">
                                    <path fill="currentColor"
                                        d="M224 448a32 32 0 0 0-32 32v384a32 32 0 0 0 32 32h576a32 32 0 0 0 32-32V480a32 32 0 0 0-32-32zm0-64h576a96 96 0 0 1 96 96v384a96 96 0 0 1-96 96H224a96 96 0 0 1-96-96V480a96 96 0 0 1 96-96">
                                    </path>
                                    <path fill="currentColor"
                                        d="M512 544a32 32 0 0 1 32 32v192a32 32 0 1 1-64 0V576a32 32 0 0 1 32-32m192-160v-64a192 192 0 1 0-384 0v64zM512 64a256 256 0 0 1 256 256v128H256V320A256 256 0 0 1 512 64">
                                    </path>
                                </svg>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- 打赏弹窗 -->
        <div v-if="showRewardDialog" class="reward-dialog-overlay">
            <div class="reward-dialog">
                <div class="dialog-header"
                    style="display: flex; justify-content: center; align-items: center; position: relative;">
                    <h3 style="margin: 0;">打赏</h3>
                    <button class="da_close-btn " @click="showRewardDialog = false"
                        style="position: absolute; right: 20px;">&times;</button>
                </div>
                <div class="reward-options">
                    <div v-for="option in rewardOptions" :key="option.value"
                        :class="['reward-option', { 'selected': selectedReward === option.value }]"
                        @click="selectReward(option.value)">
                        <span>{{ option.label }}</span>
                        <svg v-if="selectedReward === option.value" class="check-icon" viewBox="0 0 24 24">
                            <path d="M20 6L9 17l-5-5" stroke="currentColor" stroke-width="2" fill="none" />
                        </svg>
                    </div>
                </div>
                <div class="balance-info">
                    <span>账户余额 {{ accountBalance }} 起点币</span>
                    <span>本次打赏 {{ selectedReward }} 起点币</span>
                </div>
                <button class="confirm-reward-btn" @click="confirmReward">
                    确认打赏
                </button>
            </div>
        </div>
        <div v-if="showBalanceInsufficientDialog" class="insufficient-dialog-overlay">
            <div class="insufficient-dialog">
                <div class="dialog-header">
                    <h3>打赏</h3>
                    <button class="da_close-btn" @click="showBalanceInsufficientDialog = false">&times;</button>
                </div>
                <div class="insufficient-content">
                    <p class="insufficient-message">账户余额不足</p>
                    <div class="amount-info">
                        <span>本次打赏 {{ selectedReward }} 起点币</span>
                        <span>账户余额 {{ accountBalance }} 起点币·还差 {{ selectedReward - accountBalance }} 起点币</span>
                    </div>
                    <div class="quick-payment">
                        <button class="recharge-btn" @click="goToRecharge">去充值</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="tts-control-panel" v-if="showTtsControls">
        <div class="tts-voice-select">
            <label>语音:</label>
            <select v-model="selectedVoice">
                <option v-for="voice in availableVoices" :key="voice.name" :value="voice.name">{{ voice.name }}</option>
            </select>
        </div>
        <div class="tts-rate-control">
            <label>语速:</label>
            <input type="range" min="0.5" max="2" step="0.1" v-model="speechRate">
            <span>{{ speechRate }}x</span>
        </div>
        <div class="tts-pitch-control">
            <label>音调:</label>
            <input type="range" min="0.5" max="2" step="0.1" v-model="speechPitch">
            <span>{{ speechPitch }}x</span>
        </div>
    </div>
    <!-- 添加购买弹窗 -->
    <div v-if="showPurchaseDialog" class="purchase-dialog-overlay">
        <div class="purchase-dialog">
            <div class="dialog-header"
                style="display: flex; justify-content: center; align-items: center; position: relative;">
                <h3 style="margin: 0;">购买章节</h3>
                <button class="da_close-btn" @click="showPurchaseDialog = false"
                    style="position: absolute; right: 20px;">&times;</button>
            </div>
            <div class="purchase-content">
                <p>本章节价格为 ￥{{ (selectedChapter.calculatedPrice / 100).toFixed(2) }}</p>
            </div>
            <button class="confirm-reward-btn" @click="purchase_Chapter">
                确认购买
            </button>
        </div>
    </div>
    <!-- 购买弹窗后面的余额不足弹窗 -->
    <div v-if="showPurchaseInsufficientDialog" class="purchase-insufficient-overlay">
        <div class="purchase-insufficient-dialog">
            <div class="purchase-dialog-header">
                <h3>章节购买</h3>
                <button class="purchase-close-btn" @click="showPurchaseInsufficientDialog = false">&times;</button>
            </div>
            <div class="purchase-insufficient-content">
                <p class="purchase-insufficient-message">账户余额不足，无法购买本章节</p>
                <div class="purchase-amount-info">
                    <span>章节价格：{{ (selectedChapter.calculatedPrice / 100).toFixed(2) }}元</span>
                    <span>当前余额：{{ (accountBalance / 100).toFixed(2) }}元</span>
                </div>
                <div class="purchase-action-buttons">
                    <button class="purchase-recharge-btn" @click="goToRecharge">立即充值</button>
                    <button class="purchase-cancel-btn" @click="showPurchaseInsufficientDialog = false">取消</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { SelectNovel_State, readerState, current_state } from '@/stores/index';
import { useRouter } from 'vue-router';
import { ref, computed, onMounted, onUnmounted, watch } from 'vue';
import { getChapter, getNovelChaptersWithoutContent } from '@/API/Chapter_API';
import { addOrUpdateCollect } from '@/API/Collect_API'
import { getWholePurchaseStatus } from '@/API/Transaction_API'
import { getReaderBalance, getReader } from '@/API/Reader_API';
import { rewardNovel } from '@/API/Reward_API';
import { getTopLikedCommentsByChapter, createComment, getComment } from '@/API/Comment_API';
import { getRepliesByParentId, addCommentReply } from '@/API/CommentReply_API';
import { likeComment, unlikeComment, isLiked } from '@/API/Likes_API';
import { checkPurchase, purchaseChapter } from '@/API/Purchase_API';
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
const selectNovelState = SelectNovel_State();
const reader_state = readerState();
const router = useRouter();
const state = current_state();
const searchQuery = ref('');
const showDropdown = ref(false);
const showCatalog = ref(false);
const chapters = ref([]);
const sortAscending = ref(true);
const catalogStatus = ref(selectNovelState.status === '连载' ? '连载中' : '已完结');
const wholehasPurchased = ref(false)
const showRewardDialog = ref(false);                  // 是否显示打赏弹窗
const showPurchaseDialog = ref(false);
const selectedChapter = ref(null);
const showPurchaseInsufficientDialog = ref(false); // 购买余额不足弹窗状态
// 打赏相关状态和函数
const accountBalance = ref(0);                      // 账号余额
const selectedReward = ref(100);                    // 默认选中100点打赏金额
const showBalanceInsufficientDialog = ref(false);   // 是否显示余额不足弹窗
const showComments = ref(false);
const comments = ref([]);
const newComment = ref('');
const replyContent = ref('');
const activeReplyCommentId = ref(null);
const readersCache = ref({}); // 缓存读者信息
const likedComments = ref(new Set()); // 存储已点赞的评论ID
const topN = ref(10); // 获取点赞数前10的评论
const expandedReplies = ref(new Set());
const rewardOptions = [
    { value: 10, label: '10点' },
    { value: 100, label: '100点' },
    { value: 500, label: '500点' },
    { value: 1000, label: '1000点' },
    { value: 2000, label: '2000点' },
    { value: 10000, label: '1万点' },
    { value: 50000, label: '5万点' },
    { value: 100000, label: '10万点' }
];

function goToLogin() {
    router.push('/L_R/login');
}
function handletonovel() {
    router.push('/Novels/Novel_Info/home');
}
//打开主页的方法
function openMyHomePage() {
    // 打开新窗口，跳转到用户主页
    window.open(`/UserHome`, '_blank');
}
function goToRecharge() {
    router.push('/Novels/Novel_Recharge');
}
function logout() {
    state.clearUserInfo();
    localStorage.removeItem('token');
    localStorage.removeItem('name');
    localStorage.removeItem('id');
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('name');
    sessionStorage.removeItem('id');
    router.push('/L_R/login');
}

const handleSearch = () => {
    if (searchQuery.value.trim()) {
        router.push({
            path: '/Novels/Search',
            query: {
                q: searchQuery.value,
                type: 'novel'
            }
        })
    }
};
const showSettings = ref(false);
const fontSize = ref(18);
const fontFamily = ref('system');
const bgColor = ref('#e5e5e5');

const bgOptions = [
    { value: '#e5e5e5', name: '默认' },
    { value: '#f8f4e6', name: '护眼' },
    { value: '#e6f0f9', name: '淡蓝' },
    { value: '#f9e6e6', name: '淡粉' },
    { value: '#e8f9e6', name: '淡绿' },
    { value: '#1a1a1a', name: '深色' }
];

const fontOptions = [
    { value: 'system', name: '系统' },
    { value: 'SimSun', name: '宋体' },
    { value: 'KaiTi', name: '楷体' }
];
const readerBgStyle = ref({
    backgroundColor: bgColor.value
});

const updateFontSize = (change) => {
    fontSize.value = Math.max(12, Math.min(30, fontSize.value + change));
};

const updateBgColor = (color) => {
    bgColor.value = color;
    readerBgStyle.value.backgroundColor = color;
};

const updateFontFamily = (family) => {
    fontFamily.value = family;
};
const cardBgColor = computed(() => {
    if (bgColor.value === '#1a1a1a') return '#2a2a2a';
    const hex = bgColor.value.replace('#', '');
    if (hex.length === 6) {
        const r = Math.round((parseInt(hex.slice(0, 2), 16) + 255) / 2);
        const g = Math.round((parseInt(hex.slice(2, 4), 16) + 255) / 2);
        const b = Math.round((parseInt(hex.slice(4, 6), 16) + 255) / 2);
        return `rgb(${r},${g},${b})`;
    }
    return bgColor.value;
});
const textColor = computed(() => {
    return bgColor.value === '#1a1a1a' ? '#f0f0f0' : '#222';
});
const showGuide = ref(false);
const handleGuideClick = () => {
    showGuide.value = true;
};
const isFavorite = ref(reader_state.isFavorite(selectNovelState.novelId));
async function handleAddShelf() {
    if (isFavorite.value) return;
    const readerId = reader_state.readerId;
    const response = await addOrUpdateCollect(selectNovelState.novelId, readerId, 'yes');
    if (response) {
        isFavorite.value = true;
        reader_state.favoriteBooks.push({
            novelId: selectNovelState.novelId,
            novel: selectNovelState, // 保存完整作品信息
            readerId,
            isPublic: "yes",
            collectTime: new Date().toISOString()
        });
        toast("加入书架成功！", {
            "type": "success",
            "dangerouslyHTMLString": true
        })
    } else {
        toast("加入书架失败：" + response.message, {
            "type": "error",
            "dangerouslyHTMLString": true
        })
    }
}
async function changeChapter(num) {
    try {
        // 检查是否是第一章且点击上一章
        if (num === -1 && selectNovelState.chapterId === 1) {
            toast("当前为第1章，无上一章", {
                "type": "info",
                "dangerouslyHTMLString": true
            });
            return;
        }
        // 检查是否是最后一章且点击下一章
        if (num === 1 && selectNovelState.chapterId === chapters.value[chapters.value.length - 1]?.chapterId) {
            toast("当前为最后1章，无下一章", {
                "type": "info",
                "dangerouslyHTMLString": true
            });
            return;
        }
        const nextChapterId = selectNovelState.chapterId + num;
        const response = await getChapter(selectNovelState.novelId, nextChapterId);
        if (response.status !== '已发布') {
            toast("第" + nextChapterId + "章未发布!", {
                "type": "info",
                "dangerouslyHTMLString": true
            });
            return;
        }
        // 检查章节购买状态
        if (response.isCharged === '是') {
            const purchaseStatus = await checkPurchase(
                reader_state.readerId,
                selectNovelState.novelId,
                nextChapterId
            );
            if (!purchaseStatus?.hasPurchased) {
                toast("第" + nextChapterId + "章需要购买后才能阅读", {
                    "type": "info",
                    "dangerouslyHTMLString": true
                });
                return;
            }
        }
        selectNovelState.resetChapter(
            response.chapterId,
            response.title,
            response.content,
            response.wordCount,
            response.pricePerKilo,
            response.calculatedPrice,
            response.isCharged,
            response.publishTime,
            response.status
        );
        comments.value = []; // 清空评论列表
        showComments.value = false; // 切换章节时隐藏评论
        router.push('/Novels/reader');
        scrollToTop();
    } catch (error) {
        toast("章节加载失败", {
            "type": "info",
            "dangerouslyHTMLString": true
        });
    }
}
const handleKeyDown = (e) => {
    if (showGuide.value) return;
    switch (e.key) {
        case 'ArrowLeft':
            changeChapter(-1); // 上一章
            break;
        case 'ArrowRight':
            changeChapter(1); // 下一章
            break;
    }
};
const handleBack = () => {
    router.go(-1);
};
const scrollToTop = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
};
// 在获取章节列表后，检查每个章节的购买状态
const fetchChapters = async () => {
    try {
        const response = await getNovelChaptersWithoutContent(selectNovelState.novelId);
        // 获取所有章节的购买状态
        const chaptersWithPurchaseStatus = await Promise.all(
            response
                .filter(chapter => chapter.status !== '草稿')
                .map(async chapter => {
                    // 如果是收费章节，检查是否已购买
                    if (chapter.isCharged === '是') {
                        try {
                            const purchaseStatus = await checkPurchase(
                                reader_state.readerId,
                                chapter.novelId,
                                chapter.chapterId
                            );
                            return {
                                ...chapter,
                                hasPurchased: purchaseStatus?.hasPurchased || wholehasPurchased.value
                            };
                        } catch (error) {
                            console.error(`检查章节 ${chapter.chapterId} 购买状态失败:`, error);
                            return {
                                ...chapter,
                                hasPurchased: false
                            };
                        }
                    }
                    // 免费章节直接标记为已购买
                    return {
                        ...chapter,
                        hasPurchased: true
                    };
                })
        );
        chapters.value = chaptersWithPurchaseStatus;
    } catch (error) {
        console.error('获取目录失败:', error);
        toast("获取目录失败!", {
            "type": "error",
            "dangerouslyHTMLString": true
        });
    }
};

// 计算排序后的章节
const sortedChapters = computed(() => {
    const sorted = [...chapters.value];
    sorted.sort((a, b) => sortAscending.value
        ? a.chapterId - b.chapterId
        : b.chapterId - a.chapterId);
    return sorted;
});

// 切换排序顺序
const toggleSortOrder = () => {
    sortAscending.value = !sortAscending.value;
};

const purchase_Chapter = async () => {
    try {
        if (accountBalance.value < selectedChapter.value.calculatedPrice) {
            showPurchaseDialog.value = false;
            showPurchaseInsufficientDialog.value = true;
            return;
        }
        const response = await purchaseChapter({
            readerId: reader_state.readerId,
            novelId: selectedChapter.value.novelId,
            chapterId: selectedChapter.value.chapterId
        });
        if (response.success) {
            // 更新章节购买状态
            const chapterIndex = chapters.value.findIndex(
                c => c.chapterId === selectedChapter.value.chapterId
            );
            if (chapterIndex !== -1) {
                chapters.value[chapterIndex].hasPurchased = true;
            }
            reader_state.balance -= selectedChapter.value.calculatedPrice;
            toast("购买成功！", {
                type: "success",
                dangerouslyHTMLString: true
            });
            // 跳转到该章节
            goToChapter(selectedChapter.value);
        } else {
            toast("购买失败: " + (response.message || "未知错误"), {
                type: "error",
                dangerouslyHTMLString: true
            });
        }
    } catch (error) {
        console.error('购买章节失败:', error);
        if (error.response && error.response.status === 400) {
            showPurchaseDialog.value = false;
            showPurchaseInsufficientDialog.value = true;
        } else {
            toast("购买失败: " + (error.message || "未知错误"), {
                type: "error",
                dangerouslyHTMLString: true
            });
        }
    } finally {
        showPurchaseDialog.value = false;
    }
};

// goToChapter 函数，使用章节级别的购买状态
const goToChapter = async (chapter) => {
    // 如果是收费章节且未购买，显示购买弹窗
    if (chapter.isCharged === '是' && !chapter.hasPurchased) {
        selectedChapter.value = chapter;
        showPurchaseDialog.value = true;
        return;
    }
    try {
        const response = await getChapter(chapter.novelId, chapter.chapterId);
        if (response.status !== '已发布') {
            toast("第" + chapter.chapterId + "章未发布!", {
                "type": "info",
                "dangerouslyHTMLString": true
            });
            return;
        }
        selectNovelState.resetChapter(
            response.chapterId,
            response.title,
            response.content,
            response.wordCount,
            response.pricePerKilo,
            response.calculatedPrice,
            response.isCharged,
            response.publishTime,
            response.status
        );
        comments.value = []; // 清空评论列表
        showComments.value = false; // 切换章节时隐藏评论
        showCatalog.value = false;
        scrollToTop();
    } catch (error) {
        toast("章节加载失败!", {
            "type": "info",
            "dangerouslyHTMLString": true
        });
    }
};

// 选择打赏金额
const selectReward = (value) => {
    selectedReward.value = value;
}

// 获取账号余额
const fetchReaderBalance = async () => {
    try {
        const response = await getReaderBalance(reader_state.readerId);
        accountBalance.value = response.data?.balance || response?.balance || 0;
    } catch (error) {
        console.error('获取用户余额失败:', error);
        accountBalance.value = 0;
    }
};

// 确认打赏按钮逻辑
const confirmReward = async () => {
    const currentNovelId = selectNovelState.novelId;
    const currentReaderId = reader_state.readerId;
    const currentvalue = selectedReward.value;
    try {
        // 1. 检查余额是否充足
        if (accountBalance.value < selectedReward.value) {
            showBalanceInsufficientDialog.value = true;
            return;
        }
        // 2. 调用打赏API
        const response = await rewardNovel({
            readerId: currentReaderId,
            novelId: currentNovelId,
            amount: currentvalue
        });
        reader_state.balance -= currentvalue;
        toast(`成功打赏 ${currentvalue} 起点币`, {
            type: "success",
            dangerouslyHTMLString: true
        });
        console.log('打赏结果:', response.data);
        // 3. 刷新余额
        await fetchReaderBalance();
    } catch (error) {
        console.error('打赏失败:', error);
        toast(`打赏失败: ${error.message}`, {
            type: "error",
            dangerouslyHTMLString: true
        });
        await fetchReaderBalance();
    }
};
// 切换评论显示
const toggleComments = () => {
    showComments.value = !showComments.value;
    if (showComments.value && comments.value.length === 0) {
        fetchComments();
    }
};

// 获取评论
const fetchComments = async () => {
    try {
        // 获取顶级评论
        const response = await getTopLikedCommentsByChapter(
            selectNovelState.novelId,
            selectNovelState.chapterId,
            topN.value
        );
        // 为每个评论添加replies属性
        const commentsWithReplies = await Promise.all(response.map(async comment => {
            // 获取该评论的所有回复
            const replies = await getRepliesByParentId(comment.commentId);
            // 获取回复的完整内容
            const fullReplies = await Promise.all(replies.map(async reply => {
                const replyContent = await getComment(reply.commentId);
                return {
                    ...replyContent,
                    commentLevel: reply.commentLevel
                };
            }));
            // 预加载读者信息
            await getReaderInfo(comment.readerId);
            await Promise.all(fullReplies.map(reply => getReaderInfo(reply.readerId)));
            // 检查点赞状态
            if (state.isloggedin) {
                const co_isLiked = await isLiked(comment.commentId, reader_state.readerId);
                if (co_isLiked.liked) likedComments.value.add(comment.commentId);
                await Promise.all(fullReplies.map(async reply => {
                    const isReplyLiked = await isLiked(reply.commentId, reader_state.readerId);
                    if (isReplyLiked.liked) likedComments.value.add(reply.commentId);
                }));
            }
            return {
                ...comment,
                replies: fullReplies,
                commentLevel: 1 // 顶级评论
            };
        }));
        comments.value = commentsWithReplies;
    } catch (error) {
        console.error('获取评论失败:', error);
        toast("获取评论失败", {
            "type": "error",
            "dangerouslyHTMLString": true
        });
    }
};

// 获取读者信息并缓存
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

// 获取读者头像
const getReaderAvatar = (readerId) => {
    return 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' + (readersCache.value[readerId]?.avatarUrl || 'a3dc347b-45dd-4d89-8b9d-65b75477ee3d.jpg')
};

// 获取读者名称
const getReaderName = (readerId) => {
    return readersCache.value[readerId]?.readerName || '未知用户';
};

// 格式化时间
const formatTime = (timeStr) => {
    if (!timeStr) return '';
    const date = new Date(timeStr);
    return date.toLocaleString();
};

// 头像加载失败处理
const handleAvatarError = (e) => {
    e.target.src = require('@/assets/logo.png');
};

// 检查是否已点赞
const isLikeds = (commentId) => {
    return likedComments.value.has(commentId);
};

// 点赞/取消点赞
const toggleLike = async (comment) => {
    if (!state.isloggedin) {
        toast("请先登录", {
            "type": "warning",
            "dangerouslyHTMLString": true
        });
        return;
    }
    try {
        if (isLikeds(comment.commentId)) {
            await unlikeComment(comment.commentId, reader_state.readerId);
            likedComments.value.delete(comment.commentId);
            comment.likes--;
            toast("成功取消点赞！", {
                "type": "success",
                "dangerouslyHTMLString": true
            });
        } else {
            await likeComment(comment.commentId, reader_state.readerId);
            likedComments.value.add(comment.commentId);
            comment.likes++;
            toast("点赞成功", {
                "type": "success",
                "dangerouslyHTMLString": true
            });
        }
    } catch (error) {
        console.error('操作失败:', error);
        toast("操作失败", {
            "type": "error",
            "dangerouslyHTMLString": true
        });
    }
};

// 提交评论
const submitComment = async () => {
    if (!newComment.value.trim()) return;
    try {
        const response = await createComment({
            readerId: reader_state.readerId,
            novelId: selectNovelState.novelId,
            chapterId: selectNovelState.chapterId,
            title: '评论',
            content: newComment.value,
            likes: 0,
            status: "通过",
            createTime: new Date().toISOString()
        });
        // 添加到评论列表
        comments.value.unshift(response);
        newComment.value = '';
        toast("评论成功", {
            "type": "success",
            "dangerouslyHTMLString": true
        });
    } catch (error) {
        console.error('评论失败:', error);
        toast("评论失败", {
            "type": "error",
            "dangerouslyHTMLString": true
        });
    }
};

// 显示回复输入框
const showReplyInput = (commentId) => {
    if (!state.isloggedin) {
        toast.warning("请先登录");
        return;
    }
    const isActive = activeReplyCommentId.value === commentId;
    activeReplyCommentId.value = isActive ? null : commentId;
    replyContent.value = '';
};

// 取消回复
const cancelReply = () => {
    activeReplyCommentId.value = null;
    replyContent.value = '';
};

// 提交回复
const submitReply = async (parentCommentId) => {
    if (!replyContent.value.trim()) return;
    try {
        // 1. 创建评论
        const commentResponse = await createComment({
            readerId: reader_state.readerId,
            novelId: selectNovelState.novelId,
            chapterId: selectNovelState.chapterId,
            title: '回复',
            content: replyContent.value,
            likes: 0,
            status: "通过",
            createTime: new Date().toISOString()
        });
        // 2. 创建回复关系
        await addCommentReply({
            commentId: commentResponse.commentId,
            preComId: parentCommentId,
            commentLevel: 2 // 二级评论
        });
        // 3. 获取完整的回复内容
        const fullReply = await getComment(commentResponse.commentId);
        // 4. 添加到回复列表
        const parentComment = comments.value.find(c => c.commentId === parentCommentId);
        if (parentComment) {
            // 初始化replies数组如果不存在
            if (!parentComment.replies) {
                parentComment.replies = [];
            }
            // 添加回复
            parentComment.replies.push({
                ...fullReply,
                commentLevel: 2
            });
        }
        replyContent.value = '';
        activeReplyCommentId.value = null;
        toast("回复成功", {
            "type": "success",
            "dangerouslyHTMLString": true
        });
    } catch (error) {
        console.error('回复失败:', error);
        toast("回复失败", {
            "type": "error",
            "dangerouslyHTMLString": true
        });
    }
};
// 切换回复的展开状态
const toggleReplies = (commentId) => {
    if (expandedReplies.value.has(commentId)) {
        expandedReplies.value.delete(commentId);
    } else {
        expandedReplies.value.add(commentId);
    }
};
function goReaderHome(readerId) {
    router.push(`/reader/${readerId}`);
}
const viewAllComments = () => {
    router.push(`/chapter-comments/${selectNovelState.novelId}/${selectNovelState.chapterId}`);
};
function showReportDialog(commentId) {
    router.push(`/comment-report/${reader_state.readerId}/${commentId}`);
}
const isSpeaking = ref(false);
const showTtsControls = ref(false);
const selectedVoice = ref('');
const availableVoices = ref([]);
const speechRate = ref(1);
const speechPitch = ref(1);
const speechSynthesis = ref(null);
const utterance = ref(null);

// 初始化语音合成
onMounted(() => {
    if ('speechSynthesis' in window) {
        speechSynthesis.value = window.speechSynthesis;
        // 等待语音列表加载
        speechSynthesis.value.onvoiceschanged = () => {
            // 只保留中文语音
            availableVoices.value = speechSynthesis.value.getVoices().filter(voice =>
                voice.lang.includes('zh') || voice.lang.includes('cmn')
            );
            // 如果有中文语音，默认选择第一个
            if (availableVoices.value.length > 0) {
                selectedVoice.value = availableVoices.value[0].name;
            }
        };
        // 立即加载语音列表
        availableVoices.value = speechSynthesis.value.getVoices().filter(voice =>
            voice.lang.includes('zh') || voice.lang.includes('cmn')
        );
        // 如果有中文语音，默认选择第一个
        if (availableVoices.value.length > 0) {
            selectedVoice.value = availableVoices.value[0].name;
        } else {
            console.error("未找到中文语音引擎", availableVoices.value);
        }
    } else {
        toast("您的浏览器不支持语音合成功能", { type: "error" });
    }
});

const toggleTextToSpeech = () => {
    if (!speechSynthesis.value) {
        toast("您的浏览器不支持语音合成功能", { type: "error" });
        return;
    }
    if (isSpeaking.value) {
        stopSpeaking();
    } else {
        startSpeaking();
    }
};

const startSpeaking = () => {
    if (isSpeaking.value) return;
    const content = selectNovelState.cha_content;
    if (!content) {
        toast("没有可朗读的内容", { type: "warning" });
        return;
    }
    // 创建新的语音合成实例
    utterance.value = new SpeechSynthesisUtterance(content);
    // 设置语音参数
    const voice = availableVoices.value.find(v => v.name === selectedVoice.value);
    if (voice) {
        utterance.value.voice = voice;
    }
    utterance.value.rate = speechRate.value;
    utterance.value.pitch = speechPitch.value;
    utterance.value.lang = 'zh-CN'; // 设置为中文
    // 事件监听
    utterance.value.onstart = () => {
        isSpeaking.value = true;
        showTtsControls.value = true;
    };
    utterance.value.onend = () => {
        isSpeaking.value = false;
    };
    utterance.value.onerror = () => {
        isSpeaking.value = false;
        toast("已停止朗读。", { type: "success" });
    };
    // 开始朗读
    speechSynthesis.value.speak(utterance.value);
};

const stopSpeaking = () => {
    if (speechSynthesis.value && isSpeaking.value) {
        speechSynthesis.value.cancel();
        isSpeaking.value = false;
        showTtsControls.value = false;
    }
};

// 使用时可以指定空格数量，如：
// formatContent(content, 4);  // 4个空格
// formatContent(content, 6);  // 6个空格
const formatContent = (content, spaceCount = 7) => {  // 默认7个空格，可自定义
    if (!content) return '';
    const paragraphs = content.split('\n');
    // 根据spaceCount生成对应数量的空格
    const spaces = '&nbsp;'.repeat(spaceCount);
    return paragraphs.map(p => `${spaces}${p}`).join('<br>'.repeat(2));
};

// 监听弹窗打开时刷新余额
watch(
    () => showRewardDialog.value,
    async (isOpen) => {
        if (isOpen) {
            await fetchReaderBalance();
        }
    }
);
onMounted(() => {
    window.addEventListener('keydown', handleKeyDown);
    fetchChapters();
    scrollToTop();
});
onMounted(async () => {
    try {
        const status = await getWholePurchaseStatus(reader_state.readerId, selectNovelState.novelId);
        wholehasPurchased.value = status?.hasPurchased || false
    } catch (err) {
        console.error('查询买断状态失败', err)
    }
})
onUnmounted(() => {
    window.removeEventListener('keydown', handleKeyDown);
    stopSpeaking();
});
</script>

<style scoped>
.reader-bg {
    background: #e5e5e5;
    min-height: 100vh;
    width: 100vw;
}

.tts-control-panel {
    position: fixed;
    bottom: 20px;
    right: 20px;
    background: white;
    padding: 15px;
    border-radius: 8px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
    z-index: 1000;
    width: 250px;
}

.tts-voice-select,
.tts-rate-control,
.tts-pitch-control {
    margin-bottom: 10px;
}

.tts-voice-select select {
    width: 100%;
    padding: 5px;
    border: 1px solid #ddd;
    border-radius: 4px;
}

.tts-rate-control input[type="range"],
.tts-pitch-control input[type="range"] {
    width: 60%;
    margin: 0 10px;
}

.tts-rate-control span,
.tts-pitch-control span {
    display: inline-block;
    width: 40px;
    text-align: right;
}

.reader-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 0 36px;
    height: 64px;
    background: #fff;
    border-bottom: 1px solid #eee;
}

.logo {
    display: flex;
    align-items: center;
}

.logo img {
    width: 36px;
    height: 36px;
    margin-right: 8px;
}

.logo h1 {
    font-size: 20px;
    margin: 0;
    font-weight: bold;
}

.search-bar {
    display: flex;
    align-items: center;
    width: 400px;
    height: 32px;
}

.search-bar input {
    flex: 1;
    padding: 0 16px;
    height: 28px;
    border: 2px solid #ffd100;
    border-right: none;
    border-radius: 16px 0 0 16px;
    outline: none;
    font-size: 15px;
    background: #fff;
}

.search-bar button {
    height: 32px;
    padding: 0 18px;
    background-color: #ffd100;
    color: #222;
    font-weight: bold;
    border: none;
    border-radius: 0 16px 16px 0;
    cursor: pointer;
    font-size: 15px;
}

.user-actions .login-btn {
    height: 32px;
    display: flex;
    align-items: center;
    padding: 4px 16px;
    border: 2px solid #ffd100;
    border-radius: 999px;
    background: #fff;
    color: #222;
    font-size: 15px;
    font-weight: bold;
    cursor: pointer;
}

.reader-main {
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: stretch;
    width: 100%;
    margin-top: 32px;
    position: relative;
}

.left-menu {
    position: fixed;
    top: 50%;
    transform: translateY(-50%);
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    width: 100px;
    z-index: 100;
    left: 211px;
    transition: left 0.3s ease;
    padding-right: 10px;
}

.back-btn {
    background: #ffd100;
    color: #222;
    border: none;
    border-radius: 8px;
    padding: 8px 0;
    font-size: 16px;
    width: 68px;
    margin-bottom: 10px;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 4px;
}

.back-btn:hover {
    background: #f27908;
    color: #fefcfc;
}

.menu-item.guide {
    background: #f5f5f5;
    color: #222;
    border-radius: 8px;
    width: 68px;
    text-align: center;
    margin-top: 6px;
    padding: 5px 0;
    font-size: 14px;
}

.menu-item.guide:hover {
    background: #ffd100;
    color: #eb640a;
}

.reader-card {
    box-shadow: 0 2px 24px rgba(60, 117, 221, 0.08);
    border-radius: 12px;
    padding: 36px 48px;
    min-width: 600px;
    max-width: 820px;
    width: 100%;
    min-height: 700px;
    margin: 0 24px;
    display: flex;
    flex-direction: column;
    align-items: stretch;
    margin-left: 100px;
    transition: margin-right 0.3s ease, margin-left 0.3s ease;
}

.chapter-header {
    margin-bottom: 24px;
    text-align: center;
}

.chapter-title {
    font-size: 28px;
    font-weight: bold;
    margin: 0 0 10px 0;
}

.meta-row {
    display: flex;
    justify-content: center;
    gap: 36px;
    font-size: 15px;
    color: #666;
    margin-bottom: 10px;
}

.chapter-divider {
    height: 1px;
    background: transparent;
    margin: 16px 0;
    border-bottom: 1px dashed currentColor;
    opacity: 0.6;
}

.novel-content {
    font-size: 18px;
    color: #222;
    line-height: 1.7;
    white-space: pre-line !important;
    word-break: break-word;
}

/* 购买弹窗样式*/
.purchase-dialog-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, 0.7);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 9999;
}

.purchase-dialog {
    background-color: white;
    border-radius: 12px;
    width: 90%;
    max-width: 400px;
    padding: 25px;
    box-shadow: 0 5px 20px rgba(0, 0, 0, 0.3);
    animation: fadeIn 0.3s ease;
    z-index: 10000;
}

@keyframes fadeIn {
    from {
        opacity: 0;
        transform: translateY(20px);
    }

    to {
        opacity: 1;
        transform: translateY(0);
    }
}

.purchase-content {
    text-align: center;
    margin: 25px 0;
    font-size: 18px;
    color: #333;
    padding: 0 20px;
}

.purchase-content p {
    margin: 15px 0;
    font-weight: bold;
    font-size: 20px;
    color: #f56c6c;
}

.confirm-reward-btn {
    width: 100%;
    padding: 12px;
    background-color: #ff6b6b;
    color: white;
    border: none;
    border-radius: 6px;
    font-size: 16px;
    font-weight: bold;
    cursor: pointer;
    transition: background-color 0.2s;
    margin-top: 15px;
}

.confirm-reward-btn:hover {
    background-color: #ff5252;
}

.da_close-btn {
    background: none;
    border: none;
    font-size: 24px;
    cursor: pointer;
    color: #999;
    transition: color 0.2s;
}

.da_close-btn:hover {
    color: #666;
}

/* 购买余额不足弹窗 - 完全独立样式 */
.purchase-insufficient-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, 0.7);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 10001;
}

.purchase-insufficient-dialog {
    background-color: white;
    border-radius: 12px;
    width: 90%;
    max-width: 400px;
    padding: 20px;
    box-shadow: 0 5px 20px rgba(0, 0, 0, 0.2);
    animation: purchaseFadeIn 0.3s ease;
}

@keyframes purchaseFadeIn {
    from {
        opacity: 0;
        transform: translateY(20px);
    }

    to {
        opacity: 1;
        transform: translateY(0);
    }
}

.purchase-dialog-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 15px;
    padding-bottom: 10px;
    border-bottom: 1px solid #eee;
}

.purchase-dialog-header h3 {
    margin: 0;
    font-size: 18px;
}

.purchase-close-btn {
    background: none;
    border: none;
    font-size: 24px;
    cursor: pointer;
    color: #999;
}

.purchase-insufficient-content {
    padding: 10px 0;
}

.purchase-insufficient-message {
    color: #f56c6c;
    font-size: 16px;
    text-align: center;
    margin: 10px 0 20px;
}

.purchase-amount-info {
    display: flex;
    flex-direction: column;
    gap: 10px;
    margin: 20px 0;
    padding: 15px;
    background: #f9f9f9;
    border-radius: 8px;
}

.purchase-amount-info span {
    display: flex;
    justify-content: space-between;
    font-size: 15px;
}

.purchase-action-buttons {
    display: flex;
    gap: 15px;
    margin-top: 20px;
}

.purchase-recharge-btn {
    flex: 1;
    padding: 12px;
    background-color: #f56c6c;
    color: white;
    border: none;
    border-radius: 6px;
    font-size: 16px;
    cursor: pointer;
    transition: background 0.2s;
}

.purchase-recharge-btn:hover {
    background-color: #e65c5c;
}

.purchase-cancel-btn {
    flex: 1;
    padding: 12px;
    background-color: #f0f0f0;
    color: #666;
    border: none;
    border-radius: 6px;
    font-size: 16px;
    cursor: pointer;
    transition: background 0.2s;
}

.purchase-cancel-btn:hover {
    background-color: #e0e0e0;
}

.right-menu {
    position: fixed;
    right: 0;
    top: 55%;
    transform: translateY(-50%);
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100px;
    z-index: 100;
    transition: right 0.3s ease;
}

.menu-item {
    background: #f9f4f4;
    border-radius: 8px;
    color: #222;
    width: 100px;
    text-align: center;
    margin-bottom: 12px;
    padding: 10px 0;
    font-size: 16px;
    box-shadow: 0 1px 6px rgba(60, 117, 221, 0.08);
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 7px;
    transition: background 0.2s;
}

.menu-item:hover {
    background: #ffd100;
    color: #f27908;
}

.menu-item.to-top {
    margin-top: 130px;
}

.iconfont {
    font-family: "iconfont" !important;
    font-size: 20px;
    font-style: normal;
    font-weight: normal;
    display: inline-block;
    vertical-align: middle;
}

.settings-modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

.settings-content {
    background: white;
    padding: 24px;
    border-radius: 12px;
    width: 400px;
    max-width: 90%;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
}

.settings-content h3 {
    margin-top: 0;
    text-align: center;
    color: #131212;
}

.setting-group {
    margin-bottom: 20px;
}

.setting-group h4 {
    margin-bottom: 10px;
    color: #666;
}

.bg-options,
.font-family-options {
    display: flex;
    flex-wrap: wrap;
    gap: 8px;
}

.bg-options button {
    width: 60px;
    height: 30px;
    color: #f10d0d;
    border-radius: 4px;
    border: 1px solid #ddd;
    cursor: pointer;
}

.font-family-options button,
.font-size-control button {
    padding: 6px 12px;
    border-radius: 4px;
    border: 1px solid #ddd;
    background: white;
    cursor: pointer;
}

.font-size-control {
    display: flex;
    align-items: center;
    gap: 12px;
}

.font-size-control span {
    min-width: 30px;
    text-align: center;
}

button.active {
    border-color: #ffd100;
    background-color: #ffd100;
    color: #222;
}

.close-btn {
    display: block;
    margin: 20px auto 0;
    padding: 8px 24px;
    background: #ffd100;
    border: none;
    border-radius: 20px;
    cursor: pointer;
}

.guide-modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 2000;
}

.guide-content {
    background: white;
    padding: 24px;
    border-radius: 12px;
    width: 300px;
    text-align: center;
}

.guide-content h3 {
    margin-top: 0;
    margin-bottom: 20px;
    color: #333;
}

.shortcut-item {
    font-size: 16px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px 0;
    border-bottom: 1px solid #eee;
}

.key {
    background: #f5f5f5;
    padding: 4px 12px;
    border-radius: 4px;
    font-family: monospace;
    color: #333;
}

.confirm-btn {
    margin-top: 20px;
    padding: 8px 24px;
    background: #ffd100;
    border: none;
    border-radius: 20px;
    cursor: pointer;
    font-weight: bold;
}

.menu-item.in-shelf {
    background: #f0f0f0;
    color: #999;
    cursor: default;
}

.menu-item.in-shelf:hover {
    background: #f0f0f0;
    color: #999;
}

.menu-item:disabled {
    opacity: 1;
}

.chapter-nav-buttons {
    display: flex;
    justify-content: center;
    gap: 160px;
    margin: 40px auto 20px;
    width: 100%;
    max-width: 600px;
}

.nav-button {
    padding: 10px 30px;
    border: 1px solid #000;
    border-radius: 6px;
    font-size: 16px;
    cursor: pointer;
    transition: all 0.2s;
    flex: 0 0 auto;
}

.nav-button.prev {
    background-color: #f5f5f5;
    color: #333;
    border-color: #000;
}

.nav-button.next {
    background-color: #ffd100;
    color: #222;
    border-color: #000;
}

.nav-button.prev:hover {
    background-color: #e0e0e0;
    border-color: #000;
}

.nav-button.next:hover {
    background-color: #f0c000;
    border-color: #000;
}

.header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px 20px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
    z-index: 10;
    margin-bottom: 80px;
    padding-bottom: 10px;
}

.logo {
    display: flex;
    align-items: center;
}

.logo img {
    width: 36px;
    height: 36px;
    margin-right: 10px;
}

.logo h1 {
    font-size: 20px;
    margin: 0;
}

.search-bar {
    display: flex;
    align-items: center;
    width: 100%;
    max-width: 400px;
    margin: 0 auto;
    height: 40px;
}

.search-bar input {
    flex: 1;
    padding: 0 18px;
    height: 36px;
    border: 2px solid #ffd100;
    border-right: none;
    border-radius: 24px 0 0 24px;
    outline: none;
    font-size: 18px;
    background: #fff;
}

.search-bar button {
    height: 40px;
    padding: 0 32px;
    background-color: #ffd100;
    color: #222;
    font-weight: bold;
    border: none;
    border-radius: 0 24px 24px 0;
    cursor: pointer;
    font-size: 18px;
    box-shadow: none;
    transition: background 0.2s;
}

.search-bar button:hover {
    background-color: #ffea80;
}

.user-actions .login-btn {
    height: 40px;
    display: flex;
    align-items: center;
    padding: 8px 24px;
    border: 2px solid #ffd100;
    border-radius: 999px;
    background: #fff;
    color: #222;
    font-size: 18px;
    font-weight: bold;
    cursor: pointer;
    transition: border-color 0.2s;
    outline: none;
}

.user-actions .login-btn:hover {
    border-color: #ffea80;
    color: #e09c13;
}

.login-icon {
    display: flex;
    align-items: center;
    margin-right: 8px;
}

.user-dropdown {
    position: relative;
    cursor: pointer;
}

.user-avatar {
    width: 36px;
    height: 36px;
    border-radius: 50%;
    object-fit: cover;
    border: 2px solid #ffd100;
}

.dropdown-menu {
    position: absolute;
    right: 0;
    top: 100%;
    width: 180px;
    background: linear-gradient(to bottom, #f4dfa5 0%, #f3f3f0 100%);
    border-radius: 8px;
    z-index: 100;
    padding: 10px 0;
    border: 1px solid rgba(0, 0, 0, 0.05);
    overflow: hidden;
}

.user-info {
    padding: 10px 15px;
    border-bottom: 1px solid #eee;
}

.user-info p:first-child {
    font-weight: bold;
    margin-bottom: 5px;
    color: #000;
}

.user-info p:last-child {
    font-size: 12px;
    color: #888;
}

.dropdown-divider {
    height: 1px;
    background: #eee;
    margin: 5px 0;
}

.dropdown-item {
    display: block;
    padding: 8px 15px;
    color: #333;
    text-decoration: none;
    font-size: 14px;
}

.dropdown-item:hover {
    background: #f5f5f5;
    color: #f7b604;
}

.dropdown-item i {
    width: 20px;
    text-align: center;
}

/* 添加目录模态框样式 */
.catalog-modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.6);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 2000;
    backdrop-filter: blur(4px);
}

.catalog-content {
    background: white;
    border-radius: 16px;
    width: 900px;
    max-width: 90%;
    max-height: 80vh;
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.2);
    display: flex;
    flex-direction: column;
    overflow: hidden;
    animation: fadeIn 0.3s ease;
}

@keyframes fadeIn {
    from {
        opacity: 0;
        transform: translateY(20px);
    }

    to {
        opacity: 1;
        transform: translateY(0);
    }
}

.catalog-header {
    padding: 20px;
    border-bottom: 1px solid rgba(0, 0, 0, 0.08);
    background: rgba(255, 255, 255, 0.9);
}

.header-top {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 12px;
}

.header-top h3 {
    margin: 0;
    font-size: 20px;
    font-weight: 600;
    color: var(--text-color);
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    max-width: 80%;
}

.ca_close-btn {
    background: rgba(0, 0, 0, 0.05);
    border: none;
    width: 36px;
    height: 36px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    transition: all 0.2s;
}

.ca_close-btn:hover {
    background: rgba(0, 0, 0, 0.1);
}

.ca_close-btn svg {
    opacity: 0.7;
}

.header-meta {
    display: flex;
    align-items: center;
    gap: 12px;
    font-size: 14px;
}

.status-badge {
    background: #ffd100;
    color: #222;
    padding: 4px 10px;
    border-radius: 12px;
    font-size: 12px;
    font-weight: 500;
}

.chapter-count {
    color: #666;
}

.sort-btn {
    background: rgba(0, 0, 0, 0.05);
    border: none;
    padding: 6px 12px;
    border-radius: 16px;
    font-size: 13px;
    cursor: pointer;
    transition: all 0.2s;
    margin-left: auto;
}

.sort-btn:hover {
    background: rgba(0, 0, 0, 0.1);
}

.catalog-grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 12px;
    padding: 20px;
    overflow-y: auto;
    flex: 1;
}

.chapter-card {
    background: rgba(255, 255, 255, 0.7);
    border-radius: 12px;
    padding: 16px;
    cursor: pointer;
    transition: all 0.2s;
    position: relative;
    overflow: hidden;
    border: 1px solid rgba(0, 0, 0, 0.05);
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.03);
}

.chapter-card:hover {
    transform: translateY(-2px);
    box-shadow: 0 6px 12px rgba(0, 0, 0, 0.08);
    border-color: rgba(0, 0, 0, 0.1);
}

.chapter-card.current {
    background: rgba(255, 209, 0, 0.15);
    border: 1px solid rgba(255, 209, 0, 0.3);
}

.chapter-card.vip {
    background: linear-gradient(135deg, rgba(255, 240, 230, 0.7) 0%, rgba(255, 248, 240, 0.7) 100%);
    border: 1px solid rgba(255, 180, 0, 0.2);
}

.chapter-card.locked {
    cursor: not-allowed;
}

.card-content {
    display: flex;
    position: relative;
    z-index: 1;
}

.chapter-number {
    font-size: 16px;
    color: #555;
    margin-bottom: 4px;
}

.ca_chapter-title {
    font-size: 16px;
    font-weight: 500;
    color: var(--text-color);
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    margin-left: 15px;
}

.chapter-card.current .ca_chapter-title {
    color: #ff9500;
}

.vip-tag {
    position: absolute;
    right: 12px;
    background: linear-gradient(135deg, #f1a73f, #ff5e00);
    color: #222;
    font-size: 11px;
    padding: 2px 8px;
    border-radius: 10px;
    font-weight: bold;
}

.chapter-status {
    position: absolute;
    left: 215px;
    background: rgba(141, 240, 54, 0.8);
    color: #222;
    font-size: 11px;
    padding: 2px 8px;
    border-radius: 10px;
    font-weight: bold;
}

.current-badge {
    position: absolute;
    right: 54px;
    background: #ffd100;
    color: #222;
    font-size: 11px;
    padding: 2px 8px;
    border-radius: 10px;
    font-weight: bold;
}

.lock-overlay {
    position: absolute;
    right: 80px;
    bottom: 7px;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    color: #ff4d4f;
    font-size: 12px;
    font-weight: 500;
}

.lock-overlay svg {
    width: 24px;
    height: 24px;
}

/* 暗黑模式适配 */
.dark .catalog-content {
    background: #2a2a2a;
    border-color: #444;
}

.dark .catalog-header {
    background: #333;
    border-color: #444;
}

.dark .chapter-card {
    background: #333;
    border-color: #444;
}

.dark .chapter-card.current {
    background: rgba(255, 209, 0, 0.2);
    border-color: rgba(255, 209, 0, 0.4);
}

.dark .chapter-number {
    color: #aaa;
}

.dark .lock-overlay {
    background: rgba(50, 50, 50, 0.9);
}

/* 打赏弹窗样式 */
.reward-dialog-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

.reward-dialog {
    background-color: white;
    border-radius: 8px;
    width: 90%;
    max-width: 400px;
    padding: 20px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.dialog-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
}

.dialog-header h3 {
    margin: 0;
    font-size: 18px;
    font-weight: bold;
}

.da_close-btn {
    background: none;
    border: none;
    font-size: 24px;
    cursor: pointer;
    color: #999;
}

.reward-options {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 10px;
    margin-bottom: 20px;
}

.reward-option {
    position: relative;
    border: 1px solid #ddd;
    border-radius: 4px;
    padding: 10px;
    text-align: center;
    cursor: pointer;
    transition: all 0.2s;
}

.reward-option:hover {
    border-color: #ff6b6b;
}

.reward-option.selected {
    background-color: #fff0f0;
    border-color: #ff6b6b;
    color: #ff6b6b;
}

.check-icon {
    position: absolute;
    top: 4px;
    right: 4px;
    width: 16px;
    height: 16px;
    color: #ff6b6b;
}

.balance-info {
    display: flex;
    justify-content: space-between;
    margin-bottom: 20px;
    font-size: 14px;
    color: #666;
}

.confirm-reward-btn {
    width: 100%;
    padding: 12px;
    background-color: #ff6b6b;
    color: white;
    border: none;
    border-radius: 4px;
    font-size: 16px;
    cursor: pointer;
    transition: background-color 0.2s;
}

.confirm-reward-btn:hover {
    background-color: #ff5252;
}

/* 余额不足弹窗 */
.insufficient-dialog-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

.insufficient-dialog {
    background-color: white;
    border-radius: 8px;
    width: 90%;
    max-width: 400px;
    padding: 20px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.insufficient-content {
    display: flex;
    flex-direction: column;
    gap: 15px;
}

.insufficient-message {
    color: #f56c6c;
    font-size: 16px;
    text-align: center;
    margin: 10px 0;
}

.amount-info {
    display: flex;
    flex-direction: column;
    gap: 8px;
    font-size: 14px;
    color: #666;
    padding: 10px 0;
    border-bottom: 1px solid #eee;
}

.recharge-btn {
    width: 100%;
    padding: 12px;
    background-color: #f56c6c;
    color: white;
    border: none;
    border-radius: 4px;
    font-size: 16px;
    cursor: pointer;
    transition: background-color 0.2s;
}

.recharge-btn:hover {
    background-color: #e65c5c;
}

/* 新增评论区域样式 */
.no-comments {
    text-align: center;
    color: #aaa;
    margin: 40px 0 20px;
    font-size: 18px;
}

.comments-section {
    position: fixed;
    top: 0;
    right: -300px;
    width: 405px;
    height: 100vh;
    background: white;
    box-shadow: -2px 0 10px rgba(0, 0, 0, 0.1);
    padding: 20px;
    overflow-y: auto;
    z-index: 99;
    transition: right 0.3s ease;
}

.comments-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding-bottom: 15px;
    border-bottom: 1px solid #eee;
    margin-bottom: 15px;
}

.close-comments {
    background: none;
    border: none;
    cursor: pointer;
    color: #666;
}

.close-comments:hover {
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

.user-avatar:hover {
    transform: scale(1.1);
}

.comments-list {
    display: flex;
    flex-direction: column;
    gap: 20px;
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
}

.comment-avatar:hover {
    transform: scale(1.1);
}

.comment-info {
    flex: 1;
}

.comment-author {
    font-weight: bold;
    font-size: 14px;
    color: #333;
}

.comment-author:hover {
    color: #f0940a;
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
    margin-right: 30px;
}

.like-btn.liked {
    color: #f56c6c;
}

.like-btn svg {
    fill: currentColor;
}

.comment-content {
    font-size: 14px;
    line-height: 1.5;
    color: #333;
    margin-left: 46px;
}

.comment-actions {
    margin-left: 46px;
    margin-top: 10px;
}

.comment-actions button {
    background: none;
    border: none;
    color: #1890ff;
    font-size: 12px;
    cursor: pointer;
    padding: 0;
}

.reply-item {
    margin-top: 15px;
    padding-left: 15px;
    border-left: 2px solid #eee;
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
}

.reply-info {
    flex: 1;
}

.reply-author {
    font-weight: bold;
    font-size: 13px;
    color: #333;
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

.comment-actions {
    display: flex;
    gap: 50px;
    margin-left: 46px;
    margin-top: 10px;
}

.show-replies-btn {
    color: #1890ff;
    font-size: 12px;
}

.show-replies-btn:hover {
    text-decoration: underline;
}

.reply-item {
    margin-top: 15px;
    padding-left: 15px;
    border-left: 2px solid #eee;
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
}

.reply-info {
    flex: 1;
}

.reply-author {
    font-weight: bold;
    font-size: 13px;
    color: #333;
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
}

.comment-actions-re {
    background: none;
    border: none;
    color: #1890ff;
    font-size: 12px;
    cursor: pointer;
    padding: 0;
}

.comment-actions-re:hover {
    text-decoration: underline;
}

.view-more-comments {
    margin-top: 20px;
    margin-bottom: 60px;
    text-align: center;
}

.view-more-comments button {
    padding: 8px 16px;
    color: #5e5d5d;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    font-size: 16px;
    transition: background-color 0.2s;
}

.view-more-comments button:hover {
    text-decoration: underline;
}

.report-btn {
    color: #f30505;
    font-size: 12px;
    cursor: pointer;
    padding: 0;
    display: flex;
    align-items: center;
    position: absolute;
    right: 0;
    left: auto;
    transform: translateX(-55px);
}

.report-btn:hover {
    text-decoration: underline;
}

/* 响应式调整 */
@media (max-width: 768px) {
    .catalog-grid {
        grid-template-columns: 1fr;
    }

    .catalog-content {
        width: 95%;
        max-height: 85vh;
    }

    .header-top h3 {
        font-size: 18px;
    }
}

@media (max-width: 480px) {
    .header-meta {
        flex-wrap: wrap;
        row-gap: 8px;
    }

    .chapter-card {
        padding: 12px;
    }

    .chapter-number {
        font-size: 13px;
    }

    .ca_chapter-title {
        font-size: 15px;
    }
}

@media (max-width: 768px) {
    .chapter-nav-buttons {
        gap: 20px;
        margin: 30px auto 15px;
    }

    .nav-button {
        padding: 8px 20px;
        font-size: 14px;
    }
}

@media (max-width: 1200px) {
    .left-menu {
        left: calc(50% - 400px) !important;
    }
}

@media (max-width: 840px) {
    .left-menu {
        left: calc(50% - 350px) !important;
        width: 60px;
    }
}

@media (max-width: 600px) {
    .left-menu {
        flex-direction: row;
        bottom: 20px;
        top: auto;
        left: 50% !important;
        transform: translateX(-50%);
        width: auto;
        background: rgba(255, 255, 255, 0.9);
        padding: 30px;
        border-radius: 30px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        align-items: center;
    }
}
</style>