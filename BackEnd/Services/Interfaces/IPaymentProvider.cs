using OurNovel.DTOs;
namespace OurNovel.Services.Interfaces
{
    // IPaymentProvider.cs
    public interface IPaymentProvider
    {
        /// <summary>
        /// 支付提供者名称(如Alipay、WeChatPay)
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// 创建支付
        /// </summary>
        Task<PaymentResponse> CreatePaymentAsync(PaymentRequest request);

        /// <summary>
        /// 验证支付通知
        /// </summary>
        Task<bool> VerifyNotificationAsync(HttpRequest request);

        /// <summary>
        /// 处理支付通知
        /// </summary>
        Task<PaymentNotificationResult> ProcessNotificationAsync(HttpRequest request);

        /// <summary>
        /// 查询支付状态
        /// </summary>
        Task<PaymentStatus> QueryPaymentStatusAsync(string paymentId);
    }

}
