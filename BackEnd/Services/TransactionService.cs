using OurNovel.Repositories;
using Microsoft.AspNetCore.Http;
using OurNovel.Services.Interfaces;
using OurNovel.Models;
namespace OurNovel.Services
{
    public class TransactionService : BaseService<Transaction, int>
    {
        public TransactionService(IRepository<Transaction, int> repository)
          : base(repository)
        {
        }
    }
}
