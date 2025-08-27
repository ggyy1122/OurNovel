using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RecommendRepository : IRecommendRepository
{
    private readonly AppDbContext _context;

    public RecommendRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Recommend entity)
    {
        _context.Recommends.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int novelId, int readerId)
    {
        var entity = await _context.Recommends.FindAsync(novelId, readerId);
        if (entity != null)
        {
            _context.Recommends.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Recommend>> GetByReaderIdAsync(int readerId)
    {
        return await _context.Recommends
            .Include(r => r.Novel)
            .Where(r => r.ReaderId == readerId)
            .Where(r => r.Novel.Status == "连载" || r.Novel.Status == "完结")
            .ToListAsync();
    }

    public async Task<IEnumerable<Recommend>> GetByNovelIdAsync(int novelId)
    {
        return await _context.Recommends
            .Include(r => r.Reader)
            .Where(r => r.NovelId == novelId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recommend>> GetAllAsync()
    {
        return await _context.Recommends
            .Include(r => r.Novel)
            .Include(r => r.Reader)
            .ToListAsync();
    }
}
