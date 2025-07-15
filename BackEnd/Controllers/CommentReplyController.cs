using Microsoft.AspNetCore.Mvc;
using OurNovel.Models;
using OurNovel.Services;

namespace OurNovel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentReplyController : ControllerBase
    {
        private readonly ICommentReplyService _replyService;

        public CommentReplyController(ICommentReplyService replyService)
        {
            _replyService = replyService;
        }

        /// <summary>
        /// 添加一条评论回复
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddReply([FromBody] CommentReply reply)
        {
            if (reply == null || reply.CommentLevel < 1 || reply.CommentLevel > 3)
            {
                return BadRequest("评论层级无效或数据为空。");
            }

            await _replyService.AddReplyAsync(reply);
            return Ok("评论回复添加成功");
        }

        /// <summary>
        /// 根据评论ID获取它的回复关系
        /// </summary>
        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetReplyByCommentId(int commentId)
        {
            var reply = await _replyService.GetReplyByCommentIdAsync(commentId);
            if (reply == null)
                return NotFound();

            return Ok(reply);
        }

        /// <summary>
        /// 获取某条评论下的所有直接回复
        /// </summary>
        [HttpGet("parent/{parentId}")]
        public async Task<IActionResult> GetRepliesByParentId(int parentId)
        {
            var replies = await _replyService.GetRepliesByParentIdAsync(parentId);
            return Ok(replies);
        }
    }
}
