using OurNovel.Repositories;
using Microsoft.AspNetCore.Http;
using OurNovel.Services.Interfaces;
using OurNovel.Models;
using OurNovel.Data;
using Microsoft.EntityFrameworkCore;

namespace OurNovel.Services
{
    public class TransactionService : BaseService<Transaction, int>
    {
      private readonly AppDbContext _context;
        public TransactionService(IRepository<Transaction, int> repository, AppDbContext context)
          : base(repository)
        {
            _context = context;
        }
        public async Task<List<ReaderRewardRecord>> GetRewardRecordsAsync(int readerId)
        {
            return await _context.ReaderRewardRecords
                .Where(v => v.ReaderId == readerId)
                .ToListAsync();
        }

        public async Task<List<ReaderSubscriptionRecord>> GetSubscriptionRecordsAsync(int readerId)
        {
            return await _context.ReaderSubscriptionRecords
                .Where(v => v.ReaderId == readerId)
                .ToListAsync();
        }

        public async Task<List<ReaderRechargeRecord>> GetRechargeRecordsAsync(int readerId)
        {
            return await _context.ReaderRechargeRecords
                .Where(v => v.ReaderId == readerId)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetTransactionsByReaderAsync(int readerId)
        {
            return await _context.Transactions
                .Where(t => t.ReaderId == readerId)
                .OrderByDescending(t => t.Time) // 默认按时间倒序
                .ToListAsync();
        }


    }


}
