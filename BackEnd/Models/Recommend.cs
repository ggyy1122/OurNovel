using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurNovel.Models
{
    [Table("RECOMMEND")]
    public class Recommend
    {
        [Key, Column("NOVEL_ID", Order = 0)]
        public int NovelId { get; set; }

        [ForeignKey(nameof(NovelId))]
        public Novel Novel { get; set; } = null!;

        [Key, Column("READER_ID", Order = 1)]
        public int ReaderId { get; set; }

        [ForeignKey(nameof(ReaderId))]
        public Reader Reader { get; set; } = null!;

        [Column("REASON")]
        public string? Reason { get; set; }
    }
}