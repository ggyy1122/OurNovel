using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurNovel.Models
{
    [Table("NOVEL_CATEGORY")]
    public class NovelCategory
    {
        [Key, Column("NOVEL_ID", Order = 0)]
        public int NovelId { get; set; }

        [ForeignKey(nameof(NovelId))]
        public Novel Novel { get; set; } = null!;

        [Key, Column("CATEGORY_NAME", Order = 1)]
        public string CategoryName { get; set; } = null!;

        [ForeignKey(nameof(CategoryName))]
        public Category Category { get; set; } = null!;
    }
}
