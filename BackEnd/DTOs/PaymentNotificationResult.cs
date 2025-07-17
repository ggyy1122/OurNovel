namespace OurNovel.DTOs
{
    public class PaymentNotificationResult
    {
        public bool Success { get; set; }
        public string OrderNo { get; set; }
        public decimal Amount { get; set; }
        public long UserId { get; set; }
        public DateTime PaymentTime { get; set; }
        public string TransactionId { get; set; }
    }
}
