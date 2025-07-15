using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;

/// <summary>
/// Likes 仓储实现类，负责对点赞（Like）实体的数据库操作。
/// 提供增删改查、按评论统计点赞数等方法。
/// </summary>
public class LikesRepository : ILikesRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Like> _dbSet;

    /// <summary>
    /// 构造函数，初始化数据库上下文和实体集合
    /// </summary>
    /// <param name="context">应用数据库上下文</param>
    public LikesRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Like>();
    }

    /// <summary>
    /// 根据评论 ID 和读者 ID 获取点赞记录
    /// </summary>
    /// <param name="id">元组 (CommentId, ReaderId)</param>
    /// <returns>对应的 Like 实体或 null</returns>
    public async Task<Like?> GetByIdAsync((int CommentId, int ReaderId) id)
    {
        return await _dbSet.FindAsync(id.CommentId, id.ReaderId);
    }

    /// <summary>
    /// 获取某条评论的点赞数量
    /// </summary>
    /// <param name="commentId">评论 ID</param>
    /// <returns>点赞数量</returns>
    public async Task<int> GetLikeCountByCommentIdAsync(int commentId)
    {
        return await _dbSet.CountAsync(l => l.CommentId == commentId);
    }

    /// <summary>
    /// 添加新的点赞记录
    /// </summary>
    /// <param name="entity">Like 实体</param>
    public async Task AddAsync(Like entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 删除指定的点赞记录
    /// </summary>
    /// <param name="id">元组 (CommentId, ReaderId)</param>
    public async Task DeleteAsync((int CommentId, int ReaderId) id)
    {
        var entity = await _dbSet.FindAsync(id.CommentId, id.ReaderId);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// 更新点赞实体（一般不常用）
    /// </summary>
    /// <param name="entity">Like 实体</param>
    public async Task UpdateAsync(Like entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 获取所有点赞记录（通常用于后台管理或分析）
    /// </summary>
    /// <returns>所有 Like 实体的集合</returns>
    public async Task<IEnumerable<Like>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
}
