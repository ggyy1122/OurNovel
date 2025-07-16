using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Management ʵ������ݿ��������
    /// </summary>
    public class ManagementConfiguration : IEntityTypeConfiguration<Management>
    {
        public void Configure(EntityTypeBuilder<Management> entity)
        {
            entity.ToTable("MANAGEMENT");

            // ����
            entity.HasKey(e => e.ManagementId);

            entity.Property(e => e.ManagementId)
                  .HasColumnName("MANAGEMENT_ID")
                  .ValueGeneratedOnAdd(); // Oracle��ʹ��trigger+sequence�Զ�����

            entity.Property(e => e.ManagerId)
                  .HasColumnName("MANAGER_ID")
                  .IsRequired();

            entity.Property(e => e.Result)
                  .HasColumnName("RESULT")
                  .HasMaxLength(200);

            entity.Property(e => e.Time)
                  .HasColumnName("TIME")
                  .HasColumnType("DATE")
                  .HasDefaultValueSql("SYSDATE"); // ����Ĭ��ʱ��Ϊ��ǰϵͳʱ�䣨Oracle��

            // ��ѡ���������Լ�������� MANAGER ������
            // entity.HasOne<Manager>()
            //       .WithMany()
            //       .HasForeignKey(e => e.ManagerId)
            //       .OnDelete(DeleteBehavior.Restrict); // ���Զ���Ϊ Cascade
        }
    }
}
