using StockApp.DTO;
using StockApp.Entities;
using StockApp.ServiceContracts;

namespace StockApp.Mappers
{
    public class SellOrderMapper : ISellOrderMapper
    {
        public SellOrder MapToEntity(SellOrderRequest request)
        {
            return new SellOrder
            {
                SellOrderID = Guid.NewGuid(),
                StockSymbol = request.StockSymbol,
                StockName = request.StockName,
                DateAndTimeOfOrder = request.DateAndTimeOfOrder,
                Quantity = request.Quantity,
                Price = request.Price
            };
        }

        public SellOrderResponse MapToResponse(SellOrder entity)
        {
            return new SellOrderResponse
            {
                SellOrderID = entity.SellOrderID,
                StockSymbol = entity.StockSymbol,
                StockName = entity.StockName,
                DateAndTimeOfOrder = entity.DateAndTimeOfOrder,
                Quantity = entity.Quantity,
                Price = entity.Price,
                TradeAmount = entity.Quantity * entity.Price
            };
        }
    }
}
