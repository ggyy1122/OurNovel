namespace OurNovel.DTOs
{
    public class PurchaseResultDto
    {
        public int Success { get; set; }  // 用 int 替代 bool：1 表示成功，0 表示失败
        public string Message { get; set; } = string.Empty;
    }
}
