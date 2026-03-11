namespace StockApp.Application.ServiceContracts
{
    public interface IUserBalanceService
    {
        double GetBalance(Guid userID);
        bool DeductBalance(Guid userID, double amount);
        void AddBalance(Guid userID, double amount);
    }
}
