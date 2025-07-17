using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.DTOs;


namespace OurNovel.Controllers
{
    /// <summary>
    /// �½ڹ����������������߹����½�����
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
        /// �û������½�
        /// </summary>
        /// <param name="dto">���� ReaderId, NovelId, ChapterId �Ĺ�������</param>
        /// <returns>������</returns>
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
                    message = "�������ڲ�����: " + ex.Message
                });
            }
        }
    }
}
