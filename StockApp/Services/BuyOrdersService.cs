using StockApp.DTO;
using StockApp.ServiceContracts;

namespace StockApp.Services
{
    public class BuyOrdersService : IBuyOrdersService
    {
        private readonly IBuyOrderRepository _buyOrderRepository;
        private readonly IRequestValidator<BuyOrderRequest> _buyOrderValidator;
        private readonly IBuyOrderMapper _buyOrderMapper;

        public BuyOrdersService(
            IBuyOrderRepository buyOrderRepository,
            IRequestValidator<BuyOrderRequest> buyOrderValidator,
            IBuyOrderMapper buyOrderMapper)
        {
            _buyOrderRepository = buyOrderRepository;
            _buyOrderValidator = buyOrderValidator;
            _buyOrderMapper = buyOrderMapper;
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

            return Task.FromResult(_buyOrderMapper.MapToResponse(buyOrder));
        }

        public Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var buyOrderResponses = _buyOrderRepository
                .GetAll()
                .Select(_buyOrderMapper.MapToResponse)
                .ToList();

            return Task.FromResult(buyOrderResponses);
        }
    }
}
