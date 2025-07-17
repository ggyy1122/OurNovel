using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OurNovel.Models;

/// <summary>
/// NovelManagement 实体类的数据库配置
/// </summary>
public class NovelManagementConfiguration : IEntityTypeConfiguration<NovelManagement>
{
    public void Configure(EntityTypeBuilder<NovelManagement> entity)
    {
        entity.ToTable("NOVEL_MANAGEMENT");

        entity.HasKey(e => e.ManagementId);

        entity.Property(e => e.ManagementId)
            .HasColumnName("MANAGEMENT_ID")
            .IsRequired();

        entity.Property(e => e.NovelId)
            .HasColumnName("NOVEL_ID")
            .IsRequired();

        // 外键约束，管理记录表
        entity.HasOne(e => e.Management)
            .WithMany()
            .HasForeignKey(e => e.ManagementId)
            .OnDelete(DeleteBehavior.Cascade);

        // 外键约束，小说表
        entity.HasOne(e => e.Novel)
            .WithMany()
            .HasForeignKey(e => e.NovelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
