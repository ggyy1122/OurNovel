// Banner轮播数据
export const carouselItems_data = [
    { image: require('@/assets/1.jpg'), title: 'TJ中文网', description: '匠心打磨好作品' },
    { image: require('@/assets/2.jpg'), title: '热门小说推荐', description: '最新签约作品' },
    { image: require('@/assets/3.jpeg'), title: '作家专区', description: '点击进入 >' },
    { image: require('@/assets/4.jpg'), title: 'TJ中文网', description: '匠心打磨好作品' }
]

// 精选小说ID
export const featuredNovelIds = {
    male: [166, 167, 168, 169, 183, 222, 263, 422], // 男频小说ID
    female: [462, 484, 501, 521, 561, 581, 601, 666] // 女频小说ID
}

// 作者ID列表
export const authorIds = [62, 201, 123]

// 小说ID列表
export const novelIds = [166, 167, 168, 169, 183, 222, 263, 422]


// 专栏轮播小说数据
export const carouselNovels_data = [
    { novelId: 462, novelName: '如意姑娘的', coverUrl: require('@/assets/side1.jpg') },
    { novelId: 462, novelName: '写给鼹鼠先生的情', coverUrl: require('@/assets/side2.jpg') },
    { novelId: 462, novelName: '问九卿', coverUrl: require('@/assets/side3.jpg') },
    { novelId: 462, novelName: '昭娇', coverUrl: require('@/assets/side4.jpg') },
    { novelId: 462, novelName: '岁时来仪', coverUrl: require('@/assets/side5.jpg') }
]

// 公告数据
export const announcements_data = [
    { text: '[资讯] 书写抗战精神作品联展', link: 'https://mp.weixin.qq.com/s/4VeBev9GGxihH5MNVevfSg?mpshare=1&scene=1&srcid=0801zDxRpMpTTwRpb8djZYXr&sharer_shareinfo=467c009cbe86e604c7fb12947fa1170b&sharer_shareinfo_first=467c009cbe86e604c7fb12947fa1170b#wechat_redirect', type: 'news' },
    { text: '[公告] 《听说你喜欢我》原著', link: 'https://www.hongxiu.com/book/3756981504436501', type: 'notice' },
    { text: '[资讯] 25年绿书签行动来啦', link: 'https://mp.weixin.qq.com/s/c1G3OQ6-lWh5qwQ-sejJcg', type: 'news' },
    { text: '[公告] 25年作家福利已上线', link: 'https://write.qq.com/portal/college/editordetail?gender=2&typeid=75457244950928251&idx=75460605762836001', type: 'notice' },
    { text: '[公告] "风起国潮"二期征文', link: 'https://write.qq.com/portal/dashboard/actarticleDetail?id=665', type: 'notice' },
    { text: '[公告] 红袖大神段寻新书来袭', link: 'https://www.hongxiu.com/book/32553967803686009', type: 'notice' }
]


export default {
    carouselItems_data,
    carouselNovels_data,
    announcements_data,
    featuredNovelIds,
    authorIds,
    novelIds
}