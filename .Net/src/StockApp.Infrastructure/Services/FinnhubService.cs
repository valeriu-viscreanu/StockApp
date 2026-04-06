using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using System.Collections.Concurrent;

namespace StockApp.Infrastructure.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly HttpClient _httpClient;
        private readonly string _finnhubToken;
        private static readonly ConcurrentDictionary<string, (FinnhubStockQuoteResponse Quote, DateTime Expiry)> _quoteCache = new();

        public FinnhubService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _finnhubToken = configuration["FinnhubToken"] ?? string.Empty;
        }

        public async Task<FinnhubCompanyProfileResponse?> GetCompanyProfile(string stockSymbol)
        {
            if (string.IsNullOrEmpty(_finnhubToken) || _finnhubToken == "YOUR_TOKEN_HERE")
            {
                return new FinnhubCompanyProfileResponse
                {
                    Name = $"{stockSymbol} Inc.",
                    Ticker = stockSymbol,
                    Logo = $"https://logo.clearbit.com/{stockSymbol.ToLower()}.com"
                };
            }

            string url = $"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_finnhubToken}";

            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) return null;
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<FinnhubCompanyProfileResponse>();
        }

        public async Task<FinnhubStockQuoteResponse?> GetStockPriceQuote(string stockSymbol)
        {
            // Check cache first
            if (_quoteCache.TryGetValue(stockSymbol, out var cached) && cached.Expiry > DateTime.UtcNow)
            {
                return cached.Quote;
            }

            FinnhubStockQuoteResponse? result = null;

            if (string.IsNullOrEmpty(_finnhubToken) || _finnhubToken == "YOUR_TOKEN_HERE")
            {
                result = new FinnhubStockQuoteResponse
                {
                    CurrentPrice = 150.0 + (stockSymbol.Length * 5),
                    Change = 2.5,
                    PercentChange = 1.2
                };
            }
            else
            {
                string url = $"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_finnhubToken}";

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) return null;
                if (!response.IsSuccessStatusCode) return null;

                result = await response.Content.ReadFromJsonAsync<FinnhubStockQuoteResponse>();
            }

            if (result != null)
            {
                // Cache for 60 seconds
                _quoteCache[stockSymbol] = (result, DateTime.UtcNow.AddSeconds(60));
            }

            return result;
        }

        public async Task<FinnhubSearchResponse?> SearchStocks(string query)
        {
            string url = $"https://finnhub.io/api/v1/search?q={query}&token={_finnhubToken}";

            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) return null;
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<FinnhubSearchResponse>();
        }
        public async Task<FinnhubStockDataResponse?> GetStockData(string stockSymbol, string resolution, long from, long to)
        {

            if (string.IsNullOrEmpty(_finnhubToken) || _finnhubToken == "YOUR_TOKEN_HERE")
            {
                //todo change this 
                return GetMockStockData(stockSymbol);
            }

            string url = $"https://finnhub.io/api/v1/stock/candle?symbol={stockSymbol}&resolution={resolution}&from={from}&to={to}&token={_finnhubToken}";

            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) return null;
            if (!response.IsSuccessStatusCode) 
            {
                // Fallback to mock data if API call fails
                return GetMockStockData(stockSymbol);
            }

            var result = await response.Content.ReadFromJsonAsync<FinnhubStockDataResponse>();
            if (result?.Status == "no_data") return GetMockStockData(stockSymbol);

            return result;
        }

        private FinnhubStockDataResponse GetMockStockData(string symbol)
        {
            // Simple mock data for 20 days
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var dayInSeconds = 86400;
            var c = new List<double>();
            var h = new List<double>();
            var l = new List<double>();
            var o = new List<double>();
            var t = new List<long>();
            var basePrice = 150.0 + (symbol.Length * 10);

            for (int i = 20; i >= 0; i--)
            {
                var price = basePrice + (Math.Sin(i * 0.5) * 5);
                t.Add(now - (i * dayInSeconds));
                c.Add(price);
                h.Add(price + 2);
                l.Add(price - 2);
                o.Add(price - 1);
            }

            return new FinnhubStockDataResponse
            {
                Status = "ok",
                ClosePrices = c,
                HighPrices = h,
                LowPrices = l,
                OpenPrices = o,
                Timestamps = t
            };
        }
    }
}
