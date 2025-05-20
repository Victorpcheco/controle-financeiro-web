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
        public async Task<IReadOnlyList<Categoria>> ListarPaginadoAsync(int usuarioId,int pagina, int quantidadePorPagina)
        {
            return await _context.Categorias
                    .Where(c => c.UsuarioId == usuarioId)
                    .OrderBy(c => c.Id)
                    .Skip((pagina - 1) * quantidadePorPagina)
                    .Take(quantidadePorPagina)
                    .AsNoTracking()
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

        public async Task<Categoria> BuscarPorNome(string nome)
        {
            return await _context.Categorias
                .AsNoTracking() 
                .FirstOrDefaultAsync(c => c.Nome == nome);
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
