
using FinancialControl.Application.Interfaces;

namespace FinancialControl.Infrastructure.Authentication
{
    public class JwtTokenService : IJwtTokenService
    {

        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly int _tokenExpirationInMinutes;











    }
}
