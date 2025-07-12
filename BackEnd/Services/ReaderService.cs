using OurNovel.Models;
using OurNovel.Repositories;

namespace OurNovel.Services
{
    /// <summary>
    /// Reader 服务，继承基础服务，如有特殊业务再扩展
    /// </summary>
    public class ReaderService : BaseService<Reader,int>
    {
        public ReaderService(IRepository<Reader,int> repository) : base(repository)
        {
        }

        // ⚠️ 如果有特殊业务方法，可以在这里单独写
    }
}
