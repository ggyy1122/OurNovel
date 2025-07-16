using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using System.Linq;
using System.Threading.Tasks;

namespace OurNovel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NovelManagementController : ControllerBase
    {
        private readonly NovelManagementService _novelManagementService;

        public NovelManagementController(NovelManagementService novelManagementService)
        {
            _novelManagementService = novelManagementService;
        }

        /// <summary>
        /// 获取指定小说的管理日志列表（包含管理员信息，时间倒序）
        /// GET: api/NovelManagement/logs/{novelId}
        /// </summary>
        /// <param name="novelId">小说ID</param>
        /// <returns>管理日志列表</returns>
        [HttpGet("logs/{novelId}")]
        public async Task<IActionResult> GetLogs(int novelId)
        {
            var logs = await _novelManagementService.GetManagementLogsByNovelIdAsync(novelId);

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
    }
}
