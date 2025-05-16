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
    }
}
