using System;
using System.Collections.Generic;
using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface ICashRepository
    {
        void Add(Cash cash);
        void Update(Cash cash);
        void Delete(Guid cashID);
        Cash? GetBySymbol(Guid accountID, string stockSymbol);
        List<Cash> GetByAccountID(Guid accountID);
    }
}
