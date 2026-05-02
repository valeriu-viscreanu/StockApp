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

        public void Add(FinancialGoal goal)
        {
            _db.FinancialGoals.Add(goal);
            _db.SaveChanges();
        }

        public void Update(FinancialGoal goal)
        {
            _db.FinancialGoals.Update(goal);
            _db.SaveChanges();
        }

        public void Delete(Guid goalId)
        {
            var goal = _db.FinancialGoals.Find(goalId);
            if (goal != null)
            {
                _db.FinancialGoals.Remove(goal);
                _db.SaveChanges();
            }
        }

        public FinancialGoal? GetByID(Guid goalId)
        {
            return _db.FinancialGoals
                .Include(g => g.GoalType)
                .FirstOrDefault(g => g.FinancialGoalID == goalId);
        }

        public List<FinancialGoal> GetByUserID(Guid userId)
        {
            return _db.FinancialGoals
                .Include(g => g.GoalType)
                .Where(g => g.UserID == userId)
                .ToList();
        }

        public List<GoalType> GetGoalTypes()
        {
            return _db.GoalTypes.ToList();
        }
    }
}
