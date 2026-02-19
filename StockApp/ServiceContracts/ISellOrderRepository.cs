using StockApp.Entities;

namespace StockApp.ServiceContracts
{
    public interface ISellOrderRepository
    {
        void Add(SellOrder order);
        List<SellOrder> GetAll();
    }
}
