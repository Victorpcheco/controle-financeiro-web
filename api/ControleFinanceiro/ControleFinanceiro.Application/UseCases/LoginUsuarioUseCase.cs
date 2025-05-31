using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases;

public class LoginUsuarioUseCase(IValidator<LoginRequest> loginValidator,
    IUsuarioRepository repository,
    IGerarTokenUseCase geraToken,
    IGerarRefreshTokenUseCase geraRefreshToken
    ) : ILoginUsuarioUseCase
{
    private const int RefreshTokenExpirationDays = 1;
    
    public async Task<TokenResponse> LoginUsuarioAsync(LoginRequest request)
    {

        var resultValidation = await loginValidator.ValidateAsync(request);
        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);
            
        var usuario = await repository.BuscarUsuarioPorEmailAsync(request.Email);

        if (usuario == null)
            throw new ArgumentException("Usuário não encontrado.");

        var token = geraToken.GerarToken(usuario);
        var refreshToken = geraRefreshToken.GerarRefreshToken();
        var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);

        await repository.AtualizarRefreshTokenAsync(usuario, refreshToken, expiration);
        return new TokenResponse() { Token = token, RefreshToken = refreshToken };
    }
}