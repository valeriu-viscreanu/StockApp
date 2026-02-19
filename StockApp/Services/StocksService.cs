using StockApp.DTO;
using StockApp.ServiceContracts;

namespace StockApp.Services
{
    public class StocksService : IStocksService
    {
        private readonly IBuyOrderRepository _buyOrderRepository;
        private readonly ISellOrderRepository _sellOrderRepository;
        private readonly IRequestValidator<BuyOrderRequest> _buyOrderValidator;
        private readonly IRequestValidator<SellOrderRequest> _sellOrderValidator;
        private readonly IBuyOrderMapper _buyOrderMapper;
        private readonly ISellOrderMapper _sellOrderMapper;

        public StocksService(
            IBuyOrderRepository buyOrderRepository,
            ISellOrderRepository sellOrderRepository,
            IRequestValidator<BuyOrderRequest> buyOrderValidator,
            IRequestValidator<SellOrderRequest> sellOrderValidator,
            IBuyOrderMapper buyOrderMapper,
            ISellOrderMapper sellOrderMapper)
        {
            _buyOrderRepository = buyOrderRepository;
            _sellOrderRepository = sellOrderRepository;
            _buyOrderValidator = buyOrderValidator;
            _sellOrderValidator = sellOrderValidator;
            _buyOrderMapper = buyOrderMapper;
            _sellOrderMapper = sellOrderMapper;
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

        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(sellOrderRequest));
            }

            _sellOrderValidator.Validate(sellOrderRequest);

            var sellOrder = _sellOrderMapper.MapToEntity(sellOrderRequest);
            _sellOrderRepository.Add(sellOrder);

            return Task.FromResult(_sellOrderMapper.MapToResponse(sellOrder));
        }

        public Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var buyOrderResponses = _buyOrderRepository
                .GetAll()
                .Select(_buyOrderMapper.MapToResponse)
                .ToList();

            return Task.FromResult(buyOrderResponses);
        }

        public Task<List<SellOrderResponse>> GetSellOrders()
        {
            var sellOrderResponses = _sellOrderRepository
                .GetAll()
                .Select(_sellOrderMapper.MapToResponse)
                .ToList();

            return Task.FromResult(sellOrderResponses);
        }
    }
}
