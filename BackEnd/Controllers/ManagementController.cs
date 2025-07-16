using Microsoft.AspNetCore.Mvc;
using OurNovel.Models;
using OurNovel.Services;

namespace OurNovel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagementController : BaseController<Management, int>
    {
        private readonly ManagementService _service;

        public ManagementController(ManagementService service) : base(service)
        {
            _service = service;
        }

        /// <summary>
        /// ���ݹ���ԱIDɸѡ�����¼
        /// </summary>
        [HttpGet("by-manager/{managerId}")]
        public async Task<IActionResult> GetByManagerId(int managerId)
        {
            var results = await _service.GetByManagerIdAsync(managerId);
            return Ok(results);
        }
    }
}
