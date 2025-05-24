using FinancialControl.Domain.Entities;

namespace FinancialControl.Domain.Interfaces
{
    public interface IUsuarioRepository 
    {
        Task<Usuario?> ObterUsuarioAsync(string email);
        Task CriarUsuarioAsync(Usuario usuario);
        Task AtualizarRefreshTokenAsync(Usuario usuario, string refreshToken, DateTime expiration);
        Task DeletarUsuarioAsync(Usuario usuario);
    }
}
