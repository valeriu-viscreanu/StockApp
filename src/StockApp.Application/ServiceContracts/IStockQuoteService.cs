using StockApp.Application.DTO;

namespace StockApp.Application.ServiceContracts
{
    public interface IStockQuoteService
    {
        Task<FinnhubStockQuoteResponse?> GetStockPriceQuote(string stockSymbol);
    }
}
