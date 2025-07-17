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
        // 先查收藏数最高的小说ID和收藏数
        var topCollected = await _context.Collects
            .GroupBy(c => c.NovelId)
            .Select(g => new { NovelId = g.Key, CollectCount = g.Count() })
            .OrderByDescending(x => x.CollectCount)
            .Take(topN)
            .ToListAsync();

        var novelIds = topCollected.Select(x => x.NovelId).ToList();

        // 再批量查小说并加载作者信息
        var novels = await _context.Novels
            .Where(n => novelIds.Contains(n.NovelId))
            .Include(n => n.Author) // 重点：加载导航属性
            .ToListAsync();

        // 最后把两边数据join组装DTO
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
            .OrderByDescending(x => x.CollectCount) // 可选：再次排序保证顺序
            .ToList();

        return result;
    }

}
