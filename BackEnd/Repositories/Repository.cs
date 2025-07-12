using Microsoft.EntityFrameworkCore; 
using OurNovel.Data;
using OurNovel.Models;
using System.Collections.Generic;   
using System.Threading.Tasks;      

namespace OurNovel.Repositories
{
    /// <summary>
    /// 泛型仓储基类，封装对数据库表的通用CRUD操作
    /// </summary>
    /// <typeparam name="T">实体类型（如Reader、Post）</typeparam>
    /// <typeparam name="TKey">实体主键类型（如int、string）</typeparam>
    public class Repository<T,TKey> : IRepository<T,TKey> where T : class
    {
        // 数据库上下文实例，用于整个仓储的生命周期
        private readonly AppDbContext _context;

        // 对应实体类型的DbSet，用于直接操作数据库表
        private readonly DbSet<T> _dbSet;

        /// <summary>
        /// 构造函数，通过依赖注入获取数据库上下文
        /// </summary>
        /// <param name="context">数据库上下文实例</param>
        public Repository(AppDbContext context)
        {
            _context = context;
            // 从上下文中获取对应实体类型的DbSet
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// 异步获取所有实体记录
        /// </summary>
        /// <returns>实体集合（IEnumerable）</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // 转换为列表异步查询（立即执行）
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// 异步添加新实体到数据库
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        public async Task AddAsync(T entity)
        {
            // 将实体标记为Added状态
            await _dbSet.AddAsync(entity);
            // 立即提交到数据库
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 异步更新现有实体
        /// </summary>
        /// <param name="entity">要更新的实体对象</param>
        public async Task UpdateAsync(T entity)
        {
            // 将实体标记为Modified状态
            _dbSet.Update(entity);
            // 立即提交到数据库
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 异步根据主键ID查询单个实体
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns>找到的实体或null</returns>
        public async Task<T?> GetByIdAsync(TKey id)
        {
            // 使用 FindAsync 高效查询（优先从内存缓存查找）
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// 异步根据主键ID删除实体
        /// </summary>
        /// <param name="id">主键值</param>
        public async Task DeleteAsync(TKey id)
        {
            // 先查询确保实体存在
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                // 将实体标记为 Deleted 状态
                _dbSet.Remove(entity);
                // 立即提交到数据库
                await _context.SaveChangesAsync();
            }
        }

    }
}