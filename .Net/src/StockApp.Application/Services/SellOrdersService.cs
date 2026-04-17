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

        public SellOrdersService(
            ISellOrderRepository sellOrderRepository,
            ICashRepository cashRepository,
            IRequestValidator<SellOrderRequest> sellOrderValidator,
            ISellOrderMapper sellOrderMapper,
            IUserOperationRepository userOperationRepository,
            IAccountRepository accountRepository,
            IOrderStatusRepository orderStatusRepository)
        {
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
            var pendingStatus = await _orderStatusRepository.GetByName("Pending");
            sellOrder.OrderStatusID = pendingStatus?.OrderStatusID ?? Guid.Empty;
            sellOrder.OrderStatus = pendingStatus!;
            _sellOrderRepository.Add(sellOrder);

            // Update holdings (Cash)
            var account = _accountRepository.GetByUserID(sellOrderRequest.UserID) 
                ?? throw new InvalidOperationException("User has no account");

            var cash = _cashRepository.GetBySymbol(account.AccountID, sellOrderRequest.StockSymbol);
            if (cash != null)
            {
                cash.Quantity -= sellOrder.Quantity;
                if (cash.Quantity == 0)
                {
                    _cashRepository.Delete(cash.CashID);
                }
                else
                {
                    _cashRepository.Update(cash);
                }
            }

            _userOperationRepository.Add(new Domain.Entities.UserOperation
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
            _sellOrderRepository.Update(sellOrder);

            return _sellOrderMapper.MapToResponse(sellOrder);
        }

        public async Task<List<SellOrderResponse>> GetSellOrders(Guid userID)
        {
            var sellOrderResponses = _sellOrderRepository
                .GetByUserID(userID)
                .Select(_sellOrderMapper.MapToResponse)
                .ToList();

            return await Task.FromResult(sellOrderResponses);
        }
    }
}
