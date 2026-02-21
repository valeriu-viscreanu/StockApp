using StockApp.Application.DTO;

namespace StockApp.Application.ServiceContracts
{
    public interface IBuyOrdersService
    {
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
        Task<List<BuyOrderResponse>> GetBuyOrders();
    }
}
