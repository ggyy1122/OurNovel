namespace OurNovel.Models
{
    
    public class AuthorIncome
    {
        public long Id { get; set; }
        public int AuthorId { get; set; }
        public int? NovelId { get; set; }
        public string Type { get; set; } = "����"; // ����չ���ͣ����͡��ֳɵ�
        public decimal Amount { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
