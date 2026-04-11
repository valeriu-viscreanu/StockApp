using StockApp.Application.DTO;

namespace StockApp.Application.ServiceContracts
{
    public interface IStockProfileService
    {
        Task<CompanyProfileResponse?> GetCompanyProfile(string stockSymbol);
        Task<StockSearchResponse?> SearchStocks(string query);
    }
}
