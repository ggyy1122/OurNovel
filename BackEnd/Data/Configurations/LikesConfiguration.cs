using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OurNovel.Models
{
    /// <summary>
    /// Like 实体的数据库配置类，配置其表结构、主键和关联关系。
    /// </summary>
    public class LikesConfiguration : IEntityTypeConfiguration<Like>
    {
        /// <summary>
        /// 配置 Like 实体与数据库表的映射关系。
        /// </summary>
        /// <param name="builder">实体构建器</param>
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            // 映射到数据库表名为 LIKES
            builder.ToTable("LIKES");

            // 配置复合主键：CommentId 和 ReaderId 共同作为主键
            builder.HasKey(l => new { l.CommentId, l.ReaderId });

            // 将属性映射到具体的列名
            builder.Property(l => l.CommentId).HasColumnName("COMMENT_ID");
            builder.Property(l => l.ReaderId).HasColumnName("READER_ID");

            // 设置与 Comment 的一对多关系（一个评论可被多个读者点赞）
            builder.HasOne(l => l.Comment)
                   .WithMany(c => c.LikesList)
                   .HasForeignKey(l => l.CommentId)
                   .OnDelete(DeleteBehavior.Cascade); // 删除评论时，级联删除对应的点赞

            // 设置与 Reader 的一对多关系（一个读者可以点赞多个评论）
            builder.HasOne(l => l.Reader)
                   .WithMany(r => r.LikedComments)
                   .HasForeignKey(l => l.ReaderId)
                   .OnDelete(DeleteBehavior.Cascade); // 删除读者时，级联删除其点赞
        }
    }
}
