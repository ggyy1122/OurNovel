using Microsoft.AspNetCore.Mvc;
using OurNovel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// 实现读者对小说评分
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RateController : ControllerBase
{
    private readonly IRateService _service;

    public RateController(IRateService service)
    {
        _service = service;
    }

    /// <summary>
    /// 添加评分（不可更新，已存在则报错）
    /// </summary>
    /// <param name="novelId">小说 ID</param>
    /// <param name="readerId">读者 ID</param>
    /// <param name="score">评分值（0 到 10 之间）</param>
    /// <returns>操作结果</returns>
    [HttpPost]
    public async Task<IActionResult> Add(int novelId, int readerId, int score)
    {
        await _service.AddAsync(novelId, readerId, score);
        return Ok("评分成功");
    }

    /// <summary>
    /// 删除评分记录
    /// </summary>
    /// <param name="novelId">小说 ID</param>
    /// <param name="readerId">读者 ID</param>
    /// <returns>操作结果</returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(int novelId, int readerId)
    {
        await _service.DeleteAsync(novelId, readerId);
        return Ok("删除评分成功");
    }

    /// <summary>
    /// 获取某本小说的全部评分记录
    /// </summary>
    /// <param name="novelId">小说 ID</param>
    /// <returns>该小说的评分列表</returns>
    [HttpGet("novel/{novelId}")]
    public async Task<ActionResult<IEnumerable<Rate>>> GetByNovel(int novelId)
    {
        var result = await _service.GetByNovelIdAsync(novelId);
        return Ok(result);
    }

    /// <summary>
    /// 获取某位读者对小说的评分记录
    /// </summary>
    /// <param name="readerId">读者 ID</param>
    /// <returns>该读者的评分记录</returns>
    [HttpGet("reader/{readerId}")]
    public async Task<ActionResult<IEnumerable<Rate>>> GetByReader(int readerId)
    {
        var result = await _service.GetByReaderIdAsync(readerId);
        return Ok(result);
    }

    /// <summary>
    /// 获取全部评分记录
    /// </summary>
    /// <returns>所有评分数据</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rate>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }
}
