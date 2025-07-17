using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.DTOs;

namespace OurNovel.Controllers
{
    /// <summary>
    /// ���Ϳ������������û�������Ϊ
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
        /// �û��������
        /// </summary>
        /// <param name="dto">����������Ϣ������ readerId, novelId, amount��</param>
        /// <returns>���ͽ��</returns>
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
                        message = "������û�/С˵������"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "���ͳɹ�"
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
