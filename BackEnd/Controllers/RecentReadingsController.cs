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
        /// <returns></returns>
        [HttpPost("add-or-update")]
        public async Task<IActionResult> AddOrUpdate([FromQuery] int readerId, [FromQuery] int novelId)
        {
            if (readerId <= 0 || novelId <= 0)
            {
                return BadRequest("无效的 readerId 或 novelId");
            }

            await _recentReadingsService.AddOrUpdateAsync(readerId, novelId);
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
    }
}
