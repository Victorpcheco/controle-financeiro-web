using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Usuarios.RegistroUsuario;

public interface IRegistroUsuarioUseCase
{
    Task<TokenResponseDto> ExecuteAsync(RegisterRequestDto registerRequestDto);
}