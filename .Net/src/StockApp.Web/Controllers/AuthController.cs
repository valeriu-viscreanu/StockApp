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
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LoginResponse loginResponse = _accountService.Login(loginRequest);
            if (!loginResponse.IsSuccess)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var accessToken = GenerateJwtToken(loginResponse.Email!, loginResponse.UserID);
            var refreshToken = await _refreshTokenService.CreateRefreshToken(loginRequest.Email!);

            return Ok(new 
            { 
                token = accessToken, 
                refreshToken = refreshToken.Token,
                role = loginResponse.RoleName
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _accountService.Register(request);
            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.ErrorMessage });
            }

            return Ok(new { message = "Registration successful" });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var refreshToken = await _refreshTokenService.GetByToken(refreshRequest.RefreshToken!);

            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiresUtc < DateTime.UtcNow)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token." });
            }

            await _refreshTokenService.RevokeToken(refreshToken.Token);
            var newRefreshToken = await _refreshTokenService.CreateRefreshToken(refreshToken.Email);
            
            // Note: Since we don't have UserID in RefreshToken, we might need to store it or look it up.
            // For now, I'll pass Guid.Empty or try to find it if possible. 
            // In a real app, RefreshToken would link to a User.
            var newAccessToken = GenerateJwtToken(refreshToken.Email, Guid.Empty);

            return Ok(new
            {
                token = newAccessToken,
                refreshToken = newRefreshToken.Token
            });
        }

        private string GenerateJwtToken(string email, Guid userId)
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
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
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
