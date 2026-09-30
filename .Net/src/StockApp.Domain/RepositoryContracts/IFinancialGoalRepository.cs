using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface IFinancialGoalRepository
    {
        Task AddAsync(FinancialGoal goal);
        Task UpdateAsync(FinancialGoal goal);
        Task DeleteAsync(Guid goalId);
        Task<FinancialGoal?> GetByIDAsync(Guid goalId);
        Task<List<FinancialGoal>> GetByUserIDAsync(Guid userId);
        Task<List<GoalType>> GetGoalTypesAsync();
    }
}
