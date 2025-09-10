// Banner轮播数据
export const carouselItems_data = [
    { image: require('@/assets/1.jpg'), title: '热门小说推荐', description: '最新签约作品', link: 'https://www.qimao.com/shuku/195409/' },
    { image: require('@/assets/2.jpg'), title: '作家专区', description: '点击进入 >', link: 'https://zuozhe.qimao.com/front/custom-activity/2' },
    { image: require('@/assets/3.jpeg'), title: '热门玄幻', description: '炸裂登场', link: 'https://www.qimao.com/shuku/215243/' },
    { image: require('@/assets/4.jpg'), title: 'TJ小说网', description: '匠心打磨好作品', link: 'https://www.qimao.com/activity/zhengwen/detail/429?forceMode=1' }
]

// 精选小说ID
export const featuredNovelIds = {
    male: [812, 810, 814, 782, 783, 807, 789], // 男频小说ID
    female: [806, 795, 781, 813, 796, 799, 805] // 女频小说ID
}

// 作者ID列表
export const authorIds = [462, 461, 485]

// 小说ID列表
export const novelIds = [807, 789, 811, 815, 816, 813, 812, 806]


// 专栏轮播小说数据
export const carouselNovels_data = [
    { novelId: 820, novelName: '如意姑娘的', coverUrl: require('@/assets/side1.jpg') },
    { novelId: 823, novelName: '写给鼹鼠先生的情', coverUrl: require('@/assets/side2.jpg') },
    { novelId: 821, novelName: '问九卿', coverUrl: require('@/assets/side3.jpg') },
    { novelId: 819, novelName: '昭娇', coverUrl: require('@/assets/side4.jpg') },
    { novelId: 822, novelName: '岁时来仪', coverUrl: require('@/assets/side5.jpg') }
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