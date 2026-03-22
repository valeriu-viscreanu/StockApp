using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Infrastructure.Database.Repositories;

public class SellOrderRepository : ISellOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SellOrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(SellOrder order)
    {
        order.SellOrderID = Guid.NewGuid();
        _dbContext.SellOrders.Add(order);
        _dbContext.SaveChanges();
    }

    public List<SellOrder> GetAll()
    {
        return _dbContext.SellOrders.ToList();
    }

    public List<SellOrder> GetByUserID(Guid userID)
    {
        return _dbContext.SellOrders.Where(so => so.UserID == userID).ToList();
    }
}
