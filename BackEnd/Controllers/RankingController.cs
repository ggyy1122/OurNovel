using Microsoft.AspNetCore.Mvc;
using OurNovel.Models.Dto;
using OurNovel.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 排行榜单
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RankingController : ControllerBase
    {
        private readonly NovelService _novelService;

        public RankingController(NovelService novelService)
        {
            _novelService = novelService;
        }

        /// <summary>
        /// 获取收藏榜单前 n 名
        /// </summary>
        /// <param name="topN">前几名</param>
        /// <param name="status">小说状态筛选（可选）："连载"、"完结"</param>
        /// <returns>收藏榜单</returns>
        [HttpGet("collect")]
        public async Task<ActionResult<List<CollectRankingDto>>> GetTopCollectedAsync(int topN = 10, string? status = null)
        {
            var result = await _novelService.GetTopCollectedNovelsAsync(topN, status);
            return Ok(result);
        }

        /// <summary>
        /// 获取推荐榜单前 n 名
        /// </summary>
        /// <param name="top">前几名</param>
        /// <param name="status">小说状态筛选（可选）："连载"、"完结"</param>
        /// <returns>推荐榜单</returns>
        [HttpGet("recommend")]
        public async Task<ActionResult<List<RecommendRankingDto>>> GetRecommendRanking([FromQuery] int top = 10, string? status = null)
        {
            var result = await _novelService.GetTopRecommendedNovelsAsync(top, status);
            return Ok(result);
        }

        /// <summary>
        /// 获取评分榜单前 n 名
        /// </summary>
        /// <param name="top">前几名</param>
        /// <param name="status">小说状态筛选（可选）："连载"、"完结"</param>
        /// <returns>评分榜单</returns>
        [HttpGet("score")]
        public async Task<ActionResult<List<ScoreRankingDto>>> GetScoreRanking([FromQuery] int top = 10, string? status = null)
        {
            var result = await _novelService.GetTopScoredNovelsAsync(top, status);
            return Ok(result);
        }

    }
}
