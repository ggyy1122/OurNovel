using OurNovel.Repositories;
using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurNovel.Services
{
    /// <summary>
    /// 泛型基础服务，封装常规增删查改
    /// </summary>
    public class BaseService<T, TKey> where T : class
    {
        protected readonly IRepository<T, TKey> _repository;

        /// <summary>
        /// 构造函数，注入仓储实例
        /// </summary>
        public BaseService(IRepository<T, TKey> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 获取所有实体记录
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// 添加新实体
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        public virtual async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
        }

        /// <summary>
        /// 更新现有实体
        /// </summary>
        /// <param name="entity">要更新的实体对象</param>
        public virtual async Task UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
        }

        /// <summary>
        /// 根据主键ID获取单个实体
        /// </summary>
        /// <param name="id">实体主键ID</param>
        public virtual async Task<T?> GetByIdAsync(TKey id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// 根据主键ID删除实体
        /// </summary>
        /// <param name="id">要删除的实体主键ID</param>
        public virtual async Task DeleteAsync(TKey id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}