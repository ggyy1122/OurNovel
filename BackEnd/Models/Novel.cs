namespace OurNovel.Models
{
    public class Novel : IEntity<int>
    {
        public int NovelId { get; set; }                   // Novel_ID ����
        int IEntity<int>.Id
        {
            get => NovelId;
            set => NovelId = value;
        }

        public int AuthorId { get; set; }                  // �������Ӧ author_id
        //public Author Author { get; set; } = null!;        // ��������

        public string? NovelName { get; set; } = null!;    // Novel_NAME �ɿ�

        public string? Introduction { get; set; } = null!; // Introduction �ɿ�

        public DateTime? CreateTime { get; set; }          // CreateTime �ɿ�

        public string? CoverUrl { get; set; }              // CoverUrl �ɿ�

        public int? Score { get; set; }                    // Score �ɿ�

        public long? TotalWordCount { get; set; }          // TotalWordCount �ɿ�

        public int? RecommendCount { get; set; }           // RecommendCount �ɿ�

        public int? CollectedCount { get; set; }             // CollectedCount �ɿ�

        public string? Status { get; set; } = "�����";    // IS_RECOMMEND_VISIBLE "�����"/"����"/"���"

        //public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();  // ά������Chapter����
    }
}
