using OurNovel.Models;
using OurNovel.Repositories;

namespace OurNovel.Services
{
    /// <summary>
    /// Manager 服务，继承基础服务，如有特殊业务再扩展
    /// </summary>
    public class ManagerService : BaseService<Manager, int>
    {
        public ManagerService(IRepository<Manager, int> repository) : base(repository)
        {
        }

        // 目前无特殊业务，空着即可
    }
}
