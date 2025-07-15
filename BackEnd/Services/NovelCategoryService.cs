using OurNovel.Models;
using OurNovel.Repositories;

namespace OurNovel.Services
{
    public class NovelCategoryService : INovelCategoryService
    {
        private readonly INovelCategoryRepository _repository;

        /// <summary>
        /// 一本小说可以属于多个分类（修仙、热血、穿越），一个分类可以包含多部小说
        /// </summary>
        /// <param name="repository"></param>
        public NovelCategoryService(INovelCategoryRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///   添加小说与分类关系 （例如 小说1 → 加入“修仙”）
        /// </summary>
        /// <param name="novelId"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public async Task AddAsync(int novelId, string categoryName)
        {
            var entity = new NovelCategory
            {
                NovelId = novelId,
                CategoryName = categoryName
            };
            await _repository.AddAsync(entity);
        }

        /// <summary>
        ///   删除小说与分类关系 （例如 小说1 → 删除“修仙”）
        /// </summary>
        /// <param name="novelId"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int novelId, string categoryName)
        {
            await _repository.DeleteAsync(novelId, categoryName);
        }

        /// <summary>
        /// 获取所有分类及小说
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<NovelCategory>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// 获取某本小说的全部分类 
        /// </summary>
        /// <param name="novelId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetCategoriesByNovelIdAsync(int novelId)
        {
            return await _repository.GetCategoriesByNovelIdAsync(novelId);
        }

        /// <summary>
        /// 获取某个分类下的所有小说
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Novel>> GetNovelsByCategoryAsync(string categoryName)
        {
            return await _repository.GetNovelsByCategoryAsync(categoryName);
        }
    }
}
