using System;

namespace StockApp.Application.ServiceContracts
{
    public interface IAccountProfileService
    {
        double GetBalance(Guid userID);
        bool DeductBalance(Guid userID, double amount);
        void AddBalance(Guid userID, double amount);
        DateTime? GetDateOfBirth(Guid userID);
    }
}
