using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OurNovel.DTOs;
using OurNovel.Models;
using OurNovel.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OurNovel.Data;

namespace OurNovel.Controllers
{
    /// <summary>
    /// 管理员登录、注册与认证相关接口控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LogManagerController : ControllerBase
    {
        private readonly ManagerService _managerService;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public LogManagerController(ManagerService managerService, IConfiguration config, AppDbContext context)
        {
            _managerService = managerService;
            _config = config;
            _context = context;
        }

        /// <summary>
        /// 管理员注册接口（传入 ManagerName + Password）
        /// </summary>
        [HttpPost("register-manager")]
        public async Task<IActionResult> RegisterManager([FromBody] ManagerRegisterDto dto)
        {
            return await _managerService.RegisterAsync(dto.ManagerName, dto.Password);
        }

        /// <summary>
        /// 管理员登录接口，验证用户名和密码，返回 JWT Token
        /// </summary>
        [HttpPost("login-manager")]
        public IActionResult LoginManager([FromBody] ManagerRegisterDto dto)
        {
            var manager = _managerService.GetAllManagers()
                .FirstOrDefault(m => m.ManagerName == dto.ManagerName);

            if (manager == null)
                return Unauthorized("用户名不存在");

            if (!PasswordHasher.VerifyPassword(dto.Password, manager.Password))
                return Unauthorized("密码错误");

            // 生成 JWT Token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, manager.ManagerName),
                new Claim("managerId", manager.ManagerId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                managerName = manager.ManagerName,
                managerId = manager.ManagerId
            });
        }

        /// <summary>
        /// 重置管理员密码（若用户名存在则更新密码，否则返回错误信息）
        /// </summary>
        [HttpPost("reset-manager-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ManagerRegisterDto dto)
        {
            return await _managerService.ResetPasswordAsync(dto.ManagerName, dto.Password);
        }

        /// <summary>
        /// 管理员登出接口（JWT 无状态，前端清除 Token 即可）
        /// </summary>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok("前端请清除 JWT Token，本接口为无状态退出");
        }
    }
}
