using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;

namespace OurNovel.Services
{
    /// <summary>
    /// 支付宝支付服务
    /// </summary>
    public class AlipayService
    {
        private readonly AlipayConfig _config;

        public AlipayService(IConfiguration configuration)
        {
            _config = new AlipayConfig
            {
                ServerUrl = configuration["Alipay:ServerUrl"],
                AppId = configuration["Alipay:AppId"],
                PrivateKey = configuration["Alipay:PrivateKey"],
                AlipayPublicKey = configuration["Alipay:AlipayPublicKey"],
                Format = "json",
                Charset = "UTF-8",
                SignType = "RSA2"
            };
        }

        /// <summary>
        /// 创建电脑网站支付
        /// </summary>
        /// <param name="outTradeNo">商户订单号</param>
        /// <param name="totalAmount">订单金额</param>
        /// <param name="subject">订单标题</param>
        /// <param name="notifyUrl">异步通知地址</param>
        /// <param name="returnUrl">同步跳转地址</param>
        /// <returns>支付页面HTML或错误信息</returns>
        public string CreatePagePay(
            string outTradeNo,
            decimal totalAmount,
            string subject,
            string notifyUrl,
            string returnUrl)
        {
            try
            {
                IAopClient client = new DefaultAopClient(_config);
                var request = new AlipayTradePagePayRequest();

                var model = new AlipayTradePagePayModel
                {
                    OutTradeNo = outTradeNo,
                    TotalAmount = totalAmount.ToString("0.00"),
                    Subject = subject,
                    ProductCode = "FAST_INSTANT_TRADE_PAY"
                };

                request.SetBizModel(model);
                request.SetNotifyUrl(notifyUrl);
                request.SetReturnUrl(returnUrl);

                var response = client.pageExecute(request, null, "GET");

                if (!response.IsError)
                {
                    return response.Body;
                }

                throw new Exception($"支付宝接口调用失败: {response.Msg}");
            }
            catch (Exception ex)
            {
                throw new Exception("创建支付宝支付失败", ex);
            }
        }
    }
}