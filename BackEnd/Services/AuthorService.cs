using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;
using OurNovel.Repositories;

namespace OurNovel.Services
{
    /// <summary>
    /// Author 服务，继承基础服务，并扩展注册逻辑
    /// </summary>
    public class AuthorService : BaseService<Author, int>
    {
        private readonly AppDbContext _context;

        public AuthorService(IRepository<Author, int> repository, AppDbContext context)
            : base(repository)
        {
            _context = context;
        }

        /// <summary>
        /// 注册作者：AuthorName 唯一，密码将加密存储
        /// </summary>
        public async Task<IActionResult> RegisterAsync(string authorName, string password)
        {

            var authors = _context.Authors
                .Where(a => a.AuthorName == authorName)
                .ToList();

            if (authors.Count > 0)
            {
                return new BadRequestObjectResult("用户名已存在");
            }
            /*
            if (_context.Authors.Any(a => a.AuthorName == authorName))
            {
                return new BadRequestObjectResult("用户名已存在");
            }
            */
            // 生成并加密密码，BCrypt 会自动处理盐
            var hashedPassword = PasswordHasher.HashPassword(password);

            var author = new Author
            {
                AuthorName = authorName,
                Password = hashedPassword,  // 存储加密后的密码
                Earning = 0,
                Phone = null,
                AvatarUrl = null,
                RegisterTime = DateTime.Now
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return new OkObjectResult(author);
        }

        /// <summary>
        /// 重置密码（按用户名）
        /// </summary>
        public async Task<IActionResult> ResetPasswordAsync(string authorName, string newPassword)
        {
            var author = _context.Authors.FirstOrDefault(a => a.AuthorName == authorName);
            if (author == null)
            {
                return new NotFoundObjectResult("用户不存在");
            }

            author.Password = PasswordHasher.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return new OkObjectResult("密码重置成功");
        }

        public async Task<IActionResult> ChangePasswordAsync(string authorName, string oldPassword, string newPassword)
        {
            var author = await _context.Authors
                .FirstOrDefaultAsync(a => a.AuthorName == authorName);

            if (author == null)
                return new NotFoundObjectResult("作者不存在");

            // 验证旧密码（哈希比对）
            if (!PasswordHasher.VerifyPassword(oldPassword, author.Password))
                return new BadRequestObjectResult("原密码错误");

            // 使用哈希函数加密新密码
            author.Password = PasswordHasher.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return new OkObjectResult("密码修改成功");
        }



        /// <summary>
        /// 获取所有作者（用于登录查找）
        /// </summary>
        public IEnumerable<Author> GetAllAuthors()
        {
            return _context.Authors;
        }

        /// <summary>
        /// 按用户名查找作者（可选辅助方法）
        /// </summary>
        public Author? GetByAuthorName(string authorName)
        {
            return _context.Authors.FirstOrDefault(a => a.AuthorName == authorName);
        }

        /// <summary>
        /// 按作者id统计其小说总数
        /// </summary>
        public async Task<int> GetNovelCountByAuthorIdAsync(int authorId)
        {
            return await _context.Novels
                .CountAsync(n => n.AuthorId == authorId);
        }


        /// <summary>
        /// 按作者id统计其字数总数
        /// </summary>
        public async Task<long> GetTotalWordCountByAuthorIdAsync(int authorId)
        {
            return await _context.Novels
                .Where(n => n.AuthorId == authorId)
                .SumAsync(n => (long?)n.TotalWordCount) ?? 0;
        }
        /// <summary>
        /// 计算作者注册总天数
        /// </summary>
        public async Task<int?> GetAuthorRegisterDaysAsync(int authorId)
        {
            var author = await _context.Authors.FindAsync(authorId);
            if (author == null) return null;

            // 默认将 null 当作今天注册
            DateTime regTime = author.RegisterTime ?? DateTime.Now;
           int days = (int)((DateTime.Now.Date - regTime.Date).TotalDays);

            return days;
        }

        /// <summary>
        /// 根据作者 ID 获取该作者的所有小说
        /// </summary>
        public async Task<List<Novel>> GetNovelsByAuthorIdAsync(int authorId)
        {
            return await _context.Novels
                .Where(n => n.AuthorId == authorId)
                .OrderBy(n => n.NovelId)
                .ToListAsync();
        }
    }
}
