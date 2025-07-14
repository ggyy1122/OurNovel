
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
	/// <summary>
	/// Chapter ʵ��������ݿ����ã�ͨ�� Fluent API��
	/// </summary>
	public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
	{
		public void Configure(EntityTypeBuilder<Chapter> entity)
		{
			entity.ToTable("CHAPTER");

			// ? ���ø���������NOVEL_ID + CHAPTER_ID
			entity.HasKey(e => new { e.NovelId, e.ChapterId });

			// �����ֶ�ӳ��
			entity.Property(e => e.NovelId).HasColumnName("NOVEL_ID");
			entity.Property(e => e.ChapterId).HasColumnName("CHAPTER_ID");

			// �����ֶ�
			entity.Property(e => e.Title)
				  .HasColumnName("TITLE")
				  .HasMaxLength(40)
				  .IsRequired();

			// �����ֶΣ�CLOB ���ͣ�
			entity.Property(e => e.Content)
				  .HasColumnName("CONTENT")
				  .HasColumnType("CLOB");

			// �����ֶ�
			entity.Property(e => e.WordCount)
				  .HasColumnName("WORD_COUNT")
				  .IsRequired();

			// ǧ�ֵ����ֶ�
			entity.Property(e => e.PricePerKilo)
				  .HasColumnName("PRICE_PER_KILO")
				  .HasColumnType("NUMBER(10,2)")
				  .HasDefaultValue(0.50m);

			// �Ƿ��շ��ֶ�
			entity.Property(e => e.IsCharged)
				  .HasColumnName("IS_CHARGED")
				  .HasMaxLength(2);

			// ����ʱ���ֶ�
			entity.Property(e => e.PublishTime)
				  .HasColumnName("PUBLISH_TIME");

			// ״̬�ֶ�
			entity.Property(e => e.Status)
				  .HasColumnName("STATUS")
				  .HasMaxLength(10)
				  .HasDefaultValue("ͨ��");

			// �����Զ������ֶ�
			entity.Ignore(e => e.CalculatedPrice);

            // ? ������ã�CHAPTER(NOVEL_ID) �� NOVEL(NOVEL_ID)�������ж�Ӧ��С˵
            entity.HasOne(e => e.Novel)
				  .WithMany(n => n.Chapters)
				  .HasForeignKey(e => e.NovelId)
				  .IsRequired()
				  .OnDelete(DeleteBehavior.Cascade); // С˵��ɾʱ���½�Ҳɾ��
		}
	}
}
