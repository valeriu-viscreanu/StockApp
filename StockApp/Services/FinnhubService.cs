using System.Text.Json;
using StockApp.ServiceContracts;

namespace StockApp.Services
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

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            string? token = _configuration["FinnhubToken"];
            string url = $"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={token}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            Dictionary<string, object>? result = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            return result;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            string? token = _configuration["FinnhubToken"];
            string url = $"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={token}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            Dictionary<string, object>? result = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            return result;
        }
    }
}
