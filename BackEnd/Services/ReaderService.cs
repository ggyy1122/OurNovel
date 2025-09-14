using Microsoft.AspNetCore.Mvc;
using OurNovel.Data;
using OurNovel.Models;
using OurNovel.Repositories;

﻿using OurNovel.Repositories;
using Microsoft.AspNetCore.Http;
using OurNovel.Services.Interfaces;
using OurNovel.Models;


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

        public async Task<IActionResult> RegisterAsync(string readerName, string password ,string Phone)
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
                Phone = Phone,
                Balance = 0
            };

            _context.Readers.Add(reader);
            await _context.SaveChangesAsync();

            return new OkObjectResult(reader);
        }
        public async Task<IActionResult> ResetPasswordAsync(string readerName, string phone,string newPassword)
        {
            var reader = _context.Readers.FirstOrDefault(r => r.ReaderName == readerName);
            if (reader == null)
            {
                return new NotFoundObjectResult("用户不存在");
            }
            // 验证手机号码是否匹配
    if (reader.Phone != phone)
    {
        return new BadRequestObjectResult("手机号码不匹配");
    }

            reader.Password = PasswordHasher.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return new OkObjectResult("密码重置成功");
        }

        public IEnumerable<Reader> GetAllReaders() => _context.Readers;

        public Reader? GetByReaderName(string name) =>
            _context.Readers.FirstOrDefault(r => r.ReaderName == name);

        /// <summary>
        /// 根据读者ID获取余额（精确到decimal）
        /// </summary>
        public async Task<IActionResult> GetBalanceById(int readerId)
        {
            var reader = await _context.Readers.FindAsync(readerId);
            if (reader == null)
            {
                return new NotFoundObjectResult("用户不存在");
            }
            return new OkObjectResult(new { Balance = reader.Balance });
        }
    }
}
