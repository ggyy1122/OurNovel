public class AuthorChangePasswordDto
{
    public string AuthorName { get; set; } = string.Empty;
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
