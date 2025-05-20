using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(Usuario usuario);
        public string GenerateRefreshToken();
    }
}
