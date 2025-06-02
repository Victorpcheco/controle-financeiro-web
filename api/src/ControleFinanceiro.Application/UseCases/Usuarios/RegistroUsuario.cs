using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces;
using ControleFinanceiro.Application.Interfaces.Token;
using ControleFinanceiro.Application.Interfaces.Usuarios;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;
using ControleFinanceiro.Domain.Models;

namespace ControleFinanceiro.Application.UseCases.Usuarios;

public class RegistroUsuario(
    IUsuarioRepository repository,
    IValidator<RegisterRequest> registerValidator,
    IGerarRefreshToken refreshToken,
    IGerarToken token ) : IRegistroUsuario
{
    
    private const int RefreshTokenExpirationDays = 1;
    
    public async Task<TokenResponse> RegistrarUsuario(RegisterRequest request)
    {
        var resultValidation = await registerValidator.ValidateAsync(request);
        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var usuarioExiste = await repository.BuscarUsuarioPorEmailAsync(request.Email);
        if (usuarioExiste != null)
            throw new ArgumentException("Usuário já cadastrado no sistema.");

        var refresh = refreshToken.GeraRefreshToken();
        var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);
            
        var usuario = new Usuario(request.NomeCompleto,request.Email,request.SenhaHash,refresh,expiration);
        await repository.CriarUsuarioAsync(usuario);

        var jwtToken = token.GeraToken(usuario);

        return new  TokenResponse() { Token = jwtToken, RefreshToken = refresh };
    }
}