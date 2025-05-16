using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;

namespace FinancialControl.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<(IReadOnlyList<Categoria> Categorias, int Total)> ListarPaginadoAsync(int pagina, int quantidadePorPagina)
        {
            if (pagina < 1)
                throw new ArgumentException("A página deve ser maior ou igual a 1.");
            if (quantidadePorPagina < 1)
                throw new ArgumentException("A quantidade deve ser maior ou igual a 1.");

            var categorias = await _categoriaRepository.ListarPaginadoAsync(pagina, quantidadePorPagina);
            var total = await _categoriaRepository.ContarTotalAsync();

            return (categorias, total);
        }
    }
}
