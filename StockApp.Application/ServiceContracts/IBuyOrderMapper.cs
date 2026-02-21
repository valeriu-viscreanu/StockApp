using StockApp.Application.DTO;
using StockApp.Domain.Entities;

namespace StockApp.Application.ServiceContracts
{
    public interface IBuyOrderMapper
    {
        BuyOrder MapToEntity(BuyOrderRequest request);
        BuyOrderResponse MapToResponse(BuyOrder entity);
    }
}
