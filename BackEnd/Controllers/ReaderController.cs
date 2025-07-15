using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;
using OurNovel.Repositories;
using OurNovel.Services.Interfaces;
namespace OurNovel.Controllers
{
    /// <summary>
    /// 读者控制器，继承基类控制器，实现对 Reader 实体的 CRUD 操作
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderController : BaseController<Reader, int>
    {
        private readonly IImageResourceService _imageResourceService;
        private readonly IRepository<Reader, int> _readerRepository;
        private readonly ReaderService _readerService;

        // 这里注入 ReaderService，而不是 BaseService
        public ReaderController(ReaderService readerService , IImageResourceService imageResourceService,
            IRepository<Reader, int> readerRepository) : base(readerService)
        {
            _imageResourceService = imageResourceService;
            _readerRepository = readerRepository;
            _readerService = readerService;
        }

        // 如果 Reader 有特殊的业务接口，可以在这里扩展

        /// <summary>
        /// 上传读者头像，并更新头像URL
        /// </summary>
        /// <param name="readerId">读者ID</param>
        /// <param name="avatarFile">头像文件</param>
        /// <returns>头像URL</returns>
        [HttpPost("UploadAvatar")]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadAvatar([FromForm] int readerId, [FromForm] IFormFile avatarFile)
        {
            try
            {
                var avatarUrl = await _imageResourceService.UploadImageAsync<Reader, int>(
                  readerId,
                  avatarFile,
                  "AvatarUrl",
                  _readerRepository);
                return Ok(new { success = true, avatarUrl });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}