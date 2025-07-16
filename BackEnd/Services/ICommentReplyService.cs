using OurNovel.Models;

namespace OurNovel.Services
{
    public interface ICommentReplyService
    {
        Task AddReplyAsync(CommentReply reply);

        Task<CommentReply?> GetReplyByCommentIdAsync(int commentId);

        Task<IEnumerable<CommentReply>> GetRepliesByParentIdAsync(int parentCommentId);

        /// <summary>
        /// 删除评论及其所有回复，包括所有层级的子回复
        /// </summary>
        /// <param name="commentId">评论 ID</param>
        Task DeleteByCommentIdAsync(int commentId);
        Task<IEnumerable<CommentReply>> GetAllRepliesAsync();
    }
}
