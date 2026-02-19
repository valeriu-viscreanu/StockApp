using StockApp.Entities;
using StockApp.ServiceContracts;

namespace StockApp.Repositories
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
