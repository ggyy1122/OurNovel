using OurNovel.Models;

namespace OurNovel.Services
{
    public interface ICommentReplyService
    {
        Task AddReplyAsync(CommentReply reply);

        Task<CommentReply?> GetReplyByCommentIdAsync(int commentId);

        Task<IEnumerable<CommentReply>> GetRepliesByParentIdAsync(int parentCommentId);

        /// <summary>
        /// ɾ�����ۼ������лظ����������в㼶���ӻظ�
        /// </summary>
        /// <param name="commentId">���� ID</param>
        Task DeleteByCommentIdAsync(int commentId);
        Task<IEnumerable<CommentReply>> GetAllRepliesAsync();
    }
}
