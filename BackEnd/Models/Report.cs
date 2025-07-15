using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace OurNovel.Models
{
    public class Report : IEntity<int>
    {
        public int ReportId { get; set; }                         // Report_ID 主键
        int IEntity<int>.Id
        {
            get => ReportId;
            set => ReportId = value;
        }
        public string? Reason { get; set; } = null!;              // Reason 可空

        public DateTime ReportTime { get; set; } = DateTime.Now;  // ReportTime
     

        public string Progress { get; set; } = "未处理";          // Progress "未处理"/"成功"/"失败"
        
        public int CommentId { get; set; }                        // 外键，对应 CommentId
        [JsonIgnore]
        public Comment? Comment { get; set; }                     // 导航属性 

        public int ReaderId { get; set; }                         // 外键，对应 ReaderId
        [JsonIgnore]
        public Reader? Reader { get; set; }                       // 导航属性
                                                                  
    }
}
