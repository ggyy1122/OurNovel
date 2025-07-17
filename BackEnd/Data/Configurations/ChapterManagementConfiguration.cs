using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    public class ChapterManagementConfiguration : IEntityTypeConfiguration<ChapterManagement>
    {
        public void Configure(EntityTypeBuilder<ChapterManagement> entity)
        {
            entity.ToTable("CHAPTER_MANAGEMENT");
            entity.HasKey(cm => cm.ManagementId);

            entity.Property(cm => cm.ManagementId).HasColumnName("MANAGEMENT_ID");
            entity.Property(cm => cm.NovelId).HasColumnName("NOVEL_ID");
            entity.Property(cm => cm.ChapterId).HasColumnName("CHAPTER_ID");

            entity.HasOne(cm => cm.Management)
                .WithMany()
                .HasForeignKey(cm => cm.ManagementId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(cm => cm.Novel)
                .WithMany()
                .HasForeignKey(cm => cm.NovelId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(cm => cm.Chapter)
                .WithMany()
                .HasForeignKey(cm => new { cm.NovelId, cm.ChapterId })
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
