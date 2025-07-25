using Microsoft.AspNetCore.Mvc;
using OurNovel.DTOs;
using OurNovel.Services;

namespace OurNovel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WholePurchaseController : ControllerBase
    {
        private readonly WholePurchaseService _wholePurchaseService;

        public WholePurchaseController(WholePurchaseService wholePurchaseService)
        {
            _wholePurchaseService = wholePurchaseService;
        }

        /// <summary>
        /// 整本小说买断接口
        /// </summary>
        /// <param name="dto">包含 ReaderId 和 NovelId 的买断请求</param>
        /// <returns>购买结果（成功与否 + 消息）</returns>
        [HttpPost]
        public async Task<IActionResult> PurchaseWholeNovel([FromBody] WholePurchaseDto dto)
        {
            if (dto == null)
                return BadRequest("请求数据无效");

            var result = await _wholePurchaseService.PurchaseWholeNovelAsync(dto);

            if (result.Success == 1)
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// 查询读者是否已整本买断某小说
        /// </summary>
        /// <param name="readerId">读者ID</param>
        /// <param name="novelId">小说ID</param>
        /// <returns>是否买断结果</returns>
        [HttpGet("status")]
        public IActionResult CheckWholePurchaseStatus(int readerId, int novelId)
        {
            bool hasPurchased = _wholePurchaseService.HasPurchased(readerId, novelId);
            return Ok(new { hasPurchased });
        }
    }
}
