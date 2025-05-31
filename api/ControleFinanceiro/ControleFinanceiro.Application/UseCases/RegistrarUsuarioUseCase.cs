using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;
using ControleFinanceiro.Domain.Models;

namespace ControleFinanceiro.Application.UseCases;

public class RegistrarUsuarioUseCase(
    IUsuarioRepository repository,
    IValidator<RegisterRequest> registerValidator,
    IGerarRefreshTokenUseCase gerarRefreshToken,
    IGerarTokenUseCase gerarToken ) : IRegistrarUsuarioUseCase
{
    
    private const int RefreshTokenExpirationDays = 1;
    
    public async Task<TokenResponse> RegistrarUsuarioAsync(RegisterRequest request)
    {
        var resultValidation = await registerValidator.ValidateAsync(request);
        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var usuarioExiste = await repository.BuscarUsuarioPorEmailAsync(request.Email);
        if (usuarioExiste != null)
            throw new ArgumentException("Usuário já cadastrado no sistema.");

        var refreshToken = gerarRefreshToken.GerarRefreshToken();
        var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);
            
        var usuario = new Usuario(request.NomeCompleto,request.Email,request.SenhaHash,refreshToken,expiration);
        await repository.CriarUsuarioAsync(usuario);

        var jwtToken = gerarToken.GerarToken(usuario);

        return new  TokenResponse() { Token = jwtToken, RefreshToken = refreshToken };
    }
}