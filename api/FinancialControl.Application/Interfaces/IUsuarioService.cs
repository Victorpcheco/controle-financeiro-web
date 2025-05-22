
using FinancialControl.Application.DTOs;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<TokenResponseDto> RegistrarUsuarioAsync(UsuarioRegistroDto dto);
        Task<TokenResponseDto> LoginUsuarioAsync(UsuarioLoginDto dto);
        Task<bool> RemoverUsuarioAsync(string email);
    }
}
