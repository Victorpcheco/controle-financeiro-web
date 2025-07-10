using System.Security.Cryptography;
using ControleFinanceiro.Application.Interfaces;

namespace ControleFinanceiro.Infrastructure.Authentication.Token;

public class GerarRefreshToken : IGerarRefreshToken
{
    // Gera um refresh token seguro usando um gerador de números aleatórios criptograficamente seguro
    public string GeraRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }
}