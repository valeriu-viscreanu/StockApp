using StockApp.DTO;

namespace StockApp.ServiceContracts
{
    public interface IStockQuoteService
    {
        Task<FinnhubStockQuoteResponse?> GetStockPriceQuote(string stockSymbol);
    }
}
