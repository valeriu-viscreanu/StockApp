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

        public SellOrdersService(
            ISellOrderRepository sellOrderRepository,
            IRequestValidator<SellOrderRequest> sellOrderValidator,
            ISellOrderMapper sellOrderMapper)
        {
            _sellOrderRepository = sellOrderRepository;
            _sellOrderValidator = sellOrderValidator;
            _sellOrderMapper = sellOrderMapper;
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
