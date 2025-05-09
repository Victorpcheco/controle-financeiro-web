
using FinancialControl.Domain.Entities;

namespace FinancialControl.Domain.Interfaces
{
    public interface IUserRepository 
    {
        Task<User?> UserExistsAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateRefreshTokenAsync(User user, string refreshToken, DateTime expiration);
    }
}
