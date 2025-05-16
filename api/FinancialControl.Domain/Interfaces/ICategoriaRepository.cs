
using FinancialControl.Domain.Entities;

namespace FinancialControl.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IReadOnlyList<Categoria>> ListarPaginadoAsync(int pagina, int quantidadePorPagina);
        Task<int> ContarTotalAsync();
    }
}
