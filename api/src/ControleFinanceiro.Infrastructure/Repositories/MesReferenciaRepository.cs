using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Repositories;

public class MesReferenciaRepository(ApplicationDbContext context) : IMesReferenciaRepository
{
    public async Task<IReadOnlyList<MesReferencia>> ListarMesReferenciaPaginadoAsync(int usuarioId, int pagina, int quantidade)
    {
        return await context.MesesReferencia
            .Where(c => c.UsuarioId == usuarioId)
            .OrderBy(c => c.Id)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();
    }
    
    public async Task<int> ContarTotalAsync(int usuarioId)
    {
        return await context.MesesReferencia
            .CountAsync(c => c.UsuarioId == usuarioId);
    }
    
    public async Task<MesReferencia?> BuscarMesReferenciaPorId(int id)
    {
        return await context.MesesReferencia
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task AdicionarMesReferenciaAsync(MesReferencia mes)
    {
        await context.MesesReferencia.AddAsync(mes);
        await context.SaveChangesAsync();
    }
    
    public async Task AtualizarMesReferenciaAsync(MesReferencia mes)
    {
        context.MesesReferencia.Update(mes);
        await context.SaveChangesAsync();
    }
    
    public async Task DeletarMesReferenciaAsync(MesReferencia mes)
    {
        context.MesesReferencia.Remove(mes);
        await context.SaveChangesAsync();
    }
}