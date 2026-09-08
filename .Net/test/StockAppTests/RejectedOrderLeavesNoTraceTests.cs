using StockApp.Application.DTO;
using StockApp.Application.Mappers;
using StockApp.Application.ServiceContracts;
using StockApp.Application.Services;
using StockApp.Domain.Entities;
using StockApp.Infrastructure.Repositories;
using StockAppTests.Mocks;

namespace StockAppTests
{
    /// <summary>
    /// A rejected order must leave nothing behind. Both services write the order
    /// row and its ledger entry before checking funds/shares, so a rejection used
    /// to leave a phantom Pending order and a false "Bought/Sold N shares" entry
    /// in the user's history.
    /// </summary>
    public class RejectedOrderLeavesNoTraceTests
    {
        private readonly Guid _userId = Guid.NewGuid();
        private readonly InMemoryBuyOrderRepository _buyOrderRepository = new();
        private readonly InMemorySellOrderRepository _sellOrderRepository = new();
        private readonly InMemoryUserOperationRepository _userOperationRepository = new();
        private readonly InMemoryAccountRepository _accountRepository = new();
        private readonly InMemoryCashRepository _cashRepository = new();
        private readonly FakeStockQuoteService _quoteService = new();

        private readonly IBuyOrdersService _buyOrdersService;
        private readonly ISellOrdersService _sellOrdersService;

        public RejectedOrderLeavesNoTraceTests()
        {
            // No spread, so orders settle at the prices these tests submit.
            _quoteService.SetMidPrice("MSFT", 50);

            _buyOrdersService = new BuyOrdersService(
                _buyOrderRepository,
                _cashRepository,
                new DataAnnotationsRequestValidator<BuyOrderRequest>(),
                new BuyOrderMapper(),
                _userOperationRepository,
                _accountRepository,
                new InMemoryOrderStatusRepository(),
                _quoteService);

            _sellOrdersService = new SellOrdersService(
                _sellOrderRepository,
                _cashRepository,
                new DataAnnotationsRequestValidator<SellOrderRequest>(),
                new SellOrderMapper(),
                _userOperationRepository,
                _accountRepository,
                new InMemoryOrderStatusRepository(),
                _quoteService);
        }

        private Account SeedAccount(double balance)
        {
            var account = new Account
            {
                AccountID = Guid.NewGuid(),
                UserID = _userId,
                Balance = balance,
                DateOfBirth = DateTime.Parse("1990-01-01")
            };
            _accountRepository.Add(account);
            return account;
        }

        private void SeedHolding(Account account, uint quantity)
        {
            _cashRepository.Add(new Cash
            {
                CashID = Guid.NewGuid(),
                AccountID = account.AccountID,
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Quantity = quantity
            });
        }

        private BuyOrderRequest BuyRequest(uint quantity, double price) => new()
        {
            UserID = _userId,
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
            Quantity = quantity,
            Price = price
        };

        private SellOrderRequest SellRequest(uint quantity, double price) => new()
        {
            UserID = _userId,
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
            Quantity = quantity,
            Price = price
        };

        // 1. A buy the user cannot afford must not leave an order row behind.
        [Fact]
        public async Task CreateBuyOrder_InsufficientFunds_PersistsNoBuyOrder()
        {
            SeedAccount(balance: 100.0);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _buyOrdersService.CreateBuyOrder(BuyRequest(10, 50))); // costs 500

            Assert.Empty(_buyOrderRepository.GetByUserID(_userId));
        }

        // 2. ...nor a "Bought N shares" entry in the activity ledger.
        [Fact]
        public async Task CreateBuyOrder_InsufficientFunds_WritesNoLedgerEntry()
        {
            SeedAccount(balance: 100.0);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _buyOrdersService.CreateBuyOrder(BuyRequest(10, 50)));

            Assert.Empty(_userOperationRepository.GetAll());
        }

        // 3. A sell of shares the user does not own must not leave an order row.
        [Fact]
        public async Task CreateSellOrder_InsufficientShares_PersistsNoSellOrder()
        {
            var account = SeedAccount(balance: 1000.0);
            SeedHolding(account, quantity: 5);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _sellOrdersService.CreateSellOrder(SellRequest(10, 50)));

            Assert.Empty(_sellOrderRepository.GetByUserID(_userId));
        }

        // 4. A buy for a user with no account must not leave an order row either.
        [Fact]
        public async Task CreateBuyOrder_NoAccount_PersistsNoBuyOrder()
        {
            // No account seeded.
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _buyOrdersService.CreateBuyOrder(BuyRequest(1, 50)));

            Assert.Empty(_buyOrderRepository.GetByUserID(_userId));
            Assert.Empty(_userOperationRepository.GetAll());
        }

        // 5. An accepted buy still records exactly one order and one ledger entry.
        [Fact]
        public async Task CreateBuyOrder_SufficientFunds_RecordsOrderAndLedgerEntry()
        {
            var account = SeedAccount(balance: 1000.0);

            await _buyOrdersService.CreateBuyOrder(BuyRequest(10, 50));

            Assert.Single(_buyOrderRepository.GetByUserID(_userId));
            Assert.Single(_userOperationRepository.GetAll());
            Assert.Equal(500.0, account.Balance);
        }
    }
}
