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

            string url = $"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_finnhubToken}";

            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) return null;
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<FinnhubStockQuoteResponse>();

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
    }
}
