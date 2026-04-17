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

        public BuyOrdersService(
            IBuyOrderRepository buyOrderRepository,
            ICashRepository cashRepository,
            IRequestValidator<BuyOrderRequest> buyOrderValidator,
            IBuyOrderMapper buyOrderMapper,
            IUserOperationRepository userOperationRepository,
            IAccountRepository accountRepository)
        {
            _buyOrderRepository = buyOrderRepository;
            _cashRepository = cashRepository;
            _buyOrderValidator = buyOrderValidator;
            _buyOrderMapper = buyOrderMapper;
            _userOperationRepository = userOperationRepository;
            _accountRepository = accountRepository;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }

            _buyOrderValidator.Validate(buyOrderRequest);

            var buyOrder = _buyOrderMapper.MapToEntity(buyOrderRequest);
            buyOrder.Status = "Pending";
            _buyOrderRepository.Add(buyOrder);

            _userOperationRepository.Add(new Domain.Entities.UserOperation
            {
                UserOperationID = Guid.NewGuid(),
                UserID = buyOrder.UserID,
                OperationType = Domain.Enums.OperationType.BuyOrder,
                TimeStamp = DateTime.UtcNow,
                Amount = buyOrder.Price * buyOrder.Quantity,
                StockSymbol = buyOrder.StockSymbol,
                Description = $"Bought {buyOrder.Quantity} shares of {buyOrder.StockSymbol} at {buyOrder.Price:C}"
            });

            // Update holdings (Cash)
            var account = _accountRepository.GetByUserID(buyOrder.UserID) 
                ?? throw new InvalidOperationException("User has no account");

            var cash = _cashRepository.GetBySymbol(account.AccountID, buyOrder.StockSymbol);
            if (cash == null)
            {
                _cashRepository.Add(new Domain.Entities.Cash
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
                _cashRepository.Update(cash);
            }

            // Simulate order processing
            await Task.Delay(500);

            buyOrder.Status = "Processed";
            _buyOrderRepository.Update(buyOrder);

            return _buyOrderMapper.MapToResponse(buyOrder);
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders(Guid userID)
        {
            var buyOrderResponses = _buyOrderRepository
                .GetByUserID(userID)
                .Select(_buyOrderMapper.MapToResponse)
                .ToList();

            return await Task.FromResult(buyOrderResponses);
        }
    }
}
