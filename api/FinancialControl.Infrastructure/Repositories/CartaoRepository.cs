using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FinancialControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.Repositories;

public class CartaoRepository(ApplicationDbContext context) : ICartaoRepository
{
    public async Task<IReadOnlyList<Cartao>> ListarCartaoPaginadoAsync(int usuarioId, int pagina, int quantidade)
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
    
    public async Task<Cartao?> BuscarCartaoPorId(int id)
    {
       return await context.Cartoes
           .AsNoTracking()
           .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task AdicionarCartaoAsync(Cartao cartao)
    {
        await context.Cartoes.AddAsync(cartao);
        await context.SaveChangesAsync();
    }
    
    public async Task AtualizarCartaoAsync(Cartao cartao)
    {
        context.Cartoes.Update(cartao);
        await context.SaveChangesAsync();
    }
    
    public async Task DeletarCartaoAsync(Cartao cartao)
    {
        context.Cartoes.Remove(cartao);
        await context.SaveChangesAsync();
    }
    
}