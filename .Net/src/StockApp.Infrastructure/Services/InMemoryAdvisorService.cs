using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using System.Collections.Concurrent;

namespace StockApp.Infrastructure.Services
{
    public class InMemoryAdvisorService : IAdvisorService
    {
        // Simple in-memory mapping of ClientID -> AdvisorID
        // In a real in-memory provider with DBContext this might not be needed, 
        // but for a stub service like the account one, we can maintain it here.
        private static readonly ConcurrentDictionary<Guid, Guid?> _assignments = new();

        public Task<IEnumerable<AdvisorClientDto>> GetClientsForAdvisorAsync(Guid advisorId)
        {
            // Note: Since this is an in-memory mock and we don't have all users here, 
            // we return a skeleton list for demonstration if there's no data.
            var clients = _assignments
                .Where(kvp => kvp.Value == advisorId)
                .Select(kvp => new AdvisorClientDto
                {
                    ClientID = kvp.Key,
                    Email = $"client-{kvp.Key.ToString()[..8]}@test.com"
                });

            return Task.FromResult(clients);
        }

        public Task<bool> AssignClientAsync(Guid advisorId, Guid clientId)
        {
            _assignments[clientId] = advisorId;
            return Task.FromResult(true);
        }

        public Task<bool> UnassignClientAsync(Guid advisorId, Guid clientId)
        {
            if (_assignments.TryGetValue(clientId, out var existingAdvisor) && existingAdvisor == advisorId)
            {
                _assignments.TryRemove(clientId, out _);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
