using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
//using Newtonsoft.Json;

namespace OurNovel.Models
{
    /// <summary>
    /// 章节实体类，使用复合主键（NovelId + ChapterId）
    /// </summary>
    public class Chapter
    {   
        /// 所属小说ID（复合主键之一）
        public int NovelId { get; set; }

        /// 外键导航属性：章节所属小说（必须存在）
        [JsonIgnore]
        public Novel? Novel { get; set; } // 加上问号


        /// 章节编号（复合主键之一）
        public int ChapterId { get; set; }

        /// 标题，最大长度40，不能为空
        [Required]
        [MaxLength(40)]
        public string Title { get; set; } = null!;

        /// 章节内容（长文本）
        public string? Content { get; set; }

        /// 字数，不能为空
        [Required]
        public int WordCount { get; set; }

        /// 千字单价，默认0.50，保留两位小数
        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerKilo { get; set; } = 0.50m;

        /// 计算价格，自动计算（非映射字段）
        [NotMapped]
        public decimal CalculatedPrice =>
            Math.Round((WordCount / 1000m) * PricePerKilo, 2);

        /// 是否收费：是/否
        public string? IsCharged { get; set; }

        /// 发布时间
        public DateTime? PublishTime { get; set; }

        /// 状态：草稿/审核中/已发布/封禁（默认草稿）
        public string Status { get; set; } = "草稿";
    }
}
