using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface IFinancialGoalRepository
    {
        void Add(FinancialGoal goal);
        void Update(FinancialGoal goal);
        void Delete(Guid goalId);
        FinancialGoal? GetByID(Guid goalId);
        List<FinancialGoal> GetByUserID(Guid userId);
        List<GoalType> GetGoalTypes();
    }
}
