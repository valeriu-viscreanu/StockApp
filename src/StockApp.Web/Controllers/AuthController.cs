using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(IAccountService accountService, IConfiguration configuration, IRefreshTokenService refreshTokenService)
        {
            _accountService = accountService;
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isSuccess = _accountService.Login(loginRequest);
            if (!isSuccess)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var accessToken = GenerateJwtToken(loginRequest.Email!);
            var refreshToken = _refreshTokenService.CreateRefreshToken(loginRequest.Email!);

            return Ok(new 
            { 
                token = accessToken, 
                refreshToken = refreshToken.Token 
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var refreshToken = _refreshTokenService.GetByToken(refreshRequest.RefreshToken!);

            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiresUtc < DateTime.UtcNow)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token." });
            }

            _refreshTokenService.RevokeToken(refreshToken.Token);
            var newRefreshToken = _refreshTokenService.CreateRefreshToken(refreshToken.Email);
            
            var newAccessToken = GenerateJwtToken(refreshToken.Email);

            return Ok(new
            {
                token = newAccessToken,
                refreshToken = newRefreshToken.Token
            });
        }

        private string GenerateJwtToken(string email)
        {
            var jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException(
                    "JWT signing key is not configured. Please set 'Jwt:Key' in appsettings.json or user secrets.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
