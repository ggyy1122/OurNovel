using OurNovel.Models;
using OurNovel.Repositories;

namespace OurNovel.Services
{
    /// <summary>
    /// CommentsService：评论业务逻辑类，继承基础服务
    /// </summary>
    public class CommentsService : BaseService<Comment, int>
    {
        private readonly ICommentReplyRepository _replyRepository;

        public CommentsService(
            IRepository<Comment, int> repository,
            ICommentReplyRepository replyRepository)
            : base(repository)
        {
            _replyRepository = replyRepository;
        }

        /// <summary>
        /// 审核评论：设置评论状态为“通过”或“封禁”
        /// </summary>
        public async Task SetCommentStatusAsync(int commentId, string status)
        {
            var comment = await _repository.GetByIdAsync(commentId);
            if (comment == null)
                throw new Exception($"未找到 ID 为 {commentId} 的评论");

            comment.Status = status;
            await _repository.UpdateAsync(comment);
        }

        /// <summary>
        /// 点赞评论：将评论的 Likes 加一
        /// </summary>
        public async Task LikeCommentAsync(int commentId)
        {
            var comment = await _repository.GetByIdAsync(commentId);
            if (comment == null)
                throw new Exception($"未找到 ID 为 {commentId} 的评论");

            comment.Likes++;
            await _repository.UpdateAsync(comment);
        }

        /// <summary>
        /// 递归删除指定评论及其所有子评论（包括评论回复关系）
        /// </summary>
        public async Task DeleteCommentRecursivelyAsync(int commentId)
        {
            // 获取所有评论回复关系
            var allReplies = (await _replyRepository.GetAllAsync()).ToList();

            // 递归获取所有子评论 ID
            List<int> commentsToDelete = new List<int> { commentId };
            CollectChildCommentIds(commentId, allReplies, commentsToDelete);

            // 删除所有评论（从子评论到父评论）
            foreach (var id in commentsToDelete.Distinct())
            {
                await _repository.DeleteAsync(id);
            }
        }

        /// <summary>
        /// 递归获取所有子评论 ID
        /// </summary>
        private void CollectChildCommentIds(int parentId, List<CommentReply> allReplies, List<int> result)
        {
            var children = allReplies
                .Where(r => r.PreComId == parentId)
                .Select(r => r.CommentId)
                .ToList();

            foreach (var childId in children)
            {
                if (!result.Contains(childId))
                {
                    result.Add(childId);
                    CollectChildCommentIds(childId, allReplies, result);
                }
            }
        }

        /// <summary>
        /// 审核评论，修改状态为“通过”或“封禁”
        /// </summary>
        public async Task<bool> ReviewCommentAsync(int commentId, string newStatus)
        {
            // 合法性检查（只能是“通过”或“封禁”）
            if (newStatus != "通过" && newStatus != "封禁")
                return false;

            var comment = await _repository.GetByIdAsync(commentId);
            if (comment == null)
                return false;

            comment.Status = newStatus;
            await _repository.UpdateAsync(comment);
            return true;
        }
    }
}
