using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;

namespace OurNovel.Repositories
{
    public class NovelCategoryRepository : INovelCategoryRepository
    {
        private readonly AppDbContext _context;

        public NovelCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NovelCategory entity)
        {
            _context.NovelCategories.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int novelId, string categoryName)
        {
            var entity = await _context.NovelCategories
                .FindAsync(novelId, categoryName);
            if (entity != null)
            {
                _context.NovelCategories.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NovelCategory>> GetAllAsync()
        {
            return await _context.NovelCategories
                .Include(nc => nc.Novel)
                .Include(nc => nc.Category)
                .ToListAsync();
        }



        public async Task<IEnumerable<Category>> GetCategoriesByNovelIdAsync(int novelId)
        {
            return await _context.NovelCategories
                .Where(nc => nc.NovelId == novelId)
                .Select(nc => nc.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Novel>> GetNovelsByCategoryAsync(string categoryName)
        {
            return await _context.NovelCategories
                .Where(nc => nc.CategoryName == categoryName)
                .Select(nc => nc.Novel)
                .ToListAsync();
        }
    }
}
