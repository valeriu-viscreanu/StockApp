using StockApp.Application.DTO;

namespace StockApp.Application.ServiceContracts
{
    public interface IMarketDataService : IStockProfileService, IStockQuoteService
    {
        Task<StockDataResponse?> GetStockData(string stockSymbol, string resolution, long from, long to);
    }
}
