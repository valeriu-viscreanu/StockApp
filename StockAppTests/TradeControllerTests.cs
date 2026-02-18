using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using StockApp.Controllers;
using StockApp.Models;
using StockApp.Options;
using StockApp.ServiceContracts;

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
        const uint expectedQuantity = 100;

        Mock<IFinnhubService> finnhubServiceMock = new();
        finnhubServiceMock
            .Setup(service => service.GetCompanyProfile(expectedStockSymbol))
            .ReturnsAsync(new Dictionary<string, object> { ["name"] = expectedStockName });
        finnhubServiceMock
            .Setup(service => service.GetStockPriceQuote(expectedStockSymbol))
            .ReturnsAsync(new Dictionary<string, object> { ["c"] = expectedPrice });

        Mock<IStocksService> stocksServiceMock = new();
        IOptions<TradingOptions> tradingOptions = Options.Create(new TradingOptions
        {
            DefaultStockSymbol = expectedStockSymbol,
            DefaultOrderQuantity = expectedQuantity
        });

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["FinnhubToken"] = "test-token" })
            .Build();

        TradeController controller = new(
            finnhubServiceMock.Object,
            stocksServiceMock.Object,
            tradingOptions,
            configuration);

        // Act
        IActionResult result = await controller.Index(null);

        // Assert
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        StockTrade model = viewResult.Model.Should().BeOfType<StockTrade>().Subject;

        model.StockSymbol.Should().Be(expectedStockSymbol);
        model.StockName.Should().Be(expectedStockName);
        model.Price.Should().Be(expectedPrice);
        model.Quantity.Should().Be(expectedQuantity);
    }
}
