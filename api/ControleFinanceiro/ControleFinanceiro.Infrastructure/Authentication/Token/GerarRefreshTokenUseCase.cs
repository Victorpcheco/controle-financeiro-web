using System.Security.Cryptography;

namespace ControleFinanceiro.Infrastructure.Authentication.Token;

public class GerarRefreshTokenUseCase
{
    // Gera um refresh token seguro usando um gerador de números aleatórios criptograficamente seguro
    public string GerarRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }
}