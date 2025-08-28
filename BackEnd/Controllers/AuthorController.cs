using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;
using OurNovel.Repositories;
using OurNovel.Services.Interfaces;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 作者控制器，继承基类控制器，实现对 Author 实体的 CRUD 操作，并支持头像上传
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : BaseController<Author, int>
    {
        private readonly IImageResourceService _imageResourceService;
        private readonly IRepository<Author, int> _authorRepository;
        private readonly AuthorService _authorService;

        public AuthorController(
            AuthorService authorService,
            IImageResourceService imageResourceService,
            IRepository<Author, int> authorRepository
        ) : base(authorService)
        {
            _imageResourceService = imageResourceService;
            _authorRepository = authorRepository;
            _authorService = authorService;
        }

        /// <summary>
        /// 上传作者头像，并更新头像URL
        /// </summary>
        /// <param name="authorId">作者ID</param>
        /// <param name="avatarFile">头像文件</param>
        /// <returns>头像URL</returns>
        [HttpPost("UploadAvatar")]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadAvatar([FromForm] int authorId, [FromForm] IFormFile avatarFile)
        {
            try
            {
                var avatarUrl = await _imageResourceService.UploadImageAsync<Author, int>(
                    authorId,
                    avatarFile,
                    "AvatarUrl",
                    _authorRepository);

                return Ok(new { success = true, avatarUrl });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // 可继续扩展作者特殊业务接口，如作品统计、收入查询等

        /// <summary>
        /// 小说统计
        /// </summary>
        [HttpGet("{authorId}/novel-count")]
        public async Task<IActionResult> GetNovelCount(int authorId)
        {
            var count = await _authorService.GetNovelCountByAuthorIdAsync(authorId);
            return Ok(new { novelCount = count });
        }

        /// <summary>
        /// 字数统计
        /// </summary>
        [HttpGet("{authorId}/total-wordcount")]
        public async Task<IActionResult> GetTotalWordCount(int authorId)
        {
            var total = await _authorService.GetTotalWordCountByAuthorIdAsync(authorId);
            return Ok(new { totalWordCount = total });
        }


        /// <summary>
        // 新增：获取注册天数
        /// </summary>
        [HttpGet("{authorId}/register-days")]
        public async Task<IActionResult> GetRegisterDays(int authorId)
        {
            var days = await _authorService.GetAuthorRegisterDaysAsync(authorId);
            if (days == null)
                return NotFound(new { success = false, message = "作者不存在" });

            return Ok(new { success = true, registerDays = days });
        }

        /// <summary>
        /// 根据作者 ID 获取该作者的所有小说
        /// </summary>
        [HttpGet("{authorId}/novels")]
        public async Task<IActionResult> GetAuthorNovels(int authorId)
        {
            var novels = await _authorService.GetNovelsByAuthorIdAsync(authorId);
            return Ok(novels);
        }

        /// <summary>
        /// 根据作者 ID 获取该作者的筛选为‘连载和完结’的小说
        /// </summary>
        [HttpGet("{authorId}/novel")]
        public async Task<IActionResult> GetAuthorNovelWithScreening(int authorId)
        {
            var novels = await _authorService.GetNovelsByAuthorIdWithScreeningAsync(authorId);
            return Ok(novels);
        }
    }
}
