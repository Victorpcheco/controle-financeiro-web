using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FinancialControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.Repositories;

public class CartaoRepository : ICartaoRepository
{
    private readonly ApplicationDbContext _context;

    public CartaoRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyList<Cartao>> ListarPaginadoAsync(int usuarioId, int pagina, int quantidade)
    {
        return await _context.Cartoes
            .Where(c => c.UsuarioId == usuarioId)
            .OrderBy(c => c.Id)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }
    
    public async Task<int> ContarAsync(int usuarioId)
    {
        return await _context.Cartoes
            .AsNoTracking()
            .CountAsync(c => c.UsuarioId == usuarioId);
    }
    
    public async Task<Cartao?> BuscarPorIdAsync(int id)
    {
       return await _context.Cartoes
           .AsNoTracking()
           .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task AdicionarAsync(Cartao cartao)
    {
        await _context.Cartoes.AddAsync(cartao);
        await _context.SaveChangesAsync();
    }
    
    public async Task AtualizarAsync(Cartao cartao)
    {
        _context.Cartoes.Update(cartao);
        await _context.SaveChangesAsync();
    }
    
    public async Task RemoverAsync(Cartao cartao)
    {
        _context.Cartoes.Remove(cartao);
        await _context.SaveChangesAsync();
    }
    
}