using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces
{
    public interface IJwtTokenService
    {
        public string GenerateToken(User user);
        public string GenerateRefreshToken();
    }
}
