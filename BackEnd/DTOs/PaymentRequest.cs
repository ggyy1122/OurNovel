namespace OurNovel.DTOs
{
    public class PaymentRequest
    {
        public string OrderNo { get; set; }
        public decimal Amount { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public long UserId { get; set; }
        public string ReturnUrl { get; set; }
        public string NotifyUrl { get; set; }
    }

}
