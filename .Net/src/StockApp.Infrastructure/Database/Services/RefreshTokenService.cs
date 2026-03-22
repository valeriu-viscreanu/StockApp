using StockApp.Application.ServiceContracts;
using StockApp.Domain.Entities;
using System.Security.Cryptography;

namespace StockApp.Infrastructure.Database.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ApplicationDbContext _dbContext;

    public RefreshTokenService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RefreshToken> CreateRefreshToken(string email)
    {
        var tokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshToken = new RefreshToken
        {
            Token = tokenString,
            Email = email,
            ExpiresUtc = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();
        
        return refreshToken;
    }

    public async Task<RefreshToken?> GetByToken(string token)
    {
        return await _dbContext.RefreshTokens.FindAsync(token);
    }

    public async Task RevokeToken(string token)
    {
        var refreshToken = await _dbContext.RefreshTokens.FindAsync(token);
        if (refreshToken != null)
        {
            refreshToken.IsRevoked = true;
            await _dbContext.SaveChangesAsync();
        }
    }
}
