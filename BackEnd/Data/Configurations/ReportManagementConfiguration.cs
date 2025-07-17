using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

public class ReportManagementConfiguration : IEntityTypeConfiguration<ReportManagement>
{
    public void Configure(EntityTypeBuilder<ReportManagement> entity)
    {
        entity.ToTable("REPORT_MANAGEMENT");

        entity.HasKey(e => e.ManagementId);  

        entity.Property(e => e.ManagementId)
            .HasColumnName("MANAGEMENT_ID")
            .IsRequired();

        entity.Property(e => e.ReportId)
            .HasColumnName("REPORT_ID")
            .IsRequired();

        entity.HasOne(e => e.Management)
            .WithMany()
            .HasForeignKey(e => e.ManagementId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Report)
            .WithMany()
            .HasForeignKey(e => e.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
