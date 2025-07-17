namespace OurNovel.Models
{
    /// <summary>
    /// 作者收入记录表
    /// </summary>
    public class AuthorIncome
    {
        public long Id { get; set; }
        public int AuthorId { get; set; }
        public string Type { get; set; } = "打赏"; // 可扩展类型：打赏、分成等
        public decimal Amount { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
