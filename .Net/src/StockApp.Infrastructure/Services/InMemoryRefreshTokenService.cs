using StockApp.Application.ServiceContracts;
using StockApp.Domain.Entities;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace StockApp.Infrastructure.Services
{
    public class InMemoryRefreshTokenService : IRefreshTokenService
    {
        private readonly ConcurrentDictionary<string, RefreshToken> _refreshTokens = new();

        public Task<RefreshToken> CreateRefreshToken(string email)
        {
            var tokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshToken = new RefreshToken
            {
                Token = tokenString,
                Email = email,
                ExpiresUtc = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _refreshTokens.TryAdd(tokenString, refreshToken);
            return Task.FromResult(refreshToken);
        }

        public Task<RefreshToken?> GetByToken(string token)
        {
            _refreshTokens.TryGetValue(token, out var refreshToken);
            return Task.FromResult(refreshToken);
        }

        public Task RevokeToken(string token)
        {
            if (_refreshTokens.TryGetValue(token, out var refreshToken))
            {
                refreshToken.IsRevoked = true;
            }
            return Task.CompletedTask;
        }
    }
}
