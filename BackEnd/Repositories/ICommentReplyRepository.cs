using OurNovel.Models;

public interface ICommentReplyRepository
{
    Task AddAsync(CommentReply entity);
    Task<CommentReply?> GetByCommentIdAsync(int commentId);
    Task<IEnumerable<CommentReply>> GetRepliesByParentIdAsync(int parentCommentId);
    Task<IEnumerable<CommentReply>> GetAllAsync();
    Task DeleteAsync(int commentId);

    Task<List<int>> GetChildCommentIdsAsync(int parentCommentId);
    Task<int?> GetParentCommentIdAsync(int commentId);
    public Task<List<CommentReply>> GetRelatedRepliesAsync(List<int> commentIds);
}
