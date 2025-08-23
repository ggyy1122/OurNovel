using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;

namespace OurNovel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        /// <summary>
        /// 获取总小说数
        /// </summary>
        [HttpGet("total-novels")]
        public async Task<IActionResult> GetTotalNovelsCount()
        {
            try
            {
                var count = await _statisticsService.GetTotalNovelsCountAsync();
                return Ok(new { totalNovels = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "获取总小说数失败", message = ex.Message });
            }
        }

        /// <summary>
        /// 获取作者总数
        /// </summary>
        [HttpGet("total-authors")]
        public async Task<IActionResult> GetTotalAuthorsCount()
        {
            try
            {
                var count = await _statisticsService.GetTotalAuthorsCountAsync();
                return Ok(new { totalAuthors = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "获取作者总数失败", message = ex.Message });
            }
        }

        /// <summary>
        /// 获取读者总数
        /// </summary>
        [HttpGet("total-readers")]
        public async Task<IActionResult> GetTotalReadersCount()
        {
            try
            {
                var count = await _statisticsService.GetTotalReadersCountAsync();
                return Ok(new { totalReaders = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "获取读者总数失败", message = ex.Message });
            }
        }

        /// <summary>
        /// 获取待审核章节数  “首次审核”+“审核中”
        /// </summary>
        [HttpGet("pending-chapters")]
        public async Task<IActionResult> GetPendingChaptersCount()
        {
            try
            {
                var count = await _statisticsService.GetPendingChaptersCountAsync();
                return Ok(new { pendingChapters = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "获取待审核章节数失败", message = ex.Message });
            }
        }

        /// <summary>
        /// 获取待审核小说数 “待审核”
        /// </summary>
        [HttpGet("pending-novels")]
        public async Task<IActionResult> GetPendingNovelsCount()
        {
            try
            {
                var count = await _statisticsService.GetPendingNovelsCountAsync();
                return Ok(new { pendingNovels = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "获取待审核小说数失败", message = ex.Message });
            }
        }

        /// <summary>
        /// 获取待处理举报数 “未处理”
        /// </summary>
        [HttpGet("pending-reports")]
        public async Task<IActionResult> GetPendingReportsCount()
        {
            try
            {
                var count = await _statisticsService.GetPendingReportsCountAsync();
                return Ok(new { pendingReports = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "获取待处理举报数失败", message = ex.Message });
            }
        }
    }
}
