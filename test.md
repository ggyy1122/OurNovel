
[管理页面](https://github.com/zxwk1998/vue-admin-better.git)



下面是从0开始使用Git管理前端项目的详细步骤，包含初始化仓库、提交代码、关联远程仓库等操作：


### **1. 初始化本地Git仓库**
在你的前端项目根目录（例如 `my-frontend-project`）下执行：
```bash
cd my-frontend-project  # 进入项目目录
git init                # 初始化Git仓库
```


### **2. 创建 `.gitignore` 文件**


```bash
ni .gitignore
```
编辑 `.gitignore` 内容：
```gitignore
# 常见前端忽略规则
node_modules/
dist/
build/
.DS_Store
.env
npm-debug.log*
```


### **3. 首次提交代码**
```bash
git add .                # 添加所有文件到暂存区
git commit -m "初始化前端项目"  # 提交到本地仓库
```


### **4. 在远程仓库（如GitHub/GitLab）创建新仓库**
- 登录你的代码托管平台，创建一个新的空仓库（例如命名为 `my-frontend`）。
- **不要**在远程仓库初始化 README、LICENSE 等文件（保持空仓库）。


### **5. 关联本地仓库与远程仓库**
```bash
# 添加远程仓库地址
git config --global --add safe.directory D:/vue_project/data_project

git remote add origin https://github.com/ggyy1122/OurNovel.git

# 推送本地主分支到远程（首次推送需指定上游分支）
git push -u origin main  # 若使用GitHub默认分支为main
# 或使用git push -u origin master  # 若使用master分支
```


### **6. 创建并切换到 `frontend` 分支（可选）**
如果你想将前端代码放在独立分支：
```bash
git branch -d frontend
```


### **7. 日常开发流程**
```bash
# 拉取最新代码
git pull origin frontend  # 拉取远程前端分支

# 开发新功能
# ...

# 提交代码
git add .
git commit -m "添加登录页面"
git push origin frontend  # 推送到远程
```


### **8. 解决冲突（如果有）**
若多人协作导致冲突，拉取代码时会提示：
```bash
git pull origin frontend  # 拉取时发现冲突

# 手动解决冲突后
git add .
git commit -m "解决冲突"
git push origin frontend
```


### **9. 查看分支状态**
```bash
git branch       # 查看本地分支
git branch -r    # 查看远程分支
git branch -a    # 查看所有分支
```


### **10. 切换分支**
```bash
git checkout main       # 切换到main分支
git checkout frontend   # 切换到frontend分支
```


### **注意事项**
1. **使用 `.gitignore`**：避免提交不必要的文件，尤其是 `node_modules`。
2. **分支命名**：建议使用 `main`/`master` 作为主分支，`frontend` 作为前端分支。
3. **提交信息**：编写清晰的提交信息（如 `fix: 修复登录按钮样式`）。
4. **定期拉取**：开发前先 `git pull`，减少冲突。

通过以上步骤，你可以完整地使用Git管理前端项目，后续可以按需创建更多分支（如 `dev`、`feature/login` 等）进行协作开发。