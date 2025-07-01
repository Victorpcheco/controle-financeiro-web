using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Repositories;

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
        
    public async Task DeletarUsuarioComDependenciasAsync(int usuarioId)
    {
        var categorias = context.Categorias.Where(c => c.UsuarioId == usuarioId);
        var contas = context.ContasBancarias.Where(c => c.UsuarioId == usuarioId);
        var cartoes = context.Cartoes.Where(c => c.UsuarioId == usuarioId);
        var despesas = context.Movimentacoes.Where(d => d.UsuarioId == usuarioId);
        var meses = context.MesesReferencia.Where(m => m.UsuarioId == usuarioId);
        var planejamentos = context.PlanejamentosCategorias.Where(p => p.UsuarioId == usuarioId);

        context.Categorias.RemoveRange(categorias);
        context.ContasBancarias.RemoveRange(contas);
        context.Cartoes.RemoveRange(cartoes);
        context.Movimentacoes.RemoveRange(despesas);
        context.MesesReferencia.RemoveRange(meses);
        context.PlanejamentosCategorias.RemoveRange(planejamentos);

        var usuario = await context.Usuarios.FindAsync(usuarioId);
        if (usuario != null)
        {
            context.Usuarios.Remove(usuario);
        }

        await context.SaveChangesAsync();
    }
}