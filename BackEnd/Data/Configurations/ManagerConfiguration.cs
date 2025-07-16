using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Manager 实体类的数据库配置（通过 Fluent API）
    /// 实现 IEntityTypeConfiguration<T> 接口定义表结构
    /// </summary>
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        /// <summary>
        /// 配置 Manager 实体对应的数据库表结构
        /// </summary>
        /// <param name="entity">实体构建器，用于配置字段属性和关系</param>
        public void Configure(EntityTypeBuilder<Manager> entity)
        {
            entity.ToTable("MANAGER");

            entity.HasKey(e => e.ManagerId);

            entity.Property(e => e.ManagerId)
                .HasColumnName("MANAGER_ID");

            entity.Property(e => e.ManagerName)
                .HasColumnName("MANAGER_NAME")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.Password)
                .HasColumnName("PASSWORD")
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
