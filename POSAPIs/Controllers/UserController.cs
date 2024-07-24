using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using POS_System.Entities;
using POS_System.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly string _jwtSubject;

        public UserController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT key must be provided.");
            _jwtIssuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer", "JWT issuer must be provided.");
            _jwtAudience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience", "JWT audience must be provided.");
            _jwtSubject = configuration["Jwt:Subject"] ?? throw new ArgumentNullException("Jwt:Subject", "JWT subject must be provided.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                await _userService.RegisterUserAsync(user);
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthModel authModel)
        {
            var user = await _userService.AuthenticateUserAsync(authModel.EmailOrUsername, authModel.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email/username or password" });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        [Authorize]
        [HttpPost("setrole")]
        public async Task<IActionResult> SetRole([FromBody] SetRoleModel model)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var user = await _userService.GetUserByIdAsync(int.Parse(userId));

            if (user == null || user.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "Only admins can update roles" });
            }

            var success = await _userService.UpdateUserRoleAsync(model.Username, model.Role);
            if (!success)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(new { message = "User role updated successfully" });
        }

        [HttpGet("allusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var result = new List<object>();

            foreach (var user in users)
            {
                result.Add(new
                {
                    user.UserID,
                    user.Username,
                    user.Email,
                    Role = user.UserRole.ToString()
                });
            }

            return Ok(result);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString()),
                    new Claim("id", user.UserID.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class AuthModel
    {
        public string EmailOrUsername { get; set; }
        public string Password { get; set; }
    }

    public class SetRoleModel
    {
        public string Username { get; set; }
        public UserRole Role { get; set; }
    }
}
