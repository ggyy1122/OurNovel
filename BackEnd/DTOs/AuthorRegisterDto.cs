namespace OurNovel.DTOs
{
    /// <summary>
    /// 作者注册请求 DTO
    /// </summary>
    public class AuthorRegisterDto
    {
        public string AuthorName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
