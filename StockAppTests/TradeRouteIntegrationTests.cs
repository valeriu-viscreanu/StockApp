using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StockApp.DTO;
using StockApp.ServiceContracts;

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
                HttpResponseMessage response = await client.GetAsync("/Trade/Index/MSFT");

                response.StatusCode.Should().Be(HttpStatusCode.OK);
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
                services.RemoveAll<IFinnhubService>();
                services.RemoveAll<IStockProfileService>();
                services.RemoveAll<IStockQuoteService>();
                services.AddSingleton<FakeFinnhubService>();
                services.AddSingleton<IFinnhubService>(sp => sp.GetRequiredService<FakeFinnhubService>());
                services.AddSingleton<IStockProfileService>(sp => sp.GetRequiredService<FakeFinnhubService>());
                services.AddSingleton<IStockQuoteService>(sp => sp.GetRequiredService<FakeFinnhubService>());
            });
        }
    }

    private sealed class FakeFinnhubService : IFinnhubService
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

        public Task<FinnhubCompanyProfileResponse?> GetCompanyProfile(string stockSymbol)
        {
            if (!CompanyNames.TryGetValue(stockSymbol, out string? companyName))
            {
                return Task.FromResult<FinnhubCompanyProfileResponse?>(null);
            }

            return Task.FromResult<FinnhubCompanyProfileResponse?>(new FinnhubCompanyProfileResponse
            {
                Name = companyName
            });
        }

        public Task<FinnhubStockQuoteResponse?> GetStockPriceQuote(string stockSymbol)
        {
            if (!LastPrices.TryGetValue(stockSymbol, out double currentPrice))
            {
                return Task.FromResult<FinnhubStockQuoteResponse?>(null);
            }

            return Task.FromResult<FinnhubStockQuoteResponse?>(new FinnhubStockQuoteResponse
            {
                CurrentPrice = currentPrice
            });
        }
    }
}
