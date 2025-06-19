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
    
    public async Task<IReadOnlyList<Despesa>> ListarDespesasPaginadoPorDataAsync(int usuarioId, DateOnly data, int pagina, int quantidade)
    {
        return await context.Despesas
            .Where(d => d.UsuarioId == usuarioId && d.Data == data)
            .OrderByDescending(d => d.Data)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }
    
    public async Task<int> ContarDespesasPorDataAsync(int usuarioId, DateOnly data)
    {
        return await context.Despesas
            .CountAsync(d => d.UsuarioId == usuarioId && d.Data == data);
    }
    
    public async Task<IReadOnlyList<Despesa>> ListarDespesasPaginadoPorContaAsync(int usuarioId, int contaId, int pagina, int quantidade)
    {
        return await context.Despesas
            .Where(d => d.UsuarioId == usuarioId && d.ContaBancariaId == contaId)
            .OrderByDescending(d => d.Data)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }
    
    public async Task<int> ContarDespesasPorContaAsync(int usuarioId, int contaId)
    {
        return await context.Despesas
            .CountAsync(d => d.UsuarioId == usuarioId && d.ContaBancariaId == contaId);
    }
    
    public async Task<IReadOnlyList<Despesa>> ListarDespesasPaginadoPorCategoriaAsync(int usuarioId, int categoriaId, int pagina, int quantidade)
    {
        return await context.Despesas
            .Where(d => d.UsuarioId == usuarioId && d.CategoriaId == categoriaId)
            .OrderByDescending(d => d.Data)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }
    
    public async Task<int> ContarDespesasPorCategoriaAsync(int usuarioId, int categoriaId)
    {
        return await context.Despesas
            .CountAsync(d => d.UsuarioId == usuarioId && d.CategoriaId == categoriaId);
    }
    
    public async Task<IReadOnlyList<Despesa>> ListarDespesasPaginadoPorTituloAsync(int usuarioId, string titulo, int pagina, int quantidade)
    {
        return await context.Despesas
            .Where(d => d.UsuarioId == usuarioId && d.TituloDespesa.Contains(titulo))
            .OrderByDescending(d => d.Data)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }
    
    public async Task<int> ContarDespesasPorTituloAsync(int usuarioId, string titulo)
    {
        return await context.Despesas
            .CountAsync(d => d.UsuarioId == usuarioId && d.TituloDespesa.Contains(titulo));
    }
    
    public async Task<IReadOnlyList<Despesa>> ListarDespesasPaginadoPorCartaoAsync(int usuarioId, int cartaoId, int pagina, int quantidade)
    {
        return await context.Despesas
            .Where(d => d.UsuarioId == usuarioId && d.CartaoId == cartaoId)
            .OrderByDescending(d => d.Data)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }
    
    public async Task<int> ContarDespesasPorCartaoAsync(int usuarioId, int cartaoId)
    {
        return await context.Despesas
            .CountAsync(d => d.UsuarioId == usuarioId && d.CartaoId == cartaoId);
    }
}