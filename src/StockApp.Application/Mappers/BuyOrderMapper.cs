using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Domain.Entities;

namespace StockApp.Application.Mappers
{
    public class BuyOrderMapper : IBuyOrderMapper
    {
        public BuyOrder MapToEntity(BuyOrderRequest request)
        {
            return new BuyOrder
            {
                BuyOrderID = Guid.NewGuid(),
                StockSymbol = request.StockSymbol,
                StockName = request.StockName,
                DateAndTimeOfOrder = request.DateAndTimeOfOrder,
                Quantity = request.Quantity,
                Price = request.Price
            };
        }

        public BuyOrderResponse MapToResponse(BuyOrder entity)
        {
            return new BuyOrderResponse
            {
                BuyOrderID = entity.BuyOrderID,
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
