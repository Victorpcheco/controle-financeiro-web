using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Domain.Models;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Repositories;

public class CategoriaRepository(ApplicationDbContext context) : ICategoriaRepository
{
    public async Task<IReadOnlyList<Categoria>> ListarCategoriaPaginadoAsync(int usuarioId,int pagina, int quantidadePorPagina)
    {
        return await context.Categorias
            .Where(c => c.UsuarioId == usuarioId)
            .OrderBy(c => c.Id)
            .Skip((pagina - 1) * quantidadePorPagina)
            .Take(quantidadePorPagina)
            .AsNoTracking()
            .ToListAsync();
    }
        
    public async Task<int> ContarTotalAsync()
    {
        return await context.Categorias.CountAsync();
    }
        
    public async Task<Categoria?> BuscarCategoriaPorIdAsync(int id)
    {
        return await context.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Categoria?> BuscarCategoriaPorNomeAsync(string nome)
    {
        return await context.Categorias
            .AsNoTracking() 
            .FirstOrDefaultAsync(c => c.NomeCategoria == nome);
    }
        
    public async Task CriarCategoriaAsync(Categoria categoria)
    {
        await context.Categorias.AddAsync(categoria);
        await context.SaveChangesAsync();
    }
        
    public async Task AtualizarCategoriaAsync(Categoria categoria)
    {
        context.Categorias.Update(categoria);
        await context.SaveChangesAsync();
    }
        
    public async Task DeletarCategoriaAsync(Categoria categoria)
    {
        context.Categorias.Remove(categoria);
        await context.SaveChangesAsync();
    }
}