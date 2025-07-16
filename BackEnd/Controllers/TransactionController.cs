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
    }
}
