using OurNovel.Models;
using OurNovel.Repositories;
using Microsoft.AspNetCore.Http;
using OurNovel.Services.Interfaces;

namespace OurNovel.Services
{
    /// <summary>
    /// Reader 服务，继承基础服务，如有特殊业务再扩展
    /// </summary>
    public class ReaderService : BaseService<Reader, int>
    {
        private readonly IOssService _ossService;

        public ReaderService(IRepository<Reader, int> repository, IOssService ossService)
            : base(repository)
        {
            _ossService = ossService;
        }

        /// <summary>
        /// 上传读者头像，并更新头像地址
        /// </summary>
        /// <param name="readerId">读者ID</param>
        /// <param name="avatarFile">头像文件</param>
        /// <returns>头像URL</returns>
        public async Task<string> UploadAvatarAsync(int readerId, IFormFile avatarFile)
        {
            if (avatarFile == null || avatarFile.Length == 0)
                throw new ArgumentException("头像文件不能为空");

            string avatarUrl = null;

            try
            {
                // 上传文件到 OSS，返回完整 URL
                avatarUrl = await _ossService.UploadFileAsync(avatarFile);

                // 截取文件名部分
                var fileName = avatarUrl.Split('/').Last();

                // 查找用户
                var reader = await _repository.GetByIdAsync(readerId);
                if (reader == null)
                    throw new Exception($"未找到ID为 {readerId} 的读者");

                // 更新数据库，只保存文件名
                reader.AvatarUrl = fileName;
                await _repository.UpdateAsync(reader);

                // 返回完整URL给前端用
                return avatarUrl;
            }
            catch (Exception)
            {
                // 补偿删除刚才上传的文件
                if (!string.IsNullOrEmpty(avatarUrl))
                {
                    // 截取文件名部分
                    var fileName = avatarUrl.Split('/').Last();
                    _ossService.DeleteFile(fileName);
                }

                // 把异常继续抛出去，给上层处理
                throw;
            }
        }
    }
}
