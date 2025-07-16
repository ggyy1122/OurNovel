using Microsoft.AspNetCore.Mvc;
using OurNovel.Models;
using OurNovel.Services;
using System;
using System.Threading.Tasks;

namespace OurNovel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : BaseController<Report, int>
    {
        private readonly ReportService _reportService;

        public ReportsController(ReportService reportService) : base(reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// 举报评论
        /// </summary>
        [HttpPost("/api/comment/{commentId}/report")]
        public async Task<IActionResult> ReportComment(int commentId, [FromForm] int readerId, [FromForm] string reason)
        {
            try
            {
                await _reportService.ReportCommentAsync(commentId, readerId, reason);
                return Ok(new { success = true, message = "举报成功" });
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { success = false, message });
            }
        }

        /// <summary>
        /// 处理举报
        /// </summary>
        [HttpPost("/api/report/{reportId}/process")]
        public async Task<IActionResult> ProcessReport(int reportId, [FromForm] string progress)
        {
            // 从登录态获取管理员ID
            var managerIdStr = User.FindFirst("managerId")?.Value;
            if (!int.TryParse(managerIdStr, out var managerId))
            {
                return Unauthorized(new { success = false, message = "未登录或管理员身份信息无效" });
            }

            try
            {
                await _reportService.ProcessReportAsync(reportId, progress, managerId);
                return Ok(new { success = true, message = "处理成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        // GetAll 和 GetById 等基础接口继承自 BaseController，无需重复编写
    }
}
