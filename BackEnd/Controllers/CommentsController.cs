using Microsoft.AspNetCore.Mvc;
using OurNovel.Models;
using OurNovel.Services;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 评论控制器，继承基础控制器，实现对 Comment 实体的 CRUD 操作和扩展功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : BaseController<Comment, int>
    {
        private readonly CommentsService _commentsService;

        public CommentsController(CommentsService commentsService) : base(commentsService)
        {
            _commentsService = commentsService;
        }

        /// <summary>
        /// 点赞某条评论（将 Likes +1）
        /// </summary>
        /// <param name="id">评论 ID</param>
        /// <returns>操作结果</returns>
        [HttpPost("Like/{id}")]
        public async Task<IActionResult> Like(int id)
        {
            try
            {
                await _commentsService.LikeCommentAsync(id);
                return Ok(new { success = true, message = "点赞成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 设置评论状态（如“通过”或“封禁”）
        /// </summary>
        /// <param name="commentId">评论 ID</param>
        /// <param name="status">目标状态</param>
        /// <returns>操作结果</returns>
        [HttpPost("Status")]
        public async Task<IActionResult> SetStatus([FromForm] int commentId, [FromForm] string status)
        {
            try
            {
                await _commentsService.SetCommentStatusAsync(commentId, status);
                return Ok(new { success = true, message = $"状态已修改为：{status}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 获取某章节下所有通过审核的评论
        /// </summary>
        /// <param name="chapterId">章节 ID</param>
        /// <returns>评论列表</returns>
        [HttpGet("ByChapter/{chapterId}")]
        public async Task<IActionResult> GetByChapter(int chapterId)
        {
            var all = await _commentsService.GetAllAsync();
            var result = all.Where(c => c.ChapterId == chapterId && c.Status == "通过");
            return Ok(result);
        }

        /// <summary>
        /// 获取某小说下所有通过审核的评论
        /// </summary>
        /// <param name="novelId">小说 ID</param>
        /// <returns>评论列表</returns>
        [HttpGet("ByNovel/{novelId}")]
        public async Task<IActionResult> GetByNovel(int novelId)
        {
            var all = await _commentsService.GetAllAsync();
            var result = all.Where(c => c.NovelId == novelId && c.Status == "通过");
            return Ok(result);
        }
    }
}
