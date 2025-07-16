using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OurNovel.Data.Configurations
{
    public class CommentReplyConfiguration : IEntityTypeConfiguration<CommentReply>
    {
        public void Configure(EntityTypeBuilder<CommentReply> builder)
        {
            builder.HasKey(l => new { l.CommentId, l.PreComId }); // 复合主键

            builder.Property(l => l.CommentId).HasColumnName("COMMENT_ID");
            builder.Property(l => l.PreComId).HasColumnName("PRE_COM_ID");
            builder.Property(l => l.CommentLevel).HasColumnName("COMMENT_LEVEL");
            builder.ToTable("COMMENT_REPLY");

            builder.HasKey(cr => cr.CommentId);

            builder.Property(cr => cr.CommentLevel)
                   .IsRequired();

            builder.HasOne(cr => cr.Comment)
                   .WithOne()
                   .HasForeignKey<CommentReply>(cr => cr.CommentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cr => cr.ParentComment)
                   .WithMany()
                   .HasForeignKey(cr => cr.PreComId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
