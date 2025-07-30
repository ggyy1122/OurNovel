// V_READER_SUBSCRIPTION_RECORD
public class ReaderSubscriptionRecord
{
    public int ReaderId { get; set; }
    public int ChapterId { get; set; }
    public string ChapterTitle { get; set; }
    public int NovelId { get; set; }
    public string NovelTitle { get; set; }
    public decimal ConsumeAmount { get; set; }
    public DateTime ConsumeTime { get; set; }
}