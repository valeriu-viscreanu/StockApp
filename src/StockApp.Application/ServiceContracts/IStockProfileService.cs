using StockApp.Application.DTO;

namespace StockApp.Application.ServiceContracts
{
    public interface IStockProfileService
    {
        Task<FinnhubCompanyProfileResponse?> GetCompanyProfile(string stockSymbol);
    }
}
