using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

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

    public void Update(SellOrder order)
    {
        _dbContext.SellOrders.Update(order);
        _dbContext.SaveChanges();
    }

    public List<SellOrder> GetAll()
    {
        return _dbContext.SellOrders.Include(o => o.OrderStatus).ToList();
    }

    public List<SellOrder> GetByUserID(Guid userID)
    {
        return _dbContext.SellOrders
            .Include(o => o.OrderStatus)
            .Where(so => so.UserID == userID)
            .ToList();
    }
}
