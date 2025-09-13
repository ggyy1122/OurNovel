<template>
  <div class="users">
    <div class="header">
      <h2>用户管理</h2>
      <select v-model="userType">
        <option value="reader">读者用户</option>
        <option value="author">作者用户</option>
      </select>
    </div>

    <!-- 筛选区域 -->
    <div class="filter-area">
      <input v-model="filters.name" placeholder="用户名" />
      <input v-model="filters.phone" placeholder="手机号" />
      <select v-if="userType === 'reader'" v-model="filters.gender">
        <option value="">性别</option>
        <option value="男">男</option>
        <option value="女">女</option>
      </select>
      <button class="btn btn-filter" @click="applyFilters">筛选</button>
      <button class="btn btn-reset" @click="resetFilters">重置</button>
    </div>

    <!-- 加载中提示 -->
    <div v-if="loading" class="loading-indicator">
      <div class="spinner"></div>
      <span>加载中...</span>
    </div>

    <!-- 表格 -->
    <table v-else class="user-table">
      <thead>
        <tr>
          <th>头像</th>
          <th>用户名</th>
          <th>手机号</th>
          <th v-if="userType === 'reader'">性别</th>
          <th v-else>收入</th>
          <th>操作</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in filteredUsers" :key="user.id">
          <!-- 头像逻辑：支持 OSS URL 拼接，无图回退默认头像 -->
          <td>
            <img
              :src="user.avatar ? `https://novelprogram123.oss-cn-hangzhou.aliyuncs.com/${user.avatar}` : require('@/assets/default-avatar.jpeg')"
              alt="头像"
              class="avatar"
              @error="e => e.target.src = require('@/assets/default-avatar.jpeg')"
            />
          </td>

          <td>{{ user.name }}</td>
          <td>{{ user.phone }}</td>
          <td v-if="userType === 'reader'">{{ user.gender }}</td>
          <td v-else>{{ user.income/100 }} 元</td>
          <td>
            <button class="btn-delete" @click="deleteUser(user.id)">删除</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue'
import { getAllReaders, getAllAuthors, deleteReader, deleteAuthorWithNovels } from '@/API/UserManage_API.js'
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

const userType = ref('reader')

const readers = ref([])
const authors = ref([])

const filters = ref({
  name: '',
  phone: '',
  gender: ''
})

const loading = ref(false)

// 加载数据
const fetchReaders = async () => {
  loading.value = true
  try {
    const res = await getAllReaders()
    readers.value = res.map(r => ({
      id: r.readerId,
      avatar: r.avatarUrl,  // 文件名，模板里拼接 OSS URL
      name: r.readerName,
      phone: r.phone,
      gender: r.gender
    }))
  } catch (err) {
    console.error('获取读者失败:', err)
  } finally {
    loading.value = false
  }
}

const fetchAuthors = async () => {
  loading.value = true
  try {
    const res = await getAllAuthors()
    authors.value = res.map(a => ({
      id: a.authorId,
      avatar: a.avatarUrl,  // 文件名，模板里拼接 OSS URL
      name: a.authorName,
      phone: a.phone,
      income: a.earning || 0
    }))
  } catch (err) {
    console.error('获取作者失败:', err)
  } finally {
    loading.value = false
  }
}

// 根据用户类型加载数据
const loadUsers = () => {
  if (userType.value === 'reader') {
    fetchReaders()
  } else {
    fetchAuthors()
  }
}

// 页面加载 + 切换用户类型时自动加载
onMounted(loadUsers)
watch(userType, loadUsers)

// 筛选逻辑
const filteredUsers = computed(() => {
  const list = userType.value === 'reader' ? readers.value : authors.value
  const filterName = filters.value.name ?? ''
  const filterPhone = filters.value.phone ?? ''

  return list.filter(user => {
    const name = user.name ?? ''
    const phone = user.phone ?? ''
    const nameMatch = name.includes(filterName)
    const phoneMatch = phone.includes(filterPhone)
    const genderMatch = userType.value === 'reader'
      ? (!filters.value.gender || user.gender === filters.value.gender)
      : true
    return nameMatch && phoneMatch && genderMatch
  })
})

const applyFilters = () => {
  // 筛选由 computed 自动处理，无需手动触发
}

const resetFilters = () => {
  filters.value = { name: '', phone: '', gender: '' }
}

// 删除用户
const deleteUser = async (id) => {
  if (!confirm('确认删除该用户？删除作者将会同时删除该作者所有小说')) return

  try {
    if (userType.value === 'reader') {
      await deleteReader(id)
      readers.value = readers.value.filter(user => user.id !== id)
    } else {
      await deleteAuthorWithNovels(id)
      authors.value = authors.value.filter(user => user.id !== id)
    }
  } catch (err) {
    console.error('删除失败:', err)
    toast.error('删除失败，请检查权限或网络。')
  }
}
</script>

<style scoped>
.users {
  max-width: 1000px;
  margin: 30px auto;
  padding: 25px;
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.06);
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.header h2 {
  font-size: 1.6em;
  color: #555;
}

.header select {
  padding: 8px;
  font-size: 14px;
  border-radius: 6px;
  border: 1px solid #ccc;
  background-color: #f2f5f8;
  color: #555;
}

.filter-area {
  display: flex;
  gap: 10px;
  margin-bottom: 20px;
  flex-wrap: wrap;
}

.filter-area input,
.filter-area select {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 6px;
  font-size: 14px;
  background-color: #f2f5f8;
  color: #555;
}

.btn {
  padding: 8px 14px;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-size: 14px;
  transition: 0.2s;
}

.btn-filter {
  background-color: #486482;
  color: white;
}

.btn-filter:hover {
  background-color: #35495e;
}

.btn-reset {
  background-color: #ad7079;
  color: white;
}

.btn-reset:hover {
  background-color: #906269;
}

.btn-delete {
  padding: 6px 12px;
  background-color: #ad7079;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 13px;
  cursor: pointer;
}

.btn-delete:hover {
  background-color: #906269;
}

.user-table {
  width: 100%;
  border-collapse: collapse;
  background: #f2f5f8;
}

.user-table th,
.user-table td {
  padding: 12px 16px;
  border-bottom: 1px solid #f9f9f9;
  text-align: left;
  color: #555;
}

.user-table th {
  background-color: #e3eaf1;
  color: #555;
}

.user-table tr:hover {
  background-color: #e9f3fb;
}

/* 头像样式（与作者页一致） */
.avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  border: 1px solid #ccc;
  object-fit: cover;
}

/* 加载动画样式 */
.loading-indicator {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  margin: 20px 0;
  color: #666;
  font-size: 16px;
}

.spinner {
  width: 24px;
  height: 24px;
  border: 3px solid #ccc;
  border-top-color: #486482;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}
</style>