using StockApp.Application.DTO;

namespace StockApp.Application.ServiceContracts
{
    public interface IAccountService
    {
        bool Login(LoginRequest loginRequest);
    }
}
