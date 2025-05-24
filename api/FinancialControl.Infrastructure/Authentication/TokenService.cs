using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinancialControl.Infrastructure.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly string? _secretKey;
        private readonly string? _issuer;
        private readonly string? _audience;
        private readonly int _tokenExpirationInMinutes;
        
        public TokenService(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            _secretKey = jwtSettings["SecretKey"] ?? throw new ArgumentNullException("SecretKey não configurada.");
            _issuer = jwtSettings["Issuer"] ?? throw new ArgumentNullException("Issuer não configurado.");
            _audience = jwtSettings["Audience"] ?? throw new ArgumentNullException("Audience não configurado.");
            if (!int.TryParse(jwtSettings["TokenExpirationInMinutes"], out _tokenExpirationInMinutes))
                throw new ArgumentException("TokenExpirationInMinutes inválido ou não configurado.");
        }

        public string GerarToken(Usuario usuario)
        {
            // Busca as inormações do usuário no banco de dados
            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NomeCompleto),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            // Cria uma chave de segurança simétrica a partir da chave secreta (_secretKey).
            // Essa chave será usada para assinar e validar o token JWT.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            // Cria as credenciais de assinatura usando a chave de segurança criada acima.
            // O algoritmo de assinatura utilizado é o HMAC com SHA-256, que garante a integridade e autenticidade do token.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Cria o token 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                NotBefore = DateTime.Now,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_tokenExpirationInMinutes),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        // Gera um refresh token seguro usando um gerador de números aleatórios criptograficamente seguro.
        // O token é retornado como uma string Base64.
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
}
