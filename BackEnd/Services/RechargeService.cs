using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.DTOs;
using OurNovel.Models;

namespace OurNovel.Services
{
    /// <summary>
    /// 充值服务，封装充值业务逻辑
    /// </summary>
    public class RechargeService
    {
        private readonly AppDbContext _context;

        public RechargeService(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 用户执行充值行为：创建交易，更新余额
        /// </summary>
        public async Task<bool> RechargeAsync(RechargeRequestDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //类型转换
                int readerId = (int)dto.ReaderId;
                int amount = (int)dto.Amount;
                // 1. 获取读者
                var reader = await _context.Readers.FindAsync(readerId);
                if (reader == null) return false;

                // 2. 记录交易
                _context.Transactions.Add(new Transaction
                {
                    ReaderId = readerId,
                    TransType = "充值",
                    Amount = amount , 
                    Time = DateTime.Now
                });

                // 3. 更新余额
                reader.Balance += amount;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }



    }
}
