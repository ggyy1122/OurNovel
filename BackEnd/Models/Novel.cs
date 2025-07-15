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

        public int AuthorId { get; set; }                  // 外键，对应 author_id
        [JsonIgnore]
        public Author? Author { get; set; }                // 导航属性     

        public string? NovelName { get; set; } = null!;    // Novel_NAME 可空

        public string? Introduction { get; set; } = null!; // Introduction 可空

        public DateTime? CreateTime { get; set; }          // CreateTime 可空

        public string? CoverUrl { get; set; }              // CoverUrl 可空

        public int? Score { get; set; }                    // Score 可空

        public long? TotalWordCount { get; set; }          // TotalWordCount 可空

        public int? RecommendCount { get; set; }           // RecommendCount 可空

        public int? CollectedCount { get; set; }             // CollectedCount 可空

        public string? Status { get; set; } = "待审核";    // IS_RECOMMEND_VISIBLE "待审核"/"连载"/"完结"

        // public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();  // 维护反向Chapter导航
    }
}
