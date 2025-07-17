using OurNovel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OurNovel.Data;
using Microsoft.EntityFrameworkCore;

public class RecommendService : IRecommendService
{
    private readonly IRecommendRepository _repository;
    private readonly AppDbContext _context;

    public RecommendService(IRecommendRepository repository, AppDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task AddAsync(int novelId, int readerId, string? reason)
    {
        var entity = new Recommend
        {
            NovelId = novelId,
            ReaderId = readerId,
            Reason = reason
        };
        await _repository.AddAsync(entity);
    }

    public async Task DeleteAsync(int novelId, int readerId)
    {
        await _repository.DeleteAsync(novelId, readerId);
    }

    public async Task<IEnumerable<Recommend>> GetByReaderIdAsync(int readerId)
    {
        return await _repository.GetByReaderIdAsync(readerId);
    }

    public async Task<IEnumerable<Recommend>> GetByNovelIdAsync(int novelId)
    {
        return await _repository.GetByNovelIdAsync(novelId);
    }

    public async Task<IEnumerable<Recommend>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    
    // 更新单本小说的推荐数
    public async Task UpdateRecommendCountAsync(int novelId)
    {
        var count = await _context.Recommends
            .CountAsync(r => r.NovelId == novelId);

        var novel = await _context.Novels
            .FirstOrDefaultAsync(n => n.NovelId == novelId);

        if (novel != null)
        {
            novel.RecommendCount = count;
            await _context.SaveChangesAsync();
        }
    }
}