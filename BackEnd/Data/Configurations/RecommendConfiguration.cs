using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    public class RecommendConfiguration : IEntityTypeConfiguration<Recommend>
    {
        public void Configure(EntityTypeBuilder<Recommend> builder)
        {
            // 指定表名
            builder.ToTable("RECOMMEND");

            // 设置复合主键：NovelId 和 ReaderId
            builder.HasKey(r => new { r.NovelId, r.ReaderId });

            // 配置外键关系
            builder.HasOne(r => r.Novel)
                   .WithMany()
                   .HasForeignKey(r => r.NovelId);

            builder.HasOne(r => r.Reader)
                   .WithMany()
                   .HasForeignKey(r => r.ReaderId);
        }
    }
}
