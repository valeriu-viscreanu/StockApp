using StockApp.Application.DTO;

namespace StockApp.Application.ServiceContracts
{
    public interface IAccountService
    {
        LoginResponse Login(LoginRequest loginRequest);
        RegisterResponse Register(RegisterRequest registerRequest);
    }
}
