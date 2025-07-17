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
    /// ����Ա��¼��ע������֤��ؽӿڿ�����
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
        /// ����Աע��ӿڣ����� ManagerName + Password��
        /// </summary>
        [HttpPost("register-manager")]
        public async Task<IActionResult> RegisterManager([FromBody] ManagerRegisterDto dto)
        {
            return await _managerService.RegisterAsync(dto.ManagerName, dto.Password);
        }

        /// <summary>
        /// ����Ա��¼�ӿڣ���֤�û��������룬���� JWT Token
        /// </summary>
        [HttpPost("login-manager")]
        public IActionResult LoginManager([FromBody] ManagerRegisterDto dto)
        {
            var manager = _managerService.GetAllManagers()
                .FirstOrDefault(m => m.ManagerName == dto.ManagerName);

            if (manager == null)
                return Unauthorized("�û���������");

            if (!PasswordHasher.VerifyPassword(dto.Password, manager.Password))
                return Unauthorized("�������");

            // ���� JWT Token
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
        /// ���ù���Ա���루���û���������������룬���򷵻ش�����Ϣ��
        /// </summary>
        [HttpPost("reset-manager-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ManagerRegisterDto dto)
        {
            return await _managerService.ResetPasswordAsync(dto.ManagerName, dto.Password);
        }

        /// <summary>
        /// ����Ա�ǳ��ӿڣ�JWT ��״̬��ǰ����� Token ���ɣ�
        /// </summary>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok("ǰ������� JWT Token�����ӿ�Ϊ��״̬�˳�");
        }
    }
}
