using StockApp.Application.DTO;
using StockApp.Domain.Entities;

namespace StockApp.Application.ServiceContracts
{
    public interface ISellOrderMapper
    {
        SellOrder MapToEntity(SellOrderRequest request);
        SellOrderResponse MapToResponse(SellOrder entity);
    }
}
