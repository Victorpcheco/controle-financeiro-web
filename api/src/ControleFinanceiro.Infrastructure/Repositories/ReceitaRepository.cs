using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Repositories;

public class ReceitaRepository(ApplicationDbContext context) : IReceitaRepository
{
    public async Task<IReadOnlyList<Receita>> ListarReceitasPaginadoAsync(int usuarioId, int pagina, int quantidade)
    {
        return await context.Receitas
            .Where(r => r.UsuarioId == usuarioId)
            .OrderByDescending(r => r.Data)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }

    public async Task<int> ContarReceitasAsync(int usuarioId)
    {
        return await context.Receitas
            .CountAsync(r => r.UsuarioId == usuarioId);
    }

    public async Task<Receita?> BuscarReceitaPorIdAsync(int id)
    {
        return await context.Receitas
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AdicionarReceitaAsync(Receita receita)
    {
        await context.Receitas.AddAsync(receita);
        await context.SaveChangesAsync();
    }

    public async Task AtualizarReceitaAsync(Receita receita)
    {
        context.Receitas.Update(receita);
        await context.SaveChangesAsync();
    }

    public async Task RemoverReceitaAsync(Receita receita)
    {
        context.Receitas.Remove(receita);
        await context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Receita>> ListarReceitasPaginadoPorDataAsync(int usuarioId, DateOnly data, int pagina, int quantidade)
    {
        return await context.Receitas
            .Where(r => r.UsuarioId == usuarioId && r.Data == data)
            .OrderByDescending(r => r.Data)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }

    public async Task<int> ContarReceitasPorDataAsync(int usuarioId, DateOnly data)
    {
        return await context.Receitas
            .CountAsync(r => r.UsuarioId == usuarioId && r.Data == data);
    }

    public async Task<IReadOnlyList<Receita>> ListarReceitasPaginadoPorContaAsync(int usuarioId, int contaId, int pagina, int quantidade)
    {
        return await context.Receitas
            .Where(r => r.UsuarioId == usuarioId && r.ContaBancariaId == contaId)
            .OrderByDescending(r => r.Data)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }

    public async Task<int> ContarReceitasPorContaAsync(int usuarioId, int contaId)
    {
        return await context.Receitas
            .CountAsync(r => r.UsuarioId == usuarioId && r.ContaBancariaId == contaId);
    }

    public async Task<IReadOnlyList<Receita>> ListarReceitasPaginadoPorCategoriaAsync(int usuarioId, int categoriaId, int pagina, int quantidade)
    {
        return await context.Receitas
            .Where(r => r.UsuarioId == usuarioId && r.CategoriaId == categoriaId)
            .OrderByDescending(r => r.Data)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }

    public async Task<int> ContarReceitasPorCategoriaAsync(int usuarioId, int categoriaId)
    {
        return await context.Receitas
            .CountAsync(r => r.UsuarioId == usuarioId && r.CategoriaId == categoriaId);
    }

    public async Task<IReadOnlyList<Receita>> ListarReceitasPaginadoPorTituloAsync(int usuarioId, string titulo, int pagina, int quantidade)
    {
        return await context.Receitas
            .Where(r => r.UsuarioId == usuarioId && r.TituloReceita.Contains(titulo))
            .OrderByDescending(r => r.Data)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }

    public async Task<int> ContarReceitasPorTituloAsync(int usuarioId, string titulo)
    {
        return await context.Receitas
            .CountAsync(r => r.UsuarioId == usuarioId && r.TituloReceita.Contains(titulo));
    }
}