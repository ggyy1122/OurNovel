using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;
using OurNovel.Services.Interfaces;
using OurNovel.Repositories;
using OurNovel.DTOs;

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
        /// ����С˵�����ֶΣ�
        /// </summary>
        [HttpPost("/api/novel/create")]
        public async Task<IActionResult> CreateNovel([FromQuery] int authorId, [FromQuery] string novelName,[FromQuery] string introduction)
        {
            try
            {
                var novelId = await (_service as NovelService)!.CreateNovelAsync(authorId, novelName, introduction);
                return Ok(new { success = true, message = "С˵�����ɹ���״̬Ϊ�����", novelId = novelId
                });
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { success = false, message });
            }
        }

        /// <summary>
        /// ���С˵
        /// </summary>
        [HttpPut("{id}/review")]
        public async Task<IActionResult> ReviewNovel(int id, [FromQuery] string newStatus, [FromQuery] int managerId, [FromQuery] string result)
        {
            // ������˲�д����־
            var success = await (_service as NovelService)?.ReviewNovelAsync(id, newStatus, managerId, result)!;

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

        /// <summary>
        /// �޸�С˵��Ϣ
        /// </summary>
        [HttpPost("submit-edit")]
        public async Task<IActionResult> SubmitEdit([FromQuery] int originalNovelId, [FromBody] NovelEditDto editedDto)
        {
            var original = await _novelService.GetByIdAsync(originalNovelId);
            if (original == null) return NotFound("ԭС˵������");

            var newNovelId = await _novelService.SubmitNovelEditAsync(originalNovelId, new Novel
            {
                NovelName = editedDto.NovelName,
                Introduction = editedDto.Introduction,
                CoverUrl = editedDto.CoverUrl,
                Status = editedDto.Status
            });

            if (newNovelId == -1)
                return BadRequest("�޸�ʧ��");

            return Ok(new { message = "�ύ�޸ĳɹ����ȴ����", novelId = newNovelId });
        }


        /// <summary>
        /// ����С˵������
        /// </summary>
        [HttpGet("wordcount/{novelId}")]
        public async Task<IActionResult> GetTotalWordCount(int novelId)
        {
            try
            {
                var wordCount = await _novelService.GetNovelPropertyAsync(novelId, n => n.TotalWordCount, 0L);

                return Ok(new { novelId, totalWords = wordCount });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }



        /// <summary>
        /// ����С˵�Ƽ���
        /// </summary>
        [HttpGet("recommendcount/{novelId}")]
        public async Task<IActionResult> GetRecommendCount(int novelId)
        {
            try
            {
                var recommendCount = await _novelService.GetNovelPropertyAsync(novelId, n => n.RecommendCount, 0);

                return Ok(new { novelId, RecommendCount = recommendCount });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// ����С˵�ղ���
        /// </summary>
        [HttpGet("collectcount/{novelId}")]
        public async Task<IActionResult> GetCollectCount(int novelId)
        {
            try
            {
                var collectCount = await _novelService.GetNovelPropertyAsync(novelId, n => n.CollectedCount, 0);

                return Ok(new { novelId, CollectCount = collectCount });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// ��ȡС˵�����ѷ����½ڵ�ID�ͷ���ʱ��
        /// </summary>
        [HttpGet("{novelId}/latest-published-chapter")]
        public async Task<IActionResult> GetLatestPublishedChapter(int novelId)
        {
            var latestChapter = await _novelService.GetLatestPublishedChapterInfoAsync(novelId);
            if (latestChapter == null)
                return NotFound("��С˵��û�з������½�");

            return Ok(latestChapter);
        }

        /// <summary>
        /// ��ȡ�����ѷ�����С˵��Status == "����" �� "���"��
        /// </summary>
        [HttpGet("published")]
        public async Task<IActionResult> GetPublishedNovels()
        {
            var novels = await _novelService.GetPublishedNovelsAsync();
            return Ok(novels);
        }

        /// <summary>
        /// ��ȡ�����ѷ�����С˵����ҳ����novelID˳��
        /// </summary>
        [HttpGet("/published/by-id")]
        public async Task<IActionResult> GetAllNovels([FromQuery] int page = 1, [FromQuery] int pageSize = 15)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("ҳ���ÿҳ����������ڵ���1");

            var novels = await _novelService.GetNovelsByPageAsync(page, pageSize);
            return Ok(novels);
        }

        /// <summary>
        /// ��ȡ�ѷ���С˵����ҳ + ����ɸѡ���� NovelId ˳��
        /// </summary>
        [HttpGet("/published/filter-by-id")]
        public async Task<ActionResult<PagedResult<Novel>>> GetPublishedNovels(
            int page = 1,
            int pageSize = 10,
            string? category = null,
            long? minWordCount = null,
            long? maxWordCount = null,
            bool? isFinished = null)
        {
            var result = await _novelService.GetNovelsAsync(
                page, pageSize, category, minWordCount, maxWordCount, isFinished);

            return Ok(result);
        }
    }
}
