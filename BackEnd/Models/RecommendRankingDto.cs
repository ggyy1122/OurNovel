using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurNovel.Models.Dto
{
    public class RecommendRankingDto
    {
        public int NovelId { get; set; }
        public string NovelName { get; set; } = null!;
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;

        public string CoverUrl { get; set; } = null!;
        public string Introduction { get; set; } = null!;

        public string? Status { get; set; }
        public DateTime? CreateTime { get; set; }

        public double? Score { get; set; }
        public int CollectedCount { get; set; }  

        public long? TotalWordCount { get; set; }
        public decimal? TotalPrice { get; set; }


        public int RecommendCount { get; set; }  // 推荐数量
    }
}
