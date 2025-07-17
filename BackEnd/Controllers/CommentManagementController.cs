using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using System.Linq;
using System.Threading.Tasks;

namespace OurNovel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentManagementController : ControllerBase
    {
        private readonly CommentManagementService _commentsManagementService;

        public CommentManagementController(CommentManagementService commentsManagementService)
        {
            _commentsManagementService = commentsManagementService;
        }

        /// <summary>
        /// 获取指定评论的管理日志列表（包含管理员信息，时间倒序）
        /// GET: api/CommentManagement/logs/{commentId}
        /// </summary>
        /// <param name="commentId">评论ID</param>
        /// <returns>管理日志列表</returns>
        [HttpGet("logs/{commentId}")]
        public async Task<IActionResult> GetLogs(int commentId)
        {
            var logs = await _commentsManagementService.GetManagementLogsByCommentIdAsync(commentId);

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
        /// 获取所有评论的管理日志列表
        /// GET: api/CommentManagement/logs/all
        /// </summary>
        /// <returns>管理日志列表</returns>
        [HttpGet("logs/all")]
        public async Task<IActionResult> GetAllNovelLogs()
        {
            var commentManagements = await _commentsManagementService.GetAllCommentManagementLogsAsync();

            if (commentManagements == null || commentManagements.Count == 0)
            {
                return NotFound(new { success = false, message = "未找到评论管理日志" });
            }

            var result = commentManagements.Select(nm => new
            {
                nm.Management!.ManagementId,
                nm.Management.ManagerId,
                ManagerName = nm.Management.Manager != null ? nm.Management.Manager.ManagerName : null,
                nm.CommentId,
                nm.Management.Result,
                Time = nm.Management.Time?.ToString("yyyy-MM-dd HH:mm:ss"),
            });

            return Ok(new
            {
                success = true,
                data = result
            });
        }
    }
}
