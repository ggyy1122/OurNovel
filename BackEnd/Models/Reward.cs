using OurNovel.Models;

public class Reward
{
    public int TransactionId { get; set; }
    public int NovelId { get; set; }

    public Transaction Transaction { get; set; } = null!;
    public Novel Novel { get; set; } = null!;
}
