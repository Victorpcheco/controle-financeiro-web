
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces
{
    public interface IUserService
    {
        public string GenerateToken(User user);
        public string GenerateRefreshToken();
    }
}
