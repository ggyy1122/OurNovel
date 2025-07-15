using OurNovel.Models;
using OurNovel.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurNovel.Services
{
    /// <summary>
    /// Category 服务，继承基础服务，可扩展自定义业务
    /// </summary>
    public class CategoryService : BaseService<Category, string>
    {
        public CategoryService(IRepository<Category, string> repository)
            : base(repository)
        {
        }

        // 获取所有类别
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        // 添加类别
        public async Task AddCategoryAsync(string categoryName)
        {
            var category = new Category { CategoryName = categoryName };
            await _repository.AddAsync(category);
        }

        // 更新类别（先获取实体，再更新）
        public async Task PutCategoryAsync(string oldCategoryName, string newCategoryName)
        {
            var existing = await _repository.GetByIdAsync(oldCategoryName);
            if (existing == null)
            {
                throw new KeyNotFoundException($"类别 '{oldCategoryName}' 不存在");
            }

            // 如果新旧名称相同，说明没改主键，直接返回或更新其他属性
            if (oldCategoryName == newCategoryName)
            {
                // 这里只是示例，Category目前只有主键，没有其他属性
                // 如果有其他属性，可在这里更新后调用 UpdateAsync(existing);
                return;
            }

            // 检查新类别名是否已存在，避免主键冲突
            var conflict = await _repository.GetByIdAsync(newCategoryName);
            if (conflict != null)
            {
                throw new InvalidOperationException($"类别 '{newCategoryName}' 已存在，不能重命名");
            }

            // 删除旧记录，添加新记录
            await _repository.DeleteAsync(oldCategoryName);
            var newCategory = new Category { CategoryName = newCategoryName };
            await _repository.AddAsync(newCategory);
        }

        // 删除类别（先确认存在，再删除）
        public async Task DeleteCategoryAsync(string categoryName)
        {
            var category = await _repository.GetByIdAsync(categoryName);
            if (category == null)
            {
                throw new KeyNotFoundException($"类别 '{categoryName}' 不存在");
            }

            await _repository.DeleteAsync(categoryName);
        }
    }
}
