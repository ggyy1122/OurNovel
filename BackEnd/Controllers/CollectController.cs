using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using OurNovel.Models;

/// <summary>
/// 实现读者收藏小说
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CollectController : ControllerBase
{
    private readonly ICollectService _service;

    public CollectController(ICollectService service)
    {
        _service = service;
    }

    /// <summary>
    /// 添加或更新收藏记录（更新 IsPublic ）
    /// </summary>
    /// <param name="novelId">小说 ID</param>
    /// <param name="readerId">读者 ID</param>
    /// <param name="isPublic">是否公开（"yes" 或 "no"）</param>
    /// <returns>操作成功提示</returns>
    [HttpPost]
    public async Task<IActionResult> AddOrUpdate(int novelId, int readerId, string? isPublic)
    {
        await _service.AddOrUpdateAsync(novelId, readerId, isPublic);
        return Ok("收藏记录已添加或更新");
    }


    /// <summary>
    /// 取消收藏（删除一条收藏记录）
    /// </summary>
    /// <param name="novelId">小说 ID</param>
    /// <param name="readerId">读者 ID</param>
    /// <returns>操作成功提示</returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(int novelId, int readerId)
    {
        await _service.DeleteAsync(novelId, readerId);
        return Ok("取消收藏成功");
    }

    /// <summary>
    /// 获取某个读者收藏的所有小说记录
    /// </summary>
    /// <param name="readerId">读者 ID</param>
    /// <returns>该读者收藏的 Collect 列表</returns>
    [HttpGet("reader/{readerId}")]
    public async Task<ActionResult<IEnumerable<Collect>>> GetByReader(int readerId)
    {
        var collects = await _service.GetByReaderIdAsync(readerId);
        return Ok(collects);
    }

    /// <summary>
    /// 获取某部小说被哪些读者收藏
    /// </summary>
    /// <param name="novelId">小说 ID</param>
    /// <returns>收藏该小说的 Collect 列表</returns>
    [HttpGet("novel/{novelId}")]
    public async Task<ActionResult<IEnumerable<Collect>>> GetByNovel(int novelId)
    {
        var collects = await _service.GetByNovelIdAsync(novelId);
        return Ok(collects);
    }

    /// <summary>
    /// 获取所有收藏记录
    /// </summary>
    /// <returns>所有 Collect 实体列表</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Collect>>> GetAll()
    {
        var collects = await _service.GetAllAsync();
        return Ok(collects);
    }
}
