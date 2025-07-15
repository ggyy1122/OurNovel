using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class CollectRepository : ICollectRepository
{
    private readonly AppDbContext _context;

    public CollectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Collect entity)
    {
        _context.Collects.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int novelId, int readerId)
    {
        var entity = await _context.Collects.FindAsync(novelId, readerId);
        if (entity != null)
        {
            _context.Collects.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Collect>> GetByReaderIdAsync(int readerId)
    {
        return await _context.Collects
            .Include(c => c.Novel)
            .Where(c => c.ReaderId == readerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Collect>> GetByNovelIdAsync(int novelId)
    {
        return await _context.Collects
            .Include(c => c.Reader)
            .Where(c => c.NovelId == novelId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Collect>> GetAllAsync()
    {
        return await _context.Collects
            .Include(c => c.Novel)
            .Include(c => c.Reader)
            .ToListAsync();
    }
}
