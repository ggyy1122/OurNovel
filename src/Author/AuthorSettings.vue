<template>
  <!-- 账号设置页面容器 -->
  <div class="page-container">
    <!-- 页面标题 -->
    <h2>账号设置</h2>
    
    <!-- 设置表单 -->
    <form class="settings-form">
      <!-- 基本信息部分 -->
      <div class="form-section">
        <h3>基本信息</h3>
        
        <!-- 头像上传区域 -->
        <div class="form-group">
          <div class="form-row">
            <label>头像</label>
            <div class="avatar-upload">
              <!-- 头像预览 -->
              <div class="avatar-preview">
                <!-- 显示头像图片或占位符 -->
                <img :src="author.currentAuthor.avatar_url" class="avatar-img" v-if="author.currentAuthor.avatar_url">
                <div class="avatar-placeholder" v-else></div>
              </div>
              <!-- 上传控制按钮 -->
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
                <!-- 隐藏的文件输入 -->
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
        <!-- 非编辑模式下的编辑按钮 -->
        <template v-if="!author.editMode">
          <button type="button" class="edit-btn" @click="author.enterEditMode">
            修改信息
          </button>
        </template>
        <!-- 编辑模式下的取消和保存按钮 -->
        <template v-if="author.editMode">
          <button type="button" class="cancel-btn" @click="author.cancelChanges">
            取消
          </button>
          <button type="submit" class="submit-btn" @click.prevent="author.saveSettings">
            保存设置
          </button>
        </template>
      </div>
      
      <!-- 危险操作区域 -->
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
          <!-- 原密码输入 -->
          <div class="form-group">
            <label>原密码</label>
            <input 
              type="password" 
              v-model="author.oldPassword" 
              placeholder="请输入原密码"
              :class="{ 'error-input': author.passwordError.oldPassword }"
            >
            <!-- 错误提示 -->
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
            <!-- 错误提示 -->
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
            <!-- 错误提示 -->
            <span class="error-message" v-if="author.passwordError.confirmPassword">
              {{ author.passwordError.confirmPassword }}
            </span>
          </div>
        </div>
        
        <!-- 对话框操作按钮 -->
        <div class="dialog-actions">
          <button class="dialog-cancel" @click="author.hidePasswordDialog">取消</button>
          <button class="dialog-confirm" @click="author.submitPasswordChange">确认修改</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
// 导入Vue相关功能
import { ref } from 'vue'
import { useRouter } from 'vue-router'
// 导入作者状态管理
import { authorStore as author } from '@/stores/CurrentAuthor'

// 初始化路由和文件输入引用
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
const deleteAccount = () => {
  // 这里可以添加注销账号的API调用
  alert('账号已注销')
  author.hideConfirmDialog()
  router.push('/L_R/Login')
}
</script>

<style scoped>
/* 页面容器样式 */
.page-container {
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

/* 最后一个区块不需要下边框 */
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
  padding: 8px 15px;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.3s;
}

/* 上传按钮悬停效果 */
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
  padding: 8px 15px;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.3s;
}

/* 修改密码按钮悬停效果 */
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

/* 编辑按钮悬停效果 */
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

/* 取消按钮悬停效果 */
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

/* 提交按钮悬停效果 */
.submit-btn:hover {
  background-color: #27ae60;
}

/* 危险区域样式 */
.danger-zone {
  margin-top: 40px;
  padding: 20px;
  border-radius: 8px;
  background-color: #fff8f8;
  border: 1px solid #ffdddd;
}

/* 危险区域标题 */
.danger-zone h3 {
  color: #e74c3c;
  margin-bottom: 15px;
}

/* 危险区域内容布局 */
.danger-content {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

/* 危险区域文字说明 */
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

/* 删除账号按钮悬停效果 */
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

/* 对话框取消按钮悬停效果 */
.dialog-cancel:hover {
  background-color: #e0e0e0;
}

/* 对话框确认按钮样式 */
.dialog-confirm {
  background-color: #e74c3c;
  color: white;
  border: none;
}

/* 对话框确认按钮悬停效果 */
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
</style>