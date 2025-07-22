using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// WholePurchase 实体类的数据库配置（通过 Fluent API）
    /// </summary>
    public class WholePurchaseConfiguration : IEntityTypeConfiguration<WholePurchase>
    {
        public void Configure(EntityTypeBuilder<WholePurchase> entity)
        {
            // 表名
            entity.ToTable("WHOLE_PURCHASE");

            // 主键：ReaderId + NovelId 复合主键
            entity.HasKey(e => new { e.ReaderId, e.NovelId });

            // 字段映射
            entity.Property(e => e.ReaderId)
                  .HasColumnName("READER_ID");

            entity.Property(e => e.NovelId)
                  .HasColumnName("NOVEL_ID");

            entity.Property(e => e.IsBought)
                  .HasColumnName("IS_BOUGHT")
                  .HasMaxLength(2)
                  .HasDefaultValue("否");

            // 外键1：Reader
            entity.HasOne(e => e.Reader)
                  .WithMany()
                  .HasForeignKey(e => e.ReaderId)
                  .OnDelete(DeleteBehavior.Cascade);

            // 外键2：Novel
            entity.HasOne(e => e.Novel)
                  .WithMany()
                  .HasForeignKey(e => e.NovelId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
