using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Management 实体的数据库表配置类
    /// </summary>
    public class ManagementConfiguration : IEntityTypeConfiguration<Management>
    {
        public void Configure(EntityTypeBuilder<Management> entity)
        {
            entity.ToTable("MANAGEMENT");

            // 主键
            entity.HasKey(e => e.ManagementId);

            entity.Property(e => e.ManagementId)
                  .HasColumnName("MANAGEMENT_ID")
                  .ValueGeneratedOnAdd(); // Oracle中使用trigger+sequence自动生成

            entity.Property(e => e.ManagerId)
                  .HasColumnName("MANAGER_ID")
                  .IsRequired();

            entity.Property(e => e.Result)
                  .HasColumnName("RESULT")
                  .HasMaxLength(200);

            entity.Property(e => e.Time)
                  .HasColumnName("TIME")
                  .HasColumnType("DATE")
                  .HasDefaultValueSql("SYSDATE"); // 设置默认时间为当前系统时间（Oracle）

            // 可选：设置外键约束（假如 MANAGER 是主表）
            // entity.HasOne<Manager>()
            //       .WithMany()
            //       .HasForeignKey(e => e.ManagerId)
            //       .OnDelete(DeleteBehavior.Restrict); // 可自定义为 Cascade
        }
    }
}
