## 参考资料，这别人的前端搭建


[管理员界面](https://github.com/zxwk1998/vue-admin-better.git)

[小说界面](https://gitee.com/wangyubao996/vue-novel)



### 初始化本地Git仓库

创建个空文件夹，用vscode打开，作为你的前端项目根目录。


在你的前端项目根目录（例如 我的`D:\vue_project\data_project>`）下执行，vscode打开终端即可：


#### 只克隆 frontend 分支
使用 --single-branch 参数只克隆指定分支：
```bash
git clone --single-branch --branch frontend https://github.com/ggyy1122/OurNovel.git
```

若连不上，用ssh服务，否则跳过：

#### ssh配置(你https连不上仓库时)

##### **1. 检查是否已有 SSH 密钥（若你之前用过）**
```bash
ls -Force ~/.ssh   # 查看是否存在id_ed25519.pub 文件
```
- 若存在，可直接跳到步骤 3。
- 若不存在，需生成新密钥。


##### **2. 生成新的 SSH 密钥**
```bash
ssh-keygen -t ed25519 -C "your_email@example.com"  #填你github绑定的邮箱
```
按提示完成操作（可直接回车使用默认路径和密码）。


##### **3. 复制公钥到剪贴板**
```bash
cat ~/.ssh/id_ed25519.pub | clip  # Ed25519 密钥
```

##### **4. 将公钥添加到 GitHub 账户**
1. 登录 GitHub → 点击右上角头像 → Settings → SSH and GPG keys。
2. 点击 **New SSH key**。
3. 添加标题（如 `My PC`），粘贴剪贴板中的公钥内容。
4. 点击 **Add SSH key**。


##### **5. 测试 SSH 连接**
```bash
ssh -T git@github.com
```
- 首次连接会提示确认指纹，输入 `yes`。
- 若看到 `Hi username! You've successfully authenticated...`，则连接成功。它会显示叉，是对的。


##### **6. 克隆仓库**
```bash
git clone --single-branch --branch frontend git@github.com:ggyy1122/OurNovel.git
```


##### **6. 修改 Git 远程仓库 URL**
将 HTTPS 地址替换为 SSH 格式：ba
```bash
# 查看当前远程地址
git remote -v

# 修改为 SSH 地址
git remote set-url origin git@github.com:ggyy1122/OurNovel.git

# 验证修改
git remote -v
```

### 推送代码

你可以修改下test.md验证：

使用 SSH 协议推送：
```bash
git add .                
git commit -m "test:测试" 
git push -u origin master:frontend
```



### 日常开发流程
```bash
# 拉取最新代码
git pull origin frontend  # 拉取远程前端分支

# 开发新功能
# ...

# 提交代码
git add .
git commit -m "信息"
git push  # 推送到远程
```