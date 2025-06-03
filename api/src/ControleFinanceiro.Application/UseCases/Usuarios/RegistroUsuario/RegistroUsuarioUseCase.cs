using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.UseCases.Usuarios.RegistroUsuario;

public class RegistroUsuarioUseCase(
    IUsuarioRepository repository,
    IValidator<RegisterRequestDto> registerValidator,
    IGerarRefreshToken refreshToken,
    IGerarToken token ) : IRegistroUsuarioUseCase
{
    
    private const int RefreshTokenExpirationDays = 1;
    
    public async Task<TokenResponseDto> ExecuteAsync(RegisterRequestDto requestDto)
    {
        var resultValidation = await registerValidator.ValidateAsync(requestDto);
        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var usuarioExiste = await repository.BuscarUsuarioPorEmailAsync(requestDto.Email);
        if (usuarioExiste != null)
            throw new ArgumentException("Usuário já cadastrado no sistema.");

        var refresh = refreshToken.GeraRefreshToken();
        var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);
            
        var usuario = new Usuario(requestDto.NomeCompleto,requestDto.Email,requestDto.SenhaHash,refresh,expiration);
        await repository.CriarUsuarioAsync(usuario);

        var jwtToken = token.GeraToken(usuario);

        return new  TokenResponseDto() { Token = jwtToken, RefreshToken = refresh };
    }
}