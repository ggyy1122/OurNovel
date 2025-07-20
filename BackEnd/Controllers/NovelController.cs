using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;
using OurNovel.Services.Interfaces;
using OurNovel.Repositories;

namespace OurNovel.Controllers
{
    /// <summary>
    /// С˵���������̳л����������ʵ�ֶ� Novel ʵ��� CRUD ����
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NovelController : BaseController<Novel, int>
    {
        private readonly NovelService _novelService;
        private readonly IImageResourceService _imageResourceService;
        private readonly IRepository<Novel, int> _novelRepository;

        // ע�����
        public NovelController(NovelService novelService, IImageResourceService imageResourceService,
            IRepository<Novel, int> novelRepository) : base(novelService)
        {
            _novelService = novelService;
            _imageResourceService = imageResourceService;
            _novelRepository = novelRepository;
        }

        // ��� Novel �������ҵ��ӿڣ�������������չ

        /// <summary>
        /// ���С˵
        /// </summary>
        [HttpPut("{id}/review")]
        public async Task<IActionResult> ReviewNovel(int id, [FromQuery] string newStatus, [FromQuery] int managerId)
        {
            // ������˲�д����־
            var success = await (_service as NovelService)?.ReviewNovelAsync(id, newStatus, managerId)!;

            if (!success)
                return BadRequest("���ʧ�ܣ�������ID�����ڻ�״̬�Ƿ�������Ϊ�����ء�����ᡯ��");

            return Ok(new { success = true, message = "С˵״̬�Ѹ���" });
        }


        /// <summary>
        /// �ϴ�С˵���棬�����·���URL
        /// </summary>
        /// <param name="novelId">С˵ID</param>
        /// <param name="coverFile">�����ļ�</param>
        /// <returns>����URL</returns>
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



    }
}
