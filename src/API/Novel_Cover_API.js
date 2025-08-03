import request from '@/API/index'

/**
 * 上传用户头像到OSS并更新数据库
 * @param {number} novelId - 用户ID
 * @param {File} coverFile - 头像图片文件对象
 * @returns {Promise<string>} 返回头像的完整OSS URL
 * @throws {Error} 当上传失败或参数无效时抛出错误
 */
export function uploadNovelCover(novelId, coverFile) {
    const formData = new FormData();
    formData.append('novelId', novelId); 
    formData.append('coverFile', coverFile); 

    return request({
        url: '/api/Novel/UploadAvatar', 
        method: 'post',
        data: formData,
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    }).then(res => res.coverUrl); 
}