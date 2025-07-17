using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using System.Linq;
using System.Threading.Tasks;

namespace OurNovel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChapterManagementController : ControllerBase
    {
        private readonly ChapterManagementService _chapterManagementService;

        public ChapterManagementController(ChapterManagementService chapterManagementService)
        {
            _chapterManagementService = chapterManagementService;
        }

        /// <summary>
        /// 获取指定章节的管理日志列表
        /// GET: api/ChapterManagement/logs/{chapterId}
        /// </summary>
        [HttpGet("logs/{chapterId}")]
        public async Task<IActionResult> GetLogs(int chapterId,int novelId)
        {
            var logs = await _chapterManagementService.GetManagementLogsByNovelAndChapterAsync(novelId,chapterId);

            if (logs == null || logs.Count == 0)
            {
                return NotFound(new { success = false, message = "未找到相关管理日志" });
            }

            var result = logs.Select(l => new
            {
                l.ManagementId,
                l.ManagerId,
                ManagerName = l.Manager?.ManagerName,
                l.Result,
                Time = l.Time?.ToString("yyyy-MM-dd HH:mm:ss")
            });

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        /// <summary>
        /// 获取所有章节的管理日志列表
        /// GET: api/ChapterManagement/logs/all
        /// </summary>
        [HttpGet("logs/all")]
        public async Task<IActionResult> GetAllLogs()
        {
            var chapterManagements = await _chapterManagementService.GetAllChapterManagementLogsAsync();

            if (chapterManagements == null || chapterManagements.Count == 0)
            {
                return NotFound(new { success = false, message = "未找到章节管理日志" });
            }

            var result = chapterManagements.Select(cm => new
            {
                cm.Management!.ManagementId,
                cm.Management.ManagerId,
                ManagerName = cm.Management.Manager?.ManagerName,
                cm.NovelId,
                cm.ChapterId,
                cm.Management.Result,
                Time = cm.Management.Time?.ToString("yyyy-MM-dd HH:mm:ss")
            });

            return Ok(new
            {
                success = true,
                data = result
            });
        }
    }
}
