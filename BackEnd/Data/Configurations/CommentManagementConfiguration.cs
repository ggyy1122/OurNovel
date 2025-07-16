using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OurNovel.Models;

/// <summary>
/// CommentManagement 实体类的数据库配置
/// </summary>
public class CommentManagementConfiguration : IEntityTypeConfiguration<CommentManagement>
{
    public void Configure(EntityTypeBuilder<CommentManagement> entity)
    {
        entity.ToTable("COMMENT_MANAGEMENT");

        // 修改主键为 ManagementId
        entity.HasKey(e => e.ManagementId);

        entity.Property(e => e.ManagementId)
            .HasColumnName("MANAGEMENT_ID")
            .IsRequired();

        entity.Property(e => e.CommentId)
            .HasColumnName("COMMENT_ID")
            .IsRequired();

        // 外键约束
        entity.HasOne(e => e.Management)
            .WithMany()
            .HasForeignKey(e => e.ManagementId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Comment)
            .WithMany()
            .HasForeignKey(e => e.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
