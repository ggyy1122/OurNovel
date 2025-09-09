import request from '@/API/index'

/**
 * 上传用户头像到OSS并更新数据库
 * @param {number} userId - 用户ID
 * @param {File} imageFile - 头像图片文件对象
 * @returns {Promise<string>} 返回头像的完整OSS URL
 * @throws {Error} 当上传失败或参数无效时抛出错误
 */
export function uploadAuthorAvatar(authorId, avatarFile) {
    const formData = new FormData();
    formData.append('authorId', authorId); 
    formData.append('avatarFile', avatarFile); 

    return request({
        url: '/api/Author/UploadAvatar', 
        method: 'post',
        data: formData,
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    }).then(res => res.avatarUrl); // 提取返回的avatarUrl字段
}