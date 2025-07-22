namespace OurNovel.DTOs
{
    /// <summary>
    /// 整本小说买断请求数据传输对象
    /// </summary>
    public class WholePurchaseDto
    {
        public int ReaderId { get; set; }     // 读者 ID
        public int NovelId { get; set; }      // 小说 ID（对应 Novel.NovelId）
    }
}
