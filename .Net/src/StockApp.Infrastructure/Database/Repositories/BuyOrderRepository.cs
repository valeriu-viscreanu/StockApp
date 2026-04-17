using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace StockApp.Infrastructure.Database.Repositories;

public class BuyOrderRepository : IBuyOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BuyOrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(BuyOrder order)
    {
        order.BuyOrderID = Guid.NewGuid();
        _dbContext.BuyOrders.Add(order);
        _dbContext.SaveChanges();
    }

    public void Update(BuyOrder order)
    {
        _dbContext.BuyOrders.Update(order);
        _dbContext.SaveChanges();
    }

    public List<BuyOrder> GetAll()
    {
        return _dbContext.BuyOrders.Include(o => o.OrderStatus).ToList();
    }

    public List<BuyOrder> GetByUserID(Guid userID)
    {
        return _dbContext.BuyOrders
            .Include(o => o.OrderStatus)
            .Where(bo => bo.UserID == userID)
            .ToList();
    }
}
