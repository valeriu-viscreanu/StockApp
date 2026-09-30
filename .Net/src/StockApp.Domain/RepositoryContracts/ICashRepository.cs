using System;
using System.Collections.Generic;
using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface ICashRepository
    {
        Task AddAsync(Cash cash);
        Task UpdateAsync(Cash cash);
        Task DeleteAsync(Guid cashID);
        Task<Cash?> GetBySymbolAsync(Guid accountID, string stockSymbol);
        Task<List<Cash>> GetByAccountIDAsync(Guid accountID);
    }
}
