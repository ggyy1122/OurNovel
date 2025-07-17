using System.Text.Json.Serialization;

namespace OurNovel.Models
{
    public class NovelManagement : IEntity<int>
    {
        public int ManagementId { get; set; }           // 主键，管理标识符
        public int NovelId { get; set; }                 // 外键，小说标识符

        int IEntity<int>.Id
        {
            get => ManagementId;
            set => ManagementId = value;
        }

        // 导航属性
        [JsonIgnore]
        public Management? Management { get; set; }

        [JsonIgnore]
        public Novel? Novel { get; set; }
    }
}
