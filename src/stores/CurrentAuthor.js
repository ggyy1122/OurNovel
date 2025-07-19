// src/stores/CurrentAuthor.js
import { reactive } from 'vue'

export const authorStore = reactive({
  // 作者数据
  currentAuthor: {
    author_id: 'AUTH20230001',
    author_name: '张三',
    phone: '13800138000',
    password: '666',
    earnings: 666,
    avatar_url: 'https://placeholder.com/150'
  },

  // 状态数据
  editMode: false,
  showDialog: false,
  showPasswordDialogFlag: false,
  originalData: {},

  // 表单数据
  oldPassword: '',
  newPassword: '',
  confirmPassword: '',
  passwordError: {
    oldPassword: '',
    newPassword: '',
    confirmPassword: ''
  },

  // 方法
  enterEditMode() {
    this.originalData = {
      ...this.currentAuthor,
      newPassword: this.newPassword,
      confirmPassword: this.confirmPassword
    }
    this.editMode = true
  },

  triggerFileInput(fileInputRef) {
    fileInputRef.value.click()
  },

  handleAvatarChange(e) {
    const file = e.target.files[0]
    if (file) {
      const reader = new FileReader()
      reader.onload = (event) => {
        this.currentAuthor.avatar_url = event.target.result
      }
      reader.readAsDataURL(file)
    }
  },

  saveSettings() {
    if (this.newPassword && this.newPassword !== this.confirmPassword) {
      alert('两次输入的密码不一致')
      return
    }
    
    if (this.newPassword) {
      this.currentAuthor.password = this.newPassword
    }
    
    alert('设置已保存')
    this.editMode = false
  },

  cancelChanges() {
    this.currentAuthor = { ...this.originalData }
    this.newPassword = this.originalData.newPassword
    this.confirmPassword = this.originalData.confirmPassword
    this.editMode = false
  },

  showConfirmDialog() {
    this.showDialog = true
  },

  hideConfirmDialog() {
    this.showDialog = false
  },

  showPasswordDialog() {
    this.showPasswordDialogFlag = true
    this.clearPasswordFields()
  },

  hidePasswordDialog() {
    this.showPasswordDialogFlag = false
    this.clearPasswordFields()
  },

  clearPasswordFields() {
    this.oldPassword = ''
    this.newPassword = ''
    this.confirmPassword = ''
    this.passwordError = {
      oldPassword: '',
      newPassword: '',
      confirmPassword: ''
    }
  },

  validatePasswordForm() {
    let isValid = true
    this.passwordError = {
      oldPassword: '',
      newPassword: '',
      confirmPassword: ''
    }
    
    if (!this.oldPassword) {
      this.passwordError.oldPassword = '请输入原密码'
      isValid = false
    } else if (this.oldPassword !== this.currentAuthor.password) {
      this.passwordError.oldPassword = '原密码不正确'
      isValid = false
    }
    
    if (!this.newPassword) {
      this.passwordError.newPassword = '请输入新密码'
      isValid = false
    } else if (this.newPassword.length < 6) {
      this.passwordError.newPassword = '密码长度不能少于6位'
      isValid = false
    }
    
    if (!this.confirmPassword) {
      this.passwordError.confirmPassword = '请确认新密码'
      isValid = false
    } else if (this.newPassword !== this.confirmPassword) {
      this.passwordError.confirmPassword = '两次输入的密码不一致'
      isValid = false
    }
    
    return isValid
  },

  submitPasswordChange() {
    if (!this.validatePasswordForm()) {
      return
    }
    
    this.currentAuthor.password = this.newPassword
    alert('密码修改成功')
    this.hidePasswordDialog()
  }
})