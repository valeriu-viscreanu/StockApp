using StockApp.Domain.Entities;

namespace StockApp.Application.ServiceContracts
{
    public interface IRefreshTokenService
    {
        RefreshToken CreateRefreshToken(string email);
        RefreshToken? GetByToken(string token);
        void RevokeToken(string token);
    }
}
