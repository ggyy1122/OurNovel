using OurNovel.Models;

namespace OurNovel.Services
{
    public interface INovelCategoryService
    {
        Task AddAsync(int novelId, string categoryName);
        Task DeleteAsync(int novelId, string categoryName);
        Task<IEnumerable<NovelCategory>> GetAllAsync();
        Task<IEnumerable<Category>> GetCategoriesByNovelIdAsync(int novelId);
        Task<IEnumerable<Novel>> GetNovelsByCategoryAsync(string categoryName);
    }
}
