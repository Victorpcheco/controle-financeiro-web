using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FinancialControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<Categoria>> ListarPaginadoAsync(int pagina, int quantidadePorPagina)
        {
            return await _context.Categorias
                .OrderBy(c => c.Id)
                .Skip((pagina - 1) * quantidadePorPagina) // pula os registros de acordo com a página fornecida
                .Take(quantidadePorPagina) // pega a próxima quantidade de registros especificada
                .ToListAsync();
        }
        public async Task<int> ContarTotalAsync()
        {
            return await _context.Categorias.CountAsync();
        }
        public async Task<Categoria?> BuscarPorId(int id)
        {
            return await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task CriarCategoriaAsync(Categoria categoria)
        {
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();
        }
        
        public async Task AtualizarCategoriaAsync(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeletarCategoriaAsync(Categoria categoria)
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}
