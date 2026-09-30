using Microsoft.EntityFrameworkCore;
using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using StockApp.Infrastructure.Database;

namespace StockApp.Infrastructure.Database.Repositories
{
    public class FinancialGoalRepository : IFinancialGoalRepository
    {
        private readonly ApplicationDbContext _db;

        public FinancialGoalRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(FinancialGoal goal)
        {
            _db.FinancialGoals.Add(goal);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(FinancialGoal goal)
        {
            _db.FinancialGoals.Update(goal);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid goalId)
        {
            var goal = await _db.FinancialGoals.FindAsync(goalId);
            if (goal != null)
            {
                _db.FinancialGoals.Remove(goal);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<FinancialGoal?> GetByIDAsync(Guid goalId)
        {
            return await _db.FinancialGoals
                .AsNoTracking()
                .Include(g => g.GoalType)
                .FirstOrDefaultAsync(g => g.FinancialGoalID == goalId);
        }

        public async Task<List<FinancialGoal>> GetByUserIDAsync(Guid userId)
        {
            return await _db.FinancialGoals
                .AsNoTracking()
                .Include(g => g.GoalType)
                .Where(g => g.UserID == userId)
                .ToListAsync();
        }

        public async Task<List<GoalType>> GetGoalTypesAsync()
        {
            return await _db.GoalTypes.AsNoTracking().ToListAsync();
        }
    }
}
