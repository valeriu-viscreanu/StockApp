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
            _financialGoalRepository.Add(goal);
            return await Task.FromResult(_financialGoalMapper.MapToResponse(goal));
        }

        public async Task<List<FinancialGoalResponse>> GetGoalsByUserId(Guid userId)
        {
            var goals = _financialGoalRepository.GetByUserID(userId);
            var responses = goals.Select(g => _financialGoalMapper.MapToResponse(g)).ToList();
            return await Task.FromResult(responses);
        }

        public async Task<FinancialGoalResponse?> GetGoalById(Guid goalId)
        {
            var goal = _financialGoalRepository.GetByID(goalId);
            if (goal == null) return null;
            return await Task.FromResult(_financialGoalMapper.MapToResponse(goal));
        }

        public async Task<bool> UpdateGoal(Guid goalId, FinancialGoalRequest request)
        {
            var goal = _financialGoalRepository.GetByID(goalId);
            if (goal == null) return false;

            goal.Title = request.Title;
            goal.GoalTypeID = request.GoalTypeID;
            goal.TargetAmount = request.TargetAmount;
            goal.InitialAmount = request.InitialAmount;
            goal.MonthlyContribution = request.MonthlyContribution;
            goal.TargetDate = request.TargetDate;

            _financialGoalRepository.Update(goal);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteGoal(Guid goalId)
        {
            var goal = _financialGoalRepository.GetByID(goalId);
            if (goal == null) return false;

            _financialGoalRepository.Delete(goalId);
            return await Task.FromResult(true);
        }

        public async Task<bool> AddContribution(Guid goalId, double amount)
        {
            var goal = _financialGoalRepository.GetByID(goalId);
            if (goal == null) return false;

            goal.CurrentAmount += amount;
            if (goal.CurrentAmount >= goal.TargetAmount)
            {
                goal.IsCompleted = true;
            }

            _financialGoalRepository.Update(goal);
            return await Task.FromResult(true);
        }

        public async Task<List<GoalTypeResponse>> GetGoalTypes()
        {
            var goalTypes = _financialGoalRepository.GetGoalTypes();
            return await Task.FromResult(goalTypes.Select(gt => new GoalTypeResponse
            {
                GoalTypeID = gt.GoalTypeID,
                Name = gt.Name
            }).ToList());
        }
    }
}
