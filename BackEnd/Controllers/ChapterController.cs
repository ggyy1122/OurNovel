using Microsoft.AspNetCore.Mvc;
using OurNovel.Models;
using OurNovel.Services;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 章节控制器：提供章节的增删查改接口，基于复合主键 NovelId + ChapterId
    /// </summary>
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
        /// 获取指定小说下的所有章节
        /// </summary>
        [HttpGet("novel/{novelId}")]
        public async Task<IActionResult> GetChaptersByNovel(int novelId)
        {
            var chapters = await _chapterService.GetChaptersByNovelAsync(novelId);
            return Ok(chapters);
        }

        /// <summary>
        /// 获取某一章节（通过 NovelId 和 ChapterId）
        /// </summary>
        [HttpGet("{novelId}/{chapterId}")]
        public async Task<IActionResult> GetChapter(int novelId, int chapterId)
        {
            var chapter = await _chapterService.GetChapterAsync(novelId, chapterId);
            if (chapter == null)
                return NotFound();
            return Ok(chapter);
        }

        /// <summary>
        /// 添加章节
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddChapter([FromBody] Chapter chapter)
        {
            await _chapterService.AddChapterAsync(chapter);
            return Ok(new { success = true });
        }

        /// <summary>
        /// 更新章节
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateChapter([FromBody] Chapter chapter)
        {
            await _chapterService.UpdateChapterAsync(chapter);
            return Ok(new { success = true });
        }

        /// <summary>
        /// 删除章节（通过 NovelId 和 ChapterId）
        /// </summary>
        [HttpDelete("{novelId}/{chapterId}")]
        public async Task<IActionResult> DeleteChapter(int novelId, int chapterId)
        {
            await _chapterService.DeleteChapterAsync(novelId, chapterId);
            return Ok(new { success = true });
        }
    }
}
