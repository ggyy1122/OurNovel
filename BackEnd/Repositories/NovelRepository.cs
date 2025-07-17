using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;
using OurNovel.Models.Dto;
using OurNovel.Repositories;

public class NovelRepository : Repository<Novel, int>, INovelRepository
{
    private readonly AppDbContext _context;

    public NovelRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<CollectRankingDto>> GetTopCollectedNovelsAsync(int topN)
    {
        var result = await _context.Collects
            .GroupBy(c => c.NovelId)
            .Select(g => new
            {
                NovelId = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(g => g.Count)
            .Take(topN)
            .Join(_context.Novels,
                g => g.NovelId,
                n => n.NovelId,          // 注意这里改为 NovelId
                (g, n) => new CollectRankingDto
                {
                    NovelId = n.NovelId,
                    NovelName = n.NovelName ?? string.Empty,    // 对可空处理
                    AuthorName = n.Author != null ? n.Author.AuthorName : string.Empty,  // 需要 AuthorName 属性
                    CoverUrl = n.CoverUrl ?? string.Empty,
                    Introduction = n.Introduction ?? string.Empty,
                    CollectCount = g.Count
                })
            .ToListAsync();

        return result;
    }
}
