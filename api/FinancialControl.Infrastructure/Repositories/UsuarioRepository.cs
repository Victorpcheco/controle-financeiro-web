using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FinancialControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.Repositories
{
    public class UsuarioRepository(ApplicationDbContext context) : IUsuarioRepository
    {
        public async Task<Usuario?> BuscarUsuarioPorEmailAsync(string email)
        {
            return await context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CriarUsuarioAsync(Usuario usuario)
        {
            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();
        }
        
        public async Task AtualizarRefreshTokenAsync(Usuario usuario, string refreshToken, DateTime expiration)
        {
            await context.Usuarios
                .Where(u => u.Email == usuario.Email)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.RefreshToken, refreshToken)
                    .SetProperty(x => x.RefreshTokenExpiracao, expiration));
            await context.SaveChangesAsync();
        }
        
        public async Task DeletarUsuarioAsync(Usuario usuario)
        {
            context.Usuarios.Remove(usuario);
            await context.SaveChangesAsync();
        }
    }
}
