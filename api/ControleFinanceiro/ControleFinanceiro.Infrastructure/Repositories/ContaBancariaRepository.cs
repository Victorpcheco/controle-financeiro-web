using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Domain.Models;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Repositories;

public class ContaBancariaRepository(ApplicationDbContext context) : IContaBancariaRepository
{
    public async Task<IReadOnlyList<ContaBancaria>> ListarContasPaginadoAsync(int usuarioId, int pagina, int quantidadePorPagina)
    {
        return await context.ContasBancarias
            .Where(c => c.UsuarioId == usuarioId)
            .OrderBy(c => c.Id)
            .Skip((pagina - 1) * quantidadePorPagina)
            .Take(quantidadePorPagina)
            .ToListAsync();
    }
    
    public async Task<int> ContarTotalAsync()
    {
        return await context.ContasBancarias.CountAsync();
    }
    
    public async Task<ContaBancaria?> BuscarContaPorIdAsync(int id)
    {
        return await context.ContasBancarias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task CriarContaAsync(ContaBancaria contaBancaria)
    {
        await context.ContasBancarias.AddAsync(contaBancaria);
        await context.SaveChangesAsync();
    }
    
    public async Task AtualizarContaAsync(ContaBancaria contaBancaria)
    {
        context.ContasBancarias.Update(contaBancaria);
        await context.SaveChangesAsync();
    }
    
    public async Task DeletarContaAsync(ContaBancaria contaBancaria)
    {
        context.ContasBancarias.Remove(contaBancaria);
        await context.SaveChangesAsync();
    }
}
