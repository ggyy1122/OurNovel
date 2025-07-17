using Microsoft.AspNetCore.Mvc;
using OurNovel.DTOs;
using OurNovel.Services;


namespace OurNovel.Controllers
{

    /// <summary>
    /// 仅作测试，该API不使用
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestRechargeController : ControllerBase
    {
        private readonly RechargeService _rechargeService;

        public TestRechargeController(RechargeService rechargeService)
        {
            _rechargeService = rechargeService;
        }

        /// <summary>
        /// 用户充值接口
        /// </summary>
        /// <param name="dto">充值请求数据</param>
        /// <returns>充值结果</returns>
        [HttpPost]
        public async Task<IActionResult> Recharge([FromBody] RechargeRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _rechargeService.RechargeAsync(dto);

            if (result)
            {
                return Ok(new { Success = true, Message = "充值成功" });
            }

            return BadRequest(new { Success = false, Message = "充值失败，请检查用户ID是否正确" });
        }
    }
}