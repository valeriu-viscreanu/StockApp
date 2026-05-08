using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;

namespace StockApp.Infrastructure.Services
{
    public class InMemoryAccountService : IAccountService
    {
        public LoginResponse Login(LoginRequest loginRequest)
        {
            if (loginRequest.Email == "admin@test.com" && loginRequest.Password == "123")
            {
                return new LoginResponse
                {
                    IsSuccess = true,
                    UserID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Email = loginRequest.Email
                };
            }
            if (loginRequest.Email == "admin1@test.com" && loginRequest.Password == "123")
            {
                return new LoginResponse
                {
                    IsSuccess = true,
                    UserID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Email = loginRequest.Email
                };
            }
            if (loginRequest.Email == "advisor@test.com" && loginRequest.Password == "123")
            {
                return new LoginResponse
                {
                    IsSuccess = true,
                    UserID = Guid.Parse("00000000-0000-0000-0000-000000000003"), // Matching the Advisor role seed ID for consistency
                    Email = loginRequest.Email
                };
            }
            return new LoginResponse { IsSuccess = false };
        }

        public RegisterResponse Register(RegisterRequest registerRequest)
        {
            return new RegisterResponse { IsSuccess = true };
        }
    }
}
