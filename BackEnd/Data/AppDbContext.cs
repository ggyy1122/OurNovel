using Microsoft.EntityFrameworkCore;
using OurNovel.Data.Configurations;
using OurNovel.Models;

namespace OurNovel.Data
{
    /// <summary>
    /// 应用程序的数据库上下文类，继承自 EF Core 的 DbContext
    /// 用于定义数据库连接和表映射
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// 构造函数，接收配置好的 DbContextOptions 并传递给基类
        /// </summary>
        /// <param name="options">数据库上下文配置项</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }


        //以下为各个实体对应的 DbSet，提供对实体的增删查改操作
        public DbSet<Reader> Reads { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Novel> Novels { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Author> Authors { get; set; }

<<<<<<< HEAD
        public DbSet<Like> Likes { get; set; }
=======
        public DbSet<NovelCategory> NovelCategories { get; set; }
        public DbSet<Collect> Collects { get; set; }
>>>>>>> 65965fa4951a642e8d2699852cdd15b009dc6fb7

        /// <summary>
        /// 配置实体和数据库表结构映射关系的方法
        /// </summary>
        /// <param name="modelBuilder">用于构建模型的 ModelBuilder 对象</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 应用单独配置类，对 Reader 实体表映射进行详细配置
            modelBuilder.ApplyConfiguration(new ReaderConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterConfiguration());
            modelBuilder.ApplyConfiguration(new NovelConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CommentsConfiguration());
            modelBuilder.ApplyConfiguration(new LikesConfiguration());
            // ⚠️ 后续其他表的配置也在这里调用，例如：
            // modelBuilder.ApplyConfiguration(new NovelConfiguration());
            // modelBuilder.ApplyConfiguration(new ChapterConfiguration());

            // 显式指定 Category 的主键为 CategoryName
            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryName);
            modelBuilder.Entity<Category>().ToTable("CATEGORY");

            // NovelCategory 的复合主键为 （NovelId, CategoryName）
            modelBuilder.Entity<NovelCategory>()
                .HasKey(nc => new { nc.NovelId, nc.CategoryName });
            modelBuilder.Entity<NovelCategory>().ToTable("NOVEL_CATEGORY");
            modelBuilder.Entity<NovelCategory>()
                .HasOne(nc => nc.Novel)
                .WithMany()
                .HasForeignKey(nc => nc.NovelId);
            modelBuilder.Entity<NovelCategory>()
                .HasOne(nc => nc.Category)
                .WithMany()
                .HasForeignKey(nc => nc.CategoryName);

            //  Collect 的复合主键为 （NovelId, ReaderId）
            modelBuilder.Entity<Collect>()
                .HasKey(c => new { c.NovelId, c.ReaderId });
            modelBuilder.Entity<Collect>().ToTable("COLLECT");
            modelBuilder.Entity<Collect>()
                .HasOne(c => c.Novel)
                .WithMany()    
                .HasForeignKey(c => c.NovelId);
            modelBuilder.Entity<Collect>()
                .HasOne(c => c.Reader)
                .WithMany()  
                .HasForeignKey(c => c.ReaderId);
        }
    }
}


