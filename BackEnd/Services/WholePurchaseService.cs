using Microsoft.EntityFrameworkCore;
using OurNovel.Models;
using OurNovel.DTOs;
using OurNovel.Data;

namespace OurNovel.Services
{
    public class WholePurchaseService
    {
        private readonly AppDbContext _context;

        public WholePurchaseService(AppDbContext context)
        {
            _context = context;
        }
        
        public bool HasPurchased(int readerId, int novelId)
       {
         var record = _context.WholePurchase
        .FirstOrDefault(p => p.ReaderId == readerId && p.NovelId == novelId);

         return record != null && record.IsBought == "是";
        }


        /// <summary>
        /// 整本小说买断方法
        /// </summary>
        public async Task<PurchaseResultDto> PurchaseWholeNovelAsync(WholePurchaseDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var result = new PurchaseResultDto();

            try
            {
                var reader = await _context.Readers.FindAsync(dto.ReaderId);
                if (reader == null)
                    return new PurchaseResultDto { Success = 0, Message = "读者不存在" };

                var novel = await _context.Novels.FindAsync(dto.NovelId);
                if (novel == null)
                    return new PurchaseResultDto { Success = 0, Message = "小说不存在" };

                if (novel.TotalPrice == null || novel.TotalPrice <= 0)
                    return new PurchaseResultDto { Success = 0, Message = "该小说无需买断" };

                // 是否已买断
                var alreadyBought = await _context.WholePurchases
                 .Where(wp => wp.ReaderId == dto.ReaderId && wp.NovelId == dto.NovelId)
                 .Select(wp => wp.IsBought)
                 .FirstOrDefaultAsync();

                if (alreadyBought == "是")
                    return new PurchaseResultDto { Success = 0, Message = "您已买断整本小说" };


                decimal finalPrice = novel.TotalPrice.Value;

                if (reader.Balance < finalPrice)
                    return new PurchaseResultDto { Success = 0, Message = "余额不足" };



                // 扣除余额
                reader.Balance -= finalPrice;
                _context.Readers.Update(reader);





                // 写入 WholePurchase 表
                _context.WholePurchases.Add(new WholePurchase
                {
                    ReaderId = dto.ReaderId,
                    NovelId = dto.NovelId,
                    IsBought = "是"
                });

                // 处理作者收入
                var author = await _context.Authors.FindAsync(novel.AuthorId);
                if (author == null)
                    throw new Exception("作者不存在");

                _context.AuthorIncomes.Add(new AuthorIncome
                {
                    AuthorId = author.AuthorId,
                    Amount = finalPrice,
                    Type = "整本买断",
                    CreateTime = DateTime.Now
                });

                author.Earning += finalPrice;
                _context.Authors.Update(author);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                result.Success = 1;
                result.Message = "整本购买成功";
                return result;
            }
            catch (Exception ex)
            {
                // 记录详细的异常信息
                Console.WriteLine($"详细异常信息：{ex.ToString()}");


                await transaction.RollbackAsync();
                return new PurchaseResultDto { Success = 0, Message = "服务器异常：" + ex.Message };
            }

        }
    }
}
