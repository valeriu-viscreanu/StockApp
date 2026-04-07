using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Application.Services
{
    public class SellOrdersService : ISellOrdersService
    {
        private readonly ISellOrderRepository _sellOrderRepository;
        private readonly IUserHoldingRepository _userHoldingRepository;
        private readonly IRequestValidator<SellOrderRequest> _sellOrderValidator;
        private readonly ISellOrderMapper _sellOrderMapper;
        private readonly IUserOperationRepository _userOperationRepository;

        public SellOrdersService(
            ISellOrderRepository sellOrderRepository,
            IUserHoldingRepository userHoldingRepository,
            IRequestValidator<SellOrderRequest> sellOrderValidator,
            ISellOrderMapper sellOrderMapper,
            IUserOperationRepository userOperationRepository)
        {
            _sellOrderRepository = sellOrderRepository;
            _userHoldingRepository = userHoldingRepository;
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

            // Validation: Check stock ownership via holdings table
            var holding = _userHoldingRepository.GetBySymbol(sellOrderRequest.UserID, sellOrderRequest.StockSymbol);
            if (holding == null || holding.Quantity < sellOrderRequest.Quantity)
            {
                throw new ArgumentException($"You do not own enough shares of {sellOrderRequest.StockSymbol} to sell.");
            }

            var sellOrder = _sellOrderMapper.MapToEntity(sellOrderRequest);
            _sellOrderRepository.Add(sellOrder);

            // Update holdings
            holding.Quantity -= sellOrder.Quantity;
            if (holding.Quantity == 0)
            {
                _userHoldingRepository.Delete(holding.HoldingID);
            }
            else
            {
                _userHoldingRepository.Update(holding);
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
