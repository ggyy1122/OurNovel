using OurNovel.Models;

public interface ICommentReplyRepository
{
    Task AddAsync(CommentReply entity);
    Task<CommentReply?> GetByCommentIdAsync(int commentId);
    Task<IEnumerable<CommentReply>> GetRepliesByParentIdAsync(int parentCommentId);
}
