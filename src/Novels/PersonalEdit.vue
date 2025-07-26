<template>
  <div class="edit-container">
    <h2>修改个人信息</h2>
     <div class="avatar-upload">
        <img
          :src="avatarPreviewUrl.startsWith('blob:') ? avatarPreviewUrl : 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/' + avatarPreviewUrl"
          alt="头像预览"
          class="avatar-image"
        />
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
        <input v-model="phone" type="text" />
      </div>

      <div>
        <label>性别：</label>
        <select v-model="gender">
          <option value="男">男</option>
          <option value="女">女</option>
        </select>
      </div>

      <div>
        <label>是否显示收藏：</label>
        <input type="checkbox" v-model="isCollectVisible" />
      </div>

      <div>
        <label>是否显示推荐：</label>
        <input type="checkbox" v-model="isRecommendVisible" />
      </div>

      <button type="submit">保存修改</button>
    </form>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { readerState } from '@/stores/index'
import { updateReader, uploadReaderAvatar } from '@/API/Reader_API'
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

const store = readerState()

const readerName = ref('')
const phone = ref('')
const gender = ref('男')
const isCollectVisible = ref("yes")
const isRecommendVisible = ref("yes")

// 头像上传核心变量
const avatarFile = ref(null)
const avatarPreviewUrl = ref('')
const apiResponseAvatar = ref(null)
const prefix = 'https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/'
const defaultAvatar = 'e165315c-da2b-42c9-b3cf-c0457d168634.jpg'
// 头像预览地址辅助函数
function getFormattedAvatarUrl(avatarUrl) {
  if (!avatarUrl || avatarUrl.trim() === '') {
     return prefix + defaultAvatar
  }
  // 如果已经是完整URL，直接返回
  if (avatarUrl.startsWith('http://') || avatarUrl.startsWith('https://')) {
    return avatarUrl
  }
  return avatarUrl
}

function handleAvatarChange(event) {
  avatarFile.value = event.target.files[0]
  if (avatarFile.value) {
    avatarPreviewUrl.value = URL.createObjectURL(avatarFile.value) // 本地预览
  } else {
    avatarPreviewUrl.value = ''
  }
}

async function uploadAvatar() {
  try {
    if (!avatarFile.value) {
      throw new Error('请选择头像文件')
    }
    const response = await uploadReaderAvatar(store.readerId, avatarFile.value)
    apiResponseAvatar.value = response
    console.log('上传头像响应:', response)

    if (response.success && response.avatarUrl) {
      const url = response.avatarUrl
      const fileName = url.split('/').pop()
      store.avatarUrl = fileName
      avatarPreviewUrl.value = url // 预览用完整URL
      toast.success('头像上传成功')
    } else {
      toast.error('头像上传失败，接口返回异常')
    }
  } catch (error) {
    console.error('上传头像请求错误:', error)
    apiResponseAvatar.value = { error: error.message }
    toast.error('上传头像失败')
  }
}

onMounted(() => {
  readerName.value = store.readerName || ''
  phone.value = store.phone || ''
  gender.value = store.gender || '男'
  isCollectVisible.value = store.isCollectVisible ?? "yes"
  isRecommendVisible.value = store.isRecommendVisible ?? "yes"
  avatarPreviewUrl.value = getFormattedAvatarUrl(store.avatarUrl)
})

async function saveReaderChanges() {
  if (!store.readerId) {
    toast.error('未检测到登录用户，请重新登录')
    return
  }
  if (!store.password) {
    toast.error('未检测到用户密码，无法修改，请重新登录或联系管理员')
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
    isCollectVisible: isCollectVisible.value === "yes" ? "是" : "否",
    isRecommendVisible: isRecommendVisible.value === "yes" ? "是" : "否",
    balance: store.balance,
    avatarUrl: store.avatarUrl,
    backgroundUrl: store.backgroundUrl,
  }

  try {
    await updateReader(store.readerId, updateData)
    toast.success('修改成功')
    store.readerName = readerName.value
    store.phone = phone.value
    store.gender = gender.value
    store.isCollectVisible = isCollectVisible.value === "yes" ? "是" : "否"
    store.isRecommendVisible = isRecommendVisible.value === "yes" ? "是" : "否"
    avatarPreviewUrl.value = getFormattedAvatarUrl(store.avatarUrl)
  } catch (error) {
    console.error('修改失败:', error)
    toast.error('修改失败，请稍后再试')
  }
}
</script>


<style scoped>
.edit-container {
  background: linear-gradient(120deg, #f6d365 0%, #fda085 100%);
  padding: 40px 30px;
  max-width: 400px;
  margin: 50px auto;
  border-radius: 12px;
  box-shadow: 0 8px 20px rgba(0,0,0,0.1);
  font-family: "Segoe UI", sans-serif;
}

.edit-container h2 {
  text-align: center;
  color:white;
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

.upload-button {
  display: inline-block;
  padding: 8px 16px;
  background-color: #f8d302f5;
  color: white;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  transition: background-color 0.3s, transform 0.1s;
  user-select: none;
}

.upload-button:hover {
  background-color: #f26522;
}

.upload-button:active {
  transform: scale(0.98);
}
.avatar-image {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  object-fit: cover;
  margin-bottom: 10px;
  border: 2px solid #f8d302;
}


input[type="text"],
select {
  width: 60%;
  padding: 10px 12px;
  border: 1px solid #ddd;
  border-radius: 8px;
  font-size: 1em;
  transition: border-color 0.3s, box-shadow 0.3s;
}

input[type="text"]:focus,
select:focus {
  border-color: #f8d302f5;
  box-shadow: 0 0 5px rgba(247, 142, 72, 0.3);
  outline: none;
}

input[type="checkbox"] {
  transform: scale(1.2);
  accent-color: #f8d302f5;
  cursor: pointer;
}

button[type="submit"] {
  width: 100%;
  padding: 12px 0;
  background-color: #f8d302f5;
  border: none;
  border-radius: 8px;
  color: white;
  font-size: 1.05em;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.3s, transform 0.1s;
}

button[type="submit"]:hover {
  background-color: #f26522;
}

button[type="submit"]:active {
  transform: scale(0.98);
}
</style>