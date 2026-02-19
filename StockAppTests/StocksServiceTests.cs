using StockApp.DTO;
using StockApp.Mappers;
using StockApp.Repositories;
using StockApp.ServiceContracts;
using StockApp.Services;

namespace StockAppTests
{
    public class StocksServiceTests
    {
        private readonly IStocksService _stocksService;

        public StocksServiceTests()
        {
            _stocksService = new StocksService(
                new InMemoryBuyOrderRepository(),
                new InMemorySellOrderRepository(),
                new DataAnnotationsRequestValidator<BuyOrderRequest>(),
                new DataAnnotationsRequestValidator<SellOrderRequest>(),
                new BuyOrderMapper(),
                new SellOrderMapper());
        }

        #region CreateBuyOrder

        // 1. When BuyOrderRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task CreateBuyOrder_NullRequest_ThrowsArgumentNullException()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 2. When quantity is 0, it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_QuantityIsZero_ThrowsArgumentException()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 0,
                Price = 100
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 3. When quantity is 100001, it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_QuantityIsAboveMax_ThrowsArgumentException()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 100001,
                Price = 100
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 4. When price is 0, it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_PriceIsZero_ThrowsArgumentException()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 10,
                Price = 0
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 5. When price is 10001, it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_PriceIsAboveMax_ThrowsArgumentException()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 10,
                Price = 10001
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 6. When stock symbol is null, it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_StockSymbolIsNull_ThrowsArgumentException()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest
            {
                StockSymbol = null!,
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 10,
                Price = 100
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 7. When date is older than 2000-01-01, it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_DateIsOlderThanMinimum_ThrowsArgumentException()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
                Quantity = 10,
                Price = 100
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 8. Valid values should return BuyOrderResponse with auto-generated BuyOrderID
        [Fact]
        public async Task CreateBuyOrder_ValidValues_ReturnsResponse()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 50,
                Price = 200
            };

            // Act
            BuyOrderResponse response = await _stocksService.CreateBuyOrder(buyOrderRequest);

            // Assert
            Assert.NotNull(response);
            Assert.NotEqual(Guid.Empty, response.BuyOrderID);
            Assert.Equal("MSFT", response.StockSymbol);
            Assert.Equal("Microsoft", response.StockName);
            Assert.Equal(50u, response.Quantity);
            Assert.Equal(200, response.Price);
            Assert.Equal(10000, response.TradeAmount);
        }

        #endregion

        #region CreateSellOrder

        // 1. When SellOrderRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task CreateSellOrder_NullRequest_ThrowsArgumentNullException()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 2. When quantity is 0, it should throw ArgumentException
        [Fact]
        public async Task CreateSellOrder_QuantityIsZero_ThrowsArgumentException()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 0,
                Price = 100
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 3. When quantity is 100001, it should throw ArgumentException
        [Fact]
        public async Task CreateSellOrder_QuantityIsAboveMax_ThrowsArgumentException()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 100001,
                Price = 100
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 4. When price is 0, it should throw ArgumentException
        [Fact]
        public async Task CreateSellOrder_PriceIsZero_ThrowsArgumentException()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 10,
                Price = 0
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 5. When price is 10001, it should throw ArgumentException
        [Fact]
        public async Task CreateSellOrder_PriceIsAboveMax_ThrowsArgumentException()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 10,
                Price = 10001
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 6. When stock symbol is null, it should throw ArgumentException
        [Fact]
        public async Task CreateSellOrder_StockSymbolIsNull_ThrowsArgumentException()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest
            {
                StockSymbol = null!,
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 10,
                Price = 100
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 7. When date is older than 2000-01-01, it should throw ArgumentException
        [Fact]
        public async Task CreateSellOrder_DateIsOlderThanMinimum_ThrowsArgumentException()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
                Quantity = 10,
                Price = 100
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 8. Valid values should return SellOrderResponse with auto-generated SellOrderID
        [Fact]
        public async Task CreateSellOrder_ValidValues_ReturnsResponse()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 50,
                Price = 200
            };

            // Act
            SellOrderResponse response = await _stocksService.CreateSellOrder(sellOrderRequest);

            // Assert
            Assert.NotNull(response);
            Assert.NotEqual(Guid.Empty, response.SellOrderID);
            Assert.Equal("MSFT", response.StockSymbol);
            Assert.Equal("Microsoft", response.StockName);
            Assert.Equal(50u, response.Quantity);
            Assert.Equal(200, response.Price);
            Assert.Equal(10000, response.TradeAmount);
        }

        #endregion

        #region GetAllBuyOrders

        // 1. By default, returned list should be empty
        [Fact]
        public async Task GetAllBuyOrders_DefaultList_ShouldBeEmpty()
        {
            // Act
            List<BuyOrderResponse> buyOrders = await _stocksService.GetBuyOrders();

            // Assert
            Assert.Empty(buyOrders);
        }

        // 2. After adding buy orders, GetAllBuyOrders should return all of them
        [Fact]
        public async Task GetAllBuyOrders_AfterAdding_ShouldReturnAll()
        {
            // Arrange
            BuyOrderRequest request1 = new BuyOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 10,
                Price = 100
            };

            BuyOrderRequest request2 = new BuyOrderRequest
            {
                StockSymbol = "AAPL",
                StockName = "Apple",
                DateAndTimeOfOrder = DateTime.Parse("2024-02-01"),
                Quantity = 20,
                Price = 150
            };

            BuyOrderResponse response1 = await _stocksService.CreateBuyOrder(request1);
            BuyOrderResponse response2 = await _stocksService.CreateBuyOrder(request2);

            // Act
            List<BuyOrderResponse> buyOrders = await _stocksService.GetBuyOrders();

            // Assert
            Assert.Equal(2, buyOrders.Count);
            Assert.Contains(buyOrders, b => b.BuyOrderID == response1.BuyOrderID);
            Assert.Contains(buyOrders, b => b.BuyOrderID == response2.BuyOrderID);
        }

        #endregion

        #region GetAllSellOrders

        // 1. By default, returned list should be empty
        [Fact]
        public async Task GetAllSellOrders_DefaultList_ShouldBeEmpty()
        {
            // Act
            List<SellOrderResponse> sellOrders = await _stocksService.GetSellOrders();

            // Assert
            Assert.Empty(sellOrders);
        }

        // 2. After adding sell orders, GetAllSellOrders should return all of them
        [Fact]
        public async Task GetAllSellOrders_AfterAdding_ShouldReturnAll()
        {
            // Arrange
            SellOrderRequest request1 = new SellOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 10,
                Price = 100
            };

            SellOrderRequest request2 = new SellOrderRequest
            {
                StockSymbol = "AAPL",
                StockName = "Apple",
                DateAndTimeOfOrder = DateTime.Parse("2024-02-01"),
                Quantity = 20,
                Price = 150
            };

            SellOrderResponse response1 = await _stocksService.CreateSellOrder(request1);
            SellOrderResponse response2 = await _stocksService.CreateSellOrder(request2);

            // Act
            List<SellOrderResponse> sellOrders = await _stocksService.GetSellOrders();

            // Assert
            Assert.Equal(2, sellOrders.Count);
            Assert.Contains(sellOrders, s => s.SellOrderID == response1.SellOrderID);
            Assert.Contains(sellOrders, s => s.SellOrderID == response2.SellOrderID);
        }

        #endregion
    }
}
