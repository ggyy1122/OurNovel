using Microsoft.AspNetCore.Http;
using OurNovel.Models;
using OurNovel.Models.Dto;
using OurNovel.Repositories;

namespace OurNovel.Services
{
    /// <summary>
    /// Novel 服务，继承基础服务，如有特殊业务再扩展
    /// </summary>
    public class NovelService : BaseService<Novel, int>
    {
        private readonly INovelRepository _novelRepository;
        private readonly NovelManagementService _novelManagementService;

        public NovelService(IRepository<Novel, int> repository, INovelRepository novelRepository, NovelManagementService novelManagementService)
            : base(repository)
        {
            _novelRepository = novelRepository;
            _novelManagementService = novelManagementService;
        }

        /// <summary>
        /// 审核小说，修改状态为“连载”或“完结”
        /// </summary>
        public async Task<bool> ReviewNovelAsync(int novelId, string newStatus,int managerId)
        {
            // 合法性检查（业务约束）
            if (newStatus != "连载" && newStatus != "完结")
                return false;

            var novel = await _repository.GetByIdAsync(novelId);
            if (novel == null)
                return false;

            novel.Status = newStatus;
            await _repository.UpdateAsync(novel);

            var result = $"审核结束，状态修改为 {newStatus}";
            await _novelManagementService.RecordManagementAsync(managerId, result, novelId);

            return true;
        }
        /// <summary>
        /// 上传小说封面，并更新封面地址ַ
        /// </summary>
        /// <param name="novelId">小说ID</param>
        /// <param name="coverFile">封面文件</param>
        /// <returns>封面URL</returns>
        /// 

        /// <summary>
        /// 获取收藏榜单
        /// </summary>
        public async Task<List<CollectRankingDto>> GetTopCollectedNovelsAsync(int topN)
        {
            return await _novelRepository.GetTopCollectedNovelsAsync(topN);
        }

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
