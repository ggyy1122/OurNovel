namespace OurNovel.DTOs
{
    public class AlipayPaymentRequest
    {
        public string OutTradeNo { get; set; }
        public decimal TotalAmount { get; set; }
        public string Subject { get; set; }
    }
}
