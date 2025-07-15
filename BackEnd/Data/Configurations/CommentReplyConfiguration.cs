using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OurNovel.Data.Configurations
{
    public class CommentReplyConfiguration : IEntityTypeConfiguration<CommentReply>
    {
        public void Configure(EntityTypeBuilder<CommentReply> builder)
        {
            builder.ToTable("comment_reply");

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
