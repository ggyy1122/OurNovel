using OurNovel.Models;
using OurNovel.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurNovel.Services
{
    /// <summary>
    /// 举报服务，处理评论举报相关逻辑
    /// </summary>
    public class ReportService : BaseService<Report, int>
    {
        private readonly ReportManagementService _reportManagementService;

        public ReportService(IRepository<Report, int> repository, ReportManagementService reportManagementService)
            : base(repository)
        {
            _reportManagementService = reportManagementService;
        }

        /// <summary>
        /// 举报某条评论
        /// </summary>
        /// <param name="commentId">被举报的评论ID</param>
        /// <param name="readerId">举报人ID</param>
        /// <param name="reason">举报原因</param>
        public async Task ReportCommentAsync(int commentId, int readerId, string reason)
        {
            var report = new Report
            {
                CommentId = commentId,
                ReaderId = readerId,
                Reason = reason,
                ReportTime = DateTime.Now,
                Progress = "未处理"
            };

            await AddAsync(report);
        }

        /// <summary>
        /// 处理举报：更新进度状态
        /// </summary>
        /// <param name="reportId">举报ID</param>
        /// <param name="progress">处理进度：成功、失败、未处理</param>
        public async Task ProcessReportAsync(int reportId, string progress, int managerId)
        {
            var report = await GetByIdAsync(reportId);
            if (report == null)
            {
                throw new Exception("举报不存在");
            }

            if (progress != "成功" && progress != "失败" && progress != "未处理")
            {
                throw new Exception("无效的处理状态");
            }

            report.Progress = progress;
            await UpdateAsync(report);

            // 记录操作日志
            var result = $"举报处理：审核结果为 {progress}";
            await _reportManagementService.RecordManagementAsync(result, reportId,managerId);
        }
    }
}
