// src/stores/CurrentAuthor.js
import { reactive } from 'vue'
import { getAuthor, updateAuthor,deleteAuthor } from '@/API/Author_API'
import { uploadAuthorAvatar } from '@/API/Author_Avatar_API'
import { current_state } from './index'
import { changeAuthorPassword } from '@/API/Log_API'

export const authorStore = reactive({
  // 作者数据
  currentAuthor: {
    author_id: '',
    author_name: '加载中...',
    avatar_url: '', 
    introduction: '',
    phone: '',
    password: '',
    earnings: 0,
    registertime: null
  },
  // 状态数据
  phoneError: '',
  isLoading: false,
  error: null,
  editMode: false,
  showDialog: false,
  showPasswordDialogFlag: false,
  passwordChangeSuccess: false,
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
  
  async fetchAuthorData() {
    this.isLoading = true
    this.error = null
  
    try {
      const authorId = current_state().id
      const response = await getAuthor(authorId)
      
      // 根据API响应映射字段
      this.currentAuthor = {
        author_id: response.authorId || authorId,
        author_name: response.authorName || '未命名作者',
        phone: response.phone || '',
        introduction: response.introduction || '',
        password: this.currentAuthor?.password || response.password || '',
        earnings: response.earning || 0,
        avatar_url: this.getFullAvatarUrl(response.avatarUrl)  ,
        registertime: response.registerTime || null
      }
      console.log(this.currentAuthor)
    } catch (error) {
      this.error = error
      console.error('获取作者信息失败:', error)
      
      // 失败时回退
      this.currentAuthor = {
        author_id: current_state().id,
        author_name: '加载失败',
        phone: '',
        password: this.currentAuthor?.password || '',
        earnings: 0,
        avatar_url: require('@/assets/default-avatar.jpeg')
      }
    } finally {
      this.isLoading = false
    }
  },

  getFullAvatarUrl(partialPath) {
    if (!partialPath) return require('@/assets/default-avatar.jpeg');
    
    if (partialPath.startsWith('http')) return partialPath;
    
    const ossBase = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com';
    return `${ossBase}/${partialPath.replace(/^\//, '')}`;
  },

  enterEditMode() {
    if (!this.currentAuthor) return
    
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

  validatePhone() {
    const phone = this.currentAuthor.phone?.trim();
    
    this.phoneError = '';
  
    // 如果未输入，直接通过验证
    if (!phone) {
      return true;
    }
    
    if (!/^1[3-9]\d{9}$/.test(phone)) {
      this.phoneError = '请输入正确的11位手机号码';
      return false;
    }
    
    this.phoneError = '';
    return true;
  },

  async handleAvatarChange(e) {
    const file = e.target.files[0];
    if (!file) return;

    try {
      this.isLoading = true;
      
      const reader = new FileReader();
      reader.onload = (event) => {
        this.currentAuthor.avatar_url = event.target.result;
      };
      reader.readAsDataURL(file);

      const newUrl = await uploadAuthorAvatar(this.currentAuthor.author_id, file);
      this.currentAuthor.avatar_url = this.getFullAvatarUrl(newUrl);
      
    } catch (error) {
      console.error('上传失败:', error);
      this.error = error.response?.data?.message || '头像更新失败';
    } finally {
      this.isLoading = false;
      e.target.value = '';
    }
  },

  async saveSettings() {
    if (!this.currentAuthor) return

    if (!this.validatePhone()) {
      return false;
    }
    let avatarUrl = this.currentAuthor.avatar_url;
    const ossBase = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/';
    if (avatarUrl.startsWith(ossBase)) {
      avatarUrl = avatarUrl.substring(ossBase.length);
    }
    try {
      this.isLoading = true;
    
      const updateData = {
      authorId: this.currentAuthor.author_id,
      authorName: this.currentAuthor.author_name,
      phone: this.currentAuthor.phone,
      introduction: this.currentAuthor.introduction,
      password: this.newPassword || this.currentAuthor.password, 
      avatarUrl: avatarUrl,
      earnings: this.currentAuthor.earnings,
      registerTime: this.currentAuthor.registertime
    };
      
      // API更新
      await updateAuthor(this.currentAuthor.author_id, updateData);
      this.editMode = false;
      this.clearPasswordFields();
      
      // 重新获取最新数据
      await this.fetchAuthorData();
    } catch (error) {
      console.error('保存失败:', error);
      this.error = error.response?.data?.message || '保存失败';
    } finally {
      this.isLoading = false;
    }
  },

// 在authorStore中添加删除方法
  async deleteAccount() {
    try {
      this.isLoading = true;
      
      // 调用API删除作者
      await deleteAuthor(this.currentAuthor.author_id);
      
      // 清除当前作者数据
      this.currentAuthor = {
        author_id: '',
        author_name: '',
        avatar_url: require('@/assets/default-avatar.jpeg'),
        introduction: '',
        phone: '',
        password: '',
        earnings: 0,
        registertime: null
      };
      
      // 重置状态
      this.editMode = false;
      this.showDialog = false;
      return true; 
    } catch (error) {
      console.error('删除账号失败:', error);
      this.error = error.response?.data?.message || '删除账号失败';
      return false;
    } finally {
      this.isLoading = false;
    }
  },

  cancelChanges() {
    if (!this.currentAuthor || !this.originalData) return
    
    this.currentAuthor = { ...this.originalData }
    this.newPassword = this.originalData.newPassword || ''
    this.confirmPassword = this.originalData.confirmPassword || ''
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
    this.showPasswordDialogFlag = false;
    this.passwordChangeSuccess = false;
    this.clearPasswordFields();
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
    if (!this.currentAuthor) return false
    
    let isValid = true
    this.passwordError = {
      oldPassword: '',
      newPassword: '',
      confirmPassword: ''
    }
    
    if (!this.oldPassword) {
      this.passwordError.oldPassword = '请输入原密码'
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

  async submitPasswordChange() {
    if (!this.currentAuthor || !this.validatePasswordForm()) return
    
    try {
      this.isLoading = true;
      
      // 调用API更新密码
      await changeAuthorPassword({
        authorName:this.currentAuthor.author_name,
        oldPassword:this.oldPassword,
        newPassword:this.newPassword,
      });
      this.passwordChangeSuccess = true;
      setTimeout(() => {
        
        this.hidePasswordDialog();
      }, 1000);
      
      // 更新本地存储的密码
      this.currentAuthor.password = this.newPassword;
    } catch (error) {
      console.error('密码修改失败:', error);
      this.passwordError.oldPassword = '原密码不正确';
      this.oldPassword = '';
    } finally {
      this.isLoading = false;
    }
  }
  
})