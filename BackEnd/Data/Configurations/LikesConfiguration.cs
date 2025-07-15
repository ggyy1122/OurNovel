using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OurNovel.Models
{
    public class LikesConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("LIKES");  // 对应数据库中的 LIKES 表

            builder.HasKey(l => new { l.CommentId, l.ReaderId }); // 复合主键

            builder.Property(l => l.CommentId).HasColumnName("COMMENT_ID");
            builder.Property(l => l.ReaderId).HasColumnName("READER_ID");

            builder.HasOne(l => l.Comment)
                   .WithMany(c => c.LikesList)
                   .HasForeignKey(l => l.CommentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.Reader)
                   .WithMany(r => r.LikedComments)
                   .HasForeignKey(l => l.ReaderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
