namespace StockApp.ServiceContracts
{
    public interface IStockQuoteService
    {
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}
