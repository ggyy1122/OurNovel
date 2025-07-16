using System.Text.Json.Serialization;

namespace OurNovel.Models
{
    public class Reader : IEntity<int>
    {
        public int ReaderId { get; set; }               // READER_ID 主键
        int IEntity<int>.Id
        {
            get => ReaderId;
            set => ReaderId = value;
        }

        public string ReaderName { get; set; } = null!; // READER_NAME 不为空

        public string Password { get; set; } = null!;   // PASSWORD 不为空

        public string? Phone { get; set; }               // PHONE 可空

        public string? Gender { get; set; }              // GENDER 只能是 "男" 或 "女"

        public decimal Balance { get; set; } = 0m;       // BALANCE 默认0，整型

        public string? AvatarUrl { get; set; }           // AVATAR_URL 可空

        public string? BackgroundUrl { get; set; }       // BACKGROUND_URL 可空

        public string? IsCollectVisible { get; set; }    // IS_COLLECT_VISIBLE "是"/"否"

        public string? IsRecommendVisible { get; set; }  // IS_RECOMMEND_VISIBLE "是"/"否"

        [JsonIgnore]
        public ICollection<Like> LikedComments { get; set; } = new List<Like>();

    }
}
