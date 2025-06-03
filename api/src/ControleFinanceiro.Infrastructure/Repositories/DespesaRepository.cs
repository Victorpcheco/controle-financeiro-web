using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Repositories;

public class DespesaRepository(ApplicationDbContext context) : IDespesaRepository
{
    public async Task<IReadOnlyList<Despesa>> ListarDespesasPaginadoAsync(int usuarioId, int pagina, int quantidade)
    {
        return await context.Despesas
            .Where(d => d.UsuarioId == usuarioId)
            .OrderByDescending(d => d.Data)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }
    
    public async Task<int> ContarDespesasAsync(int usuarioId)
    {
        return await context.Despesas
            .CountAsync(d => d.UsuarioId == usuarioId);
    }
    
    public async Task<Despesa?> BuscarDespesaPorIdAsync(int id)
    {
        return await context.Despesas
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task AdicionarDespesaAsync(Despesa despesa)
    {
        await context.Despesas.AddAsync(despesa);
        await context.SaveChangesAsync();
    }
    
    public async Task AtualizarDespesaAsync(Despesa despesa)
    {
        context.Despesas.Update(despesa);
        await context.SaveChangesAsync();
    }
    
    public async Task RemoverDespesaAsync(Despesa despesa)
    {
        context.Despesas.Remove(despesa);
        await context.SaveChangesAsync();
    }
}