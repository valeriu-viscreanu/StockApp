using StockApp.Application.DTO;

namespace StockApp.Application.ServiceContracts
{
    public interface IFinancialGoalService
    {
        Task<FinancialGoalResponse> CreateGoal(FinancialGoalRequest request, Guid userId);
        Task<List<FinancialGoalResponse>> GetGoalsByUserId(Guid userId);
        Task<FinancialGoalResponse?> GetGoalById(Guid goalId);
        Task<bool> UpdateGoal(Guid goalId, FinancialGoalRequest request);
        Task<bool> DeleteGoal(Guid goalId);
        Task<bool> AddContribution(Guid goalId, double amount);
        Task<List<GoalTypeResponse>> GetGoalTypes();
    }
}
