/*2353238关云兮*/
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Author 实体类的数据库配置（通过 Fluent API）
    /// 实现 IEntityTypeConfiguration<T> 接口定义表结构
    /// </summary>
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        /// <summary>
        /// 配置 Author 实体对应的数据库表结构
        /// </summary>
        /// <param name="entity">实体构建器，用于配置字段属性和关系</param>
        public void Configure(EntityTypeBuilder<Author> entity)
        {
            entity.ToTable("AUTHOR");

            entity.HasKey(e => e.AuthorId);

            entity.Property(e => e.AuthorId)
                .HasColumnName("AUTHOR_ID");

            entity.Property(e => e.AuthorName)
                .HasColumnName("AUTHOR_NAME")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.Password)
                .HasColumnName("PASSWORD")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.Earning)
                .HasColumnName("EARNING")
                .HasColumnType("NUMBER(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.Phone)
                .HasColumnName("PHONE")
                .HasMaxLength(11);

            entity.Property(e => e.AvatarUrl)
                .HasColumnName("AVATAR_URL")
                .HasMaxLength(255);

            entity.Property(e => e.RegisterTime)
                  .HasColumnName("REGISTER_TIME")
                  .HasColumnType("DATE")
                  .IsRequired(false);

            entity.Property(e => e.Introduction)
                  .HasColumnName("INTRODUCTION");
        }
    }
}
