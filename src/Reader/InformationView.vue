<template>
  <div class="edit-container">
    <h2>修改个人信息</h2>
    <div class="avatar-upload">
      <img
        :src="avatarPreviewUrl.startsWith('blob:') ? avatarPreviewUrl : 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' + avatarPreviewUrl"
        alt="头像预览" class="avatar-image" />
      <label class="upload-button">
        选择头像
        <input type="file" accept="image/*" @change="handleAvatarChange" hidden />
      </label>
    </div>
    <form @submit.prevent="saveReaderChanges">
      <div>
        <label>姓名：</label>
        <input v-model="readerName" type="text" required />
      </div>

      <div>
        <label>电话：</label>
        <div class="phone-input-container">
          <input v-model="phone" type="text" maxlength="11" @input="validatePhone"
            :class="{ 'input-error': phoneError }" />
          <span class="char-counter">{{ phone.length }}/11</span>
        </div>
        <div v-if="phoneError" class="error-message">请输入11位有效的手机号码</div>
      </div>

      <div>
        <label>性别：</label>
        <select v-model="gender">
          <option value="男">男</option>
          <option value="女">女</option>
        </select>
      </div>

      <button type="submit" :disabled="phoneError">保存修改</button>
    </form>

    <!-- 账号注销区域 -->
    <div class="account-deletion">
      <h3>账号管理</h3>
      <p>永久删除您的账号及相关所有数据。此操作不可撤销，请谨慎操作。</p>
      <button class="delete-account-btn" @click="confirmAccountDeletion">
        注销账号
      </button>
    </div>

    <!-- 确认对话框 -->
    <div v-if="showDeleteConfirm" class="modal-overlay">
      <div class="confirmation-dialog">
        <h3>确认注销账号</h3>
        <p>您确定要永久删除您的账号吗？此操作将：</p>
        <ul>
          <li>删除所有个人数据</li>
          <li>清除阅读记录和收藏</li>
          <li>无法恢复账号和相关信息</li>
        </ul>
        <div class="dialog-actions">
          <button class="cancel-btn" @click="showDeleteConfirm = false">取消</button>
          <button class="confirm-delete-btn" @click="deleteAccount">确认注销</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { readerState } from '@/stores/index'
import { updateReader, uploadReaderAvatar, deleteReader } from '@/API/Reader_API'
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'
import { useRouter } from 'vue-router'

const store = readerState()
const router = useRouter()

const readerName = ref('')
const phone = ref('')
const gender = ref('男')
const avatarFile = ref(null)
const avatarPreviewUrl = ref('')
const apiResponseAvatar = ref(null)
const defaultAvatar = 'e165315c-da2b-42c9-b3cf-c0457d168634.jpg'
const showDeleteConfirm = ref(false)

// 计算属性验证电话号码
const phoneError = computed(() => {
  return phone.value && phone.value.length !== 11
})

function getFormattedAvatarUrl(avatarUrl) {
  if (!avatarUrl || avatarUrl.trim() === '') {
    return defaultAvatar
  }
  if (avatarUrl.startsWith('http://') || avatarUrl.startsWith('https://')) {
    return avatarUrl
  }
  return avatarUrl
}

// 验证电话号码输入
function validatePhone(event) {
  // 只允许输入数字
  phone.value = event.target.value.replace(/\D/g, '')
}

function handleAvatarChange(event) {
  avatarFile.value = event.target.files[0]
  if (avatarFile.value) {
    avatarPreviewUrl.value = URL.createObjectURL(avatarFile.value)
  } else {
    avatarPreviewUrl.value = ''
  }
}

async function uploadAvatar() {
  try {
    if (!avatarFile.value) throw new Error('请选择头像文件')
    const response = await uploadReaderAvatar(store.readerId, avatarFile.value)
    apiResponseAvatar.value = response
    if (response.success && response.avatarUrl) {
      const url = response.avatarUrl
      const fileName = url.split('/').pop()
      store.avatarUrl = fileName
      avatarPreviewUrl.value = url
      toast.success('头像上传成功')
    } else {
      toast.error('头像上传失败，接口返回异常')
    }
  } catch (error) {
    toast.error('上传头像失败')
  }
}

onMounted(() => {
  readerName.value = store.readerName || ''
  phone.value = store.phone || ''
  gender.value = store.gender || '男'
  avatarPreviewUrl.value = getFormattedAvatarUrl(store.avatarUrl)
})

async function saveReaderChanges() {
  if (!store.readerId) {
    toast.error('未检测到登录用户，请重新登录')
    return
  }
  if (!store.password) {
    toast.error('未检测到用户密码，无法修改')
    return
  }

  // 验证电话号码是否为11位
  if (phone.value && phone.value.length !== 11) {
    toast.error('电话号码必须是11位数字')
    return
  }

  if (avatarFile.value) {
    await uploadAvatar()
  }

  const updateData = {
    readerId: store.readerId,
    readerName: readerName.value,
    password: store.password,
    phone: phone.value,
    gender: gender.value,
    balance: store.balance,
    avatarUrl: store.avatarUrl,
    backgroundUrl: store.backgroundUrl,
  }

  try {
    await updateReader(store.readerId, updateData)
    toast.success('修改成功')
    Object.assign(store, updateData)
    avatarPreviewUrl.value = getFormattedAvatarUrl(store.avatarUrl)
  } catch (error) {
    toast.error('修改失败，请稍后再试')
  }
}

// 账号注销相关功能
function confirmAccountDeletion() {
  showDeleteConfirm.value = true
}

async function deleteAccount() {
  try {
    await deleteReader(store.readerId)
    toast.success('账号已成功注销')
    // 清除用户状态
    store.resetReaderInfo()
    //停顿1.5s
    await new Promise(resolve => setTimeout(resolve, 1500));
    // 跳转到登录页面或首页
    router.push('/L_R/Login')
  } catch (error) {
    toast.error('注销账号失败，请稍后再试')
    console.error('注销账号错误:', error)
  } finally {
    showDeleteConfirm.value = false
  }
}
</script>

<style scoped>
.edit-container {
  max-width: 500px;
  margin: 0 auto;
  padding: 20px;
}

h2 {
  text-align: center;
  color: #ed424b;
  margin-bottom: 30px;
}

form div {
  margin-bottom: 20px;
}

label {
  display: block;
  font-weight: 600;
  margin-bottom: 8px;
  color: #555;
}

.avatar-upload {
  margin-bottom: 20px;
  text-align: left;
}

.avatar-image {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  object-fit: cover;
  margin-bottom: 10px;
  border: 2px solid #ed424b;
  display: block;
}

.upload-button {
  display: block;
  width: fit-content;
  padding: 8px 16px;
  background-color: #ed424b;
  color: white;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 500;
  font-size: 18px;
  transition: background-color 0.3s, transform 0.1s;
  user-select: none;
}

.upload-button:hover {
  background-color: #f05e74;
}

.upload-button:active {
  transform: scale(0.98);
}

/* 电话输入容器样式 */
.phone-input-container {
  position: relative;
  display: inline-block;
  width: 60%;
}

.phone-input-container input {
  width: 100%;
  padding-right: 60px;
  /* 为计数器留出空间 */
}

input[type="text"],
select {
  padding: 10px 12px;
  border: 1px solid #ddd;
  border-radius: 8px;
  font-size: 1em;
  transition: border-color 0.3s, box-shadow 0.3s;
}

input[type="text"]:focus,
select:focus {
  border-color: #ed424b;
  box-shadow: 0 0 5px rgba(247, 142, 72, 0.3);
  outline: none;
}

.input-error {
  border-color: #ff4d4f;
  box-shadow: 0 0 5px rgba(255, 77, 79, 0.3);
}

.error-message {
  color: #ff4d4f;
  font-size: 0.85em;
  margin-top: 5px;
}

/* 字符计数器样式 */
.char-counter {
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
  font-size: 0.85em;
  color: #888;
  background: white;
  padding: 2px 6px;
  border-radius: 4px;
  font-weight: bold;
}

input[type="checkbox"] {
  transform: scale(1.2);
  accent-color: #ed424b;
  cursor: pointer;
}

button[type="submit"] {
  width: 100%;
  padding: 12px 0;
  background-color: #ed424b;
  border: none;
  border-radius: 8px;
  color: white;
  font-weight: 500;
  font-size: 18px;
  cursor: pointer;
  transition: background-color 0.3s, transform 0.1s;
  margin-bottom: 30px;
}

button[type="submit"]:hover:not(:disabled) {
  background-color: #f05e74;
}

button[type="submit"]:active:not(:disabled) {
  transform: scale(0.98);
}

button[type="submit"]:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

/* 账号注销区域样式 */
.account-deletion {
  margin-top: 40px;
  padding: 20px;
  border-top: 1px solid #eaeaea;
  text-align: center;
}

.account-deletion h3 {
  color: #ff4d4f;
  margin-bottom: 15px;
}

.account-deletion p {
  color: #666;
  margin-bottom: 20px;
  font-size: 0.9em;
  line-height: 1.5;
}

.delete-account-btn {
  padding: 12px 24px;
  background-color: #ff4d4f;
  color: white;
  border: none;
  border-radius: 8px;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.3s;
}

.delete-account-btn:hover {
  background-color: #d9363e;
}

/* 确认对话框样式 */
.modal-overlay {
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

.confirmation-dialog {
  background: white;
  padding: 25px;
  border-radius: 12px;
  max-width: 450px;
  width: 90%;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
}

.confirmation-dialog h3 {
  color: #ff4d4f;
  margin-bottom: 15px;
}

.confirmation-dialog p {
  margin-bottom: 15px;
  color: #333;
}

.confirmation-dialog ul {
  margin-bottom: 20px;
  padding-left: 20px;
  color: #666;
}

.confirmation-dialog li {
  margin-bottom: 8px;
}

.dialog-actions {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
}

.cancel-btn {
  padding: 10px 20px;
  background-color: #f0f0f0;
  color: #333;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.cancel-btn:hover {
  background-color: #e0e0e0;
}

.confirm-delete-btn {
  padding: 10px 20px;
  background-color: #ff4d4f;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.confirm-delete-btn:hover {
  background-color: #d9363e;
}
</style>