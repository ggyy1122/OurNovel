using Microsoft.AspNetCore.Mvc;
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
                AvatarUrl = null
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return new OkObjectResult(author);
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
    }
}
