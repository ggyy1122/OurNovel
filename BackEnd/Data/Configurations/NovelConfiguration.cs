using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Novel 实体类的数据库配置（通过 Fluent API）
    /// 实现 IEntityTypeConfiguration<T> 接口定义表结构
    /// </summary>
    public class NovelConfiguration : IEntityTypeConfiguration<Novel>
    {
        /// <summary>
        /// 配置 Novel 实体对应的数据库表结构
        /// </summary>
        /// <param name="entity">实体构建器，用于配置字段属性和关系</param>
        public void Configure(EntityTypeBuilder<Novel> entity)
        {
            // 表名
            entity.ToTable("NOVEL");

            // 主键
            entity.HasKey(e => e.NovelId);

            // 字段映射
            entity.Property(e => e.NovelId).HasColumnName("NOVEL_ID");
            entity.Property(e => e.AuthorId).HasColumnName("AUTHOR_ID");
            entity.Property(e => e.NovelName).HasColumnName("NOVEL_NAME");
            entity.Property(e => e.Introduction).HasColumnName("INTRODUCTION");
            entity.Property(e => e.CreateTime).HasColumnName("CREATE_TIME");
            entity.Property(e => e.CoverUrl).HasColumnName("COVER_URL");
            entity.Property(e => e.Score).HasColumnName("SCORE");
            entity.Property(e => e.TotalWordCount).HasColumnName("TOTAL_WORD_COUNT");
            entity.Property(e => e.RecommendCount).HasColumnName("RECOMMEND_COUNT");
            entity.Property(e => e.CollectedCount).HasColumnName("COLLECTED_COUNT");
            entity.Property(e => e.Status)
                  .HasColumnName("STATUS")
                  .HasDefaultValue("待审核");
            entity.Property(e => e.OriginalNovelId)
                  .HasColumnName("ORIGINAL_NOVEL_ID")
                  .HasDefaultValue(-1);
            entity.Property(e => e.TotalPrice)
                  .HasColumnName("TOTAL_PRICE")
                  .HasDefaultValue(0m);
        }
    }
}