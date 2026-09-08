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
    /// Orders settle at the server's own price - the ask for a buy, the bid for a
    /// sell - not at whatever price the client submitted. Otherwise a client could
    /// simply post the pre-spread mid and trade around the spread entirely.
    /// </summary>
    public class SpreadSettlementTests
    {
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Account _account;
        private readonly InMemoryAccountRepository _accountRepository = new();
        private readonly InMemoryCashRepository _cashRepository = new();
        private readonly FakeStockQuoteService _quoteService = new();

        private readonly IBuyOrdersService _buyOrdersService;
        private readonly ISellOrdersService _sellOrdersService;

        public SpreadSettlementTests()
        {
            _account = new Account
            {
                AccountID = Guid.NewGuid(),
                UserID = _userId,
                Balance = 10_000.0,
                DateOfBirth = DateTime.Parse("1990-01-01")
            };
            _accountRepository.Add(_account);

            _buyOrdersService = new BuyOrdersService(
                new InMemoryBuyOrderRepository(),
                _cashRepository,
                new DataAnnotationsRequestValidator<BuyOrderRequest>(),
                new BuyOrderMapper(),
                new InMemoryUserOperationRepository(),
                _accountRepository,
                new InMemoryOrderStatusRepository(),
                _quoteService);

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

        private void SeedHolding(uint quantity)
        {
            _cashRepository.Add(new Cash
            {
                CashID = Guid.NewGuid(),
                AccountID = _account.AccountID,
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Quantity = quantity
            });
        }

        private BuyOrderRequest BuyRequest(uint quantity, double clientPrice) => new()
        {
            UserID = _userId,
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
            Quantity = quantity,
            Price = clientPrice
        };

        private SellOrderRequest SellRequest(uint quantity, double clientPrice) => new()
        {
            UserID = _userId,
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = DateTime.Parse("2024-01-01"),
            Quantity = quantity,
            Price = clientPrice
        };

        // 1. A buy settles at the ask, not at the mid the client submitted.
        [Fact]
        public async Task CreateBuyOrder_SettlesAtAskPrice_NotTheClientSubmittedPrice()
        {
            _quoteService.SetQuote("MSFT", mid: 100.0, bid: 99.90, ask: 100.10);

            BuyOrderResponse response = await _buyOrdersService.CreateBuyOrder(BuyRequest(10, clientPrice: 100.0));

            Assert.Equal(100.10, response.Price, precision: 10);
            Assert.Equal(10_000.0 - 1001.0, _account.Balance, precision: 10); // 10 * 100.10
        }

        // 2. A sell settles at the bid, not at the mid the client submitted.
        [Fact]
        public async Task CreateSellOrder_SettlesAtBidPrice_NotTheClientSubmittedPrice()
        {
            _quoteService.SetQuote("MSFT", mid: 100.0, bid: 99.90, ask: 100.10);
            SeedHolding(10);

            SellOrderResponse response = await _sellOrdersService.CreateSellOrder(SellRequest(10, clientPrice: 100.0));

            Assert.Equal(99.90, response.Price, precision: 10);
            Assert.Equal(10_000.0 + 999.0, _account.Balance, precision: 10); // 10 * 99.90
        }

        // 3. A client cannot buy below the ask by lying about the price.
        [Fact]
        public async Task CreateBuyOrder_ClientUnderstatesPrice_StillChargedTheAsk()
        {
            _quoteService.SetQuote("MSFT", mid: 100.0, bid: 99.90, ask: 100.10);

            await _buyOrdersService.CreateBuyOrder(BuyRequest(10, clientPrice: 1.0));

            Assert.Equal(10_000.0 - 1001.0, _account.Balance, precision: 10);
        }

        // 4. Nor sell above the bid the same way.
        [Fact]
        public async Task CreateSellOrder_ClientOverstatesPrice_StillPaidTheBid()
        {
            _quoteService.SetQuote("MSFT", mid: 100.0, bid: 99.90, ask: 100.10);
            SeedHolding(10);

            await _sellOrdersService.CreateSellOrder(SellRequest(10, clientPrice: 9_000.0));

            Assert.Equal(10_000.0 + 999.0, _account.Balance, precision: 10);
        }

        // 5. Buying then selling straight back at an unchanged mid costs the spread.
        //    This is the behaviour the whole feature exists to produce.
        [Fact]
        public async Task BuyThenSellAtUnchangedMid_LosesTheSpread()
        {
            _quoteService.SetQuote("MSFT", mid: 100.0, bid: 99.90, ask: 100.10);

            await _buyOrdersService.CreateBuyOrder(BuyRequest(10, clientPrice: 100.0));
            await _sellOrdersService.CreateSellOrder(SellRequest(10, clientPrice: 100.0));

            // Paid 10 * 100.10, received 10 * 99.90 -> down 2.00 on a 10-share round trip.
            Assert.Equal(10_000.0 - 2.0, _account.Balance, precision: 10);
        }

        // 6. With no live quote the order cannot be priced, so it is rejected
        //    rather than falling back to the client's number.
        [Fact]
        public async Task CreateBuyOrder_NoQuoteAvailable_IsRejected()
        {
            // No quote set for MSFT.
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _buyOrdersService.CreateBuyOrder(BuyRequest(10, clientPrice: 100.0)));

            Assert.Equal(10_000.0, _account.Balance, precision: 10);
        }

        // 7. Affordability is judged against the ask, not the client's price: 99 shares
        //    at an ask of 100.10 costs 9909.90, which fits, but the client claiming
        //    a price of 1.00 must not make an unaffordable order look affordable.
        [Fact]
        public async Task CreateBuyOrder_AffordabilityIsCheckedAgainstTheAsk()
        {
            _quoteService.SetQuote("MSFT", mid: 100.0, bid: 99.90, ask: 100.10);

            // 100 shares at the ask costs 10,010 - more than the 10,000 balance -
            // even though the client claims it only costs 100.
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _buyOrdersService.CreateBuyOrder(BuyRequest(100, clientPrice: 1.0)));

            Assert.Equal(10_000.0, _account.Balance, precision: 10);
        }
    }
}
