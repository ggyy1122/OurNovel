using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;

namespace OurNovel.Services
{
    public interface IStatisticsService
    {
        Task<int> GetTotalNovelsCountAsync();
        Task<int> GetTotalAuthorsCountAsync();
        Task<int> GetTotalReadersCountAsync();
        Task<int> GetPendingChaptersCountAsync();
        Task<int> GetPendingNovelsCountAsync();
        Task<int> GetPendingReportsCountAsync();
    }

    public class StatisticsService : IStatisticsService
    {
        private readonly AppDbContext _context;

        public StatisticsService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取总小说数
        /// </summary>
        public async Task<int> GetTotalNovelsCountAsync()
        {
            return await _context.Novels.CountAsync();
        }

        /// <summary>
        /// 获取作者总数
        /// </summary>
        public async Task<int> GetTotalAuthorsCountAsync()
        {
            return await _context.Authors.CountAsync();
        }

        /// <summary>
        /// 获取读者总数
        /// </summary>
        public async Task<int> GetTotalReadersCountAsync()
        {
            return await _context.Readers.CountAsync();
        }

        /// <summary>
        /// 获取待审核章节数
        /// </summary>
        public async Task<int> GetPendingChaptersCountAsync()
        {
            return await _context.Chapters
                .Where(c =>  c.Status == "首次审核" || c.Status == "审核中")
                .CountAsync();
        }

        /// <summary>
        /// 获取待审核小说数
        /// </summary>
        public async Task<int> GetPendingNovelsCountAsync()
        {
            return await _context.Novels
                .Where(n => n.Status == "待审核")
                .CountAsync();
        }

        /// <summary>
        /// 获取待处理举报数
        /// </summary>
        public async Task<int> GetPendingReportsCountAsync()
        {
            return await _context.Reports
                .Where(r => r.Progress == "未处理")
                .CountAsync();
        }
    }
}
