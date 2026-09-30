using System;
using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface IUserDetailsRepository
    {
        Task AddAsync(UserDetails userDetails);
        Task UpdateAsync(UserDetails userDetails);
        Task<UserDetails?> GetByUserIDAsync(Guid userID);
    }
}
