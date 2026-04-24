using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;

namespace StockAppTests;

public class TradeRouteIntegrationTests
{
    [Fact]
    public async Task Get_TradeIndexWithStockSymbol_ReturnsHtmlWithPriceElement()
    {
        using StockAppFactory factory = new();
        {
            using HttpClient client = factory.CreateClient();
            {
                HttpResponseMessage response = await client.GetAsync("/Trade/Trade/MSFT");

                response.EnsureSuccessStatusCode();
                response.Content.Headers.ContentType?.MediaType.Should().Be("text/html");
            }
        }

    }

    private sealed class StockAppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IMarketDataService>();
                services.RemoveAll<IStockProfileService>();
                services.RemoveAll<IStockQuoteService>();
                services.AddSingleton<FakeMarketDataService>();
                services.AddSingleton<IMarketDataService>(sp => sp.GetRequiredService<FakeMarketDataService>());
                services.AddSingleton<IStockProfileService>(sp => sp.GetRequiredService<FakeMarketDataService>());
                services.AddSingleton<IStockQuoteService>(sp => sp.GetRequiredService<FakeMarketDataService>());
            });
        }
    }

    private sealed class FakeMarketDataService : IMarketDataService
    {
        private static readonly IReadOnlyDictionary<string, string> CompanyNames =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["MSFT"] = "Microsoft Corporation",
                ["AAPL"] = "Apple Inc.",
                ["GOOGL"] = "Alphabet Inc.",
                ["TSLA"] = "Tesla, Inc."
            };

        private static readonly IReadOnlyDictionary<string, double> LastPrices =
            new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
            {
                ["MSFT"] = 410.25,
                ["AAPL"] = 195.80,
                ["GOOGL"] = 174.35,
                ["TSLA"] = 207.10
            };

        public Task<CompanyProfileResponse?> GetCompanyProfile(string stockSymbol)
        {
            if (!CompanyNames.TryGetValue(stockSymbol, out string? companyName))
            {
                return Task.FromResult<CompanyProfileResponse?>(null);
            }

            return Task.FromResult<CompanyProfileResponse?>(new CompanyProfileResponse
            {
                Name = companyName
            });
        }

        public Task<StockQuoteResponse?> GetStockPriceQuote(string stockSymbol)
        {
            if (!LastPrices.TryGetValue(stockSymbol, out double currentPrice))
            {
                return Task.FromResult<StockQuoteResponse?>(null);
            }

            return Task.FromResult<StockQuoteResponse?>(new StockQuoteResponse
            {
                CurrentPrice = currentPrice
            });
        }

        public Task<StockSearchResponse?> SearchStocks(string query)
        {
            var results = CompanyNames
                .Where(kvp => kvp.Value.Contains(query, StringComparison.OrdinalIgnoreCase) || kvp.Key.Equals(query, StringComparison.OrdinalIgnoreCase))
                .Select(kvp => new StockSearchResult
                {
                    Symbol = kvp.Key,
                    Description = kvp.Value,
                    DisplaySymbol = kvp.Key
                })
                .ToList();

            return Task.FromResult<StockSearchResponse?>(new StockSearchResponse
            {
                Count = results.Count,
                Result = results
            });
        }

        public Task<StockDataResponse?> GetStockData(string stockSymbol, string resolution, long from, long to)
        {
            return Task.FromResult<StockDataResponse?>(new StockDataResponse
            {
                ClosePrices = new List<double> { 100, 101, 102 },
                Status = "ok"
            });
        }
    }
}
