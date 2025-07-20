using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly NovelSearchService _novelSearchService;
    private readonly ReaderSearchService _readerSearchService;
    private readonly AuthorSearchService _authorSearchService;

    public SearchController(
        NovelSearchService novelSearchService,
        ReaderSearchService readerSearchService,
        AuthorSearchService authorSearchService)
    {
        _novelSearchService = novelSearchService;
        _readerSearchService = readerSearchService;
        _authorSearchService = authorSearchService;
    }


    /// <summary>
    /// 通过小说名模糊搜索小说
    /// </summary>
    /// <param name="keyword">搜索关键词</param>
    /// <returns>搜索结果</returns>
    [HttpGet("novel")]
    public async Task<IActionResult> SearchNovels([FromQuery] string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return BadRequest("搜索关键词不能为空");
        }

        var novels = await _novelSearchService.SearchNovelsAsync(keyword);
        if (novels == null || novels.Count == 0)
        {
            return NotFound("未找到匹配的小说");
        }

        return Ok(novels);
    }
    /// <summary>
    /// 通过作者名模糊搜索作者
    /// </summary>
    [HttpGet("author")]
    public async Task<IActionResult> SearchAuthors([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest("搜索关键词不能为空");

        var authors = await _authorSearchService.SearchAuthorsAsync(keyword);
        if (authors == null || authors.Count == 0)
            return NotFound("未找到匹配的作者");

        return Ok(authors);
    }

    /// <summary>
    /// 通过读者名模糊搜索读者
    /// </summary>
    [HttpGet("reader")]
    public async Task<IActionResult> SearchReaders([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest("搜索关键词不能为空");

        var readers = await _readerSearchService.SearchReadersAsync(keyword);
        if (readers == null || readers.Count == 0)
            return NotFound("未找到匹配的读者");

        return Ok(readers);
    }
}
