namespace OurNovel.Models
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }        // 总记录数
        public int TotalPages { get; set; }        // 总页数
        public int Page { get; set; }              // 当前页码
        public int PageSize { get; set; }          // 每页条数
        public IEnumerable<T> Items { get; set; } = new List<T>();  // 当前页数据
    }
}
