using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Application.Services
{
    public class SellOrdersService : ISellOrdersService
    {
        private readonly ISellOrderRepository _sellOrderRepository;
        private readonly ICashRepository _cashRepository;
        private readonly IRequestValidator<SellOrderRequest> _sellOrderValidator;
        private readonly ISellOrderMapper _sellOrderMapper;
        private readonly IUserOperationRepository _userOperationRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IOrderStatusRepository _orderStatusRepository;
        private readonly IStockQuoteService _stockQuoteService;

        public SellOrdersService(
            ISellOrderRepository sellOrderRepository,
            ICashRepository cashRepository,
            IRequestValidator<SellOrderRequest> sellOrderValidator,
            ISellOrderMapper sellOrderMapper,
            IUserOperationRepository userOperationRepository,
            IAccountRepository accountRepository,
            IOrderStatusRepository orderStatusRepository,
            IStockQuoteService stockQuoteService)
        {
            _stockQuoteService = stockQuoteService;
            _sellOrderRepository = sellOrderRepository;
            _cashRepository = cashRepository;
            _sellOrderValidator = sellOrderValidator;
            _sellOrderMapper = sellOrderMapper;
            _userOperationRepository = userOperationRepository;
            _accountRepository = accountRepository;
            _orderStatusRepository = orderStatusRepository;
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(sellOrderRequest));
            }

            _sellOrderValidator.Validate(sellOrderRequest);

            var sellOrder = _sellOrderMapper.MapToEntity(sellOrderRequest);

            // Settle at the server's own bid price. The price the client submitted is
            // only ever a display hint, so it is never trusted for the money math.
            var quote = await _stockQuoteService.GetStockPriceQuote(sellOrder.StockSymbol);
            if (quote?.BidPrice is not > 0)
            {
                throw new InvalidOperationException(
                    $"Unable to price {sellOrder.StockSymbol} right now. Please try again.");
            }

            sellOrder.Price = quote.BidPrice.Value;

            // Check the account and the holdings before anything is written, so a
            // rejected order leaves no order row and no ledger entry behind.
            var account = await _accountRepository.GetByUserIDAsync(sellOrderRequest.UserID)
                ?? throw new InvalidOperationException("User has no account");

            var cash = await _cashRepository.GetBySymbolAsync(account.AccountID, sellOrderRequest.StockSymbol);
            if (cash == null || cash.Quantity < sellOrder.Quantity)
            {
                throw new InvalidOperationException($"Insufficient shares of {sellOrder.StockSymbol} to complete the sale");
            }

            var pendingStatus = await _orderStatusRepository.GetByName("Pending");
            sellOrder.OrderStatusID = pendingStatus?.OrderStatusID ?? Guid.Empty;
            sellOrder.OrderStatus = pendingStatus!;
            await _sellOrderRepository.AddAsync(sellOrder);

            // Update balance and holdings (Cash)
            double totalRevenue = sellOrder.Price * sellOrder.Quantity;
            account.Balance += totalRevenue;
            await _accountRepository.UpdateAsync(account);

            cash.Quantity -= sellOrder.Quantity;
            if (cash.Quantity == 0)
            {
                await _cashRepository.DeleteAsync(cash.CashID);
            }
            else
            {
                await _cashRepository.UpdateAsync(cash);
            }

            await _userOperationRepository.AddAsync(new Domain.Entities.UserOperation
            {
                UserOperationID = Guid.NewGuid(),
                UserID = sellOrder.UserID,
                OperationType = Domain.Enums.OperationType.SellOrder,
                TimeStamp = DateTime.UtcNow,
                Amount = sellOrder.Price * sellOrder.Quantity,
                StockSymbol = sellOrder.StockSymbol,
                Description = $"Sold {sellOrder.Quantity} shares of {sellOrder.StockSymbol} at {sellOrder.Price:C}"
            });

            // Simulate order processing
            await Task.Delay(500);

            var processedStatus = await _orderStatusRepository.GetByName("Processed");
            sellOrder.OrderStatusID = processedStatus?.OrderStatusID ?? Guid.Empty;
            sellOrder.OrderStatus = processedStatus!;
            await _sellOrderRepository.UpdateAsync(sellOrder);

            return _sellOrderMapper.MapToResponse(sellOrder);
        }

        public async Task<List<SellOrderResponse>> GetSellOrders(Guid userID)
        {
            var orders = await _sellOrderRepository.GetByUserIDAsync(userID);
            return orders.Select(_sellOrderMapper.MapToResponse).ToList();
        }
    }
}
