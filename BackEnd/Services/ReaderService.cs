using Microsoft.AspNetCore.Mvc;
using OurNovel.Data;
using OurNovel.Models;
using OurNovel.Repositories;

namespace OurNovel.Services
{
    public class ReaderService : BaseService<Reader, int>
    {
        private readonly AppDbContext _context;

        public ReaderService(IRepository<Reader, int> repository, AppDbContext context)
            : base(repository)
        {
            _context = context;
        }

        public async Task<IActionResult> RegisterAsync(string readerName, string password)
        {
            if (_context.Readers.Count(r => r.ReaderName == readerName) > 0)
            {
                return new BadRequestObjectResult("用户名已存在");
            }
            
            
         
            var hashedPassword = PasswordHasher.HashPassword(password);

            var reader = new Reader
            {
                ReaderName = readerName,
                Password = hashedPassword,
                Balance = 0
            };

            _context.Readers.Add(reader);
            await _context.SaveChangesAsync();

            return new OkObjectResult(reader);
        }
        public async Task<IActionResult> ResetPasswordAsync(string readerName, string newPassword)
        {
            var reader = _context.Readers.FirstOrDefault(r => r.ReaderName == readerName);
            if (reader == null)
            {
                return new NotFoundObjectResult("用户不存在");
            }

            reader.Password = PasswordHasher.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return new OkObjectResult("密码重置成功");
        }

        public IEnumerable<Reader> GetAllReaders() => _context.Readers;

        public Reader? GetByReaderName(string name) =>
            _context.Readers.FirstOrDefault(r => r.ReaderName == name);
    }
}
