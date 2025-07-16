using Microsoft.EntityFrameworkCore;
using OurNovel.Models;
using OurNovel.Repositories;

namespace OurNovel.Services
{
    /// <summary>
    /// Likes 服务类，实现对评论点赞功能的相关业务逻辑。
    /// 提供点赞、取消点赞、判断是否已点赞、获取点赞数和获取所有点赞记录等方法。
    /// </summary>
    public class LikesService : ILikesService
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IRepository<Comment, int> _commentRepository;

        /// <summary>
        /// 构造函数，注入点赞仓储和评论仓储接口
        /// </summary>
        /// <param name="likesRepository">点赞仓储接口</param>
        /// <param name="commentRepository">评论仓储接口</param>
        public LikesService(ILikesRepository likesRepository, IRepository<Comment, int> commentRepository)
        {
            _likesRepository = likesRepository;
            _commentRepository = commentRepository;
        }

        /// <summary>
        /// 给指定评论点赞（避免重复点赞）
        /// 同时将评论的 Likes 数字段加一
        /// </summary>
        /// <param name="commentId">评论 ID</param>
        /// <param name="readerId">读者 ID</param>
        public async Task LikeAsync(int commentId, int readerId)
        {
            var existing = await _likesRepository.GetByIdAsync((commentId, readerId));
            if (existing != null)
                return;  // 如果已经点赞过，直接返回

            var like = new Like
            {
                CommentId = commentId,
                ReaderId = readerId
            };

            await _likesRepository.AddAsync(like);

            // 点赞数加一（可优化为缓存或定期刷新统计）
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment != null)
            {
                comment.Likes++;
                await _commentRepository.UpdateAsync(comment);
            }
        }

        /// <summary>
        /// 取消对指定评论的点赞
        /// 同时将评论的 Likes 数字段减一（不能小于 0）
        /// </summary>
        /// <param name="commentId">评论 ID</param>
        /// <param name="readerId">读者 ID</param>
        public async Task UnlikeAsync(int commentId, int readerId)
        {
            var like = await _likesRepository.GetByIdAsync((commentId, readerId));
            if (like == null)
                return;  // 如果没有点赞，直接返回

            await _likesRepository.DeleteAsync((commentId, readerId));

            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment != null && comment.Likes > 0)
            {
                comment.Likes--;
                await _commentRepository.UpdateAsync(comment);
            }
        }

        /// <summary>
        /// 判断读者是否已点赞某条评论
        /// </summary>
        /// <param name="commentId">评论 ID</param>
        /// <param name="readerId">读者 ID</param>
        /// <returns>布尔值，表示是否点赞</returns>
        public async Task<bool> IsLikedAsync(int commentId, int readerId)
        {
            var like = await _likesRepository.GetByIdAsync((commentId, readerId));
            return like != null;
        }

        /// <summary>
        /// 获取某条评论的点赞数量
        /// </summary>
        /// <param name="commentId">评论 ID</param>
        /// <returns>点赞数量</returns>
        public async Task<int> GetLikeCountAsync(int commentId)
        {
            var count = await _likesRepository.GetLikeCountByCommentIdAsync(commentId);
            return count;
        }

        /// <summary>
        /// 获取所有点赞记录（适用于后台管理或统计分析）
        /// </summary>
        /// <returns>点赞实体集合</returns>
        public async Task<IEnumerable<Like>> GetAllLikesAsync()
        {
            return await _likesRepository.GetAllAsync();
        }
    }
}
