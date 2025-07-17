using OurNovel.Models;
using OurNovel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OurNovel.Services
{
    public class ChapterManagementService
    {
        private readonly AppDbContext _context;

        public ChapterManagementService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 记录管理员对章节的管理操作（管理记录 + 桥表关联）
        /// </summary>
        /// <param name="managerId">管理员ID</param>
        /// <param name="result">操作结果说明，如“通过”或“封禁”</param>
        /// <param name="novelId">小说ID</param>
        /// <param name="chapterId">章节ID</param>
        public async Task RecordManagementAsync(int managerId, string result, int novelId, int chapterId)
        {
            var management = new Management
            {
                ManagerId = managerId,
                Result = result,
                Time = DateTime.Now
            };
            _context.Managements.Add(management);
            await _context.SaveChangesAsync();

            var chapterManagement = new ChapterManagement
            {
                ManagementId = management.ManagementId,
                NovelId = novelId,
                ChapterId = chapterId
            };
            _context.ChapterManagements.Add(chapterManagement);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 根据小说ID、章节ID获取该章节的所有管理日志
        /// </summary>
        public async Task<List<Management>> GetManagementLogsByNovelAndChapterAsync(int novelId, int chapterId)
        {
            return await _context.ChapterManagements
                .Where(cm => cm.NovelId == novelId && cm.ChapterId == chapterId)
                .Include(cm => cm.Management)
                    .ThenInclude(m => m.Manager)
                .OrderByDescending(cm => cm.Management.Time ?? DateTime.MinValue)
                .Select(cm => cm.Management)
                .ToListAsync();
        }


        /// <summary>
        /// 获取所有章节的管理日志
        /// </summary>
        /// <returns>ChapterManagement 列表</returns>
        public async Task<List<ChapterManagement>> GetAllChapterManagementLogsAsync()
        {
            return await _context.ChapterManagements
                .Include(cm => cm.Management)
                    .ThenInclude(m => m.Manager)
                .OrderByDescending(cm => cm.Management.Time ?? DateTime.MinValue)
                .ToListAsync();
        }
    }
}
