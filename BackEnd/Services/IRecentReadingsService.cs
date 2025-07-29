using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRecentReadingsService
{
    Task AddOrUpdateAsync(int readerId, int novelId);
    Task DeleteAsync(int readerId, int novelId);
    Task<IEnumerable<RecentReadings>> GetByReaderIdAsync(int readerId);
}
