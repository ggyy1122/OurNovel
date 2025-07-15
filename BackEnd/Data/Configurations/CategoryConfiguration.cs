using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Category 实体类的数据库配置（通过 Fluent API）
    /// 实现 IEntityTypeConfiguration<T> 接口定义表结构
    /// </summary>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            // 指定对应的表名
            entity.ToTable("CATEGORY");

            // 主键
            entity.HasKey(e => e.CategoryName);

            // 属性映射配置
            entity.Property(e => e.CategoryName)
                .HasColumnName("CATEGORY_NAME")
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}