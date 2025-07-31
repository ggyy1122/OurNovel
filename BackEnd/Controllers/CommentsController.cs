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
        /// 设置评论状态（如“通过”或“封禁”）
        /// </summary>
        /// <param name="commentId">评论 ID</param>
        /// <param name="status">目标状态</param>
        /// <returns>操作结果</returns>
        [HttpPost("Status")]
        public async Task<IActionResult> SetStatus([FromQuery] int commentId, [FromQuery] string status, [FromQuery] int managerId, [FromQuery] string result)
        {
            try
            {
                await _commentsService.SetCommentStatusAsync(commentId, status, managerId, result);
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
        [HttpGet("ByChapter/{novelId}/{chapterId}")]
        public async Task<IActionResult> GetByChapter(int novelId, int chapterId)
        {
            var all = await _commentsService.GetAllAsync();
            var result = all.Where(c => c.NovelId == novelId && c.ChapterId == chapterId && c.Status == "通过");
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

        /// <summary>
        /// 递归删除评论及其所有子评论
        /// </summary>
        /// <param name="commentId">评论 ID</param>
        /// <returns>操作结果</returns>
        [HttpDelete("DeleteRecursive/{commentId}")]
        public async Task<IActionResult> DeleteRecursive(int commentId)
        {
            try
            {
                await _commentsService.DeleteCommentRecursivelyAsync(commentId);
                return Ok(new { success = true, message = $"已递归删除评论 {commentId} 及其子评论" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /*
        /// <summary>
        /// 审核评论
        /// </summary>
        [HttpPut("{id}/review")]
        public async Task<IActionResult> ReviewComment(int id, [FromQuery] string newStatus)
        {
            var success = await (_service as CommentsService)?.ReviewCommentAsync(id, newStatus)!;

            if (!success)
                return BadRequest("审核失败，可能是ID不存在或状态非法（必须为‘通过’或‘封禁’）");

            return Ok(new { success = true, message = "评论状态已更新" });
        }
        */

        /// <summary>
        /// 获取小说所有章节的第一级评论
        /// </summary>
        /// <param name="novelId">小说id</param>
        /// <returns></returns>
        [HttpGet("novel/{novelId}/top-level-comments")]
        public async Task<IActionResult> GetTopLevelComments(int novelId)
        {
            var comments = await _commentsService.GetTopLevelCommentsByNovelAsync(novelId);
            return Ok(comments);
        }

        /// <summary>
        /// 获取小说所有章节的第一级评论的点赞数排行
        /// </summary>
        /// <param name="novelId">小说id</param>
        /// <param name="topN">前几名</param>
        /// <returns></returns>
        [HttpGet("novel/{novelId}/top-liked-comments/{topN}")]
        public async Task<IActionResult> GetTopNTopLevelCommentsByLikes(int novelId, int topN)
        {
            var comments = await _commentsService.GetTopNTopLevelCommentsByLikesAsync(novelId, topN);
            return Ok(comments);
        }

        /// <summary>
        /// 获取指定章节的第一级评论（不作为回复的评论）
        /// </summary>
        [HttpGet("novel/{novelId}/chapter/{chapterId}/top-level-comments")]
        public async Task<IActionResult> GetTopLevelCommentsByChapter(int novelId, int chapterId)
        {
            var comments = await _commentsService.GetTopLevelCommentsByChapterAsync(novelId, chapterId);
            return Ok(comments);
        }

        /// <summary>
        /// 获取指定章节的第一级评论的点赞数排行
        /// </summary>
        [HttpGet("novel/{novelId}/chapter/{chapterId}/top-liked-comments/{topN}")]
        public async Task<IActionResult> GetTopLikedTopLevelCommentsByChapter(int novelId, int chapterId, int topN)
        {
            var comments = await _commentsService.GetTopNTopLevelCommentsByLikesByChapterAsync(novelId, chapterId, topN);
            return Ok(comments);
        }
        /// <summary>
        /// 根据小说ID获取所有评论数量
        /// </summary>
        [HttpGet("count-by-novel/{novelId}")]
        public async Task<IActionResult> GetCommentCountByNovel(int novelId)
        {
            var count = await _commentsService.GetCommentCountByNovelAsync(novelId);
            return Ok(new { novelId, commentCount = count });
        }

    }
}
