using StockApp.Application.DTO;

namespace StockApp.Application.ServiceContracts
{
    public interface IFinnhubService : IStockProfileService, IStockQuoteService
    {
        Task<FinnhubStockDataResponse?> GetStockData(string stockSymbol, string resolution, long from, long to);
    }
}
