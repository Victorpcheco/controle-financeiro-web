using FinancialControl.Application.DTOs;

namespace FinancialControl.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<TokenResponseDto> RegistrarUsuarioAsync(UsuarioRegistroDto dto);
        Task<TokenResponseDto> LoginUsuarioAsync(UsuarioLoginDto dto);
    }
}
