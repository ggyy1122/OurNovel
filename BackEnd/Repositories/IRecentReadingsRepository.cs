using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRecentReadingsRepository
{
    /// <summary>
    /// 添加或更新读者最近阅读记录（如果已有该读者-小说组合则更新时间，否则新增）
    /// </summary>
    Task AddOrUpdateAsync(RecentReadings entity);

    /// <summary>
    /// 删除指定读者的某本小说的最近阅读记录
    /// </summary>
    Task DeleteAsync(int readerId, int novelId);

    /// <summary>
    /// 获取指定读者的最近阅读记录列表，按时间降序排列
    /// </summary>
    Task<IEnumerable<RecentReadings>> GetByReaderIdAsync(int readerId);

}
