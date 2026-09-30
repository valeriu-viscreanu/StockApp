using System;
using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface IAccountRepository
    {
        Task<Account?> GetByUserIDAsync(Guid userID);
        Task UpdateAsync(Account account);
    }
}
