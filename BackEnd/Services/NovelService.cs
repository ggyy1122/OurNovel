using OurNovel.Models;
using OurNovel.Repositories;
using Microsoft.AspNetCore.Http;
namespace OurNovel.Services
{
    /// <summary>
    /// Novel 服务，继承基础服务，如有特殊业务再扩展
    /// </summary>
    public class NovelService : BaseService<Novel, int>
    {

        public NovelService(IRepository<Novel, int> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// 上传小说封面，并更新封面地址
        /// </summary>
        /// <param name="novelId">小说ID</param>
        /// <param name="coverFile">封面文件</param>
        /// <returns>封面URL</returns>
        /// 

        /*
        public async Task<string> UploadCoverAsync(int novelId, IFormFile coverFile)
        {
            if (coverFile == null || coverFile.Length == 0)
                throw new ArgumentException("封面文件不能为空");

            string coverUrl = null;

            try
            {
                // 上传文件
                coverUrl = await _fileStorageService.UploadAsync(coverFile, "covers");

                // 查找小说
                var novel = await _repository.GetByIdAsync(novelId);
                if (novel == null)
                    throw new Exception($"未找到ID为 {novelId} 的小说");

                // 更新数据库
                novel.CoverUrl = coverUrl;
                await _repository.UpdateAsync(novel);

                return coverUrl;
            }
            catch (Exception)
            {
                //  补偿删除刚才上传的文件
                if (!string.IsNullOrEmpty(coverUrl))
                {
                    _fileStorageService.Delete(coverUrl, "covers");
                }

                // 把异常继续抛出去，给上层处理
                throw;
            }
        }
        */

    }
}
