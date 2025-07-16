using Microsoft.AspNetCore.Mvc;
using OurNovel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// 实现读者推荐小说（附加推荐理由）
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RecommendController : ControllerBase
{
    private readonly IRecommendService _service;

    public RecommendController(IRecommendService service)
    {
        _service = service;
    }

    /// <summary>
    /// 推荐小说（新增一条推荐记录）
    /// </summary>
    /// <param name="novelId">小说 ID</param>
    /// <param name="readerId">读者 ID</param>
    /// <param name="reason">推荐理由（可选）</param>
    /// <returns>操作成功提示</returns>
    [HttpPost]
    public async Task<IActionResult> Add(int novelId, int readerId, string? reason)
    {
        await _service.AddAsync(novelId, readerId, reason);
        return Ok("推荐成功");
    }

    /// <summary>
    /// 取消推荐（删除一条推荐记录）
    /// </summary>
    /// <param name="novelId">小说 ID</param>
    /// <param name="readerId">读者 ID</param>
    /// <returns>操作成功提示</returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(int novelId, int readerId)
    {
        await _service.DeleteAsync(novelId, readerId);
        return Ok("取消推荐成功");
    }

    /// <summary>
    /// 获取某个读者推荐的所有小说
    /// </summary>
    /// <param name="readerId">读者 ID</param>
    /// <returns>该读者推荐记录列表</returns>
    [HttpGet("reader/{readerId}")]
    public async Task<ActionResult<IEnumerable<Recommend>>> GetByReader(int readerId)
    {
        var recommends = await _service.GetByReaderIdAsync(readerId);
        return Ok(recommends);
    }

    /// <summary>
    /// 获取某部小说被哪些读者推荐
    /// </summary>
    /// <param name="novelId">小说 ID</param>
    /// <returns>推荐该小说的 Recommend 列表</returns>
    [HttpGet("novel/{novelId}")]
    public async Task<ActionResult<IEnumerable<Recommend>>> GetByNovel(int novelId)
    {
        var recommends = await _service.GetByNovelIdAsync(novelId);
        return Ok(recommends);
    }

    /// <summary>
    /// 获取所有推荐记录
    /// </summary>
    /// <returns>所有 Recommend 实体列表</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recommend>>> GetAll()
    {
        var recommends = await _service.GetAllAsync();
        return Ok(recommends);
    }
}