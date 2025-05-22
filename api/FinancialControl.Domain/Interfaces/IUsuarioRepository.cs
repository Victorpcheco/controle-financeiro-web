
using FinancialControl.Domain.Entities;

namespace FinancialControl.Domain.Interfaces
{
    public interface IUsuarioRepository 
    {
        Task<Usuario?> UsuarioExisteAsync(string email);
        Task AdicionarUsuarioAsync(Usuario usuario);
        Task AtualizarRefreshTokenAsync(Usuario usuario, string refreshToken, DateTime expiration);
        Task DeletarAsync(Usuario usuario);
    }
}
