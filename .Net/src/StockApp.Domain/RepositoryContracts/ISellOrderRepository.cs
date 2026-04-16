using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface ISellOrderRepository
    {
        void Add(SellOrder order);
        void Update(SellOrder order);
        List<SellOrder> GetAll();
        List<SellOrder> GetByUserID(Guid userID);
    }
}
