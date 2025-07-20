## API
目前已把读者相关的API实现，就封装成函数，在API文件夹中，一个表对应一个文件。

每个api都在API_Test中都有相应的测试（比如它怎么用的，需要的参数，怎么用它返回的数据）

在构造数据时，元素尽量不要为空，容易出现问题。

## reader全局变量

为方便读取于reader有关的数据，我在登录时把reader对象全保存在了stores的index.js中，所以基本可以不再使用reader的API。除了写个人主页的那个人，因为有修改个人信息的功能要实现。

用法：
```js
//先实例化
import {readerState } from '@/stores/index';
const reader_state = readerState();

//用，直接reader_state.···
reader_state.readerId       //属性和函数都可.
```
新增了select_novel,当前选择的小说信息

## 功能
### 1.登录登出注册✅

### 2.章节

### 3.书籍

### 4.分类✅

### 5.排行榜（周慧星）

### 6.搜索（等后端API）

### 7.收藏评分

### 8.举报

### 9.评论回复 举报 点赞

### 10.管理❌

### 11.交易-充值

### 12.交易-解锁章节

### 13.交易-打赏

### 14.个人信息（  一人   ）`Novel_ReaderInfomation.vue`

修改个人信息，展示个人书架（收藏、推荐）、阅读记录等等，可以自己添加。

个人信息：调reader的API

书架：参考这个获取当前读者收藏的书籍（传你的id）,放入readerState的  `favoriteBooks: []`, 放的是整个novel对象
![alt text](image.png)
然后遍历它展示即可。历史记录一样，有个 `readHistory: [],`     。

界面参考：个人信息肯定要丰富点。
![alt text](image-1.png)

### 作品主页

[参考](https://mangguoshufang.com/1/260/info.html)

一人：`Novel_Info.vue  `和`Novel_Info_home.vue`

界面美观，开始阅读（默认第一章，先放个按钮）、收藏（调API写入数据库，并加入到`favoriteBooks: []`）、评分、推荐、旁边作者信息展示，打赏按钮？
![alt text](image-2.png)
![alt text](image-3.png)

一人：`Novel_Info_Chapter.vue`和`Novel_Info_Comment.vue`

章节展示：只要展示就好，点击后进入阅读界面我来弄。点击做个函数响应（ 把全局变量selectedChapter赋值下就行了）
![alt text](image-4.png)
![alt text](image-5.png)
评论（自由发挥），因为我们评论基于章节不是小说。回复、点赞

周慧星：
排行榜、把剩余api弄下，更新下api，阅读界面

