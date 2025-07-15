using OurNovel.Models;

namespace OurNovel.Services
{
    public interface ICommentReplyService
    {
        Task AddReplyAsync(CommentReply reply);
        Task<CommentReply?> GetReplyByCommentIdAsync(int commentId);
        Task<IEnumerable<CommentReply>> GetRepliesByParentIdAsync(int parentCommentId);
    }
}
