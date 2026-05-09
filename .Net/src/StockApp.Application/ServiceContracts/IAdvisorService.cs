using StockApp.Application.DTO;

namespace StockApp.Application.ServiceContracts
{
    public interface IAdvisorService
    {
        Task<IEnumerable<AdvisorClientDto>> GetClientsForAdvisorAsync(Guid advisorId);
        Task<bool> AssignClientAsync(Guid advisorId, Guid clientId);
        Task<bool> UnassignClientAsync(Guid advisorId, Guid clientId);
    }
}
