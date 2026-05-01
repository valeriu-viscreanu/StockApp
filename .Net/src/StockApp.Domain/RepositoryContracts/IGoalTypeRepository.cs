using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface IGoalTypeRepository
    {
        Task<GoalType?> GetByName(string name);
        List<GoalType> GetAll();
    }
}
