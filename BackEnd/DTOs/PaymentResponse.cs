namespace OurNovel.DTOs
{
    public class PaymentResponse
    {
        public bool Success { get; set; }
        public string PaymentUrl { get; set; }
        public string PaymentId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
