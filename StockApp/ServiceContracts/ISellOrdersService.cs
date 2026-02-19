using StockApp.DTO;

namespace StockApp.ServiceContracts
{
    public interface ISellOrdersService
    {
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
