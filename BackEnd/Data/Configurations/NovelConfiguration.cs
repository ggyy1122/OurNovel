using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurNovel.Models;

namespace OurNovel.Data.Configurations
{
    /// <summary>
    /// Novel ʵ��������ݿ����ã�ͨ�� Fluent API��
    /// ʵ�� IEntityTypeConfiguration<T> �ӿڶ����ṹ
    /// </summary>
    public class NovelConfiguration : IEntityTypeConfiguration<Novel>
    {
        /// <summary>
        /// ���� Novel ʵ���Ӧ�����ݿ��ṹ
        /// </summary>
        /// <param name="entity">ʵ�幹���������������ֶ����Ժ͹�ϵ</param>
        public void Configure(EntityTypeBuilder<Novel> entity)
        {
            // ����
            entity.ToTable("NOVEL");

            // ����
            entity.HasKey(e => e.NovelId);

            // �ֶ�ӳ��
            entity.Property(e => e.NovelId).HasColumnName("NOVEL_ID");
            entity.Property(e => e.AuthorId).HasColumnName("AUTHOR_ID");
            entity.Property(e => e.NovelName).HasColumnName("NOVEL_NAME");
            entity.Property(e => e.Introduction).HasColumnName("INTRODUCTION");
            entity.Property(e => e.CreateTime).HasColumnName("CREATE_TIME");
            entity.Property(e => e.CoverUrl).HasColumnName("COVER_URL");
            entity.Property(e => e.Score).HasColumnName("SCORE");
            entity.Property(e => e.TotalWordCount).HasColumnName("TOTAL_WORD_COUNT");
            entity.Property(e => e.RecommendCount).HasColumnName("RECOMMEND_COUNT");
            entity.Property(e => e.CollectedCount).HasColumnName("COLLECTED_COUNT");
            entity.Property(e => e.Status)
                  .HasColumnName("STATUS")
                  .HasDefaultValue("�����");
            /*
            // �����ϵ��һ�� Novel ��Ӧһ�� Author�����һ��
            entity.HasOne(e => e.Author)
                  .WithMany(a => a.Novels)
                  .HasForeignKey(e => e.AuthorId)
                  .OnDelete(DeleteBehavior.Cascade);  // ɾ������ʱ�����С˵Ҳɾ����������ɸģ�
             * 
             */


        }
    }
}
