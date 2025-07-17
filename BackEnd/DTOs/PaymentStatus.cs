namespace OurNovel.DTOs
{
    public enum PaymentStatus
    {
        Pending,    // 等待支付
        Success,    // 支付成功
        Failed,     // 支付失败
        Closed,     // 交易关闭
        Refunded    // 已退款
    }
}
