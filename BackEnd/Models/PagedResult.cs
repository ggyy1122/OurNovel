namespace OurNovel.Models
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }        // �ܼ�¼��
        public int TotalPages { get; set; }        // ��ҳ��
        public int Page { get; set; }              // ��ǰҳ��
        public int PageSize { get; set; }          // ÿҳ����
        public IEnumerable<T> Items { get; set; } = new List<T>();  // ��ǰҳ����
    }
}
