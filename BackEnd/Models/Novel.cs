using System.Text.Json.Serialization;
namespace OurNovel.Models
{
    public class Novel : IEntity<int>
    {
        public int NovelId { get; set; }                   // Novel_ID 主键
        int IEntity<int>.Id
        {
            get => NovelId;
            set => NovelId = value;
        }

        public int AuthorId { get; set; }                  // 外键，对应 AuthorId
        [JsonIgnore]
        public Author? Author { get; set; }                // 导航属性     

        public string? NovelName { get; set; } = null!;    // Novel_NAME 可空

        public string? Introduction { get; set; } = null!; // Introduction 可空

        public DateTime? CreateTime { get; set; }          // CreateTime 可空

        public string? CoverUrl { get; set; }              // CoverUrl 可空

        public double? Score { get; set; }                 // Score 可空

        public long? TotalWordCount { get; set; } = 0;     // TotalWordCount，默认0

        public int? RecommendCount { get; set; } = 0;      // RecommendCount，默认0

        public int? CollectedCount { get; set; } = 0;      // CollectedCount，默认0

        public string? Status { get; set; } = "待审核";    // Status "待审核"/"连载"/"完结"/"封禁"

        public int OriginalNovelId { get; set; } = -1;     // OriginalNovelId，默认-1

        public decimal? TotalPrice { get; set; } = 0;      // TotalPrice，默认0

        // public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();  // 维护反向Chapter导航

    }
}
