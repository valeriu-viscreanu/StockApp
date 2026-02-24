using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Infrastructure.Repositories
{
    public class InMemoryBuyOrderRepository : IBuyOrderRepository
    {
        private readonly List<BuyOrder> _orders = new();

        public void Add(BuyOrder order)
        {
            _orders.Add(order);
        }

        public List<BuyOrder> GetAll()
        {
            return _orders.ToList();
        }
    }
}
