using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    public class CollectConfiguration : IEntityTypeConfiguration<Collect>
    {
        public void Configure(EntityTypeBuilder<Collect> builder)
        {
            // 设置表名
            builder.ToTable("COLLECT");

            // 设置复合主键
            builder.HasKey(c => new { c.NovelId, c.ReaderId });

            // 设置外键关系
            builder.HasOne(c => c.Novel)
                   .WithMany()
                   .HasForeignKey(c => c.NovelId);

            builder.HasOne(c => c.Reader)
                   .WithMany()
                   .HasForeignKey(c => c.ReaderId);
        }
    }
}
