using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Transaction 实体类的数据库配置
    /// 对应 Oracle 的 MAIN_USER.TRANSACTION 表
    /// </summary>
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> entity)
        {
            // 基础表结构
            entity.ToTable("TRANSACTION", "MAIN_USER");
            entity.HasKey(e => e.TransactionId);

            // 列映射（全大写匹配Oracle）
            entity.Property(e => e.TransactionId)
         .HasColumnName("TRANSACTION_ID")
         .ValueGeneratedOnAdd()
         .UseIdentityColumn();

            entity.Property(e => e.ReaderId)
                .HasColumnName("READER_ID")
                .IsRequired();

            entity.Property(e => e.TransType)
                .HasColumnName("TRANS_TYPE")
                .HasMaxLength(10);

            entity.Property(e => e.Amount)
                .HasColumnName("AMOUNT")
                .HasColumnType("NUMBER(*,0)")
                .IsRequired();

            entity.Property(e => e.Time)
                .HasColumnName("TIME")
                .HasColumnType("DATE");

        }
    }
}