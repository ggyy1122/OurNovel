using OurNovel.Models;
using System.Text.Json.Serialization;

public class ReportManagement : IEntity<int>
{
    // 主键，和数据库一致
    public int ManagementId { get; set; }

    // 外键
    public int ReportId { get; set; }

    int IEntity<int>.Id
    {
        get => ManagementId;
        set => ManagementId = value;
    }

    // 导航属性
    [JsonIgnore]
    public Management? Management { get; set; }

    [JsonIgnore]
    public Report? Report { get; set; }
}
