using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Domain.Models;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Repositories;

public class CartaoRepository(ApplicationDbContext context) : ICartaoRepository
{
    public async Task<IReadOnlyList<CartaoCredito>> ListarCartaoPaginadoAsync(int usuarioId, int pagina, int quantidade)
    {
        return await context.Cartoes
            .Where(c => c.UsuarioId == usuarioId)
            .OrderBy(c => c.Id)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }
    
    public async Task<int> ContarTotalAsync(int usuarioId)
    {
        return await context.Cartoes
            .AsNoTracking()
            .CountAsync(c => c.UsuarioId == usuarioId);
    }
    
    public async Task<CartaoCredito?> BuscarCartaoPorId(int id)
    {
        return await context.Cartoes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task AdicionarCartaoAsync(CartaoCredito CartaoCredito)
    {
        await context.Cartoes.AddAsync(CartaoCredito);
        await context.SaveChangesAsync();
    }
    
    public async Task AtualizarCartaoAsync(CartaoCredito CartaoCredito)
    {
        context.Cartoes.Update(CartaoCredito);
        await context.SaveChangesAsync();
    }
    
    public async Task DeletarCartaoAsync(CartaoCredito CartaoCredito)
    {
        context.Cartoes.Remove(CartaoCredito);
        await context.SaveChangesAsync();
    }
    
}