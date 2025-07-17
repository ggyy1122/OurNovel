using System.Text.Json.Serialization;

namespace OurNovel.Models
{
    public class ChapterManagement : IEntity<int>
    {
        public int ManagementId { get; set; }
        public int NovelId { get; set; }
        public int ChapterId { get; set; }

        int IEntity<int>.Id
        {
            get => ManagementId;
            set => ManagementId = value;
        }

        [JsonIgnore]
        public Management? Management { get; set; }

        [JsonIgnore]
        public Novel? Novel { get; set; }

        [JsonIgnore]
        public Chapter? Chapter { get; set; }
    }
}
