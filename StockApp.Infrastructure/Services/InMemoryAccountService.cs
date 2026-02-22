using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;

namespace StockApp.Infrastructure.Services
{
    public class InMemoryAccountService : IAccountService
    {
        public bool Login(LoginRequest loginRequest)
        {
            if (loginRequest.Email == "admin@test.com" && loginRequest.Password == "admin123")
            {
                return true;
            }
            return false;
        }
    }
}
