using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 作者控制器，继承基类控制器，实现对 Author 实体的 CRUD 操作
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : BaseController<Author, int>
    {
        public AuthorController(BaseService<Author, int> service) : base(service)
        {
        }

        // 如果 Author 有特殊的业务接口，可以在这里扩展
    }
}
