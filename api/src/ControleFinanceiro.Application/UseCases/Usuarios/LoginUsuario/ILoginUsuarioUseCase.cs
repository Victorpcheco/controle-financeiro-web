using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Usuarios.LoginUsuario;

public interface ILoginUsuarioUseCase
{
    Task<TokenResponseDto> ExecuteAsync(LoginRequestDto requestDto);
}