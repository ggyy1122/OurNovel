using Microsoft.AspNetCore.Mvc;
using OurNovel.Models;
using OurNovel.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 读者最近阅读
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RecentReadingsController : ControllerBase
    {
        private readonly IRecentReadingsService _recentReadingsService;

        public RecentReadingsController(IRecentReadingsService recentReadingsService)
        {
            _recentReadingsService = recentReadingsService;
        }

        /// <summary>
        /// 添加或更新读者最近阅读记录
        /// </summary>
        /// <param name="readerId">读者ID</param>
        /// <param name="novelId">小说ID</param>
        /// <param name="chapterId">章节ID（可选，默认为1）</param>
        /// <returns></returns>
        [HttpPost("add-or-update")]
        public async Task<IActionResult> AddOrUpdate(
            [FromQuery] int readerId,
            [FromQuery] int novelId,
            [FromQuery] int chapterId = 1)
        {
            if (readerId <= 0 || novelId <= 0 || chapterId <= 0)
            {
                return BadRequest("无效的 readerId、novelId 或 chapterId");
            }

            await _recentReadingsService.AddOrUpdateAsync(readerId, novelId, chapterId);
            return Ok("添加或更新成功");
        }

        /// <summary>
        /// 删除指定读者的某本小说的最近阅读记录
        /// </summary>
        /// <param name="readerId">读者ID</param>
        /// <param name="novelId">小说ID</param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int readerId, [FromQuery] int novelId)
        {
            if (readerId <= 0 || novelId <= 0)
            {
                return BadRequest("无效的 readerId 或 novelId");
            }

            await _recentReadingsService.DeleteAsync(readerId, novelId);
            return Ok("删除成功");
        }

        /// <summary>
        /// 获取指定读者的最近阅读记录列表，按时间降序排列
        /// </summary>
        /// <param name="readerId">读者ID</param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<RecentReadings>>> GetByReaderId([FromQuery] int readerId)
        {
            if (readerId <= 0)
            {
                return BadRequest("无效的 readerId");
            }

            var list = await _recentReadingsService.GetByReaderIdAsync(readerId);
            return Ok(list);
        }

        /// <summary>
        /// 获取读者最近阅读的章节ID
        /// </summary>
        /// <param name="readerId">读者ID</param>
        /// <param name="novelId">小说ID</param>
        /// <returns>最近阅读的章节ID，如果没有记录则返回1（首章）</returns>
        [HttpGet("last-read-chapter")]
        public async Task<ActionResult<int>> GetLastReadChapterId([FromQuery] int readerId, [FromQuery] int novelId)
        {
            if (readerId <= 0 || novelId <= 0)
            {
                return BadRequest("无效的 readerId 或 novelId");
            }

            var chapterId = await _recentReadingsService.GetLastReadChapterIdAsync(readerId, novelId);
            return Ok(chapterId);
        }
    }
}