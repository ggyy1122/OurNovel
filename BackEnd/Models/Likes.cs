namespace OurNovel.Models
{
    public class Like
    {
        public int CommentId { get; set; }
        public Comment Comment { get; set; }

        public int ReaderId { get; set; }
        public Reader Reader { get; set; }
    }
}