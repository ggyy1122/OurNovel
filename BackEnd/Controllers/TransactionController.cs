using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurNovel.Models;
using OurNovel.Services;

namespace OurNovel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : BaseController<Transaction, int>
    {
        private readonly TransactionService _transactionService;

        // 注入  TransactionService，而不是 BaseService
        public TransactionController(TransactionService transactionService) : base(transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("reward/{readerId}")]
        public async Task<IActionResult> GetRewardRecords(int readerId)
        {
            var result = await _transactionService.GetRewardRecordsAsync(readerId);
            return Ok(result);
        }

        [HttpGet("subscription/{readerId}")]
        public async Task<IActionResult> GetSubscriptionRecords(int readerId)
        {
            var result = await _transactionService.GetSubscriptionRecordsAsync(readerId);
            return Ok(result);
        }

        [HttpGet("recharge/{readerId}")]
        public async Task<IActionResult> GetRechargeRecords(int readerId)
        {
            var result = await _transactionService.GetRechargeRecordsAsync(readerId);
            return Ok(result);
        }

        [HttpGet("transaction/{readerId}")]
        public async Task<IActionResult> GetTransactionRecords(int readerId)
        {
            var result = await _transactionService.GetTransactionsByReaderAsync(readerId);
            return Ok(result);
        }





    }
}
