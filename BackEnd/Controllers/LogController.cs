using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly AuthorService _authorService;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public LogController(AuthorService authorService, IConfiguration config, AppDbContext context)
        {
            _authorService = authorService;
            _config = config;
            _context = context;
        }

        /// <summary>
        /// 作者注册接口（传入 AuthorName + Password）
        /// </summary>
        [HttpPost("register-author")]
        public async Task<IActionResult> RegisterAuthor([FromBody] AuthorRegisterDto dto)
        {
            return await _authorService.RegisterAsync(dto.AuthorName, dto.Password);
        }

        /// <summary>
        /// 作者登录接口，返回 JWT Token
        /// </summary>
        [HttpPost("login-author")]
        public IActionResult LoginAuthor([FromBody] AuthorRegisterDto dto)
        {
            var author = _authorService.GetAllAuthors()
                .FirstOrDefault(a => a.AuthorName == dto.AuthorName);

            if (author == null)
                return Unauthorized("用户名不存在");

            // 只验证密码，不修改数据库
            if (!PasswordHasher.VerifyPassword(dto.Password, author.Password))
            {
                return Unauthorized("密码错误");
            }

            // 生成 JWT Token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, author.AuthorName),
                new Claim("authorId", author.AuthorId.ToString())
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

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                authorName = author.AuthorName,
                authorId = author.AuthorId
            });
        }

        /// <summary>
        /// 登出接口（前端清除 token 即可）
        /// </summary>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok("前端请自行清除 token，本接口为无状态 JWT 登出");
        }
    }
}
