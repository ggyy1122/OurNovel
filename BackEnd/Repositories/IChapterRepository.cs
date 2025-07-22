// 文件: Repositories/IChapterRepository.cs
using OurNovel.Models;

namespace OurNovel.Repositories
{
    public interface IChapterRepository
    {
        Task<IEnumerable<Chapter>> GetByNovelIdAsync(int novelId);
        Task<Chapter?> GetByIdAsync(int novelId, int chapterId);
        Task AddAsync(Chapter chapter);
        Task UpdateAsync(Chapter chapter);
        Task DeleteAsync(int novelId, int chapterId);
        Task<IEnumerable<Chapter>> GetAllAsync();
        Task<Novel?> GetNovelByIdAsync(int novelId);
        Task UpdateNovelAsync(Novel novel);
    }
}
