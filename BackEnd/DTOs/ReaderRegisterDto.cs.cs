namespace OurNovel.DTOs
{
    public class ReaderRegisterDto
    {
        public string ReaderName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ReaderRegisterDtoWithPhone
    {
        public string ReaderName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
