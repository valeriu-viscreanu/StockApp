using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Application.Services
{
    public class BuyOrdersService : IBuyOrdersService
    {
        private readonly IBuyOrderRepository _buyOrderRepository;
        private readonly IRequestValidator<BuyOrderRequest> _buyOrderValidator;
        private readonly IBuyOrderMapper _buyOrderMapper;
        private readonly IUserOperationRepository _userOperationRepository;

        public BuyOrdersService(
            IBuyOrderRepository buyOrderRepository,
            IRequestValidator<BuyOrderRequest> buyOrderValidator,
            IBuyOrderMapper buyOrderMapper,
            IUserOperationRepository userOperationRepository)
        {
            _buyOrderRepository = buyOrderRepository;
            _buyOrderValidator = buyOrderValidator;
            _buyOrderMapper = buyOrderMapper;
            _userOperationRepository = userOperationRepository;
        }

        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }

            _buyOrderValidator.Validate(buyOrderRequest);

            var buyOrder = _buyOrderMapper.MapToEntity(buyOrderRequest);
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

            return Task.FromResult(_buyOrderMapper.MapToResponse(buyOrder));
        }

        public Task<List<BuyOrderResponse>> GetBuyOrders(Guid userID)
        {
            var buyOrderResponses = _buyOrderRepository
                .GetByUserID(userID)
                .Select(_buyOrderMapper.MapToResponse)
                .ToList();

            return Task.FromResult(buyOrderResponses);
        }
    }
}
