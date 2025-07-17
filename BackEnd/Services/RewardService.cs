using OurNovel.Models;
using OurNovel.Repositories;
using OurNovel.DTOs;
using Microsoft.EntityFrameworkCore;
using OurNovel.Data;

namespace OurNovel.Services
{
    /// <summary>
    /// 打赏服务，封装打赏业务逻辑
    /// </summary>
    public class RewardService
    {
        private readonly AppDbContext _context;

        public RewardService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 用户执行打赏行为：创建交易，扣余额，写入打赏表和作者收入表
        /// </summary>
        public async Task<bool> RewardAsync(RewardRequestDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //一开始统一类型转换
                int readerId = (int)dto.ReaderId;
                int novelId = (int)dto.NovelId;
                int amount = (int)dto.Amount;

                // 1. 查询读者并验证余额
                var reader = await _context.Readers.FindAsync(readerId);
                if (reader == null || reader.Balance < amount)
                    return false;

                // 2. 插入交易记录
                var trans = new Transaction
                {
                    ReaderId = readerId,
                    TransType = "打赏",
                    Amount = amount,
                    Time = DateTime.Now
                };
                _context.Transactions.Add(trans);
                await _context.SaveChangesAsync(); // 拿到 TransactionId

                // 3. 扣除余额
                reader.Balance -= amount;
                _context.Readers.Update(reader);

                // 4. 插入 Reward 表记录
                _context.Rewards.Add(new Reward
                {
                    TransactionId = trans.TransactionId,
                    NovelId = novelId
                });

                // 5. 查找小说并写入作者收入记录
                var novel = await _context.Novels.FindAsync(novelId);
                if (novel == null)
                    throw new Exception("小说不存在"); 

                _context.AuthorIncomes.Add(new AuthorIncome
                {
                    AuthorId = novel.AuthorId,
                    Amount = dto.Amount, // 注意：这里保留 decimal 类型，Income.Amount 可为 decimal
                    Type = "打赏",
                    CreateTime = DateTime.Now
                });

                // 查找作者
                var author = await _context.Authors.FindAsync(novel.AuthorId);
                if (author == null)
                    throw new Exception("作者不存在");

                // 累加作者的收入
                author.Earning += dto.Amount;
                _context.Authors.Update(author);


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
