using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Purchase ʵ��������ݿ����ã�ͨ�� Fluent API��
    /// </summary>
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> entity)
        {
            // ����
            entity.ToTable("PURCHASE");

            // ������TRANSACTION_ID
            entity.HasKey(e => e.TransactionId);

            // �ֶ�ӳ��
            entity.Property(e => e.TransactionId)
                  .HasColumnName("TRANSACTION_ID");

            entity.Property(e => e.NovelId)
                  .HasColumnName("NOVEL_ID");

            entity.Property(e => e.ChapterId)
                  .HasColumnName("CHAPTER_ID");

            // ���1��Transaction (һ��һ)
            entity.HasOne(e => e.Transaction)
                  .WithOne()
                  .HasForeignKey<Purchase>(e => e.TransactionId)
                  .OnDelete(DeleteBehavior.Cascade);

            // ���2��Chapter (���һ)
            entity.HasOne(e => e.Chapter)
                  .WithMany()
                  .HasForeignKey(e => new { e.NovelId, e.ChapterId })
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
