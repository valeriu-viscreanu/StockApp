using System;
using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface IUserDetailsRepository
    {
        void Add(UserDetails userDetails);
        void Update(UserDetails userDetails);
        UserDetails? GetByUserID(Guid userID);
    }
}
