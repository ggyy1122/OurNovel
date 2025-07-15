using OurNovel.Models;
using OurNovel.Repositories;
using OurNovel.Services.FileStorage.Interfaces;

namespace OurNovel.Services
{
    /// <summary>
    /// CommentsService：评论业务逻辑类，继承基础服务
    /// </summary>
    public class CommentsService : BaseService<Comment, int>
    {
        public CommentsService(IRepository<Comment, int> repository)
            : base(repository)
        {
        }

        // 在这里添加评论相关的特殊业务逻辑方法

        /// <summary>
        /// 审核评论：设置评论状态为“通过”或“封禁”
        /// </summary>
        /// <param name="commentId">评论ID</param>
        /// <param name="status">状态（通过/封禁）</param>
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
        /// <param name="commentId">评论ID</param>
        public async Task LikeCommentAsync(int commentId)
        {
            var comment = await _repository.GetByIdAsync(commentId);
            if (comment == null)
                throw new Exception($"未找到 ID 为 {commentId} 的评论");

            comment.Likes++;
            await _repository.UpdateAsync(comment);
        }
    }
}
