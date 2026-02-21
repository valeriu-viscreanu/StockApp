using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface IBuyOrderRepository
    {
        void Add(BuyOrder order);
        List<BuyOrder> GetAll();
    }
}
