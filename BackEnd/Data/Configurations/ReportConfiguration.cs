using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Report 实体类的数据库配置
    /// </summary>
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> entity)
        {
            // 表名
            entity.ToTable("REPORT");

            // 主键
            entity.HasKey(e => e.ReportId);

            // 字段映射
            entity.Property(e => e.ReportId).HasColumnName("REPORT_ID");

            entity.Property(e => e.Reason)
                  .HasColumnName("REASON")
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(e => e.ReportTime)
                  .HasColumnName("REPORT_TIME");

            entity.Property(e => e.Progress)
                  .HasColumnName("PROGRESS")
                  .HasMaxLength(10)
                  .IsRequired()
                  .HasDefaultValue("未处理");

            entity.Property(e => e.CommentId)
                  .HasColumnName("COMMENT_ID")
                  .IsRequired();

            entity.Property(e => e.ReaderId)
                  .HasColumnName("READER_ID")
                  .IsRequired();

            // 外键关系（可选配置，如果数据库启用外键约束）
            entity.HasOne(e => e.Comment)
                  .WithMany()
                  .HasForeignKey(e => e.CommentId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Reader)
                  .WithMany()
                  .HasForeignKey(e => e.ReaderId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
