using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurNovel.Repositories
{
    /// <summary>
    /// 泛型仓储接口，定义常用的增删查改方法
    /// </summary>
    public interface IRepository<T, TKey> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();        // 查询所有
        Task<T?> GetByIdAsync(TKey id);            // 根据主键查找
        Task AddAsync(T entity);                   // 新增
        Task UpdateAsync(T entity);                // 更新
        Task DeleteAsync(TKey id);                 // 删除
    }
}
