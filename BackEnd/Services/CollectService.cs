using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;

public class CollectService : ICollectService
{
    private readonly ICollectRepository _repository;
    private readonly AppDbContext _context;
    public CollectService(ICollectRepository repository, AppDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task AddAsync(int novelId, int readerId, string? isPublic)
    {
        var entity = new Collect
        {
            NovelId = novelId,
            ReaderId = readerId,
            IsPublic = isPublic
        };
        await _repository.AddAsync(entity);
    }

    public async Task AddOrUpdateAsync(int novelId, int readerId, string? isPublic)
    {
        var entity = new Collect
        {
            NovelId = novelId,
            ReaderId = readerId,
            IsPublic = isPublic
        };
        await _repository.AddOrUpdateAsync(entity);
    }

    public async Task DeleteAsync(int novelId, int readerId)
    {
        await _repository.DeleteAsync(novelId, readerId);
    }

    public async Task<IEnumerable<Collect>> GetByReaderIdAsync(int readerId)
    {
        return await _repository.GetByReaderIdAsync(readerId);
    }

    public async Task<IEnumerable<Collect>> GetByNovelIdAsync(int novelId)
    {
        return await _repository.GetByNovelIdAsync(novelId);
    }

    public async Task<IEnumerable<Collect>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    // 更新单本小说的收藏数
    public async Task UpdateCollectCountAsync(int novelId)
    {
        var count = await _context.Collects
            .CountAsync(r => r.NovelId == novelId);

        var novel = await _context.Novels
            .FirstOrDefaultAsync(n => n.NovelId == novelId);

        if (novel != null)
        {
            novel.CollectedCount = count;
            await _context.SaveChangesAsync();
        }
    }
}
