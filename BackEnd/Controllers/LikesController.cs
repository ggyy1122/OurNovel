using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 点赞控制器，提供对评论点赞与取消点赞的操作
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly ILikesService _likesService;

        public LikesController(ILikesService likesService)
        {
            _likesService = likesService;
        }

        /// <summary>
        /// 给指定评论点赞
        /// </summary>
        /// <param name="commentId">评论 ID</param>
        /// <param name="readerId">读者 ID</param>
        /// <returns>操作结果</returns>
        [HttpPost("Like")]
        public async Task<IActionResult> Like([FromForm] int commentId, [FromForm] int readerId)
        {
            try
            {
                await _likesService.LikeAsync(commentId, readerId);
                return Ok(new { success = true, message = "点赞成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 取消点赞某条评论
        /// </summary>
        /// <param name="commentId">评论 ID</param>
        /// <param name="readerId">读者 ID</param>
        /// <returns>操作结果</returns>
        [HttpPost("Unlike")]
        public async Task<IActionResult> Unlike([FromForm] int commentId, [FromForm] int readerId)
        {
            try
            {
                await _likesService.UnlikeAsync(commentId, readerId);
                return Ok(new { success = true, message = "已取消点赞" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 检查读者是否已点赞某条评论
        /// </summary>
        /// <param name="commentId">评论 ID</param>
        /// <param name="readerId">读者 ID</param>
        /// <returns>是否已点赞</returns>
        [HttpGet("IsLiked")]
        public async Task<IActionResult> IsLiked([FromQuery] int commentId, [FromQuery] int readerId)
        {
            var liked = await _likesService.IsLikedAsync(commentId, readerId);
            return Ok(new { commentId, readerId, liked });
        }

        /// <summary>
        /// 获取某条评论的点赞数量
        /// </summary>
        /// <param name="commentId">评论 ID</param>
        /// <returns>点赞数</returns>
        [HttpGet("Count/{commentId}")]
        public async Task<IActionResult> GetLikeCount(int commentId)
        {
            var count = await _likesService.GetLikeCountAsync(commentId);
            return Ok(new { commentId, likeCount = count });
        }
    }
}
