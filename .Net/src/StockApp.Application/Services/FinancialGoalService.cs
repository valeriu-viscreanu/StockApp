using StockApp.Application.DTO;
using StockApp.Application.Mappers;
using StockApp.Application.ServiceContracts;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Application.Services
{
    public class FinancialGoalService : IFinancialGoalService
    {
        private readonly IFinancialGoalRepository _financialGoalRepository;
        private readonly IFinancialGoalMapper _financialGoalMapper;

        public FinancialGoalService(IFinancialGoalRepository financialGoalRepository, IFinancialGoalMapper financialGoalMapper)
        {
            _financialGoalRepository = financialGoalRepository;
            _financialGoalMapper = financialGoalMapper;
        }

        public async Task<FinancialGoalResponse> CreateGoal(FinancialGoalRequest request, Guid userId)
        {
            var goal = _financialGoalMapper.MapToEntity(request, userId);
            await _financialGoalRepository.AddAsync(goal);
            return _financialGoalMapper.MapToResponse(goal);
        }

        public async Task<List<FinancialGoalResponse>> GetGoalsByUserId(Guid userId)
        {
            var goals = await _financialGoalRepository.GetByUserIDAsync(userId);
            return goals.Select(g => _financialGoalMapper.MapToResponse(g)).ToList();
        }

        public async Task<FinancialGoalResponse?> GetGoalById(Guid goalId)
        {
            var goal = await _financialGoalRepository.GetByIDAsync(goalId);
            if (goal == null) return null;
            return _financialGoalMapper.MapToResponse(goal);
        }

        public async Task<bool> UpdateGoal(Guid goalId, FinancialGoalRequest request)
        {
            var goal = await _financialGoalRepository.GetByIDAsync(goalId);
            if (goal == null) return false;

            goal.Title = request.Title;
            goal.GoalTypeID = request.GoalTypeID;
            goal.TargetAmount = request.TargetAmount;
            goal.InitialAmount = request.InitialAmount;
            goal.MonthlyContribution = request.MonthlyContribution;
            goal.TargetDate = request.TargetDate;

            await _financialGoalRepository.UpdateAsync(goal);
            return true;
        }

        public async Task<bool> DeleteGoal(Guid goalId)
        {
            var goal = await _financialGoalRepository.GetByIDAsync(goalId);
            if (goal == null) return false;

            await _financialGoalRepository.DeleteAsync(goalId);
            return true;
        }

        public async Task<bool> AddContribution(Guid goalId, double amount)
        {
            var goal = await _financialGoalRepository.GetByIDAsync(goalId);
            if (goal == null) return false;

            goal.CurrentAmount += amount;
            if (goal.CurrentAmount >= goal.TargetAmount)
            {
                goal.IsCompleted = true;
            }

            await _financialGoalRepository.UpdateAsync(goal);
            return true;
        }

        public async Task<List<GoalTypeResponse>> GetGoalTypes()
        {
            var goalTypes = await _financialGoalRepository.GetGoalTypesAsync();
            return goalTypes.Select(gt => new GoalTypeResponse
            {
                GoalTypeID = gt.GoalTypeID,
                Name = gt.Name
            }).ToList();
        }
    }
}
