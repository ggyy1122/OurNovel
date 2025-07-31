using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// AuthorIncome 实体的表结构配置
    /// </summary>
    public class AuthorIncomeConfiguration : IEntityTypeConfiguration<AuthorIncome>
    {
        public void Configure(EntityTypeBuilder<AuthorIncome> entity)
        {
            entity.ToTable("AUTHOR_INCOME");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.AuthorId)
                .HasColumnName("AUTHOR_ID");

            entity.Property(e => e.NovelId)
              .HasColumnName("NOVEL_ID");

            entity.Property(e => e.Type)
                .HasColumnName("TYPE")
                .HasMaxLength(20);

            entity.Property(e => e.Amount)
                .HasColumnName("AMOUNT")
                .HasColumnType("NUMBER(10,2)");

            entity.Property(e => e.CreateTime)
                .HasColumnName("CREATE_TIME")
                .HasDefaultValueSql("SYSTIMESTAMP");
        }
    }
}
