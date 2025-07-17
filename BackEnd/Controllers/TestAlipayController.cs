using Microsoft.AspNetCore.Mvc;
using OurNovel.DTOs;
using OurNovel.Services;

namespace OurNovel.Controllers
{

    /// <summary>
    /// 仅做测试，不使用
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AlipayTestController : ControllerBase
    {
        private readonly AlipayService _alipayService;
        private readonly IConfiguration _configuration;

        public AlipayTestController(AlipayService alipayService, IConfiguration configuration)
        {
            _alipayService = alipayService;
            _configuration = configuration;
        }

        /// <summary>
        /// 测试支付宝支付URL生成
        /// </summary>
        [HttpPost("test-url")]
        public IActionResult TestAlipayUrl([FromBody] AlipayTestRequest request)
        {
            try
            {
                // 从配置获取基础URL（替换为您的实际域名）
                string baseUrl = _configuration["BaseUrl"] ?? "https://yourdomain.com";

                // 调用支付宝服务
                string result = _alipayService.CreatePagePay(
                    outTradeNo: request.OutTradeNo ?? $"test_{DateTime.Now:yyyyMMddHHmmss}",
                    totalAmount: request.Amount,
                    subject: request.Subject ?? "测试商品",
                    notifyUrl: $"{baseUrl}/api/alipay/notify",
                    returnUrl: $"{baseUrl}/payment/return"
                );

                // 返回原始响应和类型分析
                return Ok(new
                {
                    RawResponse = result,
                    IsUrl = Uri.IsWellFormedUriString(result, UriKind.Absolute),
                    IsHtml = result.Contains("<form") || result.Contains("<html"),
                    Length = result.Length
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }

    public class AlipayTestRequest
    {
        public string? OutTradeNo { get; set; }
        public decimal Amount { get; set; }
        public string? Subject { get; set; }
    }
}