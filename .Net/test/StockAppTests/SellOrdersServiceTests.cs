using StockApp.Application.DTO;
using StockApp.Application.Mappers;
using StockApp.Domain.Entities;
using StockApp.Infrastructure.Repositories;
using StockApp.Application.ServiceContracts;
using StockApp.Application.Services;
using StockAppTests.Mocks;

namespace StockAppTests
{
    public class SellOrdersServiceTests
    {
        private readonly ISellOrdersService _sellOrdersService;
        private readonly InMemoryAccountRepository _accountRepository;
        private readonly InMemoryCashRepository _cashRepository;
        private readonly FakeStockQuoteService _quoteService;

        public SellOrdersServiceTests()
        {
            // No spread, so orders settle at the same prices these tests submit.
            _quoteService = new FakeStockQuoteService();
            _quoteService.SetMidPrice("MSFT", 200);
            _quoteService.SetMidPrice("AAPL", 150);

            _accountRepository = new InMemoryAccountRepository();
            _cashRepository = new InMemoryCashRepository();

            SeedHolding(Guid.Empty, "MSFT", "Microsoft", 1_000_000);

            _sellOrdersService = new SellOrdersService(
                new InMemorySellOrderRepository(),
                _cashRepository,
                new DataAnnotationsRequestValidator<SellOrderRequest>(),
                new SellOrderMapper(),
                new InMemoryUserOperationRepository(),
                _accountRepository,
                new InMemoryOrderStatusRepository(),
                _quoteService);
        }

        // Ensures a seeded Account (with a large balance) and matching Cash holding exist for
        // the given user, so a sell order for that symbol/user can succeed.
        private void SeedHolding(Guid userId, string symbol, string name, uint quantity)
        {
            var account = _accountRepository.GetByUserID(userId);
            if (account == null)
            {
                account = new Account
                {
                    AccountID = Guid.NewGuid(),
                    UserID = userId,
                    Balance = 1_000_000.0,
                    DateOfBirth = DateTime.Parse("1990-01-01")
                };
                _accountRepository.Add(account);
            }

            _cashRepository.Add(new Cash
            {
                CashID = Guid.NewGuid(),
                AccountID = account.AccountID,
                StockSymbol = symbol,
                StockName = name,
                Quantity = quantity
            });
        }

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
                await _sellOrdersService.CreateSellOrder(sellOrderRequest);
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
                await _sellOrdersService.CreateSellOrder(sellOrderRequest);
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
                await _sellOrdersService.CreateSellOrder(sellOrderRequest);
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
                await _sellOrdersService.CreateSellOrder(sellOrderRequest);
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
                await _sellOrdersService.CreateSellOrder(sellOrderRequest);
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
                await _sellOrdersService.CreateSellOrder(sellOrderRequest);
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
                await _sellOrdersService.CreateSellOrder(sellOrderRequest);
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
            SellOrderResponse response = await _sellOrdersService.CreateSellOrder(sellOrderRequest);

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

        #region GetAllSellOrders

        // 1. By default, returned list should be empty
        [Fact]
        public async Task GetAllSellOrders_DefaultList_ShouldBeEmpty()
        {
            // Act
            List<SellOrderResponse> sellOrders = await _sellOrdersService.GetSellOrders(Guid.Empty);

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
                Price = 100,
                UserID = Guid.NewGuid()
            };
            SeedHolding(request1.UserID, "MSFT", "Microsoft", 100);
            SeedHolding(request1.UserID, "AAPL", "Apple", 100);

            SellOrderRequest request2 = new SellOrderRequest
            {
                StockSymbol = "AAPL",
                StockName = "Apple",
                DateAndTimeOfOrder = DateTime.Parse("2024-02-01"),
                Quantity = 20,
                Price = 150,
                UserID = request1.UserID
            };

            SellOrderResponse response1 = await _sellOrdersService.CreateSellOrder(request1);
            SellOrderResponse response2 = await _sellOrdersService.CreateSellOrder(request2);

            // Act
            List<SellOrderResponse> sellOrders = await _sellOrdersService.GetSellOrders(request1.UserID);

            // Assert
            Assert.Equal(2, sellOrders.Count);
            Assert.Contains(sellOrders, s => s.SellOrderID == response1.SellOrderID);
            Assert.Contains(sellOrders, s => s.SellOrderID == response2.SellOrderID);
        }

        #endregion
    }
}
