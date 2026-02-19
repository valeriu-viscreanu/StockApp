using StockApp.DTO;
using StockApp.Entities;

namespace StockApp.ServiceContracts
{
    public interface IBuyOrderMapper
    {
        BuyOrder MapToEntity(BuyOrderRequest request);
        BuyOrderResponse MapToResponse(BuyOrder entity);
    }
}
