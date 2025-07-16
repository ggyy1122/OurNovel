using OurNovel.Models;
using OurNovel.Data;
using Microsoft.EntityFrameworkCore;

namespace OurNovel.Services
{
    /// <summary>
    /// 管理员操作日志服务，专注记录对小说的管理操作日志及桥表
    /// </summary>
    public class NovelManagementService
    {
        private readonly AppDbContext _context;

        public NovelManagementService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 记录管理员对小说的操作日志
        /// </summary>
        /// <param name="managerId">管理员ID</param>
        /// <param name="result">操作结果描述（如“审核通过”、“封禁”）</param>
        /// <param name="novelId">被操作小说ID</param>
        public async Task RecordManagementAsync(int managerId, string result, int novelId)
        {
            // 1. 新增管理日志
            var management = new Management
            {
                ManagerId = managerId,
                Result = result,
                Time = DateTime.Now
            };
            _context.Managements.Add(management);
            await _context.SaveChangesAsync();

            // 2. 新增管理日志和小说关联桥表
            var novelManagement = new NovelManagement
            {
                ManagementId = management.ManagementId,
                NovelId = novelId
            };
            _context.NovelManagements.Add(novelManagement);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 根据小说ID获取该小说所有管理日志列表（含管理员信息）
        /// </summary>
        /// <param name="novelId">小说ID</param>
        /// <returns>管理日志列表</returns>
        public async Task<List<Management>> GetManagementLogsByNovelIdAsync(int novelId)
        {
            return await _context.NovelManagements
                .Where(nm => nm.NovelId == novelId)
                .Include(nm => nm.Management)
                    .ThenInclude(m => m.Manager)
                .OrderByDescending(nm => nm.Management.Time ?? DateTime.MinValue)
                .Select(nm => nm.Management)
                .ToListAsync();
        }
    }
}
