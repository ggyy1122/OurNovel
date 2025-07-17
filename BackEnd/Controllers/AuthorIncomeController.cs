using Microsoft.AspNetCore.Mvc;
using OurNovel.Services;
using OurNovel.Models;

namespace OurNovel.Controllers
{
    /// <summary>
    /// ����������������ṩ�����¼��ѯ�ӿ�
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
        /// ��ȡ���ߵ����������¼
        /// </summary>
        [HttpGet("list/{authorId}")]
        public async Task<ActionResult<IEnumerable<AuthorIncome>>> GetIncomesByAuthor(long authorId)
        {
            var result = await _incomeService.GetByAuthorIdAsync(authorId);
            return Ok(result);
        }

        /// <summary>
        /// ��ȡ���ߵ���������
        /// </summary>
        [HttpGet("total/{authorId}")]
        public async Task<ActionResult<decimal>> GetTotalIncome(long authorId)
        {
            var total = await _incomeService.GetTotalIncomeAsync(authorId);
            return Ok(new { authorId, totalIncome = total });
        }
    }
}
