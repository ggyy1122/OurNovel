using Microsoft.AspNetCore.Mvc;
using OurNovel.Data;
using OurNovel.Models;
using OurNovel.Repositories;

namespace OurNovel.Services
{
    /// <summary>
    /// Manager 服务，继承通用服务基类，可扩展登录/注册逻辑
    /// </summary>
    public class ManagerService : BaseService<Manager, int>
    {
        private readonly AppDbContext _context;

        public ManagerService(IRepository<Manager, int> repository, AppDbContext context)
            : base(repository)
        {
            _context = context;
        }

        public async Task<IActionResult> RegisterAsync(string managerName, string password)
        {
            if (_context.Managers.Count(m => m.ManagerName == managerName) > 0)
            {
                return new BadRequestObjectResult("用户名已存在");
            }

            var hashedPassword = PasswordHasher.HashPassword(password);

            var manager = new Manager
            {
                ManagerName = managerName,
                Password = hashedPassword
            };

            _context.Managers.Add(manager);
            await _context.SaveChangesAsync();

            return new OkObjectResult(manager);
        }

        public IEnumerable<Manager> GetAllManagers()
        {
            return _context.Managers;
        }

        public Manager? GetByManagerName(string managerName)
        {
            return _context.Managers.FirstOrDefault(m => m.ManagerName == managerName);
        }
    }
}
