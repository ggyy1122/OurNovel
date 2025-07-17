using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 作者收入控制器，提供收入记录查询接口
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorIncomeController : ControllerBase
    {
        private readonly AuthorIncomeService _incomeService;

        public AuthorIncomeController(AuthorIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        /// <summary>
        /// 获取作者的所有收入记录
        /// </summary>
        [HttpGet("list/{authorId}")]
        public async Task<ActionResult<IEnumerable<AuthorIncome>>> GetIncomesByAuthor(long authorId)
        {
            var result = await _incomeService.GetByAuthorIdAsync(authorId);
            return Ok(result);
        }

        /// <summary>
        /// 获取作者的总收入金额
        /// </summary>
        [HttpGet("total/{authorId}")]
        public async Task<ActionResult<decimal>> GetTotalIncome(long authorId)
        {
            var total = await _incomeService.GetTotalIncomeAsync(authorId);
            return Ok(new { authorId, totalIncome = total });
        }
    }
}
