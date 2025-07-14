
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
	/// <summary>
	/// Chapter 实体类的数据库配置（通过 Fluent API）
	/// </summary>
	public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
	{
		public void Configure(EntityTypeBuilder<Chapter> entity)
		{
			entity.ToTable("CHAPTER");

			// ? 设置复合主键：NOVEL_ID + CHAPTER_ID
			entity.HasKey(e => new { e.NovelId, e.ChapterId });

			// 主键字段映射
			entity.Property(e => e.NovelId).HasColumnName("NOVEL_ID");
			entity.Property(e => e.ChapterId).HasColumnName("CHAPTER_ID");

			// 标题字段
			entity.Property(e => e.Title)
				  .HasColumnName("TITLE")
				  .HasMaxLength(40)
				  .IsRequired();

			// 内容字段（CLOB 类型）
			entity.Property(e => e.Content)
				  .HasColumnName("CONTENT")
				  .HasColumnType("CLOB");

			// 字数字段
			entity.Property(e => e.WordCount)
				  .HasColumnName("WORD_COUNT")
				  .IsRequired();

			// 千字单价字段
			entity.Property(e => e.PricePerKilo)
				  .HasColumnName("PRICE_PER_KILO")
				  .HasColumnType("NUMBER(10,2)")
				  .HasDefaultValue(0.50m);

			// 是否收费字段
			entity.Property(e => e.IsCharged)
				  .HasColumnName("IS_CHARGED")
				  .HasMaxLength(2);

			// 发布时间字段
			entity.Property(e => e.PublishTime)
				  .HasColumnName("PUBLISH_TIME");

			// 状态字段
			entity.Property(e => e.Status)
				  .HasColumnName("STATUS")
				  .HasMaxLength(10)
				  .HasDefaultValue("通过");

			// 忽略自动计算字段
			entity.Ignore(e => e.CalculatedPrice);

            // ? 外键配置：CHAPTER(NOVEL_ID) → NOVEL(NOVEL_ID)，必须有对应的小说
            entity.HasOne(e => e.Novel)
				  .WithMany(n => n.Chapters)
				  .HasForeignKey(e => e.NovelId)
				  .IsRequired()
				  .OnDelete(DeleteBehavior.Cascade); // 小说被删时，章节也删除

            entity.Property(c => c.NovelId)
        .HasColumnName("NOVEL_ID");
        }
	}
}
