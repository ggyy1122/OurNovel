using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;
using OurNovel.Services.Interfaces;
using OurNovel.Repositories;

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
        private readonly IImageResourceService _imageResourceService;
        private readonly IRepository<Novel, int> _novelRepository;

        // 注入服务
        public NovelController(NovelService novelService, IImageResourceService imageResourceService,
            IRepository<Novel, int> novelRepository) : base(novelService)
        {
            _novelService = novelService;
            _imageResourceService = imageResourceService;
            _novelRepository = novelRepository;
        }

        // 如果 Novel 有特殊的业务接口，可以在这里扩展

        /// <summary>
        /// 审核小说
        /// </summary>
        [HttpPut("{id}/review")]
        public async Task<IActionResult> ReviewNovel(int id, [FromQuery] string newStatus, [FromQuery] int managerId, [FromQuery] string result)
        {
            // 调用审核并写入日志
            var success = await (_service as NovelService)?.ReviewNovelAsync(id, newStatus, managerId, result)!;

            if (!success)
                return BadRequest("审核失败，可能是ID不存在或状态非法（必须为‘连载’或‘完结’）");

            return Ok(new { success = true, message = "小说状态已更新" });
        }


        /// <summary>
        /// 上传小说封面，并更新封面URL
        /// </summary>
        /// <param name="novelId">小说ID</param>
        /// <param name="coverFile">封面文件</param>
        /// <returns>封面URL</returns>
        [HttpPost("UploadAvatar")]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadAvatar([FromForm] int novelId, [FromForm] IFormFile coverFile)
        {
            try
            {
                var coverUrl = await _imageResourceService.UploadImageAsync<Novel, int>(
                  novelId,
                  coverFile,
                  "CoverUrl",
                  _novelRepository);
                return Ok(new { success = true, coverUrl });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        /// <summary>
        /// 修改小说信息
        /// </summary>
        [HttpPost("submit-edit")]
        public async Task<IActionResult> SubmitEdit(int originalNovelId, [FromBody] Novel edited)
        {
            var novelId = await _novelService.SubmitNovelEditAsync(originalNovelId, edited);
            if (novelId == -1) return NotFound("原小说不存在");
            return Ok(new { message = "提交修改成功，等待审核", newNovelId = novelId });
        }

    }
}
