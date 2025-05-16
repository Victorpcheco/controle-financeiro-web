using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FinancialControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> UserExistsAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
             
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRefreshTokenAsync(User user, string refreshToken, DateTime expiration)
        {
            await _context.Users
                .Where(u => u.Email == user.Email)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.RefreshToken, refreshToken)
                    .SetProperty(x => x.RefreshTokenExpiracao, expiration));
            await _context.SaveChangesAsync();
        }

    }
}
