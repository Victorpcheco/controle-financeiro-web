using ControleFinanceiro.Domain.Models;

namespace ControleFinanceiro.Domain.Interfaces;

public interface IUsuarioRepository 
{
    Task<Usuario?> BuscarUsuarioPorEmailAsync(string email);
    Task CriarUsuarioAsync(Usuario usuario);
    Task AtualizarRefreshTokenAsync(Usuario usuario, string refreshToken, DateTime expiration);
    Task DeletarUsuarioComDependenciasAsync(int usuarioId);
}