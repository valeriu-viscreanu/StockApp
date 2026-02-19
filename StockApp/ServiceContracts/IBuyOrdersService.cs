using StockApp.DTO;

namespace StockApp.ServiceContracts
{
    public interface IBuyOrdersService
    {
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
        Task<List<BuyOrderResponse>> GetBuyOrders();
    }
}
