<template>
  <div class="edit-container">
    <h2>修改个人信息</h2>
    <div class="upload-row">
      <div class="avatar-upload">
        <img
          :src="avatarPreviewUrl.startsWith('blob:') ? avatarPreviewUrl : 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' + avatarPreviewUrl"
          alt="头像预览" class="avatar-image" />
        <label class="upload-button">
          选择头像
          <input type="file" accept="image/*" @change="handleAvatarChange" hidden />
        </label>
      </div>
      <!-- 背景上传区域 -->
      <div class="background-upload">
        <img
          :src="backgroundPreviewUrl.startsWith('blob:') ? backgroundPreviewUrl : 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' + backgroundPreviewUrl"
          alt="背景预览" class="background-image" />
        <label class="upload-button">
          选择背景
          <input type="file" accept="image/*" @change="handleBackgroundChange" hidden />
        </label>
      </div>
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
        <div v-if="phoneError" class="error-message1">请输入11位有效的手机号码</div>
      </div>

      <div>
        <label>性别：</label>
        <select v-model="gender">
          <option value="男">男</option>
          <option value="女">女</option>
        </select>
      </div>

      <div>
        <label>收藏是否可见：</label>
        <select v-model="isCollectVisible">
          <option value="是">是</option>
          <option value="否">否</option>
        </select>
      </div>

      <div>
        <label>推荐是否可见：</label>
        <select v-model="isRecommendVisible">
          <option value="是">是</option>
          <option value="否">否</option>
        </select>
      </div>

      <button type="submit" :disabled="phoneError">保存修改</button>
    </form>

    <!-- 修改密码区域 -->
    <div class="password-change-section">
      <h3>修改密码</h3>
      <button class="change-password-btn" @click="showPasswordModal = true">
        修改密码
      </button>
    </div>
    <!-- 修改密码模态框 -->
    <div v-if="showPasswordModal" class="modal-overlay">
      <div class="password-modal">
        <h3>修改密码</h3>
        <form @submit.prevent="changePassword">
          <div class="input-group">
            <input :type="showOldPassword ? 'text' : 'password'" v-model="oldPassword" placeholder="请输入原密码"
              class="modal-input" />
            <span class="input-eye" @click="showOldPassword = !showOldPassword">
              <svg v-if="!showOldPassword" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20"
                height="20">
                <path fill="currentColor"
                  d="M512 160c320 0 512 352 512 352S832 864 512 864 0 512 0 512s192-352 512-352m0 64c-225.28 0-384.128 208.064-436.8 288 52.608 79.872 211.456 288 436.8 288 225.28 0 384.128-208.064 436.8-288-52.608-79.872-211.456-288-436.8-288zm0 64a224 224 0 1 1 0 448 224 224 0 0 1 0-448m0 64a160.192 160.192 0 0 0-160 160c0 88.192 71.744 160 160 160s160-71.808 160-160-71.744-160-160-160">
                </path>
              </svg>
              <svg v-else xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20" height="20">
                <path fill="currentColor"
                  d="M876.8 156.8c0-9.6-3.2-16-9.6-22.4-6.4-6.4-12.8-9.6-22.4-9.6-9.6 0-16 3.2-22.4 9.6L736 220.8c-64-32-137.6-51.2-224-60.8-160 16-288 73.6-377.6 176C44.8 438.4 0 496 0 512s48 73.6 134.4 176c22.4 25.6 44.8 48 73.6 67.2l-86.4 89.6c-6.4 6.4-9.6 12.8-9.6 22.4 0 9.6 3.2 16 9.6 22.4 6.4 6.4 12.8 9.6 22.4 9.6 9.6 0 16-3.2 22.4-9.6l704-710.4c3.2-6.4 6.4-12.8 6.4-22.4Zm-646.4 528c-76.8-70.4-128-128-153.6-172.8 28.8-48 80-105.6 153.6-172.8C304 272 400 230.4 512 224c64 3.2 124.8 19.2 176 44.8l-54.4 54.4C598.4 300.8 560 288 512 288c-64 0-115.2 22.4-160 64s-64 96-64 160c0 48 12.8 89.6 35.2 124.8L256 707.2c-9.6-6.4-19.2-16-25.6-22.4Zm140.8-96c-12.8-22.4-19.2-48-19.2-76.8 0-44.8 16-83.2 48-112 32-28.8 67.2-48 112-48 28.8 0 54.4 6.4 73.6 19.2zM889.599 336c-12.8-16-28.8-28.8-41.6-41.6l-48 48c73.6 67.2 124.8 124.8 150.4 169.6-28.8 48-80 105.6-153.6 172.8-73.6 67.2-172.8 108.8-284.8 115.2-51.2-3.2-99.2-12.8-140.8-28.8l-48 48c57.6 22.4 118.4 38.4 188.8 44.8 160-16 288-73.6 377.6-176C979.199 585.6 1024 528 1024 512s-48.001-73.6-134.401-176Z">
                </path>
                <path fill="currentColor"
                  d="M511.998 672c-12.8 0-25.6-3.2-38.4-6.4l-51.2 51.2c28.8 12.8 57.6 19.2 89.6 19.2 64 0 115.2-22.4 160-64 41.6-41.6 64-96 64-160 0-32-6.4-64-19.2-89.6l-51.2 51.2c3.2 12.8 6.4 25.6 6.4 38.4 0 44.8-16 83.2-48 112-32 28.8-67.2 48-112 48Z">
                </path>
              </svg>
            </span>
          </div>
          <div v-if="oldPasswordError" class="error-message">原密码不正确</div>

          <div class="input-group">
            <input :type="showNewPassword ? 'text' : 'password'" v-model="newPassword" placeholder="请输入新密码"
              class="modal-input" />
            <span class="input-eye" @click="showNewPassword = !showNewPassword">
              <svg v-if="!showNewPassword" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20"
                height="20">
                <path fill="currentColor"
                  d="M512 160c320 0 512 352 512 352S832 864 512 864 0 512 0 512s192-352 512-352m0 64c-225.28 0-384.128 208.064-436.8 288 52.608 79.872 211.456 288 436.8 288 225.28 0 384.128-208.064 436.8-288-52.608-79.872-211.456-288-436.8-288zm0 64a224 224 0 1 1 0 448 224 224 0 0 1 0-448m0 64a160.192 160.192 0 0 0-160 160c0 88.192 71.744 160 160 160s160-71.808 160-160-71.744-160-160-160">
                </path>
              </svg>
              <svg v-else xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20" height="20">
                <path fill="currentColor"
                  d="M876.8 156.8c0-9.6-3.2-16-9.6-22.4-6.4-6.4-12.8-9.6-22.4-9.6-9.6 0-16 3.2-22.4 9.6L736 220.8c-64-32-137.6-51.2-224-60.8-160 16-288 73.6-377.6 176C44.8 438.4 0 496 0 512s48 73.6 134.4 176c22.4 25.6 44.8 48 73.6 67.2l-86.4 89.6c-6.4 6.4-9.6 12.8-9.6 22.4 0 9.6 3.2 16 9.6 22.4 6.4 6.4 12.8 9.6 22.4 9.6 9.6 0 16-3.2 22.4-9.6l704-710.4c3.2-6.4 6.4-12.8 6.4-22.4Zm-646.4 528c-76.8-70.4-128-128-153.6-172.8 28.8-48 80-105.6 153.6-172.8C304 272 400 230.4 512 224c64 3.2 124.8 19.2 176 44.8l-54.4 54.4C598.4 300.8 560 288 512 288c-64 0-115.2 22.4-160 64s-64 96-64 160c0 48 12.8 89.6 35.2 124.8L256 707.2c-9.6-6.4-19.2-16-25.6-22.4Zm140.8-96c-12.8-22.4-19.2-48-19.2-76.8 0-44.8 16-83.2 48-112 32-28.8 67.2-48 112-48 28.8 0 54.4 6.4 73.6 19.2zM889.599 336c-12.8-16-28.8-28.8-41.6-41.6l-48 48c73.6 67.2 124.8 124.8 150.4 169.6-28.8 48-80 105.6-153.6 172.8-73.6 67.2-172.8 108.8-284.8 115.2-51.2-3.2-99.2-12.8-140.8-28.8l-48 48c57.6 22.4 118.4 38.4 188.8 44.8 160-16 288-73.6 377.6-176C979.199 585.6 1024 528 1024 512s-48.001-73.6-134.401-176Z">
                </path>
                <path fill="currentColor"
                  d="M511.998 672c-12.8 0-25.6-3.2-38.4-6.4l-51.2 51.2c28.8 12.8 57.6 19.2 89.6 19.2 64 0 115.2-22.4 160-64 41.6-41.6 64-96 64-160 0-32-6.4-64-19.2-89.6l-51.2 51.2c3.2 12.8 6.4 25.6 6.4 38.4 0 44.8-16 83.2-48 112-32 28.8-67.2 48-112 48Z">
                </path>
              </svg>
            </span>
          </div>

          <div class="input-group">
            <input :type="showConfirmPassword ? 'text' : 'password'" v-model="confirmPassword" placeholder="请再次输入新密码"
              class="modal-input" />
            <span class="input-eye" @click="showConfirmPassword = !showConfirmPassword">
              <svg v-if="!showConfirmPassword" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20"
                height="20">
                <path fill="currentColor"
                  d="M512 160c320 0 512 352 512 352S832 864 512 864 0 512 0 512s192-352 512-352m0 64c-225.28 0-384.128 208.064-436.8 288 52.608 79.872 211.456 288 436.8 288 225.28 0 384.128-208.064 436.8-288-52.608-79.872-211.456-288-436.8-288zm0 64a224 224 0 1 1 0 448 224 224 0 0 1 0-448m0 64a160.192 160.192 0 0 0-160 160c0 88.192 71.744 160 160 160s160-71.808 160-160-71.744-160-160-160">
                </path>
              </svg>
              <svg v-else xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="20" height="20">
                <path fill="currentColor"
                  d="M876.8 156.8c0-9.6-3.2-16-9.6-22.4-6.4-6.4-12.8-9.6-22.4-9.6-9.6 0-16 3.2-22.4 9.6L736 220.8c-64-32-137.6-51.2-224-60.8-160 16-288 73.6-377.6 176C44.8 438.4 0 496 0 512s48 73.6 134.4 176c22.4 25.6 44.8 48 73.6 67.2l-86.4 89.6c-6.4 6.4-9.6 12.8-9.6 22.4 0 9.6 3.2 16 9.6 22.4 6.4 6.4 12.8 9.6 22.4 9.6 9.6 0 16-3.2 22.4-9.6l704-710.4c3.2-6.4 6.4-12.8 6.4-22.4Zm-646.4 528c-76.8-70.4-128-128-153.6-172.8 28.8-48 80-105.6 153.6-172.8C304 272 400 230.4 512 224c64 3.2 124.8 19.2 176 44.8l-54.4 54.4C598.4 300.8 560 288 512 288c-64 0-115.2 22.4-160 64s-64 96-64 160c0 48 12.8 89.6 35.2 124.8L256 707.2c-9.6-6.4-19.2-16-25.6-22.4Zm140.8-96c-12.8-22.4-19.2-48-19.2-76.8 0-44.8 16-83.2 48-112 32-28.8 67.2-48 112-48 28.8 0 54.4 6.4 73.6 19.2zM889.599 336c-12.8-16-28.8-28.8-41.6-41.6l-48 48c73.6 67.2 124.8 124.8 150.4 169.6-28.8 48-80 105.6-153.6 172.8-73.6 67.2-172.8 108.8-284.8 115.2-51.2-3.2-99.2-12.8-140.8-28.8l-48 48c57.6 22.4 118.4 38.4 188.8 44.8 160-16 288-73.6 377.6-176C979.199 585.6 1024 528 1024 512s-48.001-73.6-134.401-176Z">
                </path>
                <path fill="currentColor"
                  d="M511.998 672c-12.8 0-25.6-3.2-38.4-6.4l-51.2 51.2c28.8 12.8 57.6 19.2 89.6 19.2 64 0 115.2-22.4 160-64 41.6-41.6 64-96 64-160 0-32-6.4-64-19.2-89.6l-51.2 51.2c3.2 12.8 6.4 25.6 6.4 38.4 0 44.8-16 83.2-48 112-32 28.8-67.2 48-112 48Z">
                </path>
              </svg>
            </span>
          </div>
          <div v-if="confirmPasswordError" class="error-message">两次输入的密码不一致</div>

          <div class="modal-actions">
            <button type="button" class="cancel-btn" @click="closePasswordModal">取消</button>
            <button type="submit" :disabled="passwordChangeDisabled">确认修改</button>
          </div>
        </form>
      </div>
    </div>

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
import { ref, onMounted, computed, watch } from 'vue'
import { readerState } from '@/stores/index'
import { updateReader, uploadReaderAvatar, uploadReaderBackground, deleteReader } from '@/API/Reader_API'
import { resetReaderPassword } from '@/API/Log_API'
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
const defaultAvatar = 'e165315c-da2b-42c9-b3cf-c0457d168634.jpg'
const showDeleteConfirm = ref(false)
const isCollectVisible = ref('是')
const isRecommendVisible = ref('是')
const backgroundFile = ref(null)
const backgroundPreviewUrl = ref('')
const defaultBackground = 'e26001d0-badc-4d6f-b7ef-e5ebe47642f0.png'

// 密码修改相关
const showPasswordModal = ref(false)
const showOldPassword = ref(false)
const showNewPassword = ref(false)
const showConfirmPassword = ref(false)
const oldPassword = ref('')
const newPassword = ref('')
const confirmPassword = ref('')
const oldPasswordError = ref(false)

// 计算属性验证电话号码
const phoneError = computed(() => {
  return phone.value && phone.value.length !== 11
})

// 计算属性验证确认密码
const confirmPasswordError = computed(() => {
  return newPassword.value !== confirmPassword.value && confirmPassword.value !== ''
})

// 计算属性判断密码修改按钮是否禁用
const passwordChangeDisabled = computed(() => {
  return !oldPassword.value || !newPassword.value || !confirmPassword.value ||
    confirmPasswordError.value || oldPasswordError.value
})

// 监听原密码变化，验证是否正确
watch(oldPassword, (newVal) => {
  if (newVal && store.true_password) {
    oldPasswordError.value = newVal !== store.true_password
  } else {
    oldPasswordError.value = false
  }
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

// 背景变更处理函数
function handleBackgroundChange(event) {
  backgroundFile.value = event.target.files[0]
  if (backgroundFile.value) {
    backgroundPreviewUrl.value = URL.createObjectURL(backgroundFile.value)
  } else {
    backgroundPreviewUrl.value = ''
  }
}

// 上传背景函数
async function uploadBackground() {
  try {
    if (!backgroundFile.value) throw new Error('请选择背景文件')
    const response = await uploadReaderBackground(store.readerId, backgroundFile.value)
    if (response.success && response.avatarUrl) {
      const url = response.avatarUrl
      const fileName = url.split('/').pop()
      store.backgroundUrl = fileName
      backgroundPreviewUrl.value = url
      toast.success('背景上传成功')
    } else {
      toast.error('背景上传失败，接口返回异常')
    }
  } catch (error) {
    toast.error('上传背景失败')
  }
}

// 格式化背景URL的函数
function getFormattedBackgroundUrl(backgroundUrl) {
  if (!backgroundUrl || backgroundUrl.trim() === '') {
    return defaultBackground
  }
  if (backgroundUrl.startsWith('http://') || backgroundUrl.startsWith('https://')) {
    return backgroundUrl
  }
  return backgroundUrl
}

onMounted(() => {
  readerName.value = store.readerName || ''
  phone.value = store.phone || ''
  gender.value = store.gender || '男'
  isCollectVisible.value = store.isCollectVisible || '是'
  isRecommendVisible.value = store.isRecommendVisible || '是'
  avatarPreviewUrl.value = getFormattedAvatarUrl(store.avatarUrl)
  backgroundPreviewUrl.value = getFormattedBackgroundUrl(store.backgroundUrl)
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

  if (backgroundFile.value) {
    await uploadBackground()
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
    isCollectVisible: isCollectVisible.value,
    isRecommendVisible: isRecommendVisible.value
  }

  try {
    await updateReader(store.readerId, updateData)
    toast.success('修改成功')
    Object.assign(store, updateData)
    avatarPreviewUrl.value = getFormattedAvatarUrl(store.avatarUrl)
    backgroundPreviewUrl.value = getFormattedBackgroundUrl(store.backgroundUrl)
  } catch (error) {
    toast.error('修改失败，请稍后再试')
  }
}

// 关闭密码修改模态框
function closePasswordModal() {
  showPasswordModal.value = false
  // 清空表单
  oldPassword.value = ''
  newPassword.value = ''
  confirmPassword.value = ''
  oldPasswordError.value = false
}

// 修改密码功能
async function changePassword() {
  if (newPassword.value !== confirmPassword.value) {
    toast.error('两次输入的密码不一致')
    return
  }
  if (oldPassword.value !== store.true_password) {
    toast.error('原密码不正确')
    return
  }
  try {
    const dto = {
      readerName: store.readerName,
      password: newPassword.value
    }
    const response = await resetReaderPassword(dto)
    if (response) {
      toast.success('密码修改成功')
      // 更新store中的密码
      store.true_password = newPassword.value
      // 关闭模态框
      closePasswordModal()
    } else {
      toast.error('密码修改失败')
    }
  } catch (error) {
    toast.error('密码修改失败，请稍后再试')
    console.error('修改密码错误:', error)
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
input[type="password"],
select {
  padding: 10px 12px;
  border: 1px solid #ddd;
  border-radius: 8px;
  font-size: 1em;
  transition: border-color 0.3s, box-shadow 0.3s;
  width: 60%;
}

input[type="text"]:focus,
input[type="password"]:focus,
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
  text-align: center;
}

.error-message1 {
  color: #ff4d4f;
  font-size: 0.85em;
  margin-top: 5px;
}

.upload-row {
  display: flex;
  justify-content: space-between;
  gap: 10px;
  margin-bottom: 20px;
}

.avatar-upload {
  flex: 2;
  text-align: center;
}

.background-upload {
  flex: 5;
  text-align: center;
}

.background-image {
  width: 100%;
  height: 130px;
  border-radius: 8px;
  object-fit: cover;
  margin-bottom: 10px;
  border: 2px solid #ed424b;
  display: block;
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
}

button[type="submit"]:hover {
  background-color: #f05e74;
}

button[type="submit"]:active {
  transform: scale(0.98);
}

button[type="submit"]:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

/* 修改密码区域样式 */
.password-change-section {
  margin-top: 30px;
  padding-top: 20px;
  border-top: 1px solid #eee;
}

.password-change-section h3 {
  color: #ed424b;
  margin-bottom: 15px;
}

.change-password-btn {
  padding: 10px 20px;
  background-color: #ed424b;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 500;
  transition: background-color 0.3s, transform 0.1s;
}

.change-password-btn:hover {
  background-color: #f05e74;
}

.change-password-btn:active {
  transform: scale(0.98);
}

/* 模态框样式 */
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

.password-modal {
  background-color: white;
  padding: 25px;
  border-radius: 12px;
  width: 90%;
  max-width: 400px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
}

.password-modal h3 {
  color: #ed424b;
  margin-bottom: 20px;
  text-align: center;
}

.input-group {
  position: relative;
  margin-bottom: 20px;
}

.modal-input {
  width: 100%;
  padding: 10px 40px 10px 40px;
  border: 1px solid #ddd;
  border-radius: 8px;
  font-size: 0.8em;
  transition: border-color 0.3s, box-shadow 0.3s;
  display: block;
  margin: 0 auto;
}

.modal-input:focus {
  border-color: #ed424b;
  box-shadow: 0 0 5px rgba(247, 142, 72, 0.3);
  outline: none;
}

.input-eye {
  position: absolute;
  right: 75px;
  top: 60%;
  transform: translateY(-50%);
  color: #888;
  cursor: pointer;
  z-index: 1;
}

.modal-actions {
  display: flex;
  justify-content: space-between;
  margin-top: 20px;
}

.modal-actions button {
  padding: 10px 20px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 500;
  transition: background-color 0.3s, transform 0.1s;
}

.modal-actions button[type="submit"] {
  background-color: #ed424b;
  color: white;
  width: 48%;
}

.modal-actions button[type="submit"]:hover {
  background-color: #f05e74;
}

.modal-actions button[type="submit"]:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.cancel-btn {
  background-color: #f0f0f0;
  color: #555;
  width: 48%;
}

.cancel-btn:hover {
  background-color: #e0e0e0;
}

/* 账号注销区域样式 */
.account-deletion {
  margin-top: 30px;
  padding-top: 20px;
  border-top: 1px solid #eee;
}

.account-deletion h3 {
  color: #ed424b;
  margin-bottom: 10px;
}

.account-deletion p {
  color: #666;
  margin-bottom: 15px;
  line-height: 1.5;
}

.delete-account-btn {
  padding: 10px 20px;
  background-color: #ff4d4f;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 500;
  transition: background-color 0.3s, transform 0.1s;
}

.delete-account-btn:hover {
  background-color: #ff7875;
}

.delete-account-btn:active {
  transform: scale(0.98);
}

/* 确认对话框样式 */
.confirmation-dialog {
  background-color: white;
  padding: 25px;
  border-radius: 12px;
  width: 90%;
  max-width: 400px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
}

.confirmation-dialog h3 {
  color: #ed424b;
  margin-bottom: 15px;
  text-align: center;
}

.confirmation-dialog p {
  color: #666;
  margin-bottom: 15px;
  line-height: 1.5;
}

.confirmation-dialog ul {
  color: #666;
  margin-bottom: 20px;
  padding-left: 20px;
}

.confirmation-dialog li {
  margin-bottom: 8px;
}

.dialog-actions {
  display: flex;
  justify-content: space-between;
}

.dialog-actions button {
  padding: 10px 20px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 500;
  transition: background-color 0.3s, transform 0.1s;
}

.confirm-delete-btn {
  background-color: #ff4d4f;
  color: white;
  width: 48%;
}

.confirm-delete-btn:hover {
  background-color: #ff7875;
}

.confirm-delete-btn:active {
  transform: scale(0.98);
}
</style>