using StockApp.Application.DTO;

namespace StockApp.Application.ServiceContracts
{
    public interface ISellOrdersService
    {
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
