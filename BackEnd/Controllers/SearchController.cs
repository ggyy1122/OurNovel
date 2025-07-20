using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly NovelSearchService _novelSearchService;

    public SearchController(NovelSearchService novelSearchService)
    {
        _novelSearchService = novelSearchService;
    }

    /// <summary>
    /// 通过小说名模糊搜索小说
    /// </summary>
    /// <param name="keyword">搜索关键词</param>
    /// <returns>搜索结果</returns>
    [HttpGet("search")]
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
}
