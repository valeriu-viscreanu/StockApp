using System;
using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface IAccountRepository
    {
        Account? GetByUserID(Guid userID);
        void Update(Account account);
    }
}
