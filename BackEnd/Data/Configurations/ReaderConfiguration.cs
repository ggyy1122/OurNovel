using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Reader 实体类的数据库配置（通过 Fluent API）
    ///实现 IEntityTypeConfiguration<T> 接口定义表结构
    /// </summary>
    public class ReaderConfiguration : IEntityTypeConfiguration<Reader>
    {
        /// <summary>
        /// 配置 Reader 实体对应的数据库表结构
        /// </summary>
        /// <param name="entity">实体构建器，用于配置字段属性和关系</param>
        public void Configure(EntityTypeBuilder<Reader> entity)
        {
            entity.ToTable("READER");

            entity.HasKey(e => e.ReaderId);

            entity.Property(e => e.ReaderId).HasColumnName("READER_ID");
            entity.Property(e => e.ReaderName).HasColumnName("READER_NAME").IsRequired();
            entity.Property(e => e.Password).HasColumnName("PASSWORD").IsRequired();
            entity.Property(e => e.Phone).HasColumnName("PHONE");
            entity.Property(e => e.Gender).HasColumnName("GENDER");
            entity.Property(e => e.Balance).HasColumnName("BALANCE").HasColumnType("NUMBER(18,2)").HasDefaultValue(0m);
            entity.Property(e => e.AvatarUrl).HasColumnName("AVATAR_URL");
            entity.Property(e => e.BackgroundUrl).HasColumnName("BACKGROUND_URL");
            entity.Property(e => e.IsCollectVisible).HasColumnName("IS_COLLECT_VISIBLE");
            entity.Property(e => e.IsRecommendVisible).HasColumnName("IS_RECOMMEND_VISIBLE");
        }
    }
}
