using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Purchase 实体类的数据库配置（通过 Fluent API）
    /// </summary>
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> entity)
        {
            // 表名
            entity.ToTable("PURCHASE");

            // 主键：TRANSACTION_ID
            entity.HasKey(e => e.TransactionId);

            // 字段映射
            entity.Property(e => e.TransactionId)
                  .HasColumnName("TRANSACTION_ID");

            entity.Property(e => e.NovelId)
                  .HasColumnName("NOVEL_ID");

            entity.Property(e => e.ChapterId)
                  .HasColumnName("CHAPTER_ID");

            // 外键1：Transaction (一对一)
            entity.HasOne(e => e.Transaction)
                  .WithOne()
                  .HasForeignKey<Purchase>(e => e.TransactionId)
                  .OnDelete(DeleteBehavior.Cascade);

            // 外键2：Chapter (多对一)
            entity.HasOne(e => e.Chapter)
                  .WithMany()
                  .HasForeignKey(e => new { e.NovelId, e.ChapterId })
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
