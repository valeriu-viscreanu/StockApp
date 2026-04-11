using StockApp.Application.DTO;

namespace StockApp.Application.ServiceContracts
{
    public interface IStockQuoteService
    {
        Task<StockQuoteResponse?> GetStockPriceQuote(string stockSymbol);
    }
}
