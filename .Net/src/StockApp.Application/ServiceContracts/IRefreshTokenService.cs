using StockApp.Domain.Entities;

namespace StockApp.Application.ServiceContracts
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> CreateRefreshToken(string email);
        Task<RefreshToken?> GetByToken(string token);
        Task RevokeToken(string token);
    }
}
