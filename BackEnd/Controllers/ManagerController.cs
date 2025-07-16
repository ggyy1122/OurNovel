using Microsoft.AspNetCore.Mvc;
using OurNovel.Models;
using OurNovel.Services;

namespace OurNovel.Controllers
{
    /// <summary>
    /// Manager 控制器，继承通用基类控制器，提供基本的 CRUD 接口
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : BaseController<Manager, int>
    {
        public ManagerController(ManagerService service) : base(service)
        {
        }

        // 如有特殊接口，可在此扩展
    }
}
