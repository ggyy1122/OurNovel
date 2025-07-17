using System.Text.Json.Serialization;

namespace OurNovel.Models
{
    public class CommentManagement : IEntity<int>
    {
        // 删除 CommentManagementId，因为数据库表无此列
        // 使用 ManagementId 作为主键

        public int ManagementId { get; set; }           // 主键，管理记录ID，同时也是外键
        public int CommentId { get; set; }              // 外键，评论ID

        int IEntity<int>.Id
        {
            get => ManagementId;
            set => ManagementId = value;
        }

        // 导航属性
        [JsonIgnore]
        public Management? Management { get; set; }

        [JsonIgnore]
        public Comment? Comment { get; set; }
    }
}
