using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using OurNovel.Data;
using OurNovel.DTOs;
using OurNovel.Models;
using OurNovel.Models.Dto;
using OurNovel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace OurNovel.Services
{
    /// <summary>
    /// Novel 服务，继承基础服务，如有特殊业务再扩展
    /// </summary>
    public class NovelService : BaseService<Novel, int>
    {
        private readonly INovelRepository _novelRepository;
        private readonly NovelManagementService _novelManagementService;
        private readonly AppDbContext _context;

        public NovelService(IRepository<Novel, int> repository, 
                            INovelRepository novelRepository, 
                            NovelManagementService novelManagementService,
                            AppDbContext context)
            : base(repository)
        {
            _novelRepository = novelRepository;
            _novelManagementService = novelManagementService;
            _context = context;
        }

        /// <summary>
        /// 创建小说
        /// </summary>
        public async Task<int> CreateNovelAsync(int authorId, string novelName, string introduction)
        {
            var novel = new Novel
            {
                AuthorId = authorId,
                NovelName = novelName,
                Introduction = introduction,
                CreateTime = DateTime.Now,
                CoverUrl = null,
                Score = 0,
                TotalWordCount = 0,
                RecommendCount = 0,
                CollectedCount = 0,
                Status = "待审核",
                OriginalNovelId = -1,
                TotalPrice = 0
            };

            await AddAsync(novel);
            return novel.NovelId;
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

            if (novel.OriginalNovelId != -1)
            {
                if (newStatus == "连载")
                {
                    var original = await _repository.GetByIdAsync(novel.OriginalNovelId);
                    original.NovelName = novel.NovelName;
                    original.Introduction = novel.Introduction;
                    original.CoverUrl = novel.CoverUrl;
                    //original.CreateTime = novel.CreateTime;
                    if (original.Status == "封禁")
                    {
                        original.Status = "连载";
                    }

                    await _repository.UpdateAsync(original);
                    await _repository.DeleteAsync(novel.NovelId); // 删除临时稿

                    await _novelManagementService.RecordManagementAsync(managerId, result, original.NovelId);
                }
                else
                {
                    await _repository.DeleteAsync(novel.NovelId); // 删除临时稿
                    await _novelManagementService.RecordManagementAsync(managerId, result, novel.OriginalNovelId);
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
        public async Task<List<CollectRankingDto>> GetTopCollectedNovelsAsync(int topN, string? status = null)
        {
            return await _novelRepository.GetTopCollectedNovelsAsync(topN, status);
        }

        /// <summary>
        /// 获取推荐榜单
        /// </summary>
        public async Task<List<RecommendRankingDto>> GetTopRecommendedNovelsAsync(int topN, string? status = null)
        {
            return await _novelRepository.GetTopRecommendedNovelsAsync(topN, status);
        }

        /// <summary>
        /// 获取评分榜单
        /// </summary>
        public async Task<List<ScoreRankingDto>> GetTopScoredNovelsAsync(int topN, string? status = null)
        {
            return await _novelRepository.GetTopScoredNovelsAsync(topN, status);
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

            // 情况1：原小说还在待审核，直接更新原小说
            if (original.Status == "待审核")
            {
                original.NovelName = edited.NovelName ?? original.NovelName;
                original.Introduction = edited.Introduction ?? original.Introduction;
                original.CoverUrl = edited.CoverUrl ?? original.CoverUrl;
                original.Score = edited.Score ?? original.Score;
                original.TotalWordCount = edited.TotalWordCount ?? original.TotalWordCount;
                original.RecommendCount = edited.RecommendCount ?? original.RecommendCount;
                original.CollectedCount = edited.CollectedCount ?? original.CollectedCount;
                //original.CreateTime = DateTime.Now;
                original.TotalPrice = edited.TotalPrice ?? original.TotalPrice;

                await _context.SaveChangesAsync();
                return original.NovelId;
            }

            // 情况2：仅修改小说状态
            if ((original.Status == "连载" || original.Status == "完结")
                && !string.IsNullOrEmpty(edited.Status))
            {
                bool nameSame = (edited.NovelName ?? original.NovelName) == original.NovelName;
                bool introSame = (edited.Introduction ?? original.Introduction) == original.Introduction;
                bool coverSame = (edited.CoverUrl ?? original.CoverUrl) == original.CoverUrl;

                if (nameSame && introSame && coverSame)
                {
                    // 只修改状态
                    original.Status = edited.Status;
                    //original.CreateTime = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return original.NovelId;
                }
                // 否则继续走副本逻辑
            }

            // 情况3：原小说已发布，检查是否已有副本
            // 查找是否已有副本
            var existingCopy = await _context.Novels
                .FirstOrDefaultAsync(n => n.OriginalNovelId == originalNovelId);

            if (existingCopy != null)
            {
                // 更新副本
                existingCopy.NovelName = edited.NovelName ?? original.NovelName;
                existingCopy.Introduction = edited.Introduction ?? original.Introduction;
                existingCopy.CoverUrl = edited.CoverUrl ?? original.CoverUrl;
                existingCopy.Score = edited.Score ?? original.Score;
                existingCopy.TotalWordCount = edited.TotalWordCount ?? original.TotalWordCount;
                existingCopy.RecommendCount = edited.RecommendCount ?? original.RecommendCount;
                existingCopy.CollectedCount = edited.CollectedCount ?? original.CollectedCount;
                existingCopy.Status = "待审核";
                existingCopy.CreateTime = original.CreateTime;
                existingCopy.TotalPrice = edited.TotalPrice ?? original.TotalPrice;

                await _context.SaveChangesAsync();
                return existingCopy.NovelId;
            }
            else
            {
                // 新增副本
                var newNovel = new Novel
                {
                    AuthorId = original.AuthorId,
                    NovelName = edited.NovelName ?? original.NovelName,
                    Introduction = edited.Introduction ?? original.Introduction,
                    CoverUrl = edited.CoverUrl ?? original.CoverUrl,
                    Score = edited.Score ?? original.Score,
                    TotalWordCount = edited.TotalWordCount ?? original.TotalWordCount,
                    RecommendCount = edited.RecommendCount ?? original.RecommendCount,
                    CollectedCount = edited.CollectedCount ?? original.CollectedCount,
                    Status = "待审核",
                    CreateTime = original.CreateTime,
                    OriginalNovelId = originalNovelId,
                    TotalPrice = edited.TotalPrice ?? original.TotalPrice
                };

                await _repository.AddAsync(newNovel);
                return newNovel.NovelId;
            }
        }

        /// <summary>
        /// 小说最新章节
        /// </summary>
        public async Task<LatestPublishedChapterDto?> GetLatestPublishedChapterInfoAsync(int novelId)
        {
            return await _context.Chapters
                .Where(c => c.NovelId == novelId && c.Status == "已发布")
                .OrderByDescending(c => c.ChapterId)
                .Select(c => new LatestPublishedChapterDto
                {
                    ChapterId = c.ChapterId,
                    PublishTime = c.PublishTime
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取所有连载或完结的小说
        /// </summary>
        public async Task<List<Novel>> GetPublishedNovelsAsync()
        {
            return await _context.Novels
                .Where(n => n.Status == "连载" || n.Status == "完结")
                .OrderBy(n => n.NovelId) 
                .ToListAsync();
        }


        /// <summary>
        /// 获取所有已发布的小说（分页，按novelID顺序）
        /// </summary>
        public async Task<List<Novel>> GetNovelsByPageAsync(int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;

            return await _context.Novels
                .Where(n => n.Status == "连载" || n.Status == "完结")
                .OrderBy(n => n.NovelId)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }
        /// <summary>
        /// 获取小说列表（支持分类、字数区间、是否完结等条件 + 分页）
        /// </summary>
        public async Task<PagedResult<Novel>> GetNovelsAsync(
            int page,
            int pageSize,
            string? category = null,
            long? minWordCount = null,
            long? maxWordCount = null,
            bool? isFinished = null)
        {
            var query = _context.Novels
                .Where(n => n.Status == "连载" || n.Status == "完结")
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = from novel in query
                        join nc in _context.NovelCategories on novel.NovelId equals nc.NovelId
                        where nc.CategoryName == category
                        select novel;
            }

            if (minWordCount.HasValue)
                query = query.Where(n => n.TotalWordCount >= minWordCount.Value);

            if (maxWordCount.HasValue)
                query = query.Where(n => n.TotalWordCount <= maxWordCount.Value);

            if (isFinished.HasValue)
                query = isFinished.Value
                    ? query.Where(n => n.Status == "完结")
                    : query.Where(n => n.Status == "连载");

            int totalCount = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query
                .OrderBy(n => n.NovelId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Novel>
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                Page = page,
                PageSize = pageSize,
                Items = items
            };
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
