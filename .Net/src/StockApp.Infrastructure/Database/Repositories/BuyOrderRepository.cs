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

    public async Task AddAsync(BuyOrder order)
    {
        order.BuyOrderID = Guid.NewGuid();
        _dbContext.BuyOrders.Add(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(BuyOrder order)
    {
        _dbContext.BuyOrders.Update(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<BuyOrder>> GetAllAsync()
    {
        return await _dbContext.BuyOrders.AsNoTracking().Include(o => o.OrderStatus).ToListAsync();
    }

    public async Task<List<BuyOrder>> GetByUserIDAsync(Guid userID)
    {
        return await _dbContext.BuyOrders
            .AsNoTracking()
            .Include(o => o.OrderStatus)
            .Where(bo => bo.UserID == userID)
            .ToListAsync();
    }
}
