using Microsoft.AspNetCore.Mvc;
using OurNovel.Models;
using OurNovel.Services;

namespace OurNovel.Controllers
{
    /// <summary>
    /// �½ڿ��������ṩ�½ڵ���ɾ��Ľӿڣ����ڸ������� NovelId + ChapterId
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
        /// ��ȡָ��С˵�µ������½�
        /// </summary>
        [HttpGet("novel/{novelId}")]
        public async Task<IActionResult> GetChaptersByNovel(int novelId)
        {
            var chapters = await _chapterService.GetChaptersByNovelAsync(novelId);
            return Ok(chapters);
        }

        /// <summary>
        /// ��ȡĳһ�½ڣ�ͨ�� NovelId �� ChapterId��
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
        /// ����½�
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddChapter([FromBody] Chapter chapter)
        {
            await _chapterService.AddChapterAsync(chapter);
            return Ok(new { success = true });
        }

        /// <summary>
        /// �����½�
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateChapter([FromBody] Chapter chapter)
        {
            await _chapterService.UpdateChapterAsync(chapter);
            return Ok(new { success = true });
        }

        /// <summary>
        /// ɾ���½ڣ�ͨ�� NovelId �� ChapterId��
        /// </summary>
        [HttpDelete("{novelId}/{chapterId}")]
        public async Task<IActionResult> DeleteChapter(int novelId, int chapterId)
        {
            await _chapterService.DeleteChapterAsync(novelId, chapterId);
            return Ok(new { success = true });
        }
    }
}
