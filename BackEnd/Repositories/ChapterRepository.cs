using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;

namespace OurNovel.Repositories
{
    /// <summary>
    /// Chapter 仓储实现类，适配复合主键（NovelId + ChapterId）
    /// </summary>
    public class ChapterRepository : IChapterRepository
    {
        private readonly AppDbContext _context;

        public ChapterRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取某部小说下的所有章节
        /// </summary>
        public async Task<IEnumerable<Chapter>> GetByNovelIdAsync(int novelId)
        {
            return await _context.Chapters
                                 .Where(c => c.NovelId == novelId)
                                 .OrderBy(c => c.ChapterId) // 可选排序
                                 .ToListAsync();
        }

        /// <summary>
        /// 根据复合主键获取章节
        /// </summary>
        public async Task<Chapter?> GetByIdAsync(int novelId, int chapterId)
        {
            return await _context.Chapters
                                 .FirstOrDefaultAsync(c => c.NovelId == novelId && c.ChapterId == chapterId);
        }

        /// <summary>
        /// 添加新章节
        /// </summary>
        public async Task AddAsync(Chapter chapter)
        {
            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 更新章节
        /// </summary>
        public async Task UpdateAsync(Chapter chapter)
        {
            _context.Chapters.Update(chapter);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 删除章节
        /// </summary>
        public async Task DeleteAsync(int novelId, int chapterId)
        {
            var chapter = await GetByIdAsync(novelId, chapterId);
            if (chapter != null)
            {
                _context.Chapters.Remove(chapter);
                await _context.SaveChangesAsync();
            }
        }
    }
}
