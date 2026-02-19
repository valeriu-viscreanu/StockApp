using StockApp.DTO;
using StockApp.Entities;

namespace StockApp.ServiceContracts
{
    public interface ISellOrderMapper
    {
        SellOrder MapToEntity(SellOrderRequest request);
        SellOrderResponse MapToResponse(SellOrder entity);
    }
}
