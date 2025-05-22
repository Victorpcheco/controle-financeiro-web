using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FinancialControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;
        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> UsuarioExisteAsync(string email)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
             
        }

        public async Task AdicionarUsuarioAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarRefreshTokenAsync(Usuario usuario, string refreshToken, DateTime expiration)
        {
            await _context.Usuarios
                .Where(u => u.Email == usuario.Email)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.RefreshToken, refreshToken)
                    .SetProperty(x => x.RefreshTokenExpiracao, expiration));
            await _context.SaveChangesAsync();
        }
        
        public async Task DeletarAsync(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }

    }
}
