using OurNovel.Repositories;
using Microsoft.AspNetCore.Http;
using OurNovel.Services.Interfaces;
using OurNovel.Models;

namespace OurNovel.Services
{
    /// <summary>
    /// Reader 服务，继承基础服务，如有特殊业务再扩展
    /// </summary>
    public class ReaderService : BaseService<Reader, int>
    {
        private readonly IOssService _ossService;

        public ReaderService(IRepository<Reader, int> repository)
            : base(repository)
        {
        }
    }
}