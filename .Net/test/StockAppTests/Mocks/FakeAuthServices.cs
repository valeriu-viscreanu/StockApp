using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Domain.Entities;

namespace StockAppTests.Mocks
{
    /// <summary>Returns one canned login result, whatever the credentials.</summary>
    public class FakeAccountService : IAccountService
    {
        private readonly LoginResponse _loginResponse;

        public FakeAccountService(LoginResponse loginResponse)
        {
            _loginResponse = loginResponse;
        }

        public LoginResponse Login(LoginRequest loginRequest) => _loginResponse;

        public RegisterResponse Register(RegisterRequest registerRequest) =>
            new() { IsSuccess = true };
    }

    public class FakeRefreshTokenService : IRefreshTokenService
    {
        public Task<RefreshToken> CreateRefreshToken(string email) =>
            Task.FromResult(new RefreshToken { Token = "refresh-token", Email = email });

        public Task<RefreshToken?> GetByToken(string token) =>
            Task.FromResult<RefreshToken?>(null);

        public Task RevokeToken(string token) => Task.CompletedTask;
    }
}
