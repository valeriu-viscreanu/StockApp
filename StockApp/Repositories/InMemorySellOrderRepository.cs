using StockApp.Entities;
using StockApp.ServiceContracts;

namespace StockApp.Repositories
{
    public class InMemorySellOrderRepository : ISellOrderRepository
    {
        private readonly List<SellOrder> _orders = new();

        public void Add(SellOrder order)
        {
            _orders.Add(order);
        }

        public List<SellOrder> GetAll()
        {
            return _orders.ToList();
        }
    }
}
