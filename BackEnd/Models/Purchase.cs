using OurNovel.Models;

public class Purchase
{
    public int TransactionId { get; set; }
    public int NovelId { get; set; }
    public int ChapterId { get; set; }

    public Transaction Transaction { get; set; } = null!;
    public Chapter Chapter { get; set; } = null!;
}
