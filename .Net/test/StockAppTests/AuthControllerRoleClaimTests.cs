using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Controllers;
using StockAppTests.Mocks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StockAppTests
{
    /// <summary>
    /// [Authorize(Roles = "...")] reads ClaimTypes.Role off the bearer token, so the
    /// role has to be in the JWT. Without it every role-restricted endpoint refuses
    /// everyone, admins included.
    /// </summary>
    public class AuthControllerRoleClaimTests
    {
        private static AuthController CreateController(LoginResponse loginResponse)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "ThisIsASecretKeyForTests-MustBeAtLeast32Bytes!",
                    ["Jwt:Issuer"] = "StockApp",
                    ["Jwt:Audience"] = "StockAppUsers"
                })
                .Build();

            return new AuthController(
                new FakeAccountService(loginResponse),
                configuration,
                new FakeRefreshTokenService());
        }

        private static async Task<JwtSecurityToken> LoginAndReadToken(LoginResponse loginResponse)
        {
            AuthController controller = CreateController(loginResponse);

            IActionResult result = await controller.Login(new LoginRequest
            {
                Email = loginResponse.Email,
                Password = "123"
            });

            var ok = Assert.IsType<OkObjectResult>(result);
            string token = ok.Value!.GetType().GetProperty("token")!.GetValue(ok.Value)!.ToString()!;

            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }

        // 1. An admin's token carries the role, so [Authorize(Roles = "Admin")] passes.
        [Fact]
        public async Task Login_AsAdmin_PutsTheAdminRoleInTheToken()
        {
            JwtSecurityToken token = await LoginAndReadToken(new LoginResponse
            {
                IsSuccess = true,
                UserID = Guid.NewGuid(),
                Email = "admin@test.com",
                RoleName = "Admin"
            });

            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        }

        // 2. A customer's token carries their own role, not an elevated one.
        [Fact]
        public async Task Login_AsCustomer_DoesNotGrantAdmin()
        {
            JwtSecurityToken token = await LoginAndReadToken(new LoginResponse
            {
                IsSuccess = true,
                UserID = Guid.NewGuid(),
                Email = "customer@test.com",
                RoleName = "Customer"
            });

            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Customer");
            Assert.DoesNotContain(token.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        }

        // 3. A user with no role gets no role claim, rather than a fabricated one.
        [Fact]
        public async Task Login_WithNoRole_OmitsTheRoleClaim()
        {
            JwtSecurityToken token = await LoginAndReadToken(new LoginResponse
            {
                IsSuccess = true,
                UserID = Guid.NewGuid(),
                Email = "norole@test.com",
                RoleName = null
            });

            Assert.DoesNotContain(token.Claims, c => c.Type == ClaimTypes.Role);
        }

        // 4. The user id still travels with the token - role handling must not
        //    disturb what the trading endpoints rely on.
        [Fact]
        public async Task Login_StillCarriesTheUserId()
        {
            Guid userId = Guid.NewGuid();

            JwtSecurityToken token = await LoginAndReadToken(new LoginResponse
            {
                IsSuccess = true,
                UserID = userId,
                Email = "admin@test.com",
                RoleName = "Admin"
            });

            Assert.Contains(token.Claims,
                c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId.ToString());
        }
    }
}
