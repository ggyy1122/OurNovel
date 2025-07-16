using OurNovel.Models;
using OurNovel.Repositories;

namespace OurNovel.Services
{
    public class CommentReplyService : ICommentReplyService
    {
        private readonly ICommentReplyRepository _replyRepository;

        public CommentReplyService(ICommentReplyRepository replyRepository)
        {
            _replyRepository = replyRepository;
        }

        public async Task AddReplyAsync(CommentReply reply)
        {
            await _replyRepository.AddAsync(reply);
        }

        public async Task<CommentReply?> GetReplyByCommentIdAsync(int commentId)
        {
            return await _replyRepository.GetByCommentIdAsync(commentId);
        }

        public async Task<IEnumerable<CommentReply>> GetRepliesByParentIdAsync(int parentCommentId)
        {
            return await _replyRepository.GetRepliesByParentIdAsync(parentCommentId);
        }
        public async Task DeleteByCommentIdAsync(int commentId)
        {
            var reply = await _replyRepository.GetByCommentIdAsync(commentId);
            if (reply != null)
            {
                await _replyRepository.DeleteAsync(commentId);
            }
        }
        public async Task<IEnumerable<CommentReply>> GetAllRepliesAsync()
        {
            return await _replyRepository.GetAllAsync();
        }

    }
}
