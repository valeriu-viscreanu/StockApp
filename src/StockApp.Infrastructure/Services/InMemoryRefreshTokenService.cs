using StockApp.Application.ServiceContracts;
using StockApp.Domain.Entities;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace StockApp.Infrastructure.Services
{
    public class InMemoryRefreshTokenService : IRefreshTokenService
    {
        private readonly ConcurrentDictionary<string, RefreshToken> _refreshTokens = new();

        public RefreshToken CreateRefreshToken(string email)
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
            return refreshToken;
        }

        public RefreshToken? GetByToken(string token)
        {
            _refreshTokens.TryGetValue(token, out var refreshToken);
            return refreshToken;
        }

        public void RevokeToken(string token)
        {
            if (_refreshTokens.TryGetValue(token, out var refreshToken))
            {
                refreshToken.IsRevoked = true;
            }
        }
    }
}
