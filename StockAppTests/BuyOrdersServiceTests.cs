using StockApp.DTO;
using StockApp.Mappers;
using StockApp.Repositories;
using StockApp.ServiceContracts;
using StockApp.Services;

namespace StockAppTests
{
    public class BuyOrdersServiceTests
    {
        private readonly IBuyOrdersService _buyOrdersService;

        public BuyOrdersServiceTests()
        {
            _buyOrdersService = new BuyOrdersService(
                new InMemoryBuyOrderRepository(),
                new DataAnnotationsRequestValidator<BuyOrderRequest>(),
                new BuyOrderMapper());
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
                await _buyOrdersService.CreateBuyOrder(buyOrderRequest);
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
                await _buyOrdersService.CreateBuyOrder(buyOrderRequest);
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
                await _buyOrdersService.CreateBuyOrder(buyOrderRequest);
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
                await _buyOrdersService.CreateBuyOrder(buyOrderRequest);
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
                await _buyOrdersService.CreateBuyOrder(buyOrderRequest);
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
                await _buyOrdersService.CreateBuyOrder(buyOrderRequest);
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
                await _buyOrdersService.CreateBuyOrder(buyOrderRequest);
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
            BuyOrderResponse response = await _buyOrdersService.CreateBuyOrder(buyOrderRequest);

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

        #region GetAllBuyOrders

        // 1. By default, returned list should be empty
        [Fact]
        public async Task GetAllBuyOrders_DefaultList_ShouldBeEmpty()
        {
            // Act
            List<BuyOrderResponse> buyOrders = await _buyOrdersService.GetBuyOrders();

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

            BuyOrderResponse response1 = await _buyOrdersService.CreateBuyOrder(request1);
            BuyOrderResponse response2 = await _buyOrdersService.CreateBuyOrder(request2);

            // Act
            List<BuyOrderResponse> buyOrders = await _buyOrdersService.GetBuyOrders();

            // Assert
            Assert.Equal(2, buyOrders.Count);
            Assert.Contains(buyOrders, b => b.BuyOrderID == response1.BuyOrderID);
            Assert.Contains(buyOrders, b => b.BuyOrderID == response2.BuyOrderID);
        }

        #endregion
    }
}
