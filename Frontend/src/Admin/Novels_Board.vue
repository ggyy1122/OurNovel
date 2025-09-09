<template>
    <div class="novels">
        <div class="header">
            <h3>小说管理</h3>
            <button @click="showAddDialog = true">添加小说</button>
        </div>

        <table class="novel-table">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>书名</th>
                    <th>作者</th>
                    <th>操作</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="novel in novels" :key="novel.id">
                    <td>{{ novel.id }}</td>
                    <td>{{ novel.title }}</td>
                    <td>{{ novel.author }}</td>
                    <td>
                        <button @click="editNovel(novel)">编辑</button>
                        <button @click="deleteNovel(novel.id)">删除</button>
                    </td>
                </tr>
            </tbody>
        </table>

        <!-- 添加/编辑弹窗 -->
        <div v-if="showAddDialog" class="dialog">
            <div class="dialog-content">
                <h4>{{ editingNovel ? '编辑小说' : '添加小说' }}</h4>

                <div class="form-group">
                    <label>书名：</label>
                    <input v-model="form.title" placeholder="请输入书名">
                </div>

                <div class="form-group">
                    <label>作者：</label>
                    <input v-model="form.author" placeholder="请输入作者">
                </div>

                <div class="buttons">
                    <button @click="saveNovel">保存</button>
                    <button @click="showAddDialog = false">取消</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, reactive } from 'vue'

// 模拟数据
const novels = ref([
    { id: 1, title: '斗破苍穹', author: '天蚕土豆' },
    { id: 2, title: '凡人修仙传', author: '忘语' }
])

const showAddDialog = ref(false)
const editingNovel = ref(null)

const form = reactive({
    title: '',
    author: ''
})

// 添加/编辑小说
const saveNovel = () => {
    if (editingNovel.value) {
        // 更新现有小说
        const index = novels.value.findIndex(n => n.id === editingNovel.value.id)
        novels.value[index] = { ...form, id: editingNovel.value.id }
    } else {
        // 添加新小说
        novels.value.push({
            id: novels.value.length + 1,
            ...form
        })
    }

    showAddDialog.value = false
    resetForm()
}

// 编辑小说
const editNovel = (novel) => {
    editingNovel.value = novel
    form.title = novel.title
    form.author = novel.author
    showAddDialog.value = true
}

// 删除小说
const deleteNovel = (id) => {
    if (confirm('确定要删除这本小说吗？')) {
        novels.value = novels.value.filter(novel => novel.id !== id)
    }
}

// 重置表单
const resetForm = () => {
    form.title = ''
    form.author = ''
    editingNovel.value = null
}
</script>

<style scoped>
.header {
    display: flex;
    justify-content: space-between;
    margin-bottom: 20px;
}

.novel-table {
    width: 100%;
    border-collapse: collapse;
}

.novel-table th,
.novel-table td {
    border: 1px solid #ddd;
    padding: 8px;
    text-align: left;
}

.novel-table th {
    background-color: #f2f2f2;
}

.novel-table tr:nth-child(even) {
    background-color: #f9f9f9;
}

.dialog {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
}

.dialog-content {
    background: white;
    padding: 20px;
    border-radius: 5px;
    width: 400px;
}

.form-group {
    margin-bottom: 15px;
}

.form-group label {
    display: block;
    margin-bottom: 5px;
}

.form-group input {
    width: 100%;
    padding: 8px;
}

.buttons {
    display: flex;
    justify-content: flex-end;
    gap: 10px;
    margin-top: 20px;
}
</style>