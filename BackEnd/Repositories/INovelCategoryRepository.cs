using OurNovel.Models;

namespace OurNovel.Repositories
{
    public interface INovelCategoryRepository
    {
        Task AddAsync(NovelCategory entity);
        Task DeleteAsync(int novelId, string categoryName);
        Task<IEnumerable<NovelCategory>> GetAllAsync();
        Task<IEnumerable<Category>> GetCategoriesByNovelIdAsync(int novelId);
        Task<IEnumerable<Novel>> GetNovelsByCategoryAsync(string categoryName);
    }
}
