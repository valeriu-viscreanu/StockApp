using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Controllers;
using StockApp.Models;
using StockApp.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Imposter.Abstractions;
using FluentAssertions;

namespace StockAppTests;

public class TradeControllerTests
{
    [Fact]
    public async Task Index_WhenStockParameterIsNull_ReturnsViewWithStockTradeModel()
    {
        // Arrange
        const string expectedStockSymbol = "MSFT";
        const string expectedStockName = "Microsoft Corporation";
        const double expectedPrice = 410.25;
        const uint expectedQuantity = 1;

        var stockProfileServiceMock = new IStockProfileServiceImposter();
        stockProfileServiceMock
            .GetCompanyProfile(expectedStockSymbol)
            .ReturnsAsync(new CompanyProfileResponse { Name = expectedStockName });

        var stockQuoteServiceMock = new IStockQuoteServiceImposter();
        stockQuoteServiceMock
            .GetStockPriceQuote(expectedStockSymbol)
            .ReturnsAsync(new StockQuoteResponse { CurrentPrice = expectedPrice });

        var buyOrdersServiceMock = new IBuyOrdersServiceImposter();
        var sellOrdersServiceMock = new ISellOrdersServiceImposter();
        IOptions<TradingOptions> tradingOptions = Options.Create(new TradingOptions
        {
            DefaultStockSymbol = expectedStockSymbol,
            DefaultOrderQuantity = expectedQuantity
        });

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["FinnhubToken"] = "test-token" })
            .Build();

        var accountProfileServiceMock = new IAccountProfileServiceImposter();

        TradeController controller = new(
            stockProfileServiceMock.Instance(),
            stockQuoteServiceMock.Instance(),
            buyOrdersServiceMock.Instance(),
            sellOrdersServiceMock.Instance(),
            accountProfileServiceMock.Instance(),
            tradingOptions,
            configuration);

        var user = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        }, "mock"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = user }
        };

        // Act
        IActionResult result = await controller.Trade(null);

        // Assert
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        StockTrade model = viewResult.Model.Should().BeOfType<StockTrade>().Subject;

        model.StockSymbol.Should().Be(expectedStockSymbol);
        model.StockName.Should().Be(expectedStockName);
        model.Price.Should().Be(expectedPrice);
        model.Quantity.Should().Be(expectedQuantity);
    }
}
