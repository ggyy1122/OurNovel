namespace OurNovel.DTOs
{
    public class CommentWithRepliesDto
    {
        public Comment ParentComment { get; set; } = null!;  // 父评论
        public List<Comment> ChildComments { get; set; } = new();  // 子评论列表
    }
}
