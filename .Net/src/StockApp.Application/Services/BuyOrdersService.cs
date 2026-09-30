using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Application.Services
{
    public class BuyOrdersService : IBuyOrdersService
    {
        private readonly IBuyOrderRepository _buyOrderRepository;
        private readonly ICashRepository _cashRepository;
        private readonly IRequestValidator<BuyOrderRequest> _buyOrderValidator;
        private readonly IBuyOrderMapper _buyOrderMapper;
        private readonly IUserOperationRepository _userOperationRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IOrderStatusRepository _orderStatusRepository;
        private readonly IStockQuoteService _stockQuoteService;

        public BuyOrdersService(
            IBuyOrderRepository buyOrderRepository,
            ICashRepository cashRepository,
            IRequestValidator<BuyOrderRequest> buyOrderValidator,
            IBuyOrderMapper buyOrderMapper,
            IUserOperationRepository userOperationRepository,
            IAccountRepository accountRepository,
            IOrderStatusRepository orderStatusRepository,
            IStockQuoteService stockQuoteService)
        {
            _stockQuoteService = stockQuoteService;
            _buyOrderRepository = buyOrderRepository;
            _cashRepository = cashRepository;
            _buyOrderValidator = buyOrderValidator;
            _buyOrderMapper = buyOrderMapper;
            _userOperationRepository = userOperationRepository;
            _accountRepository = accountRepository;
            _orderStatusRepository = orderStatusRepository;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }

            _buyOrderValidator.Validate(buyOrderRequest);

            var buyOrder = _buyOrderMapper.MapToEntity(buyOrderRequest);

            // Settle at the server's own ask price. The price the client submitted is
            // only ever a display hint, so it is never trusted for the money math.
            var quote = await _stockQuoteService.GetStockPriceQuote(buyOrder.StockSymbol);
            if (quote?.AskPrice is not > 0)
            {
                throw new InvalidOperationException(
                    $"Unable to price {buyOrder.StockSymbol} right now. Please try again.");
            }

            buyOrder.Price = quote.AskPrice.Value;

            // Check the account and the funds before anything is written, so a
            // rejected order leaves no order row and no ledger entry behind.
            var account = await _accountRepository.GetByUserIDAsync(buyOrder.UserID)
                ?? throw new InvalidOperationException("User has no account");

            double totalCost = buyOrder.Price * buyOrder.Quantity;
            if (account.Balance < totalCost)
            {
                throw new InvalidOperationException("Insufficient funds for this purchase.");
            }

            var pendingStatus = await _orderStatusRepository.GetByName("Pending");
            buyOrder.OrderStatusID = pendingStatus?.OrderStatusID ?? Guid.Empty;
            buyOrder.OrderStatus = pendingStatus!;
            await _buyOrderRepository.AddAsync(buyOrder);

            await _userOperationRepository.AddAsync(new Domain.Entities.UserOperation
            {
                UserOperationID = Guid.NewGuid(),
                UserID = buyOrder.UserID,
                OperationType = Domain.Enums.OperationType.BuyOrder,
                TimeStamp = DateTime.UtcNow,
                Amount = totalCost,
                StockSymbol = buyOrder.StockSymbol,
                Description = $"Bought {buyOrder.Quantity} shares of {buyOrder.StockSymbol} at {buyOrder.Price:C}"
            });

            // Update balance and holdings (Cash)
            account.Balance -= totalCost;
            await _accountRepository.UpdateAsync(account);

            var cash = await _cashRepository.GetBySymbolAsync(account.AccountID, buyOrder.StockSymbol);
            if (cash == null)
            {
                await _cashRepository.AddAsync(new Domain.Entities.Cash
                {
                    CashID = Guid.NewGuid(),
                    AccountID = account.AccountID,
                    StockSymbol = buyOrder.StockSymbol,
                    StockName = buyOrder.StockName,
                    Quantity = buyOrder.Quantity
                });
            }
            else
            {
                cash.Quantity += buyOrder.Quantity;
                await _cashRepository.UpdateAsync(cash);
            }

            // Simulate order processing
            await Task.Delay(500);

            var processedStatus = await _orderStatusRepository.GetByName("Processed");
            buyOrder.OrderStatusID = processedStatus?.OrderStatusID ?? Guid.Empty;
            buyOrder.OrderStatus = processedStatus!;
            await _buyOrderRepository.UpdateAsync(buyOrder);

            return _buyOrderMapper.MapToResponse(buyOrder);
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders(Guid userID)
        {
            var orders = await _buyOrderRepository.GetByUserIDAsync(userID);
            return orders.Select(_buyOrderMapper.MapToResponse).ToList();
        }
    }
}
