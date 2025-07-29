using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurNovel.Models
{
    [Table("RECENT_READINGS")]
    public class RecentReadings
    {
        [Key, Column("READER_ID", Order = 0)]
        public int ReaderId { get; set; }

        [ForeignKey(nameof(ReaderId))]
        public Reader Reader { get; set; } = null!;

        [Key, Column("NOVEL_ID", Order = 1)]
        public int NovelId { get; set; }

        [ForeignKey(nameof(NovelId))]
        public Novel Novel { get; set; } = null!;

        [Column("RECENT_READING_TIME")]
        public DateTime RecentReadingTime { get; set; }

    }
}
