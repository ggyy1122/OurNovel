using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;

public class LikesRepository : ILikesRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Like> _dbSet;

    public LikesRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Like>();
    }

    public async Task<Like?> GetByIdAsync((int CommentId, int ReaderId) id)
    {
        return await _dbSet.FindAsync(id.CommentId, id.ReaderId);
    }

    public async Task<int> GetLikeCountByCommentIdAsync(int commentId)
    {
        return await _dbSet.CountAsync(l => l.CommentId == commentId);
    }

    public async Task AddAsync(Like entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync((int CommentId, int ReaderId) id)
    {
        var entity = await _dbSet.FindAsync(id.CommentId, id.ReaderId);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(Like entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Like>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

}
