namespace OurNovel.DTOs
{
    public class RewardRequestDto
    {
        public long ReaderId { get; set; }
        public long NovelId { get; set; }
        public decimal Amount { get; set; }
    }
}
