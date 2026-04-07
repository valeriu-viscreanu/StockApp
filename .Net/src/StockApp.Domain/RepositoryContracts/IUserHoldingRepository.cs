using System;
using System.Collections.Generic;
using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface IUserHoldingRepository
    {
        void Add(UserHolding holding);
        void Update(UserHolding holding);
        void Delete(Guid holdingID);
        UserHolding? GetBySymbol(Guid userID, string stockSymbol);
        List<UserHolding> GetByUserID(Guid userID);
    }
}
