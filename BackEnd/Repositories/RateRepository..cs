using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RateRepository : IRateRepository
{
    private readonly AppDbContext _context;

    public RateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddOrUpdateAsync(Rate rate)
    {
        var existing = await _context.Rates.FindAsync(rate.NovelId, rate.ReaderId);
        if (existing != null)
        {
            existing.Score = rate.Score;
            existing.RatingTime = rate.RatingTime;
        }
        else
        {
            _context.Rates.Add(rate);
        }
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int novelId, int readerId)
    {
        var rate = await _context.Rates.FindAsync(novelId, readerId);
        if (rate != null)
        {
            _context.Rates.Remove(rate);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Rate>> GetByNovelIdAsync(int novelId)
    {
        return await _context.Rates
            .Include(r => r.Reader)
            .Where(r => r.NovelId == novelId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rate>> GetByReaderIdAsync(int readerId)
    {
        return await _context.Rates
            .Include(r => r.Novel)
            .Where(r => r.ReaderId == readerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rate>> GetAllAsync()
    {
        return await _context.Rates
            .Include(r => r.Novel)
            .Include(r => r.Reader)
            .ToListAsync();
    }
}
