using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 小说控制器，继承基类控制器，实现对 Novel 实体的 CRUD 操作
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NovelController : BaseController<Novel, int>
    {
        private readonly NovelService _novelService;

        // 注入 NovelService，而不是 BaseService
        public NovelController(NovelService novelService) : base(novelService)
        {
            _novelService = novelService;
        }

        // 如果 Novel 有特殊的业务接口，可以在这里扩展

        /// <summary>
        /// 上传小说封面，并更新封面URL
        /// </summary>
        /// <param name="novelId">小说ID</param>
        /// <param name="coverFile">封面文件</param>
        /// <returns>封面URL</returns>
        [HttpPost("UploadCover")]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadCover([FromForm] int novelId, [FromForm] IFormFile coverFile)
        {
            try
            {
                var coverUrl = await _novelService.UploadCoverAsync(novelId, coverFile);
                return Ok(new { success = true, coverUrl });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
