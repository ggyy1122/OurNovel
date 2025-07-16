using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    public class NovelCategoryConfiguration : IEntityTypeConfiguration<NovelCategory>
    {
        public void Configure(EntityTypeBuilder<NovelCategory> builder)
        {
            // 指定表名
            builder.ToTable("NOVEL_CATEGORY");

            // 配置复合主键
            builder.HasKey(nc => new { nc.NovelId, nc.CategoryName });

            // 配置外键关系
            builder.HasOne(nc => nc.Novel)
                   .WithMany()
                   .HasForeignKey(nc => nc.NovelId);

            builder.HasOne(nc => nc.Category)
                   .WithMany()
                   .HasForeignKey(nc => nc.CategoryName);
        }
    }
}
