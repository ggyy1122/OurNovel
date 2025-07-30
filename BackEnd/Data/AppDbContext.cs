using Microsoft.EntityFrameworkCore;
using OurNovel.Data.Configurations;
using OurNovel.Models;
using OurNovel.Models;
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
        public DbSet<Report> Reports { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Manager> Managers { get; set; } = null!;
        public DbSet<Management> Managements { get; set; }
        public DbSet<ReportManagement> ReportManagements { get; set; }
        public DbSet<CommentManagement> CommentManagements { get; set; }
        public DbSet<NovelManagement> NovelManagements { get; set; }
        public DbSet<ChapterManagement> ChapterManagements { get; set; }

        public DbSet<Like> Likes { get; set; }
        public DbSet<NovelCategory> NovelCategories { get; set; }
        public DbSet<Collect> Collects { get; set; }
        public DbSet<Recommend> Recommends { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<CommentReply> CommentReplies { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<AuthorIncome> AuthorIncomes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<WholePurchase> WholePurchases { get; set; }
        public DbSet<WholePurchase> WholePurchase { get; set; }
        
        public DbSet<RecentReadings> RecentReadings { get; set; }

        public DbSet<ReaderRewardRecord> ReaderRewardRecords { get; set; }
        public DbSet<ReaderSubscriptionRecord> ReaderSubscriptionRecords { get; set; }
        public DbSet<ReaderRechargeRecord> ReaderRechargeRecords { get; set; }
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
            modelBuilder.ApplyConfiguration(new CommentReplyConfiguration());

            modelBuilder.ApplyConfiguration(new RecommendConfiguration());
            modelBuilder.ApplyConfiguration(new NovelCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CollectConfiguration());
            modelBuilder.ApplyConfiguration(new RateConfiguration());

            modelBuilder.ApplyConfiguration(new ReportConfiguration());
            modelBuilder.ApplyConfiguration(new ManagerConfiguration());
            modelBuilder.ApplyConfiguration(new ManagementConfiguration());


            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new RewardConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorIncomeConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseConfiguration());
            modelBuilder.ApplyConfiguration(new ReportManagementConfiguration());
            modelBuilder.ApplyConfiguration(new CommentManagementConfiguration());
            modelBuilder.ApplyConfiguration(new NovelManagementConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterManagementConfiguration());
            modelBuilder.ApplyConfiguration(new WholePurchaseConfiguration());
            
            modelBuilder.ApplyConfiguration(new RecentReadingsConfiguration());

            modelBuilder.Entity<ReaderRewardRecord>(entity =>
            {
                entity.HasNoKey().ToView("V_READER_REWARD_RECORD");

                entity.Property(e => e.ReaderId).HasColumnName("READER_ID");
                entity.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID");
                entity.Property(e => e.Amount).HasColumnName("AMOUNT");
                entity.Property(e => e.RewardTime).HasColumnName("REWARD_TIME");
                entity.Property(e => e.NovelId).HasColumnName("NOVEL_ID");
                entity.Property(e => e.NovelTitle).HasColumnName("NOVEL_TITLE");
                entity.Property(e => e.AuthorId).HasColumnName("AUTHOR_ID");
                entity.Property(e => e.AuthorName).HasColumnName("AUTHOR_NAME");
            });
            modelBuilder.Entity<ReaderSubscriptionRecord>(entity =>
            {
                entity.HasNoKey().ToView("V_READER_SUBSCRIPTION_RECORD");

                entity.Property(e => e.ReaderId).HasColumnName("READER_ID");
                entity.Property(e => e.ChapterId).HasColumnName("CHAPTER_ID");
                entity.Property(e => e.ChapterTitle).HasColumnName("CHAPTER_TITLE");
                entity.Property(e => e.NovelId).HasColumnName("NOVEL_ID");
                entity.Property(e => e.NovelTitle).HasColumnName("NOVEL_TITLE");
                entity.Property(e => e.ConsumeAmount).HasColumnName("CONSUME_AMOUNT");
                entity.Property(e => e.ConsumeTime).HasColumnName("CONSUME_TIME");
            });
            modelBuilder.Entity<ReaderRechargeRecord>(entity =>
            {
                entity.HasNoKey().ToView("V_READER_RECHARGE_RECORD");

                entity.Property(e => e.ReaderId).HasColumnName("READER_ID");
                entity.Property(e => e.RechargeAmount).HasColumnName("RECHARGE_AMOUNT");
                entity.Property(e => e.VirtualCoin).HasColumnName("VIRTUAL_COIN");
                entity.Property(e => e.RechargeTime).HasColumnName("RECHARGE_TIME");
            });

            // ⚠️ 后续其他表的配置也在这里调用，例如：
            // modelBuilder.ApplyConfiguration(new NovelConfiguration());
            // modelBuilder.ApplyConfiguration(new ChapterConfiguration());

            // 显式指定 Category 的主键为 CategoryName
            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryName);
            modelBuilder.Entity<Category>().ToTable("CATEGORY");

        }
    }
}


