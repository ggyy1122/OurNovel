using OurNovel.Models;
using OurNovel.Repositories;
using System;
using System.Threading.Tasks;

namespace OurNovel.Services
{
    /// <summary>
    /// CommentsService：评论业务逻辑类，继承基础服务，带管理日志写入
    /// </summary>
    public class CommentsService : BaseService<Comment, int>
    {

        private readonly ICommentReplyRepository _replyRepository;
        private readonly CommentManagementService _commentManagementService;

        public CommentsService(
            IRepository<Comment, int> repository,
            CommentManagementService commentManagementService,ICommentReplyRepository replyRepository)
            : base(repository)
        {
            _replyRepository = replyRepository;
            _commentManagementService = commentManagementService;
        }

        /// <param name="commentId">评论ID</param>
        /// <param name="status">状态（通过/封禁）</param>
        /// <param name="managerId">操作管理员ID</param>
        public async Task SetCommentStatusAsync(int commentId, string status, int managerId, string result)
        {
            if (status != "通过" && status != "封禁")
                throw new Exception("非法的评论状态，应为“通过”或“封禁”");

            var comment = await _repository.GetByIdAsync(commentId);
            if (comment == null)
                throw new Exception($"未找到 ID 为 {commentId} 的评论");

            comment.Status = status;
            await _repository.UpdateAsync(comment);

            await _commentManagementService.RecordManagementAsync(managerId, result, commentId);
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
        /*
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
        */

        /// <summary>
        /// 获取指定小说所有章节的第一级评论（即不作为回复的评论）
        /// </summary>
        public async Task<List<Comment>> GetTopLevelCommentsByNovelAsync(int novelId)
        {
            // 获取小说下所有评论
            var allComments = (await _repository.GetAllAsync())
                .Where(c => c.NovelId == novelId)
                .ToList();

            // 获取所有回复记录（涉及子评论）
            var repliedCommentIds = (await _replyRepository.GetAllAsync())
                .Select(r => r.CommentId)
                .Distinct()
                .ToHashSet();

            // 第一级评论：不在 repliedCommentIds 中的评论
            var topLevel = allComments
                .Where(c => !repliedCommentIds.Contains(c.CommentId))
                .ToList();

            return topLevel;
        }

        /// <summary>
        /// 获取小说所有章节的第一级评论的点赞数排行
        /// </summary>
        public async Task<List<Comment>> GetTopNTopLevelCommentsByLikesAsync(int novelId, int topN)
        {
            // 获取小说下所有评论
            var allComments = (await _repository.GetAllAsync())
                .Where(c => c.NovelId == novelId)
                .ToList();

            // 获取所有子评论 CommentReply 的 CommentId 列表
            var repliedCommentIds = (await _replyRepository.GetAllAsync())
                .Select(r => r.CommentId)
                .Distinct()
                .ToHashSet();

            // 过滤出第一级评论
            var topLevelComments = allComments
                .Where(c => !repliedCommentIds.Contains(c.CommentId))
                .ToList();

            // 排序取前 N 条（按点赞数降序）
            var topNComments = topLevelComments
                .OrderByDescending(c => c.Likes)  
                .Take(topN)
                .ToList();

            return topNComments;
        }

    }
}
