namespace StockApp.ServiceContracts
{
    public interface IStockProfileService
    {
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
    }
}
