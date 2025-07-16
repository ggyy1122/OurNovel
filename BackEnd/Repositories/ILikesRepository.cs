using OurNovel.Models;
using OurNovel.Repositories;

public interface ILikesRepository : IRepository<Like, (int CommentId, int ReaderId)>
{
    Task<int> GetLikeCountByCommentIdAsync(int commentId);  // 示例：获取某评论的点赞数
}