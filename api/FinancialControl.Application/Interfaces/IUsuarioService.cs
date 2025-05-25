using FinancialControl.Application.DTOs;

namespace FinancialControl.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<TokenResponseDto> RegistrarUsuarioAsync(UsuarioRequestDto dto);
        Task<TokenResponseDto> LoginUsuarioAsync(UsuarioLoginDto dto);
    }
}
