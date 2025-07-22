using OurNovel.Models;

public class WholePurchase
{
    /// <summary>
    /// WholePurchase：整本小说买断记录
    /// </summary>
    public int ReaderId { get; set; }
    public int NovelId { get; set; }
    public string IsBought { get; set; } = "否";

    public Reader Reader { get; set; } = null!;
    public Novel Novel { get; set; } = null!;
}
