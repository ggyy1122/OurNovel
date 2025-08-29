using OurNovel.Models;
using OurNovel.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RecentReadingsService : IRecentReadingsService
{
    private readonly IRecentReadingsRepository _repository;

    public RecentReadingsService(IRecentReadingsRepository repository)
    {
        _repository = repository;
    }

    public async Task AddOrUpdateAsync(int readerId, int novelId, int chapterId)
    {
        var entity = new RecentReadings
        {
            ReaderId = readerId,
            NovelId = novelId,
            ChapterId = chapterId,
            RecentReadingTime = System.DateTime.Now
        };
        await _repository.AddOrUpdateAsync(entity);
    }

    public async Task DeleteAsync(int readerId, int novelId)
    {
        await _repository.DeleteAsync(readerId, novelId);
    }

    public async Task<IEnumerable<RecentReadings>> GetByReaderIdAsync(int readerId)
    {
        return await _repository.GetByReaderIdAsync(readerId);
    }

    public async Task<int> GetLastReadChapterIdAsync(int readerId, int novelId)
    {
        var record = await _repository.GetByReaderAndNovelAsync(readerId, novelId);
        return record?.ChapterId ?? 1;   // 如果没有阅读记录，返回首章ID 1
    }
}