using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;

namespace OurNovel.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly AppDbContext _context;

        public ChapterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Chapter>> GetByNovelIdAsync(int novelId)
        {
            return await _context.Chapters
                                 .Where(c => c.NovelId == novelId)
                                 .OrderBy(c => c.ChapterId)
                                 .ToListAsync();
        }

        public async Task<Chapter?> GetByIdAsync(int novelId, int chapterId)
        {
            return await _context.Chapters
                                 .FirstOrDefaultAsync(c => c.NovelId == novelId && c.ChapterId == chapterId);
        }

        public async Task AddAsync(Chapter chapter)
        {
            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Chapter chapter)
        {
            _context.Chapters.Update(chapter);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int novelId, int chapterId)
        {
            var chapter = await GetByIdAsync(novelId, chapterId);
            if (chapter != null)
            {
                _context.Chapters.Remove(chapter);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Chapter>> GetAllAsync()
        {
            return await _context.Chapters.ToListAsync();
        }
    }
}
