using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using OurNovel.DTOs;
using OurNovel.Services;

[ApiController]
[Route("api/[controller]")]
public class RechargeController : ControllerBase
{
    private readonly AlipayService _alipayService;
    private readonly RechargeService _rechargeService;
    private readonly IConfiguration _config;

    public RechargeController(
        AlipayService alipayService,
        RechargeService rechargeService,
        IConfiguration config)
    {
        _alipayService = alipayService;
        _rechargeService = rechargeService;
        _config = config;
    }

    /// <summary>
    /// 发起充值并返回支付URL
    /// </summary>
    [HttpPost("start")]
    public async Task<IActionResult> StartRecharge([FromBody] RechargeRequestDto dto)
    {
        // 参数校验
        if (dto.Amount <= 0)
            return BadRequest("金额必须大于0");

        // 生成唯一订单号 (格式: recharge_[用户ID]_[时间戳])
        string outTradeNo = $"recharge_{dto.ReaderId}_{DateTime.Now:yyyyMMddHHmmss}";

        // 调用支付宝
        string baseUrl = _config["BaseUrl"]!;
        string paymentUrl = _alipayService.CreatePagePay(
            outTradeNo: outTradeNo,
            totalAmount: dto.Amount,
            subject: $"用户{dto.ReaderId}余额充值",
            notifyUrl: $"{baseUrl}/api/recharge/notify",
            returnUrl: $"{baseUrl}/recharge/result?userId={dto.ReaderId}"
        );

        return Ok(new { PaymentUrl = paymentUrl });
    }

    /// <summary>
    /// 支付宝异步通知（后台调用）
    /// </summary>
    [HttpPost("notify")]
    public async Task<IActionResult> HandleAlipayNotify()
    {
        try
        {
            // 1. 验证支付宝签名（需实现）
            var form = await Request.ReadFormAsync();
            if (!VerifyAlipaySignature(form))
                return BadRequest("签名验证失败");

            // 2. 解析订单信息
            string outTradeNo = form["out_trade_no"].ToString();
            decimal amount = decimal.Parse(form["total_amount"]);
            int userId = int.Parse(outTradeNo.Split('_')[1]); // 从订单号提取用户ID

            // 3. 调用充值服务
            var result = await _rechargeService.RechargeAsync(new RechargeRequestDto
            {
                ReaderId = userId,
                Amount = amount*100
            });

            return result ? Ok("success") : BadRequest("充值失败");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    private bool VerifyAlipaySignature(IFormCollection form)
    {
        // 实现支付宝签名验证逻辑
        return true;
    }
}