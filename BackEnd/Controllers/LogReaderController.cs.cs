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
    /// 读者登录注册及认证控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LogReaderController : ControllerBase
    {
        private readonly ReaderService _readerService;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public LogReaderController(ReaderService readerService, IConfiguration config, AppDbContext context)
        {
            _readerService = readerService;
            _config = config;
            _context = context;
        }

        /// <summary>
        /// 读者注册接口（传入 ReaderName + Password）
        /// </summary>
        [HttpPost("register-reader")]
        public async Task<IActionResult> RegisterReader([FromBody] ReaderRegisterDto dto)
        {
            return await _readerService.RegisterAsync(dto.ReaderName, dto.Password);
        }

        /// <summary>
        /// 读者登录接口，验证用户名和密码，返回 JWT Token
        /// </summary>
        [HttpPost("login-reader")]
        public IActionResult LoginReader([FromBody] ReaderRegisterDto dto)
        {
            var reader = _readerService.GetAllReaders()
                .FirstOrDefault(r => r.ReaderName == dto.ReaderName);

            if (reader == null)
                return Unauthorized("用户名不存在");

            if (!PasswordHasher.VerifyPassword(dto.Password, reader.Password))
                return Unauthorized("密码错误");

            // 生成 JWT Token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, reader.ReaderName),
                new Claim("readerId", reader.ReaderId.ToString())
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
                readerName = reader.ReaderName,
                readerId = reader.ReaderId
            });
        }

        /// <summary>
        /// 重置密码接口：传入用户名和新密码，若用户存在则更新密码
        /// </summary>
        [HttpPost("reset-reader-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ReaderRegisterDto dto)
        {
            return await _readerService.ResetPasswordAsync(dto.ReaderName, dto.Password);
        }

        /// <summary>
        /// 登出接口（JWT 无状态，前端只需清除 Token 即可）
        /// </summary>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok("前端请清除 JWT Token，本接口为无状态退出");
        }
    }
}
