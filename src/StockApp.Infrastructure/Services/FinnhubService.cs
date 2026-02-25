using System.Text.Json;
using Microsoft.Extensions.Configuration;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;

namespace StockApp.Infrastructure.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FinnhubService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<FinnhubCompanyProfileResponse?> GetCompanyProfile(string stockSymbol)
        {
            string? token = _configuration["FinnhubToken"];
            string url = $"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={token}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            FinnhubCompanyProfileResponse? result = JsonSerializer.Deserialize<FinnhubCompanyProfileResponse>(responseBody);

            return result;
        }

        public async Task<FinnhubStockQuoteResponse?> GetStockPriceQuote(string stockSymbol)
        {
            string? token = _configuration["FinnhubToken"];
            string url = $"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={token}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            FinnhubStockQuoteResponse? result = JsonSerializer.Deserialize<FinnhubStockQuoteResponse>(responseBody);

            return result;
        }

        public async Task<FinnhubSearchResponse?> SearchStocks(string query)
        {
            string? token = _configuration["FinnhubToken"];
            string url = $"https://finnhub.io/api/v1/search?q={query}&token={token}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            FinnhubSearchResponse? result = JsonSerializer.Deserialize<FinnhubSearchResponse>(responseBody);

            return result;
        }
    }
}
