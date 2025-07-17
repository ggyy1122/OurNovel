using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Reward ʵ��ı�ṹ����
    /// </summary>
    public class RewardConfiguration : IEntityTypeConfiguration<Reward>
    {
        public void Configure(EntityTypeBuilder<Reward> entity)
        {
            entity.ToTable("REWARD");

            entity.HasKey(e => e.TransactionId); // ������ TransactionId

            entity.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID");
            entity.Property(e => e.NovelId).HasColumnName("NOVEL_ID");

            entity.HasOne(e => e.Transaction)
                .WithOne() // �������ã��޵�������
                .HasForeignKey<Reward>(e => e.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Novel)
                .WithMany()
                .HasForeignKey(e => e.NovelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
