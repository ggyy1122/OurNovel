using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurNovel.Models.Dto
{
    public class CollectRankingDto
    {
        public int NovelId { get; set; }
        public string NovelName { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public string CoverUrl { get; set; } = null!;
        public string Introduction { get; set; } = null!;
        public int CollectCount { get; set; }  // 收藏数量
    }
}
