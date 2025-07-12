using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 读者控制器，继承基类控制器，实现对 Reader 实体的 CRUD 操作
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderController : BaseController<Reader, int>
    {
        public ReaderController(BaseService<Reader, int> service) : base(service)
        {
        }

        // 如果 Reader 有特殊的业务接口，可以在这里扩展
    }
}
