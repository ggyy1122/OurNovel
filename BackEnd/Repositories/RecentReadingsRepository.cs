﻿using OurNovel.Data;
using OurNovel.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RecentReadingsRepository : IRecentReadingsRepository
{
    private readonly AppDbContext _context;

    public RecentReadingsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddOrUpdateAsync(RecentReadings entity)
    {
        // 先查询是否存在该读者-小说组合的记录
        var existing = await _context.RecentReadings
            .FirstOrDefaultAsync(r => r.ReaderId == entity.ReaderId && r.NovelId == entity.NovelId);

        if (existing == null)
        {
            // 不存在，新增
            await _context.RecentReadings.AddAsync(entity);
        }
        else
        {
            // 存在，更新最近阅读时间和章节ID
            existing.RecentReadingTime = DateTime.Now;
            existing.ChapterId = entity.ChapterId;
            _context.RecentReadings.Update(existing);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int readerId, int novelId)
    {
        var entity = await _context.RecentReadings
            .FirstOrDefaultAsync(r => r.ReaderId == readerId && r.NovelId == novelId);

        if (entity != null)
        {
            _context.RecentReadings.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<RecentReadings>> GetByReaderIdAsync(int readerId)
    {
        return await _context.RecentReadings
            .Include(r => r.Novel)
            .Where(r => r.ReaderId == readerId)
            .Where(r => r.Novel.Status == "连载" || r.Novel.Status == "完结")
            .OrderByDescending(r => r.RecentReadingTime)  
            .ToListAsync();
    }

    public async Task<RecentReadings?> GetByReaderAndNovelAsync(int readerId, int novelId)
    {
        return await _context.RecentReadings
            .FirstOrDefaultAsync(r => r.ReaderId == readerId && r.NovelId == novelId);
    }
}