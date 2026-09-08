using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;

namespace StockAppTests.Mocks
{
    /// <summary>
    /// Serves canned quotes so tests can control the server-side price an order
    /// settles at. Symbols with no quote set return null, which is how tests
    /// exercise the "cannot price this order" path.
    /// </summary>
    public class FakeStockQuoteService : IStockQuoteService
    {
        private readonly Dictionary<string, StockQuoteResponse> _quotes = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>Quote with no spread: bid and ask both equal the mid.</summary>
        public void SetMidPrice(string symbol, double mid) => SetQuote(symbol, mid, bid: mid, ask: mid);

        public void SetQuote(string symbol, double mid, double bid, double ask)
        {
            _quotes[symbol] = new StockQuoteResponse
            {
                CurrentPrice = mid,
                BidPrice = bid,
                AskPrice = ask
            };
        }

        public Task<StockQuoteResponse?> GetStockPriceQuote(string stockSymbol)
        {
            _quotes.TryGetValue(stockSymbol, out var quote);
            return Task.FromResult(quote);
        }
    }
}
