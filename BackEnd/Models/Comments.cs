using OurNovel.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Comment : IEntity<int>
{
    public int CommentId { get; set; }

    public int ReaderId { get; set; }
    public int NovelId { get; set; }
    public int ChapterId { get; set; }

    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public int Likes { get; set; } = 0;
    public string Status { get; set; } = "通过";
    public DateTime? CreateTime { get; set; }


    [JsonIgnore]
    public Reader? Reader { get; set; } = null!;


    [JsonIgnore]

    public Novel? Novel { get; set; } = null!;

    [JsonIgnore]
    public Chapter? Chapter { get; set; } = null!;

    int IEntity<int>.Id
    {
        get => CommentId;
        set => CommentId = value;
    }
}
