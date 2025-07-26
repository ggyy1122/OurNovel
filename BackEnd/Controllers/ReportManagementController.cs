// File: Controllers/ReportManagementController.cs

using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OurNovel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportManagementController : ControllerBase
    {
        private readonly ReportManagementService _reportManagementService;

        public ReportManagementController(ReportManagementService reportManagementService)
        {
            _reportManagementService = reportManagementService;
        }

        /// <summary>
        /// 获取指定举报的所有管理处理日志（含管理员信息）
        /// </summary>
        /// <param name="reportId">举报ID</param>
        /// <returns>管理日志列表</returns>
        [HttpGet("{reportId}/logs")]
        public async Task<IActionResult> GetReportManagementLogs(int reportId)
        {
            var logs = await _reportManagementService.GetManagementLogsByReportIdAsync(reportId);

            var result = logs.Select(m => new
            {
                m.ManagementId,
                m.ManagerId,
                ManagerName = m.Manager?.ManagerName,
                m.Result,
                Time = m.Time?.ToString("yyyy-MM-dd HH:mm:ss")
            });

            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// 获取所有举报的管理日志列表
        /// GET: api/NovelManagement/logs/all
        /// </summary>
        /// <returns>管理日志列表</returns>
        [HttpGet("logs/all")]
        public async Task<IActionResult> GetAllNovelLogs()
        {
            var reportManagements = await _reportManagementService.GetAllReportManagementLogsAsync();

            if (reportManagements == null || reportManagements.Count == 0)
            {
                return NotFound(new { success = false, message = "未找到小说管理日志" });
            }

            var result = reportManagements.Select(nm => new
            {
                nm.Management!.ManagementId,
                nm.Management.ManagerId,
                ManagerName = nm.Management.Manager != null ? nm.Management.Manager.ManagerName : null,
                nm.ReportId,
                nm.Management.Result,
                Time = nm.Management.Time?.ToString("yyyy-MM-dd HH:mm:ss"),
            });

            return Ok(new
            {
                success = true,
                data = result
            });
        }
        /// <summary>
        /// 获取指定读者发布的所有举报及其管理处理进度
        /// </summary>
        /// <param name="readerId">读者ID</param>
        /// <returns>举报列表及处理进度</returns>
        [HttpGet("reader/{readerId}/reports-with-logs")]
        public async Task<IActionResult> GetReportsWithLogsByReader(int readerId)
        {
            var data = await _reportManagementService.GetReportsWithManagementLogsByReaderIdAsync(readerId);

            return Ok(new { success = true, data });
        }
    }
}
