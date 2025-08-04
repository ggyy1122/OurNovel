using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;
using OurNovel.Services.Interfaces;
using OurNovel.Repositories;
using OurNovel.DTOs;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 小说控制器，继承基类控制器，实现对 Novel 实体的 CRUD 操作
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NovelController : BaseController<Novel, int>
    {
        private readonly NovelService _novelService;
        private readonly IImageResourceService _imageResourceService;
        private readonly IRepository<Novel, int> _novelRepository;

        // 注入服务
        public NovelController(NovelService novelService, IImageResourceService imageResourceService,
            IRepository<Novel, int> novelRepository) : base(novelService)
        {
            _novelService = novelService;
            _imageResourceService = imageResourceService;
            _novelRepository = novelRepository;
        }

        // 如果 Novel 有特殊的业务接口，可以在这里扩展

        /// <summary>
        /// 创建小说（简化字段）
        /// </summary>
        [HttpPost("/api/novel/create")]
        public async Task<IActionResult> CreateNovel([FromQuery] int authorId, [FromQuery] string novelName,[FromQuery] string introduction)
        {
            try
            {
                var novelId = await (_service as NovelService)!.CreateNovelAsync(authorId, novelName, introduction);
                return Ok(new { success = true, message = "小说创建成功，状态为待审核", novelId = novelId
                });
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { success = false, message });
            }
        }

        /// <summary>
        /// 审核小说
        /// </summary>
        [HttpPut("{id}/review")]
        public async Task<IActionResult> ReviewNovel(int id, [FromQuery] string newStatus, [FromQuery] int managerId, [FromQuery] string result)
        {
            // 调用审核并写入日志
            var success = await (_service as NovelService)?.ReviewNovelAsync(id, newStatus, managerId, result)!;

            if (!success)
                return BadRequest("审核失败，可能是ID不存在或状态非法（必须为‘连载’或‘完结’）");

            return Ok(new { success = true, message = "小说状态已更新" });
        }


        /// <summary>
        /// 上传小说封面，并更新封面URL
        /// </summary>
        /// <param name="novelId">小说ID</param>
        /// <param name="coverFile">封面文件</param>
        /// <returns>封面URL</returns>
        [HttpPost("UploadAvatar")]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadAvatar([FromForm] int novelId, [FromForm] IFormFile coverFile)
        {
            try
            {
                var coverUrl = await _imageResourceService.UploadImageAsync<Novel, int>(
                  novelId,
                  coverFile,
                  "CoverUrl",
                  _novelRepository);
                return Ok(new { success = true, coverUrl });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 修改小说信息
        /// </summary>
        [HttpPost("submit-edit")]
        public async Task<IActionResult> SubmitEdit([FromQuery] int originalNovelId, [FromBody] NovelEditDto edited)
        {
            var original = await _novelService.GetByIdAsync(originalNovelId);
            if (original == null) return NotFound("原小说不存在");

            var newNovel = new Novel
            {
                AuthorId = original.AuthorId,
                NovelName = edited.NovelName ?? original.NovelName,
                Introduction = edited.Introduction ?? original.Introduction,
                CoverUrl = edited.CoverUrl ?? original.CoverUrl,
                Score = original.Score,
                TotalWordCount = original.TotalWordCount,
                RecommendCount = original.RecommendCount,
                CollectedCount = original.CollectedCount,
                Status = "待审核",
                CreateTime = original.CreateTime,
                OriginalNovelId = originalNovelId,
                TotalPrice = original.TotalPrice
            };

            await _novelService.AddAsync(newNovel);
            return Ok(new { message = "提交修改成功，等待审核", newNovelId = newNovel.NovelId });
        }


        /// <summary>
        /// 返回小说总字数
        /// </summary>
        [HttpGet("wordcount/{novelId}")]
        public async Task<IActionResult> GetTotalWordCount(int novelId)
        {
            try
            {
                var wordCount = await _novelService.GetNovelPropertyAsync(novelId, n => n.TotalWordCount, 0L);

                return Ok(new { novelId, totalWords = wordCount });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }



        /// <summary>
        /// 返回小说推荐数
        /// </summary>
        [HttpGet("recommendcount/{novelId}")]
        public async Task<IActionResult> GetRecommendCount(int novelId)
        {
            try
            {
                var recommendCount = await _novelService.GetNovelPropertyAsync(novelId, n => n.RecommendCount, 0);

                return Ok(new { novelId, RecommendCount = recommendCount });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 返回小说收藏数
        /// </summary>
        [HttpGet("collectcount/{novelId}")]
        public async Task<IActionResult> GetCollectCount(int novelId)
        {
            try
            {
                var collectCount = await _novelService.GetNovelPropertyAsync(novelId, n => n.CollectedCount, 0);

                return Ok(new { novelId, CollectCount = collectCount });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 获取小说最新已发布章节的ID和发布时间
        /// </summary>
        [HttpGet("{novelId}/latest-published-chapter")]
        public async Task<IActionResult> GetLatestPublishedChapter(int novelId)
        {
            var latestChapter = await _novelService.GetLatestPublishedChapterInfoAsync(novelId);
            if (latestChapter == null)
                return NotFound("该小说还没有发布的章节");

            return Ok(latestChapter);
        }

        /// <summary>
        /// 获取所有已发布的小说（Status == "连载" 或 "完结"）
        /// </summary>
        [HttpGet("published")]
        public async Task<IActionResult> GetPublishedNovels()
        {
            var novels = await _novelService.GetPublishedNovelsAsync();
            return Ok(novels);
        }

        /// <summary>
        /// 获取所有连载或完结的小说（分页，每页3条）
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllNovels([FromQuery] int page = 1)
        {
            if (page < 1)
                return BadRequest("页码必须大于等于1");

            var novels = await _novelService.GetNovelsByPageAsync(page);
            return Ok(novels);
        }
    }
}
