using StockApp.Entities;

namespace StockApp.ServiceContracts
{
    public interface IBuyOrderRepository
    {
        void Add(BuyOrder order);
        List<BuyOrder> GetAll();
    }
}
