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
        var topCollected = await _context.Collects
            .GroupBy(c => c.NovelId)
            .Select(g => new { NovelId = g.Key, CollectCount = g.Count() })
            .OrderByDescending(x => x.CollectCount)
            .Take(topN)
            .ToListAsync();

        var novelIds = topCollected.Select(x => x.NovelId).ToList();

        var novels = await _context.Novels
            .Where(n => novelIds.Contains(n.NovelId))
            .Include(n => n.Author) // 重点：加载导航属性
            .ToListAsync();

        var result = topCollected.Join(novels,
            c => c.NovelId,
            n => n.NovelId,
            (c, n) => new CollectRankingDto
            {
                NovelId = n.NovelId,
                NovelName = n.NovelName ?? string.Empty,
                AuthorName = n.Author?.AuthorName ?? "未知作者",
                CoverUrl = n.CoverUrl ?? string.Empty,
                Introduction = n.Introduction ?? string.Empty,
                CollectCount = c.CollectCount
            })
            .OrderByDescending(x => x.CollectCount) 
            .ToList();

        return result;
    }

    public async Task<List<RecommendRankingDto>> GetTopRecommendedNovelsAsync(int topN)
    {
        var novels = await _context.Novels
            .Include(n => n.Author) // 加载作者信息
            .Where(n => n.RecommendCount > 0)
            .OrderByDescending(n => n.RecommendCount)
            .Take(topN)
            .ToListAsync();

        var result = novels.Select(n => new RecommendRankingDto
        {
            NovelId = n.NovelId,
            NovelName = n.NovelName ?? string.Empty,
            AuthorName = n.Author?.AuthorName ?? "未知作者",
            CoverUrl = n.CoverUrl ?? string.Empty,
            Introduction = n.Introduction ?? string.Empty,
            RecommendCount = n.RecommendCount ?? 0
        }).ToList();

        return result;
    }

    public async Task<List<ScoreRankingDto>> GetTopScoredNovelsAsync(int topN)
    {
        var novels = await _context.Novels
            .Include(n => n.Author)
            .Where(n => n.Score > 0)
            .OrderByDescending(n => n.Score)
            .Take(topN)
            .ToListAsync();

        var result = novels.Select(n => new ScoreRankingDto
        {
            NovelId = n.NovelId,
            NovelName = n.NovelName ?? string.Empty,
            AuthorName = n.Author?.AuthorName ?? "未知作者",
            CoverUrl = n.CoverUrl ?? string.Empty,
            Introduction = n.Introduction ?? string.Empty,
            Score = n.Score ?? 0
        }).ToList();

        return result;
    }


}
