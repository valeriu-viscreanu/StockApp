using StockApp.Application.DTO;
using StockApp.Domain.Entities;

namespace StockApp.Application.Mappers
{
    public interface IFinancialGoalMapper
    {
        FinancialGoal MapToEntity(FinancialGoalRequest request, Guid userId);
        FinancialGoalResponse MapToResponse(FinancialGoal goal);
    }

    public class FinancialGoalMapper : IFinancialGoalMapper
    {
        public FinancialGoal MapToEntity(FinancialGoalRequest request, Guid userId)
        {
            return new FinancialGoal
            {
                FinancialGoalID = Guid.NewGuid(),
                UserID = userId,
                GoalTypeID = request.GoalTypeID,
                Title = request.Title,
                TargetAmount = request.TargetAmount,
                InitialAmount = request.InitialAmount,
                MonthlyContribution = request.MonthlyContribution,
                CurrentAmount = request.InitialAmount, // Start with initial amount
                CreatedDate = DateTime.UtcNow,
                TargetDate = request.TargetDate,
                IsCompleted = false
            };
        }

        public FinancialGoalResponse MapToResponse(FinancialGoal goal)
        {
            return new FinancialGoalResponse
            {
                FinancialGoalID = goal.FinancialGoalID,
                UserID = goal.UserID,
                GoalTypeID = goal.GoalTypeID,
                GoalTypeName = goal.GoalType?.Name ?? string.Empty,
                Title = goal.Title,
                TargetAmount = goal.TargetAmount,
                InitialAmount = goal.InitialAmount,
                MonthlyContribution = goal.MonthlyContribution,
                CurrentAmount = goal.CurrentAmount,
                CreatedDate = goal.CreatedDate,
                TargetDate = goal.TargetDate,
                IsCompleted = goal.IsCompleted
            };
        }
    }
}
