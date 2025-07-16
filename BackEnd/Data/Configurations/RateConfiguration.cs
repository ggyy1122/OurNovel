using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    public class RateConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.ToTable("RATE");

            builder.HasKey(r => new { r.NovelId, r.ReaderId });

            builder.Property(r => r.Score)
                   .HasColumnName("SCORE")
                   .IsRequired();

            builder.Property(r => r.RatingTime)
                   .HasColumnName("RATING_TIME")
                   .IsRequired();

            builder.HasOne(r => r.Novel)
                   .WithMany()
                   .HasForeignKey(r => r.NovelId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Reader)
                   .WithMany()
                   .HasForeignKey(r => r.ReaderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}