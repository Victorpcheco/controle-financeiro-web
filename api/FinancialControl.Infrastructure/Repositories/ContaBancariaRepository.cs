using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FinancialControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.Repositories;

public class ContaBancariaRepository : IContaBancariaRepository
{
    private readonly ApplicationDbContext _context;

    public ContaBancariaRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyList<ContaBancaria>> ListarContasPaginado(int usuarioId, int pagina, int quantidadePorPagina)
    {
        return await _context.ContasBancarias
            .Where(c => c.UsuarioId == usuarioId)
            .OrderBy(c => c.Id)
            .Skip((pagina - 1) * quantidadePorPagina)
            .Take(quantidadePorPagina)
            .ToListAsync();
    }
    public async Task<int> ContarTotalAsync()
    {
        return await _context.ContasBancarias.CountAsync();
    }
    public async Task<ContaBancaria> BuscarPorId(int id)
    {
        return await _context.ContasBancarias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task AdicionarContaBancaria(ContaBancaria contaBancaria)
    {
        await _context.ContasBancarias.AddAsync(contaBancaria);
        await _context.SaveChangesAsync();
    }
    public async Task AtualizarContaBancaria(ContaBancaria contaBancaria)
    {
        _context.ContasBancarias.Update(contaBancaria);
        await _context.SaveChangesAsync();
    }
    public async Task DeletarContaBancaria(ContaBancaria contaBancaria)
    {
        _context.ContasBancarias.Remove(contaBancaria);
        await _context.SaveChangesAsync();
    }
}