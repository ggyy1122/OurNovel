using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Comment 实体类的 Fluent API 配置
    /// </summary>
    public class CommentsConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> entity)
        {
            entity.ToTable("COMMENTS");

            entity.HasKey(e => e.CommentId);

            entity.Property(e => e.CommentId)
                  .HasColumnName("COMMENT_ID");

            entity.Property(e => e.ReaderId)
                 .HasColumnName("READER_ID")
                 .IsRequired();

            entity.Property(e => e.NovelId)
                  .HasColumnName("NOVEL_ID")
                .IsRequired();

             entity.Property(e => e.ChapterId)
               .HasColumnName("CHAPTER_ID")
               .IsRequired();

            entity.Property(e => e.Title)
                  .HasColumnName("TITLE")
                  .HasMaxLength(40)
                  .IsRequired();

            entity.Property(e => e.Content)
                  .HasColumnName("CONTENT")
                  .HasColumnType("CLOB");

            entity.Property(e => e.Likes)
                  .HasColumnName("LIKES")
                  .HasDefaultValue(0);

            entity.Property(e => e.Status)
                  .HasColumnName("STATUS")
                  .HasMaxLength(10)
                  .HasDefaultValue("通过");

            entity.Property(e => e.CreateTime)
                  .HasColumnName("CREATE_TIME")
                  .HasColumnType("DATE");

            // 外键约束

            entity.HasOne(c => c.Reader)
                      .WithMany()
                      .HasForeignKey(c => c.ReaderId)
                      .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Novel)
                  .WithMany()
                  .HasForeignKey(c => c.NovelId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Chapter)
                   .WithMany()
                  .HasForeignKey(c => new { c.NovelId, c.ChapterId })
                  .HasPrincipalKey(ch => new { ch.NovelId, ch.ChapterId })
                  .OnDelete(DeleteBehavior.Restrict);

        }
    }
}