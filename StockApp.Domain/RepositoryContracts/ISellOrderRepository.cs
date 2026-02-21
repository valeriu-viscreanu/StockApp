using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface ISellOrderRepository
    {
        void Add(SellOrder order);
        List<SellOrder> GetAll();
    }
}
