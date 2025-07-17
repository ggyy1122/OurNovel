using OurNovel.Models;
using OurNovel.Data;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OurNovel.Services
{
    /// <summary>
    /// CommentsManagementService：专注于记录管理员对评论的管理日志和维护桥表关联
    /// </summary>
    public class CommentManagementService
    {
        private readonly AppDbContext _context;

        public CommentManagementService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 记录管理员对评论的管理操作（管理记录 + 桥表关联）
        /// </summary>
        /// <param name="managerId">管理员ID</param>
        /// <param name="result">操作结果说明，如“通过审核”或“封禁处理”</param>
        /// <param name="commentId">评论ID</param>
        public async Task RecordManagementAsync(int managerId, string result, int commentId)
        {
            // 新增管理记录
            var management = new Management
            {
                ManagerId = managerId,
                Result = result,
                Time = DateTime.Now
            };
            _context.Managements.Add(management);
            await _context.SaveChangesAsync();

            // 新增桥表关联
            var commentManagement = new CommentManagement
            {
                ManagementId = management.ManagementId,
                CommentId = commentId
            };
            _context.CommentManagements.Add(commentManagement);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 根据评论ID获取该评论的所有管理日志（含管理员信息）
        /// </summary>
        public async Task<List<Management>> GetManagementLogsByCommentIdAsync(int commentId)
        {
            return await _context.CommentManagements
                .Where(cm => cm.CommentId == commentId)
                .Include(cm => cm.Management)
                    .ThenInclude(m => m.Manager)
                .OrderByDescending(cm => cm.Management.Time)
                .Select(cm => cm.Management)
                .ToListAsync();
        }

        /// <summary>
        /// 获取所有评论的管理日志
        /// </summary>
        /// <returns>CommentManagement 列表</returns>
        public async Task<List<CommentManagement>> GetAllCommentManagementLogsAsync()
        {
            return await _context.CommentManagements
                .Include(nm => nm.Management)
                    .ThenInclude(m => m.Manager)
                .OrderByDescending(nm => nm.Management.Time ?? DateTime.MinValue)
                .ToListAsync();
        }
    }
}
