using System.Linq.Expressions;
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
        /// 审核小说，修改状态为“连载”或“封禁”
        /// </summary>
        public async Task<bool> ReviewNovelAsync(int novelId, string newStatus,int managerId, string result)
        {
            // 合法性检查（业务约束）
            if (newStatus != "连载" && newStatus != "封禁")
                return false;

            var novel = await _repository.GetByIdAsync(novelId);
            if (novel == null)
                return false;

            if (novel.OriginalNovelId != -1 && newStatus == "连载")
            {
                var original = await _repository.GetByIdAsync(novel.OriginalNovelId);
                if (original != null)
                {
                    original.NovelName = novel.NovelName;
                    original.Introduction = novel.Introduction;
                    original.CoverUrl = novel.CoverUrl;
                    original.CreateTime = novel.CreateTime;

                    await _repository.UpdateAsync(original);
                    await _repository.DeleteAsync(novel.NovelId); // 删除临时稿
                }
            }
            else {
                novel.Status = newStatus;
                await _repository.UpdateAsync(novel);
                await _novelManagementService.RecordManagementAsync(managerId, result, novelId);
            }
            

            return true;
        }

        /// <summary>
        /// 获取收藏榜单
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public async Task<List<CollectRankingDto>> GetTopCollectedNovelsAsync(int topN)
        {
            return await _novelRepository.GetTopCollectedNovelsAsync(topN);
        }

        /// <summary>
        /// 获取推荐榜单
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public async Task<List<RecommendRankingDto>> GetTopRecommendedNovelsAsync(int topN)
        {
            return await _novelRepository.GetTopRecommendedNovelsAsync(topN);
        }

        /// <summary>
        /// 获取评分榜单
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public async Task<List<ScoreRankingDto>> GetTopScoredNovelsAsync(int topN)
        {
            return await _novelRepository.GetTopScoredNovelsAsync(topN);
        }


        /// <summary>
        /// 根据小说ID获取指定的字符串类型属性值
        /// </summary>
        /// <param name="novelId">要查询的小说ID</param>
        /// <param name="propertySelector">属性选择器，指定要获取的字符串类型属性</param>
        public async Task<TResult> GetNovelPropertyAsync<TResult>(int novelId, Expression<Func<Novel, TResult>> propertySelector, TResult defaultValue = default)
        {
            try
            {
                var result = await _novelRepository.GetNovelPropertyAsync(novelId, propertySelector);
                return EqualityComparer<TResult>.Default.Equals(result, default(TResult)) ? defaultValue : result;
            }
            catch
            {
                return defaultValue;
            }
        }


        /// <summary>
        /// 修改小说信息
        /// </summary>
        public async Task<int> SubmitNovelEditAsync(int originalNovelId, Novel edited)
        {
            var original = await _repository.GetByIdAsync(originalNovelId);
            if (original == null)
                return -1;

            var newNovel = new Novel
            {
                AuthorId = original.AuthorId,
                NovelName = edited.NovelName ?? original.NovelName,
                Introduction = edited.Introduction ?? original.Introduction,
                CoverUrl = edited.CoverUrl ?? original.CoverUrl,
                TotalWordCount = edited.TotalWordCount ?? original.TotalWordCount,
                // 其他字段...
                Status = "待审核",
                CreateTime = DateTime.Now,
                OriginalNovelId = originalNovelId,
                Score = 0,
                CollectedCount = 0,
                RecommendCount = 0,
                TotalPrice = 0
            };

            await _repository.AddAsync(newNovel);
            return newNovel.NovelId;
        }


        /// <summary>
        /// 上传小说封面，并更新封面地址ַ
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
