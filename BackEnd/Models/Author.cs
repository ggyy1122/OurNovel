/*2353238关云兮*/
namespace OurNovel.Models
{
    public class Author : IEntity<int>
    {
        public int AuthorId { get; set; }               // AUTHOR_ID 主键
        int IEntity<int>.Id
        {
            get => AuthorId;
            set => AuthorId = value;
        }

        public string AuthorName { get; set; } = null!; // AUTHOR_NAME 非空

        public string Password { get; set; } = null!;   // PASSWORD 非空

        public decimal Earning { get; set; } = 0m;      // EARNING 默认为0，decimal 对应 Number(18,2)

        public string? Phone { get; set; }              // PHONE 可空

        public string? AvatarUrl { get; set; }          // AVATAR_URL 可空
    }
}
