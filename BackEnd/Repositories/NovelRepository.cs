using System.Linq.Expressions;
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

    public async Task<List<CollectRankingDto>> GetTopCollectedNovelsAsync(int topN, string? status = null)
    {
        var novelsQuery = _context.Novels
            .Where(n => n.CollectedCount >= 0);

        // 添加状态筛选
        if (!string.IsNullOrEmpty(status))
        {
            novelsQuery = novelsQuery.Where(n => n.Status == status);
        }
        else
        {
            // status 为 null 时，只筛选"连载"或"完结"状态
            novelsQuery = novelsQuery.Where(n => n.Status == "连载" || n.Status == "完结");
        }

        var novels = await novelsQuery
            .Include(n => n.Author)
            .OrderByDescending(n => n.CollectedCount)
            .Take(topN)
            .ToListAsync();

        var result = novels.Select(n => new CollectRankingDto
        {
            NovelId = n.NovelId,
            NovelName = n.NovelName ?? string.Empty,
            AuthorId = n.AuthorId,
            AuthorName = n.Author?.AuthorName ?? "未知作者",
            CoverUrl = n.CoverUrl ?? string.Empty,
            Introduction = n.Introduction ?? string.Empty,
            CollectedCount = n.CollectedCount ?? 0,
            Score = n.Score,
            TotalWordCount = n.TotalWordCount,
            Status = n.Status ?? "未知状态",
            TotalPrice = n.TotalPrice ?? 0,
            RecommendCount = n.RecommendCount ?? 0,
            CreateTime = n.CreateTime
        }).ToList();

        return result;
    }


    public async Task<List<RecommendRankingDto>> GetTopRecommendedNovelsAsync(int topN, string? status = null)
    {
        var novelsQuery = _context.Novels
            .Where(n => n.RecommendCount >= 0);

        // 添加状态筛选
        if (!string.IsNullOrEmpty(status))
        {
            novelsQuery = novelsQuery.Where(n => n.Status == status);
        }
        else
        {
            // status 为 null 时，只筛选"连载"或"完结"状态
            novelsQuery = novelsQuery.Where(n => n.Status == "连载" || n.Status == "完结");
        }

        var novels = await novelsQuery
            .Include(n => n.Author)
            .OrderByDescending(n => n.RecommendCount)
            .Take(topN)
            .ToListAsync();

        var result = novels.Select(n => new RecommendRankingDto
        {
            NovelId = n.NovelId,
            NovelName = n.NovelName ?? string.Empty,
            AuthorId = n.AuthorId,
            AuthorName = n.Author?.AuthorName ?? "未知作者",
            CoverUrl = n.CoverUrl ?? string.Empty,
            Introduction = n.Introduction ?? string.Empty,
            RecommendCount = n.RecommendCount ?? 0,
            Score = n.Score,
            TotalWordCount = n.TotalWordCount,
            Status = n.Status ?? "未知状态",
            TotalPrice = n.TotalPrice ?? 0,
            CreateTime = n.CreateTime,
            CollectedCount = n.CollectedCount ?? 0
        }).ToList();

        return result;
    }


    public async Task<List<ScoreRankingDto>> GetTopScoredNovelsAsync(int topN, string? status = null)
    {
        var novelsQuery = _context.Novels
            .Where(n => n.Score >= 0);

        // 添加状态筛选
        if (!string.IsNullOrEmpty(status))
        {
            novelsQuery = novelsQuery.Where(n => n.Status == status);
        }
        else
        {
            // status 为 null 时，只筛选"连载"或"完结"状态
            novelsQuery = novelsQuery.Where(n => n.Status == "连载" || n.Status == "完结");
        }

        var novels = await novelsQuery
            .Include(n => n.Author)
            .OrderByDescending(n => n.Score)
            .Take(topN)
            .ToListAsync();

        var result = novels.Select(n => new ScoreRankingDto
        {
            NovelId = n.NovelId,
            NovelName = n.NovelName ?? string.Empty,
            AuthorId = n.AuthorId,
            AuthorName = n.Author?.AuthorName ?? "未知作者",
            CoverUrl = n.CoverUrl ?? string.Empty,
            Introduction = n.Introduction ?? string.Empty,
            RecommendCount = n.RecommendCount ?? 0,
            Score = n.Score ?? 0,
            TotalWordCount = n.TotalWordCount,
            Status = n.Status ?? "未知状态",
            TotalPrice = n.TotalPrice ?? 0,
            CreateTime = n.CreateTime,
            CollectedCount = n.CollectedCount ?? 0
        }).ToList();

        return result;
    }

    /// <summary>
    /// 根据小说ID获取小说某个属性
    /// </summary>
    public async Task<TResult> GetNovelPropertyAsync<TResult>(int novelId, Expression<Func<Novel, TResult>> propertySelector)
    {
        return await _context.Novels
            .Where(n => n.NovelId == novelId)
            .Select(propertySelector)
            .FirstOrDefaultAsync();
    }

}
