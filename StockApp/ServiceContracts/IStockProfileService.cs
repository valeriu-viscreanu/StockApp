using StockApp.DTO;

namespace StockApp.ServiceContracts
{
    public interface IStockProfileService
    {
        Task<FinnhubCompanyProfileResponse?> GetCompanyProfile(string stockSymbol);
    }
}
