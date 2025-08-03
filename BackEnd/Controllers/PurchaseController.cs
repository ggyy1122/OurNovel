using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.DTOs;


namespace OurNovel.Controllers
{
    /// <summary>
    /// 章节购买控制器，处理读者购买章节请求
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseService _purchaseService;

        public PurchaseController(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        /// <summary>
        /// 用户购买章节
        /// </summary>
        /// <param name="dto">包含 ReaderId, NovelId, ChapterId 的购买请求</param>
        /// <returns>购买结果</returns>
        [HttpPost]
        public async Task<IActionResult> Purchase([FromBody] ChapterPurchaseDto dto)
        {
            try
            {
                var result = await _purchaseService.PurchaseChapterAsync(dto);

                if (result.Success == 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = result.Message
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


        /// <summary>
        /// 查询某个读者是否已购买指定小说章节
        /// </summary>
        /// <param name="readerId">读者ID</param>
        /// <param name="novelId">小说ID</param>
        /// <param name="chapterId">章节ID</param>
        /// <returns>是否已购买</returns>
        [HttpGet("check")]
     
        public async Task<IActionResult> CheckPurchase([FromQuery] int readerId, [FromQuery] int novelId, [FromQuery] int chapterId)
        {
            try
            {
                bool hasPurchased = await _purchaseService.HasPurchasedChapterAsync(readerId, novelId, chapterId);
                return Ok(new { success = true, hasPurchased });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "服务器内部错误: " + ex.Message });
            }
        }

    }
}
