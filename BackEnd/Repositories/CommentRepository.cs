using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;

namespace OurNovel.Repositories
{
    public class CommentRepository : IRepository<Comment, int>
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Comment> _dbSet;
        public CommentRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Comment>();
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(Comment entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}