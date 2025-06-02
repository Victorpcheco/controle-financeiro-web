using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces;
using ControleFinanceiro.Application.Interfaces.Token;
using ControleFinanceiro.Application.Interfaces.Usuarios;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Usuarios;

public class LoginUsuario(IValidator<LoginRequest> loginValidator,
    IUsuarioRepository repository,
    IGerarToken token,
    IGerarRefreshToken refreshToken
    ) : ILoginUsuario
{
    private const int RefreshTokenExpirationDays = 1;
    
    public async Task<TokenResponse> Login(LoginRequest request)
    {

        var resultValidation = await loginValidator.ValidateAsync(request);
        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);
            
        var usuario = await repository.BuscarUsuarioPorEmailAsync(request.Email);

        if (usuario == null)
            throw new ArgumentException("Usuário não encontrado.");

        var jwtToken = token.GeraToken(usuario);
        var refresh = refreshToken.GeraRefreshToken();
        var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);

        await repository.AtualizarRefreshTokenAsync(usuario, refresh, expiration);
        return new TokenResponse() { Token = jwtToken, RefreshToken = refresh };
    }
}