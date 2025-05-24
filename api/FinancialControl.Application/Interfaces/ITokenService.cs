using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces
{
    public interface ITokenService
    {
        public string GerarToken(Usuario usuario);
        public string GerarRefreshToken();
    }
}
