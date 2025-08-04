using OurNovel.DTOs;
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

        /// <summary>
        /// 获取指定章节的第一级评论
        /// </summary>
        public async Task<List<Comment>> GetTopLevelCommentsByChapterAsync(int novelId, int chapterId)
        {
            var allComments = (await _repository.GetAllAsync())
                .Where(c => c.NovelId == novelId && c.ChapterId == chapterId)
                .ToList();

            var repliedCommentIds = (await _replyRepository.GetAllAsync())
                .Select(r => r.CommentId)
                .Distinct()
                .ToHashSet();

            var topLevelComments = allComments
                .Where(c => !repliedCommentIds.Contains(c.CommentId))
                .ToList();

            return topLevelComments;
        }

        /// <summary>
        /// 获取指定章节的第一级评论点赞数排行
        /// </summary>
        public async Task<List<Comment>> GetTopNTopLevelCommentsByLikesByChapterAsync(int novelId, int chapterId, int topN)
        {
            var allComments = (await _repository.GetAllAsync())
                .Where(c => c.NovelId == novelId && c.ChapterId == chapterId)
                .ToList();

            var repliedCommentIds = (await _replyRepository.GetAllAsync())
                .Select(r => r.CommentId)
                .Distinct()
                .ToHashSet();

            var topLevelComments = allComments
                .Where(c => !repliedCommentIds.Contains(c.CommentId))
                .OrderByDescending(c => c.Likes)
                .Take(topN)
                .ToList();

            return topLevelComments;
        }
        /// <summary>
        /// 根据小说ID获取所有评论数量
        /// </summary>
        public async Task<int> GetCommentCountByNovelAsync(int novelId)
        {
            var allComments = await _repository.GetAllAsync();
            return allComments.Count(c => c.NovelId == novelId);
        }
        public async Task<List<CommentWithRepliesDto>> GetCommentsByReaderIdAsync(int readerId)
        {
            var allComments = await _repository.GetAllAsync();
            // 2. 获取该读者相关的评论（自己发的）
            var readerComments = allComments.Where(c => c.ReaderId == readerId).ToList();
            var readerCommentIds = readerComments.Select(c => c.CommentId).ToList();

            // 3. 获取与读者评论相关的回复关系（包含父是我、我是子）
            var relatedReplies = await _replyRepository.GetRelatedRepliesAsync(readerCommentIds);

            // 4. 提取所有相关 commentId
            var relatedCommentIds = relatedReplies
                .SelectMany(r => new[] { r.CommentId, r.PreComId ?? 0 })
                .Where(id => id != 0)
                .Distinct()
                .ToList();

            // 5. 过滤出相关的评论（读者自己和与之有父子关系的）
            var relatedComments = allComments
                .Where(c => relatedCommentIds.Contains(c.CommentId) || readerCommentIds.Contains(c.CommentId))
                .ToDictionary(c => c.CommentId);

            // 6. 构建评论结构：parentId -> List<child>
            var parentToChildren = new Dictionary<int, List<Comment>>();

            foreach (var reply in relatedReplies)
            {
                if (reply.PreComId.HasValue && relatedComments.TryGetValue(reply.CommentId, out var child))
                {
                    var parentId = reply.PreComId.Value;
                    if (!parentToChildren.ContainsKey(parentId))
                        parentToChildren[parentId] = new List<Comment>();
                    parentToChildren[parentId].Add(child);
                }
            }

            // 7. 构建最终结果，避免重复 ParentComment
            var visitedParentIds = new HashSet<int>();
            var result = new List<CommentWithRepliesDto>();

            foreach (var comment in readerComments)
            {
                // 如果是父评论
                if (parentToChildren.ContainsKey(comment.CommentId))
                {
                    if (!visitedParentIds.Contains(comment.CommentId))
                    {
                        result.Add(new CommentWithRepliesDto
                        {
                            ParentComment = comment,
                            ChildComments = parentToChildren[comment.CommentId]
                        });
                        visitedParentIds.Add(comment.CommentId);
                    }
                }
                else
                {
                    // 如果是子评论，找到父评论
                    var parentId = relatedReplies
                        .FirstOrDefault(r => r.CommentId == comment.CommentId)?.PreComId;

                    if (parentId.HasValue && relatedComments.TryGetValue(parentId.Value, out var parentComment))
                    {
                        if (!visitedParentIds.Contains(parentId.Value))
                        {
                            result.Add(new CommentWithRepliesDto
                            {
                                ParentComment = parentComment,
                                ChildComments = parentToChildren.ContainsKey(parentId.Value)
                                    ? parentToChildren[parentId.Value]
                                    : new List<Comment>()
                            });
                            visitedParentIds.Add(parentId.Value);
                        }
                    }
                    else
                    {
                        // 既不是父也不是子，作为独立项
                        if (!visitedParentIds.Contains(comment.CommentId))
                        {
                            result.Add(new CommentWithRepliesDto
                            {
                                ParentComment = comment,
                                ChildComments = new List<Comment>()
                            });
                            visitedParentIds.Add(comment.CommentId);
                        }
                    }
                }
            }

            return result;
        }





        }
    }
