using Microsoft.EntityFrameworkCore;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Infrastructure.Database;

namespace StockApp.Infrastructure.Database.Services
{
    public class AdvisorService : IAdvisorService
    {
        private readonly ApplicationDbContext _db;

        public AdvisorService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<AdvisorClientDto>> GetClientsForAdvisorAsync(Guid advisorId)
        {
            return await _db.Users
                .Where(u => u.AdvisorID == advisorId)
                .Select(u => new AdvisorClientDto
                {
                    ClientID = u.UserID,
                    Email = u.Email
                })
                .ToListAsync();
        }

        public async Task<bool> AssignClientAsync(Guid advisorId, Guid clientId)
        {
            var client = await _db.Users.FindAsync(clientId);
            if (client == null) return false;

            client.AdvisorID = advisorId;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnassignClientAsync(Guid advisorId, Guid clientId)
        {
            var client = await _db.Users.FindAsync(clientId);
            if (client == null || client.AdvisorID != advisorId) return false;

            client.AdvisorID = null;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
