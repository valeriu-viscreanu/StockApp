using StockApp.Application.DTO;
using StockApp.Application.Mappers;
using StockApp.Application.ServiceContracts;
using StockApp.Application.Services;
using StockApp.Domain.Entities;
using StockApp.Infrastructure.Repositories;
using StockAppTests.Mocks;

namespace StockAppTests
{
    public class TradeBalanceRoundTripTests
    {
        private sealed class TestServices
        {
            public required IBuyOrdersService BuyOrdersService { get; init; }
            public required ISellOrdersService SellOrdersService { get; init; }
            public required InMemoryUserOperationRepository UserOperationRepository { get; init; }
        }

        private static TestServices CreateServices(Account seededAccount)
        {
            var accountRepository = new InMemoryAccountRepository();
            accountRepository.Add(seededAccount);

            var cashRepository = new InMemoryCashRepository();
            var userOperationRepository = new InMemoryUserOperationRepository();

            IBuyOrdersService buyOrdersService = new BuyOrdersService(
                new InMemoryBuyOrderRepository(),
                cashRepository,
                new DataAnnotationsRequestValidator<BuyOrderRequest>(),
                new BuyOrderMapper(),
                userOperationRepository,
                accountRepository,
                new InMemoryOrderStatusRepository());

            ISellOrdersService sellOrdersService = new SellOrdersService(
                new InMemorySellOrderRepository(),
                cashRepository,
                new DataAnnotationsRequestValidator<SellOrderRequest>(),
                new SellOrderMapper(),
                userOperationRepository,
                accountRepository,
                new InMemoryOrderStatusRepository());

            return new TestServices
            {
                BuyOrdersService = buyOrdersService,
                SellOrdersService = sellOrdersService,
                UserOperationRepository = userOperationRepository
            };
        }

        // 1. Buying then selling the same shares at the same price should leave the balance unchanged.
        [Fact]
        public async Task BuyThenSell_SamePriceAndQuantity_BalanceReturnsToStartingValue()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var account = new Account
            {
                AccountID = Guid.NewGuid(),
                UserID = userId,
                Balance = 1000.0,
                DateOfBirth = DateTime.Parse("1990-01-01")
            };
            var services = CreateServices(account);

            BuyOrderRequest buyOrderRequest = new BuyOrderRequest
            {
                UserID = userId,
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 10,
                Price = 50 // costs 500
            };

            SellOrderRequest sellOrderRequest = new SellOrderRequest
            {
                UserID = userId,
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-02"),
                Quantity = 10,
                Price = 50 // proceeds 500
            };

            // Act
            await services.BuyOrdersService.CreateBuyOrder(buyOrderRequest);
            double balanceAfterBuy = account.Balance;

            await services.SellOrdersService.CreateSellOrder(sellOrderRequest);
            double balanceAfterSell = account.Balance;

            // Assert
            Assert.Equal(500.0, balanceAfterBuy);
            Assert.Equal(1000.0, balanceAfterSell); // back to starting balance, not 1500
        }

        // 2. A buy costing more than the available balance must be rejected, not silently accepted.
        [Fact]
        public async Task CreateBuyOrder_CostExceedsBalance_ThrowsAndLeavesBalanceUnchanged()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var account = new Account
            {
                AccountID = Guid.NewGuid(),
                UserID = userId,
                Balance = 100.0,
                DateOfBirth = DateTime.Parse("1990-01-01")
            };
            var services = CreateServices(account);

            BuyOrderRequest buyOrderRequest = new BuyOrderRequest
            {
                UserID = userId,
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 10,
                Price = 50 // costs 500, only 100 available
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await services.BuyOrdersService.CreateBuyOrder(buyOrderRequest);
            });
            Assert.Equal(100.0, account.Balance);
        }

        // 3. Each buy/sell should write exactly one ledger row at the service layer -
        //    guards against the double-logging bug (currently in the controllers) coming back.
        [Fact]
        public async Task BuyAndSell_EachWriteExactlyOneUserOperation()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var account = new Account
            {
                AccountID = Guid.NewGuid(),
                UserID = userId,
                Balance = 1000.0,
                DateOfBirth = DateTime.Parse("1990-01-01")
            };
            var services = CreateServices(account);

            BuyOrderRequest buyOrderRequest = new BuyOrderRequest
            {
                UserID = userId,
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
                Quantity = 10,
                Price = 50
            };

            SellOrderRequest sellOrderRequest = new SellOrderRequest
            {
                UserID = userId,
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2024-01-02"),
                Quantity = 10,
                Price = 50
            };

            // Act
            await services.BuyOrdersService.CreateBuyOrder(buyOrderRequest);
            await services.SellOrdersService.CreateSellOrder(sellOrderRequest);

            // Assert
            List<UserOperation> operations = services.UserOperationRepository.GetAll();
            Assert.Equal(2, operations.Count);
            Assert.Single(operations, o => o.OperationType == StockApp.Domain.Enums.OperationType.BuyOrder);
            Assert.Single(operations, o => o.OperationType == StockApp.Domain.Enums.OperationType.SellOrder);
        }
    }
}
