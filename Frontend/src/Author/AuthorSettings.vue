<template>
  <!-- 账号设置页面容器 -->
  <div class="setting-page">
    <h2>账号设置</h2>
    <!-- 设置表单 -->
    <form class="settings-form">
      <!-- 基本信息部分 -->
      <div class="form-section">
        <h3>基本信息</h3>
        <div class="form-group">
          <div class="form-row">
            <label>头像</label>
            <div class="avatar-upload">
              <div class="avatar-preview">
                <!-- 显示头像图片或占位符 -->
                <img :src="author.currentAuthor.avatar_url" class="avatar-img" v-if="author.currentAuthor.avatar_url">
                <div class="avatar-placeholder" v-else></div>
              </div>
              <!-- 上传按钮 -->
              <div class="upload-controls">
                <button 
                  type="button" 
                  class="upload-btn" 
                  @click="triggerFileInput"
                  :disabled="!author.editMode"
                  :class="{ 'disabled-btn': !author.editMode }"
                >
                  更换头像
                </button>
                <!-- 文件输入 -->
                <input type="file" accept="image/*" ref="fileInput" @change="handleAvatarChange">
              </div>
            </div>
          </div>
        </div>
        
        <!-- 作者名称输入 -->
        <div class="form-group">
          <div class="form-row">
            <label>作者名称</label>
            <div class="input-wrapper">
              <input 
                type="text" 
                v-model="author.currentAuthor.author_name" 
                :disabled="!author.editMode"
                :class="{ 'disabled-input': !author.editMode }"
              >
            </div>
          </div>
        </div>

        <!-- 作者简介输入 -->
        <div class="form-group">
          <div class="form-row">
            <label>作者简介</label>
            <div class="input-wrapper">
              <textarea 
                v-model="author.currentAuthor.introduction" 
                :disabled="!author.editMode"
                :class="{ 'disabled-input': !author.editMode }"
                rows="3"  
              ></textarea>
            </div>
          </div>
        </div>

        <!-- 作者ID显示 -->
        <div class="form-group">
          <div class="form-row">
            <label>作者ID</label>
            <div class="input-wrapper">
              <input 
                type="text" 
                v-model="author.currentAuthor.author_id" 
                disabled
                class="disabled-input"
              >
            </div>
          </div>
        </div>

        <!-- 注册时间显示 -->
         <div class="form-group">
          <div class="form-row">
            <label>注册时间</label>
            <div class="input-wrapper">
              <input 
                type="text" 
                v-model="author.currentAuthor.registertime" 
                disabled
                class="disabled-input"
              >
            </div>
          </div>
        </div>

      </div>
      
      <!-- 安全设置部分 -->
      <div class="form-section">
        <h3>安全设置</h3>
        
        <!-- 手机号码输入 -->
        <div class="form-group">
          <div class="form-row">
            <label>手机号码</label>
            <div class="input-wrapper">
              <input 
                type="tel" 
                v-model="author.currentAuthor.phone" 
                :disabled="!author.editMode"
                :class="{ 'disabled-input': !author.editMode }"
              >
              <span class="error-message" v-if="author.phoneError">
                {{ author.phoneError }}
              </span>
            </div>
          </div>
        </div>
        
        <!-- 修改密码按钮 -->
        <div class="form-group">
          <div class="form-row">
            <label>修改密码</label>
            <div class="input-wrapper">
              <button 
                type="button" 
                class="change-password-btn"
                @click="author.showPasswordDialog"
              >
                修改密码
              </button>
            </div>
          </div>
        </div>
      </div>
      
      <!-- 表单操作按钮 -->
      <div class="form-actions">
        <!-- 非编辑模式 -->
        <template v-if="!author.editMode">
          <button type="button" class="edit-btn" @click="author.enterEditMode">
            修改信息
          </button>
        </template>
        <!-- 编辑模式 -->
        <template v-if="author.editMode">
          <button type="button" class="cancel-btn" @click="author.cancelChanges">
            取消
          </button>
          <button type="submit" class="submit-btn" @click.prevent="author.saveSettings">
            保存设置
          </button>
        </template>
      </div>
      
      <!-- 注销区域 -->
      <div class="danger-zone">
        <h3>危险操作</h3>
        <div class="danger-content">
          <p>注销账号后将无法恢复，所有数据将被永久删除</p>
          <button 
            type="button" 
            class="delete-btn"
            @click="author.showConfirmDialog"
          >
            注销账号
          </button>
        </div>
      </div>
    </form>
    
    <!-- 注销确认对话框 -->
    <div class="confirm-dialog" v-if="author.showDialog">
      <div class="dialog-content">
        <h3>确认注销账号？</h3>
        <p>此操作不可逆，您将永久失去所有数据</p>
        <div class="dialog-actions">
          <button class="dialog-cancel" @click="author.hideConfirmDialog">取消</button>
          <button class="dialog-confirm" @click="deleteAccount">确认注销</button>
        </div>
      </div>
    </div>
    
    <!-- 修改密码对话框 -->
    <div class="password-dialog" v-if="author.showPasswordDialogFlag">
      <div class="dialog-content">
        <h3>修改密码</h3>
        <!-- 密码修改表单 -->
        <div class="password-form">
          <div class="form-group">
            <label>原密码</label>
            <input 
              type="password" 
              v-model="author.oldPassword" 
              placeholder="请输入原密码"
              :class="{ 'error-input': author.passwordError.oldPassword }"
            >
            <span class="error-message" v-if="author.passwordError.oldPassword">
              {{ author.passwordError.oldPassword }}
            </span>
          </div>
          
          <!-- 新密码输入 -->
          <div class="form-group">
            <label>新密码</label>
            <input 
              type="password" 
              v-model="author.newPassword" 
              placeholder="请输入新密码"
              :class="{ 'error-input': author.passwordError.newPassword }"
            >
            <span class="error-message" v-if="author.passwordError.newPassword">
              {{ author.passwordError.newPassword }}
            </span>
          </div>
          
          <!-- 确认新密码输入 -->
          <div class="form-group">
            <label>确认新密码</label>
            <input 
              type="password" 
              v-model="author.confirmPassword" 
              placeholder="请再次输入新密码"
              :class="{ 'error-input': author.passwordError.confirmPassword }"
            >
            <span class="error-message" v-if="author.passwordError.confirmPassword">
              {{ author.passwordError.confirmPassword }}
            </span>
          </div>
          <span v-if="author.passwordChangeSuccess" class="success-message">
              密码修改成功！
          </span>
        </div>
        
        <!-- 操作按钮 -->
        <div class="dialog-actions">
          <button class="dialog-cancel" @click="author.hidePasswordDialog">取消</button>
          <button class="dialog-confirm" @click="author.submitPasswordChange">确认修改</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { authorStore as author } from '@/stores/CurrentAuthor'
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

const router = useRouter()
const fileInput = ref(null)

// 触发文件输入点击
const triggerFileInput = () => {
  author.triggerFileInput(fileInput)
}

// 处理头像变更
const handleAvatarChange = (e) => {
  author.handleAvatarChange(e)
}

// 删除账号操作
const deleteAccount = async () => {
  try {
    const confirmed = confirm('确定要永久删除账号吗？此操作不可撤销！');
    if (!confirmed) return;

    const success = await author.deleteAccount();
    
    if (success) {
      toast.success('账号已成功注销');
      router.push('/L_R/Login');
    } else {
      toast.error('注销失败：' + (author.error || '未知错误'));
    }
  } catch (error) {
    console.error('注销出错:', error);
    toast.error('注销过程中发生错误');
  } finally {
    author.hideConfirmDialog();
  }
}
</script>

<style scoped>
/* 页面容器样式 */
.setting-page {
  padding: 30px;
  max-width: 1000px;
  margin: 0 auto;
}

/* 页面标题样式 */
h2 {
  margin-bottom: 25px;
  color: #2c3e50;
  font-size: 24px;
}

/* 设置表单样式 */
.settings-form {
  background: white;
  padding: 30px;
  border-radius: 5px;
  box-shadow: 0 4px 15px rgba(0,0,0,0.08);
}

/* 表单区块样式 */
.form-section {
  margin-bottom: 30px;
  padding-bottom: 20px;
  border-bottom: 1px solid #f0f0f0;
}

/* 最后一个区块 */
.form-section:last-child {
  border-bottom: none;
}

/* 区块标题样式 */
.form-section h3 {
  margin-bottom: 20px;
  color: #2c3e50;
  font-size: 18px;
}

/* 表单行布局 */
.form-row {
  display: flex;
  align-items: center;
  margin-bottom: 15px;
}

/* 表单标签样式 */
.form-row label {
  min-width: 100px;
  font-weight: 600;
  color: #34495e;
}

/* 输入框容器 */
.input-wrapper {
  flex: 1;
}

/* 禁用状态输入框样式 */
.disabled-input {
  background: transparent;
  border: none;
  padding: 8px 0;
  width: 100%;
  color: #333;
  font-size: 14px;
}

/* 正常输入框样式 */
input:not(.disabled-input) {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 14px;
  transition: all 0.3s;
}

/* 输入框聚焦效果 */
input:not(.disabled-input):focus {
  border-color: #3498db;
  outline: none;
  box-shadow: 0 0 0 2px rgba(52, 152, 219, 0.2);
}

/* 头像上传区域样式 */
.avatar-upload {
  display: flex;
  align-items: center;
  gap: 20px;
}

/* 头像预览区域 */
.avatar-preview {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  background-color: #f0f0f0;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 2px solid #eee;
  overflow: hidden; /* 新增：确保图片不会超出容器 */
  position: relative; /* 为绝对定位的图片做准备 */
}

/* 头像图片样式 */
.avatar-img {
  width: 100%;
  height: 100%; 
  object-fit: cover; 
  object-position: center; 
}

/* 头像占位符 */
.avatar-placeholder {
  width: 40px;
  height: 40px;
  background-color: #7f8c8d;
  border-radius: 50%;
}

/* 上传按钮样式 */
.upload-btn {
  background: none;
  border: 1px solid #3498db;
  color: #3498db;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.3s;
}

.upload-btn:hover:not(:disabled) {
  background-color: #3498db;
  color: white;
}

/* 禁用按钮样式 */
.disabled-btn {
  border: 1px solid #ccc;
  color: #ccc;
  cursor: not-allowed;
}

/* 隐藏文件输入 */
.upload-controls input[type="file"] {
  display: none;
}

/* 修改密码按钮样式 */
.change-password-btn {
  background: none;
  border: 1px solid #3498db;
  color: #3498db;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.3s;
}

.change-password-btn:hover {
  background-color: #3498db;
  color: white;
}

/* 表单操作按钮区域 */
.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 15px;
  margin-top: 30px;
}

/* 编辑按钮样式 */
.edit-btn {
  padding: 10px 20px;
  background-color: #3498db;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.3s;
}

.edit-btn:hover {
  background-color: #2980b9;
}

/* 取消按钮样式 */
.cancel-btn {
  padding: 10px 20px;
  background-color: #f5f5f5;
  color: #7f8c8d;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.3s;
}

.cancel-btn:hover {
  background-color: #e0e0e0;
}

/* 提交按钮样式 */
.submit-btn {
  padding: 10px 20px;
  background-color: #2ecc71;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.3s;
}

.submit-btn:hover {
  background-color: #27ae60;
}

/* 简介区域 */
textarea {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 14px;
  min-height: 80px; 
  resize: vertical; 
  font-family: inherit; 
  line-height: 1.5; 
}

textarea:focus {
  border-color: #3498db;
  outline: none;
  box-shadow: 0 0 0 2px rgba(52, 152, 219, 0.2);
}

/* 禁用状态的 textarea */
textarea.disabled-input {
  background: transparent;
  border: none;
  padding: 8px 0;
  resize: none; 
}
/* 危险区域样式 */
.danger-zone {
  margin-top: 40px;
  padding: 20px;
  border-radius: 8px;
  background-color: #fff8f8;
  border: 1px solid #ffdddd;
}

.danger-zone h3 {
  color: #e74c3c;
  margin-bottom: 15px;
}

.danger-content {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.danger-content p {
  color: #7f8c8d;
  font-size: 14px;
}

/* 删除账号按钮样式 */
.delete-btn {
  align-self: flex-start;
  padding: 10px 20px;
  background-color: #fff;
  border: 1px solid #e74c3c;
  color: #e74c3c;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.3s;
}

.delete-btn:hover {
  background-color: #e74c3c;
  color: white;
}

/* 对话框基础样式 */
.confirm-dialog,
.password-dialog {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0,0,0,0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

/* 对话框内容区域 */
.dialog-content {
  background-color: white;
  padding: 25px;
  border-radius: 10px;
  width: 400px;
  max-width: 90%;
  box-shadow: 0 5px 20px rgba(0,0,0,0.2);
}

/* 对话框标题 */
.dialog-content h3 {
  margin-bottom: 15px;
  color: #2c3e50;
}

/* 对话框文字说明 */
.dialog-content p {
  margin-bottom: 25px;
  color: #7f8c8d;
}

/* 对话框操作按钮区域 */
.dialog-actions {
  display: flex;
  justify-content: flex-end;
  gap: 15px;
}

/* 对话框取消按钮 */
.dialog-cancel,
.dialog-confirm {
  padding: 8px 20px;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.3s;
}

/* 对话框取消按钮样式 */
.dialog-cancel {
  background-color: #f5f5f5;
  color: #7f8c8d;
  border: none;
}

.dialog-cancel:hover {
  background-color: #e0e0e0;
}

/* 对话框确认按钮样式 */
.dialog-confirm {
  background-color: #e74c3c;
  color: white;
  border: none;
}

.dialog-confirm:hover {
  background-color: #c0392b;
}

/* 密码表单样式 */
.password-form {
  margin-bottom: 20px;
}

/* 密码表单组样式 */
.password-form .form-group {
  margin-bottom: 15px;
}

/* 密码表单标签样式 */
.password-form label {
  display: block;
  margin-bottom: 8px;
  font-weight: 500;
  color: #34495e;
}

/* 密码输入框样式 */
.password-form input {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 14px;
}

/* 密码输入框聚焦效果 */
.password-form input:focus {
  border-color: #3498db;
  outline: none;
  box-shadow: 0 0 0 2px rgba(52, 152, 219, 0.2);
}

/* 错误输入框样式 */
.error-input {
  border-color: #e74c3c !important;
}

/* 错误提示信息样式 */
.error-message {
  color: #e74c3c;
  font-size: 13px;
  margin-top: 5px;
  display: block;
}

/* 成功提示样式 */
.success-message {
  padding: 15px;
  background: #f6ffed;
  border: 1px solid #b7eb8f;
  border-radius: 4px;
  color: #52c41a;
  text-align: center;
  margin-bottom: 20px;
  display: block;
}

</style>