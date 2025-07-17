using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.DTOs;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 打赏控制器，处理用户打赏行为
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RewardController : ControllerBase
    {
        private readonly RewardService _rewardService;

        public RewardController(RewardService rewardService)
        {
            _rewardService = rewardService;
        }

        /// <summary>
        /// 用户发起打赏
        /// </summary>
        /// <param name="dto">打赏请求信息（包含 readerId, novelId, amount）</param>
        /// <returns>打赏结果</returns>
        [HttpPost]
        public async Task<IActionResult> Reward([FromBody] RewardRequestDto dto)
        {
            try
            {
                var success = await _rewardService.RewardAsync(dto);
                if (!success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "余额不足或用户/小说不存在"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "打赏成功"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "服务器内部错误: " + ex.Message
                });
            }
        }
    }
}
