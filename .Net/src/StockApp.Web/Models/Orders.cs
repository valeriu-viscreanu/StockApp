using StockApp.Application.DTO;

namespace StockApp.Models
{
    public class Orders
    {
        public List<BuyOrderResponse> BuyOrders { get; set; } = new List<BuyOrderResponse>();
        public List<SellOrderResponse> SellOrders { get; set; } = new List<SellOrderResponse>();
        public List<HoldingResponse> CurrentHoldings { get; set; } = new List<HoldingResponse>();
    }
}
