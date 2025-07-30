public class ReaderRewardRecord
{
    public int ReaderId { get; set; }
    public int TransactionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime RewardTime { get; set; }
    public int NovelId { get; set; }
    public string NovelTitle { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; }
}