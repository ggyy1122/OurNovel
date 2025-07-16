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
    }
}
