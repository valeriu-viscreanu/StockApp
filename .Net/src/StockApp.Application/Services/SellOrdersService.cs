using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Application.Services
{
    public class SellOrdersService : ISellOrdersService
    {
        private readonly ISellOrderRepository _sellOrderRepository;
        private readonly IRequestValidator<SellOrderRequest> _sellOrderValidator;
        private readonly ISellOrderMapper _sellOrderMapper;
        private readonly IUserOperationRepository _userOperationRepository;

        public SellOrdersService(
            ISellOrderRepository sellOrderRepository,
            IRequestValidator<SellOrderRequest> sellOrderValidator,
            ISellOrderMapper sellOrderMapper,
            IUserOperationRepository userOperationRepository)
        {
            _sellOrderRepository = sellOrderRepository;
            _sellOrderValidator = sellOrderValidator;
            _sellOrderMapper = sellOrderMapper;
            _userOperationRepository = userOperationRepository;
        }

        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(sellOrderRequest));
            }

            _sellOrderValidator.Validate(sellOrderRequest);

            var sellOrder = _sellOrderMapper.MapToEntity(sellOrderRequest);
            _sellOrderRepository.Add(sellOrder);

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

            return Task.FromResult(_sellOrderMapper.MapToResponse(sellOrder));
        }

        public Task<List<SellOrderResponse>> GetSellOrders(Guid userID)
        {
            var sellOrderResponses = _sellOrderRepository
                .GetByUserID(userID)
                .Select(_sellOrderMapper.MapToResponse)
                .ToList();

            return Task.FromResult(sellOrderResponses);
        }
    }
}
