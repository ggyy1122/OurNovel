using Microsoft.AspNetCore.Mvc;
using OurNovel.Models;
using OurNovel.Services;

namespace OurNovel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChapterController : ControllerBase
    {
        private readonly ChapterService _chapterService;

        public ChapterController(ChapterService chapterService)
        {
            _chapterService = chapterService;
        }

        /// <summary>
        /// 获取所有"首次审核"、"审核中"的章节
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var chapters = await _chapterService.GetAllAsync();
            return Ok(chapters);
        }


        /// <summary>
        /// 获取指定小说下的所有章节
        /// </summary>
        [HttpGet("novel/{novelId}")]
        public async Task<IActionResult> GetChaptersByNovel(int novelId)
        {
            var chapters = await _chapterService.GetByNovelIdAsync(novelId);
            return Ok(chapters);
        }

        /// <summary>
        /// 新：获取小说的所有章节
        /// </summary>
        [HttpGet("novels/{novelId}/chapters")]
        public async Task<IActionResult> GetChapters(int novelId)
        {
            var chapters = await _chapterService.GetByNovelIdAsync(novelId);

            var result = chapters.Select(c => new
            {
                c.NovelId,
                c.ChapterId,
                c.Title,
                c.WordCount,
                c.PricePerKilo,
                c.IsCharged,
                c.PublishTime,
                c.Status,
                CalculatedPrice = Math.Round((c.WordCount / 1000m) * c.PricePerKilo, 0)
            });

            return Ok(result);
        }

        /// <summary>
        /// 获取指定章节
        /// </summary>
        [HttpGet("{novelId:int}/{chapterId:int}")]
        public async Task<IActionResult> GetChapter(int novelId, int chapterId)
        {
            var chapter = await _chapterService.GetByIdAsync(novelId, chapterId);
            if (chapter == null) return NotFound();
            return Ok(chapter);
        }

        /// <summary>
        /// 添加章节（正文直接通过 Content 属性提交）
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Chapter chapter)
        {
            await _chapterService.AddAsync(chapter);
            return CreatedAtAction(nameof(GetChapter), new { novelId = chapter.NovelId, chapterId = chapter.ChapterId }, chapter);
        }

        /// <summary>
        /// 更新章节
        /// </summary>
        [HttpPut("{novelId:int}/{chapterId:int}")]
        public async Task<IActionResult> Update(int novelId, int chapterId, [FromBody] Chapter chapter)
        {
            if (chapter.NovelId != novelId || chapter.ChapterId != chapterId)
                return BadRequest("路径参数与实体ID不一致");

            await _chapterService.UpdateAsync(chapter);
            return NoContent();
        }

        /// <summary>
        /// 删除章节
        /// </summary>
        [HttpDelete("{novelId:int}/{chapterId:int}")]
        public async Task<IActionResult> Delete(int novelId, int chapterId)
        {
            await _chapterService.DeleteAsync(novelId, chapterId);
            return NoContent();
        }

        /// <summary>
        /// 审核章节
        /// </summary>
        [HttpPut("novels/{novelId}/chapters/{chapterId}/review")]
        public async Task<IActionResult> ReviewChapter(int novelId, int chapterId, [FromQuery] string newStatus, [FromQuery] int managerId, [FromQuery] string result)
        {
            var success = await (_chapterService as ChapterService)?.ReviewChapterAsync(novelId, chapterId, newStatus, managerId, result)!;

            if (!success)
                return BadRequest("审核失败，可能是章节不存在或状态非法（必须为‘已发布’或‘封禁’）");

            return Ok(new { success = true, message = "章节状态已更新" });
        }
    }
}
