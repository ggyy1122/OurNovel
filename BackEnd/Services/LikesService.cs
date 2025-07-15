using OurNovel.Models;
using OurNovel.Repositories;

namespace OurNovel.Services
{
    public class LikesService : ILikesService
    {
        private readonly ILikesRepository _likesRepository; 
        private readonly IRepository<Comment, int> _commentRepository;

        public LikesService(ILikesRepository likesRepository, IRepository<Comment, int> commentRepository)
        {
            _likesRepository = likesRepository;
            _commentRepository = commentRepository;
        }

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

            // 点赞数加一（可以通过数据库统计来替代）
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment != null)
            {
                comment.Likes++;
                await _commentRepository.UpdateAsync(comment);
            }
        }

        public async Task UnlikeAsync(int commentId, int readerId)
        {
            var like = await _likesRepository.GetByIdAsync((commentId, readerId));
            if (like == null)
                return;  // 如果没有点赞，直接返回

            await _likesRepository.DeleteAsync((commentId, readerId));

            // 点赞数减一（可以通过数据库统计来替代）
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment != null && comment.Likes > 0)
            {
                comment.Likes--;
                await _commentRepository.UpdateAsync(comment);
            }
        }

        public async Task<bool> IsLikedAsync(int commentId, int readerId)
        {
            var like = await _likesRepository.GetByIdAsync((commentId, readerId));
            return like != null;
        }

        public async Task<int> GetLikeCountAsync(int commentId)
        {
            var count = await _likesRepository.GetLikeCountByCommentIdAsync(commentId);
            return count;
        }
    }
}
