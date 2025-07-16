using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using OurNovel.Models;

public class CommentReply
{
    [Key]
    public int CommentId { get; set; }  // 当前评论 ID（同时是主键）

    public int? PreComId { get; set; }  // 父评论 ID（可以为 null）

    [Range(1, 3)]
    public int CommentLevel { get; set; }

    // 导航属性（可选）
    [JsonIgnore]
    [ForeignKey(nameof(CommentId))]
    public Comment? Comment { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(PreComId))]
    public Comment? ParentComment { get; set; }
}
