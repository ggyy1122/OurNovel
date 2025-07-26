using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;
using OurNovel.Services;
using System;
using System.Threading.Tasks;

public class ReportManagementService
{
    private readonly AppDbContext _context;

    public ReportManagementService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 记录管理员对举报的管理操作（管理日志 + 桥表关联）
    /// </summary>
    /// <param name="managerId">管理员ID</param>
    /// <param name="result">操作结果说明，如“举报审核通过”或“举报无效”</param>
    /// <param name="reportId">举报ID</param>
    public async Task RecordManagementAsync(string result, int reportId, int managerId)
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

        // 2. 新增桥表关联
        var reportManagement = new ReportManagement
        {
            ManagementId = management.ManagementId,
            ReportId = reportId
        };
        _context.ReportManagements.Add(reportManagement);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Management>> GetManagementLogsByReportIdAsync(int reportId)
    {
        return await _context.ReportManagements
            .Where(rm => rm.ReportId == reportId)
            .Include(rm => rm.Management)
                .ThenInclude(m => m.Manager)
            .OrderByDescending(rm => rm.Management.Time)
            .Select(rm => rm.Management)
            .ToListAsync();
    }

    /// <summary>
    /// 获取所有举报的管理日志
    /// </summary>
    /// <returns>ReportManagement 列表</returns>
    public async Task<List<ReportManagement>> GetAllReportManagementLogsAsync()
    {
        return await _context.ReportManagements
            .Include(nm => nm.Management)
                .ThenInclude(m => m.Manager)
            .OrderByDescending(nm => nm.Management.Time ?? DateTime.MinValue)
            .ToListAsync();
    }

    public async Task<List<object>> GetReportsWithManagementLogsByReaderIdAsync(int readerId)
    {
        var query = from report in _context.Reports
                    where report.ReaderId == readerId
                    join rm in _context.ReportManagements on report.ReportId equals rm.ReportId into rmsGroup
                    from rm in rmsGroup.DefaultIfEmpty()
                    join management in _context.Managements on rm.ManagementId equals management.ManagementId into mGroup
                    from management in mGroup.DefaultIfEmpty()
                    join comment in _context.Comments on report.CommentId equals comment.CommentId into cGroup
                    from comment in cGroup.DefaultIfEmpty()
                    select new
                    {
                        report.ReportId,
                        report.Reason,
                        ReportTime = report.ReportTime,
                        report.Progress,
                        CommentId = comment != null ? comment.CommentId : (int?)null,
                        CommentContent = comment != null ? comment.Content : null,
                        ManagementId = management != null ? management.ManagementId : (int?)null,
                        ManagerId = management != null ? management.ManagerId : (int?)null,
                        Result = management != null ? management.Result : null,
                        Time = management != null ? management.Time : (DateTime?)null
                    };

        var flatList = await query.ToListAsync();

        var grouped = flatList.GroupBy(r => new { r.ReportId, r.Reason, r.ReportTime, r.Progress, r.CommentId, r.CommentContent })
            .Select(g => new
            {
                g.Key.ReportId,
                g.Key.Reason,
                ReportTime = g.Key.ReportTime.ToString("yyyy-MM-dd HH:mm:ss"),
                g.Key.Progress,
                CommentId = g.Key.CommentId,
                CommentContent = g.Key.CommentContent,
                ManagementLogs = g.Where(x => x.ManagementId != null)
                                  .OrderByDescending(x => x.Time)
                                  .Select(x => new
                                  {
                                      x.ManagementId,
                                      x.ManagerId,
                                      x.Result,
                                      Time = x.Time?.ToString("yyyy-MM-dd HH:mm:ss")
                                  }).ToList()
            }).ToList();

        return grouped.Cast<object>().ToList();
    }

}
