using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    public class RecentReadingsConfiguration : IEntityTypeConfiguration<RecentReadings>
    {
        public void Configure(EntityTypeBuilder<RecentReadings> builder)
        {
            builder.ToTable("RECENT_READINGS");

            // 复合主键
            builder.HasKey(rr => new { rr.ReaderId, rr.NovelId });

            // 关联 Reader
            builder.HasOne(rr => rr.Reader)
                   .WithMany()
                   .HasForeignKey(rr => rr.ReaderId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 关联 Novel
            builder.HasOne(rr => rr.Novel)
                   .WithMany()
                   .HasForeignKey(rr => rr.NovelId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 配置 RecentReadingTime 列，如果有额外配置需求，可以补充
            builder.Property(rr => rr.RecentReadingTime)
                   .IsRequired();
        }
    }
}
