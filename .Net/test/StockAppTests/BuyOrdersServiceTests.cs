using StockApp.Application.DTO;
using StockApp.Application.Mappers;
using StockApp.Domain.Entities;
using StockApp.Infrastructure.Repositories;
using StockApp.Application.ServiceContracts;
using StockApp.Application.Services;
using StockAppTests.Mocks;

namespace StockAppTests
{
    public class BuyOrdersServiceTests
    {
        private readonly IBuyOrdersService _buyOrdersService;
        private readonly InMemoryAccountRepository _accountRepository;
        private readonly FakeStockQuoteService _quoteService;

        public BuyOrdersServiceTests()
        {
            // No spread, so orders settle at the same prices these tests submit.
            _quoteService = new FakeStockQuoteService();
            _quoteService.SetMidPrice("MSFT", 200);
            _quoteService.SetMidPrice("AAPL", 150);

            _accountRepository = new InMemoryAccountRepository();
            _accountRepository.Add(new Account
            {
                AccountID = Guid.NewGuid(),
                UserID = Guid.Empty,
                Balance = 1_000_000.0,
                DateOfBirth = DateTime.Parse("1990-01-01")
            });

            _buyOrdersService = new BuyOrdersService(
                new InMemoryBuyOrderRepository(),
                new InMemoryCashRepository(),
                new DataAnnotationsRequestValidator<BuyOrderRequest>(),
                new BuyOrderMapper(),
                new InMemoryUserOperationRepository(),
                _accountRepository,
                new InMemoryOrderStatusRepository(),
                _quoteService);
        }

        private void SeedAccount(Guid userId, double balance = 1_000_000.0)
        {
            _accountRepository.Add(new Account
            {
                AccountID = Guid.NewGuid(),
                UserID = userId,
                Balance = balance,
                DateOfBirth = DateTime.Parse("1990-01-01")
            });
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
            List<BuyOrderResponse> buyOrders = await _buyOrdersService.GetBuyOrders(Guid.Empty);

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
                Price = 100,
                UserID = Guid.NewGuid()
            };
            SeedAccount(request1.UserID);

            BuyOrderRequest request2 = new BuyOrderRequest
            {
                StockSymbol = "AAPL",
                StockName = "Apple",
                DateAndTimeOfOrder = DateTime.Parse("2024-02-01"),
                Quantity = 20,
                Price = 150,
                UserID = request1.UserID
            };

            BuyOrderResponse response1 = await _buyOrdersService.CreateBuyOrder(request1);
            BuyOrderResponse response2 = await _buyOrdersService.CreateBuyOrder(request2);

            // Act
            List<BuyOrderResponse> buyOrders = await _buyOrdersService.GetBuyOrders(request1.UserID);

            // Assert
            Assert.Equal(2, buyOrders.Count);
            Assert.Contains(buyOrders, b => b.BuyOrderID == response1.BuyOrderID);
            Assert.Contains(buyOrders, b => b.BuyOrderID == response2.BuyOrderID);
        }

        #endregion
    }
}
