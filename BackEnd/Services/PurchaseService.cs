using Microsoft.EntityFrameworkCore;
using OurNovel.Models;
using OurNovel.DTOs;
using OurNovel.Data;

namespace OurNovel.Services
{
    public class PurchaseService
    {
        private readonly AppDbContext _context;

        public PurchaseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseResultDto> PurchaseChapterAsync(ChapterPurchaseDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var result = new PurchaseResultDto();

            try
            {
                var reader = await _context.Readers.FindAsync(dto.ReaderId);
                if (reader == null)
                    return new PurchaseResultDto { Success = 0, Message = "读者不存在" };

                var chapter = await _context.Chapters
                    .FirstOrDefaultAsync(c => c.NovelId == dto.NovelId && c.ChapterId == dto.ChapterId);
                if (chapter == null)
                    return new PurchaseResultDto { Success = 0, Message = "章节不存在" };

                if (!string.Equals(chapter.IsCharged?.Trim(), "是", StringComparison.OrdinalIgnoreCase))
                    return new PurchaseResultDto { Success = 0, Message = "该章节无需购买" };

                /*

                // 判断是否已购买
                bool alreadyPurchased = await (
                    from p in _context.Purchases
                    join t in _context.Transactions
                        on p.TransactionId equals t.TransactionId
                    where p.NovelId == dto.NovelId
                        && p.ChapterId == dto.ChapterId
                        && t.ReaderId == dto.ReaderId
                    select p
                ).AnyAsync();

                if (alreadyPurchased)
                    return new PurchaseResultDto { Success = 0, Message = "您已购买过该章节" };
                */
                var purchases = await (
                    from p in _context.Purchases
                    join t in _context.Transactions
                         on p.TransactionId equals t.TransactionId
                    where p.NovelId == dto.NovelId
                             && p.ChapterId == dto.ChapterId
                             && t.ReaderId == dto.ReaderId
                    select p
                 ).ToListAsync();

                bool alreadyPurchased = purchases.Any();

                // 计算价格
                decimal finalPrice = chapter.CalculatedPrice;



                if (reader.Balance < finalPrice)
                    return new PurchaseResultDto { Success = 0, Message = "余额不足" };

                // 添加交易记录
                var trans = new Transaction
                {
                    ReaderId = dto.ReaderId,
                    TransType = "解锁章节",
                    Amount = finalPrice,
                    Time = DateTime.Now
                };
                _context.Transactions.Add(trans);
                await _context.SaveChangesAsync();

                // 扣余额
                reader.Balance -= finalPrice;
                _context.Readers.Update(reader);

                // 写入购买记录
                _context.Purchases.Add(new Purchase
                {
                    TransactionId = trans.TransactionId,
                    NovelId = dto.NovelId,
                    ChapterId = dto.ChapterId
                });

                // 作者相关
                var novel = await _context.Novels.FindAsync(dto.NovelId);
                if (novel == null)
                    throw new Exception("小说不存在");

                var author = await _context.Authors.FindAsync(novel.AuthorId);
                if (author == null)
                    throw new Exception("作者不存在");

                _context.AuthorIncomes.Add(new AuthorIncome
                {
                    AuthorId = author.AuthorId,
                    NovelId = dto.NovelId,
                    Amount = finalPrice,
                    Type = "章节购买",
                    CreateTime = DateTime.Now
                });

                author.Earning += finalPrice;
                _context.Authors.Update(author);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                result.Success = 1;
                result.Message = "购买成功";
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new PurchaseResultDto { Success = 0, Message = "服务器异常：" + ex.Message };
            }
        }
    }
}
