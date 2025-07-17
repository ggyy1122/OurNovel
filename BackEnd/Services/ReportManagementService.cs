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

}
